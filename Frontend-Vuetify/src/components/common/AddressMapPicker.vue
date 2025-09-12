<template>
  <div class="address-map-picker">
    <div v-if="!hideSearch" class="search-container mb-3">
      <div class="form-group">
        <label for="address-search">Adresse suchen</label>
        <div class="input-group">
          <input
            type="text"
            class="form-control"
            id="address-search"
            v-model="searchQuery"
            @input="onSearchInput"
            @keydown.enter.prevent="searchAddress"
            placeholder="Geben Sie eine Adresse ein..."
          />
          <button
            class="btn btn-primary"
            type="button"
            @click="searchAddress"
            :disabled="!searchQuery || isLoading"
          >
            <i class="ri-search-line" v-if="!isLoading"></i>
            <i class="ri-loader-4-line spinning" v-else></i>
            Suchen
          </button>
        </div>
      </div>
      
      <!-- Search Results Dropdown -->
      <div v-if="searchResults.length > 0" class="search-results">
        <div class="list-group">
          <button
            v-for="result in searchResults"
            :key="result.placeId"
            type="button"
            class="list-group-item list-group-item-action"
            @click="selectAddress(result)"
          >
            <div class="d-flex w-100 justify-content-between">
              <h6 class="mb-1">{{ result.displayName }}</h6>
              <small>{{ result.country }}</small>
            </div>
            <small class="text-muted">{{ result.city }}, {{ result.postcode }}</small>
          </button>
        </div>
      </div>
    </div>

    <!-- Map Container -->
    <div class="map-container" ref="mapContainer">
      <div id="map" style="width: 100%; height: 400px;"></div>
    </div>

    <!-- Selected Address Display -->
    <div v-if="selectedAddress" class="selected-address mt-3">
      <div class="alert alert-success">
        <h6><i class="ri-map-pin-line"></i> Ausgewählte Adresse:</h6>
        <p class="mb-1"><strong>{{ selectedAddress.displayName }}</strong></p>
        <small class="text-muted">
          {{ $options.filters && $options.filters.formatAddressDisplay ? $options.filters.formatAddressDisplay(selectedAddress) : (selectedAddress.road || '') + (selectedAddress.houseNumber ? ' ' + selectedAddress.houseNumber : '') + (selectedAddress.postcode ? ', ' + selectedAddress.postcode : '') + (selectedAddress.city ? ' ' + selectedAddress.city : '') + (selectedAddress.country ? ', ' + selectedAddress.country : '') }}
        </small>
      </div>
    </div>

    <!-- Current Location Button -->
    <div class="text-center mt-3">
      <button
        type="button"
        class="btn btn-outline-secondary me-2"
        @click="getCurrentLocation"
        :disabled="isLocating"
      >
        <i class="ri-gps-line" v-if="!isLocating"></i>
        <i class="ri-loader-4-line spinning" v-else></i>
        Meinen Standort verwenden
      </button>
      
      <!-- Confirm Button -->
      <button
        v-if="showConfirmButton && selectedAddress"
        type="button"
        class="btn btn-primary"
        @click="confirmSelection"
      >
        <i class="ri-check-line"></i>
        Standort bestätigen
      </button>
    </div>
  </div>
</template>

<script>
import { ref, onMounted, onUnmounted, watch, nextTick } from 'vue'
import axiosInstance from '@/interceptor/interceptor'
import toast from '@/components/toaster/toast'

export default {
  name: 'AddressMapPicker',
  emits: ['address-selected'],
  props: {
    initialAddress: {
      type: Object,
      default: null
    },
    required: {
      type: Boolean,
      default: true
    },
    hideSearch: {
      type: Boolean,
      default: false
    },
    showConfirmButton: {
      type: Boolean,
      default: false
    }
  },
  setup(props, { emit }) {
    const searchQuery = ref('')
    const searchResults = ref([])
    const selectedAddress = ref(null)
    const isLoading = ref(false)
    const isLocating = ref(false)
    const mapContainer = ref(null)
    
    let map = null
    let marker = null
    let searchTimeout = null

    // Initialize map (using OpenStreetMap with Leaflet)
    let initMapRetryCount = 0;
    const maxRetries = 10;
    let isComponentMounted = true;
    
    const initMap = () => {
      // Check if component is still mounted
      if (!isComponentMounted) {
        console.log('Component unmounted, skipping map initialization');
        return;
      }
      
      // Check retry count to prevent infinite loop
      if (initMapRetryCount >= maxRetries) {
        console.error('Max retries reached for map initialization. Giving up.');
        toast.error('Karte konnte nicht geladen werden. Bitte laden Sie die Seite neu.');
        return;
      }
      
      initMapRetryCount++;
      
      // Check if map container exists
      const mapElement = document.getElementById('map')
      if (!mapElement) {
        console.error(`Map container not found, retry ${initMapRetryCount}/${maxRetries}...`)
        setTimeout(initMap, 200)
        return
      }

      // Default center (Germany)
      const defaultLat = 51.1657
      const defaultLng = 10.4515
      const defaultZoom = 6

      try {
      // Create map
      map = L.map('map').setView([defaultLat, defaultLng], defaultZoom)

      // Add OpenStreetMap tiles
      L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '© OpenStreetMap contributors'
      }).addTo(map)

      // Add click handler
      map.on('click', onMapClick)

      // Set initial address if provided
      if (props.initialAddress) {
        setAddressOnMap(props.initialAddress)
        }
        
        console.log('Map initialized successfully');
      } catch (error) {
        console.error('Error initializing map:', error)
        if (initMapRetryCount < maxRetries && isComponentMounted) {
          // Retry after a short delay
          setTimeout(initMap, 500)
        } else {
          toast.error('Karte konnte nicht geladen werden. Bitte laden Sie die Seite neu.');
        }
      }
    }

    const onMapClick = async (e) => {
      const lat = e.latlng.lat
      const lng = e.latlng.lng
      
      try {
        isLoading.value = true
        console.log('DEBUG: onMapClick called with coordinates:', lat, lng)
        
        // Reverse geocoding
        const response = await axiosInstance.get('geocoding/reverse', {
          params: { lat: lat, lon: lng }
        })
        
        console.log('DEBUG: onMapClick response:', response)
        const address = response.data
        setAddressOnMap(address)
        
      } catch (error) {
        console.error('Reverse geocoding error:', error)
        console.error('ERROR Details:', {
          message: error.message,
          status: error.response?.status,
          statusText: error.response?.statusText,
          data: error.response?.data,
          config: error.config,
          coordinates: { lat, lng }
        })
        toast.error('Adressdaten konnten nicht abgerufen werden. Bitte versuchen Sie es erneut.')
      } finally {
        isLoading.value = false
      }
    }

    const onSearchInput = () => {
      if (searchTimeout) {
        clearTimeout(searchTimeout)
      }
      
      searchTimeout = setTimeout(() => {
        if (searchQuery.value.length >= 3) {
          getSuggestions()
        } else {
          searchResults.value = []
        }
      }, 300)
    }

    const getSuggestions = async () => {
      if (!searchQuery.value || searchQuery.value.length < 3) return
      
      try {
        console.log('DEBUG: getSuggestions called with query:', searchQuery.value)
        console.log('DEBUG: axiosInstance baseURL:', axiosInstance.defaults.baseURL)
        
        const response = await axiosInstance.get('geocoding/search', {
          params: { query: searchQuery.value }
        })
        
        console.log('DEBUG: getSuggestions response:', response)
        searchResults.value = response.data || []
        
      } catch (error) {
        console.error('Address suggestions error:', error)
        console.error('ERROR Details:', {
          message: error.message,
          status: error.response?.status,
          statusText: error.response?.statusText,
          data: error.response?.data,
          config: error.config
        })
        searchResults.value = []
        toast.error('Fehler beim Laden der Adressvorschläge: ' + error.message)
      }
    }

    const searchAddress = async () => {
      if (!searchQuery.value) return
      
      try {
        isLoading.value = true
        console.log('DEBUG: searchAddress called with query:', searchQuery.value)
        
        const response = await axiosInstance.get('geocoding/search', {
          params: { query: searchQuery.value }
        })
        
        console.log('DEBUG: searchAddress response:', response)
        const results = response.data || []
        if (results.length > 0) {
          selectAddress(results[0])
        } else {
          toast.warning('Keine Ergebnisse gefunden')
        }
        
      } catch (error) {
        console.error('Address search error:', error)
        console.error('ERROR Details:', {
          message: error.message,
          status: error.response?.status,
          statusText: error.response?.statusText,
          data: error.response?.data,
          config: error.config,
          query: searchQuery.value
        })
        toast.error('Fehler bei der Adresssuche: ' + error.message)
      } finally {
        isLoading.value = false
      }
    }

    const selectAddress = (address) => {
      setAddressOnMap(address)
      searchResults.value = []
      searchQuery.value = address.displayName
    }

    const setAddressOnMap = (address) => {
      selectedAddress.value = address
      
      // Update map view
      if (map) {
        map.setView([address.latitude, address.longitude], 15)
        
        // Remove existing marker
        if (marker) {
          map.removeLayer(marker)
        }
        
        // Add new marker
        marker = L.marker([address.latitude, address.longitude])
          .addTo(map)
          .bindPopup(address.displayName)
          .openPopup()
      }
      
      // Emit selected address
      emit('address-selected', address)
    }

    const getCurrentLocation = () => {
      if (!navigator.geolocation) {
        toast.error('Geolocation wird von diesem Browser nicht unterstützt')
        return
      }
      
      isLocating.value = true
      
      navigator.geolocation.getCurrentPosition(
        async (position) => {
          const lat = position.coords.latitude
          const lng = position.coords.longitude
          
          try {
            // Reverse geocoding for current position
            const response = await axiosInstance.get('geocoding/reverse', {
              params: { lat: lat, lon: lng }
            })
            
            const address = response.data
            setAddressOnMap(address)
            
          } catch (error) {
            console.error('Current location geocoding error:', error)
            toast.error('Fehler beim Abrufen der Adresse für Ihren Standort')
          } finally {
            isLocating.value = false
          }
        },
        (error) => {
          console.error('Geolocation error:', error)
          toast.error('Fehler beim Ermitteln Ihres Standorts')
          isLocating.value = false
        },
        {
          enableHighAccuracy: true,
          timeout: 10000,
          maximumAge: 300000
        }
      )
    }

    let isInitializing = false;

    onMounted(() => {
      if (isInitializing) return; // Prevent multiple initializations
      isInitializing = true;
      
      // Wait for next tick to ensure DOM is ready
      nextTick(() => {
      // Load Leaflet if not already loaded
      if (typeof L === 'undefined') {
        // Add Leaflet CSS
        const css = document.createElement('link')
        css.rel = 'stylesheet'
        css.href = 'https://unpkg.com/leaflet@1.9.4/dist/leaflet.css'
        document.head.appendChild(css)
        
        // Add Leaflet JS
        const script = document.createElement('script')
        script.src = 'https://unpkg.com/leaflet@1.9.4/dist/leaflet.js'
        script.onload = () => {
            setTimeout(() => {
              initMap()
              isInitializing = false
            }, 500)
          }
          script.onerror = () => {
            console.error('Failed to load Leaflet library')
            toast.error('Kartenbibliothek konnte nicht geladen werden.')
            isInitializing = false
        }
        document.head.appendChild(script)
      } else {
          setTimeout(() => {
            initMap()
            isInitializing = false
          }, 500)
      }
      })
    })

    onUnmounted(() => {
      isComponentMounted = false; // Mark component as unmounted
      if (searchTimeout) {
        clearTimeout(searchTimeout)
      }
      if (map) {
        try {
        map.remove()
          map = null
        } catch (error) {
          console.log('Error removing map:', error)
        }
      }
    })

    const confirmSelection = () => {
      if (selectedAddress.value) {
        emit('address-selected', selectedAddress.value)
      }
    }

    return {
      searchQuery,
      searchResults,
      selectedAddress,
      isLoading,
      isLocating,
      mapContainer,
      onSearchInput,
      searchAddress,
      selectAddress,
      getCurrentLocation,
      confirmSelection
    }
  }
}
</script>

<style scoped>
.address-map-picker {
  width: 100%;
}

.search-container {
  position: relative;
}

.form-group {
  margin-bottom: 1rem;
}

.form-group label {
  display: block;
  margin-bottom: 0.5rem;
  font-weight: 500;
  color: #333;
}

.input-group {
  display: flex;
  gap: 0.5rem;
}

.form-control {
  flex: 1;
  padding: 0.75rem;
  border: 1px solid #ddd;
  border-radius: 0.375rem;
  font-size: 1rem;
  transition: border-color 0.15s ease-in-out, box-shadow 0.15s ease-in-out;
}

.form-control:focus {
  outline: none;
  border-color: #0d6efd;
  box-shadow: 0 0 0 0.2rem rgba(13, 110, 253, 0.25);
}

.btn {
  padding: 0.75rem 1rem;
  border: 1px solid transparent;
  border-radius: 0.375rem;
  font-size: 1rem;
  font-weight: 400;
  text-align: center;
  cursor: pointer;
  transition: all 0.15s ease-in-out;
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
}

.btn-primary {
  color: #fff;
  background-color: #0d6efd;
  border-color: #0d6efd;
}

.btn-primary:hover:not(:disabled) {
  background-color: #0b5ed7;
  border-color: #0a58ca;
}

.btn-outline-secondary {
  color: #6c757d;
  border-color: #6c757d;
}

.btn-outline-secondary:hover:not(:disabled) {
  color: #fff;
  background-color: #6c757d;
  border-color: #6c757d;
}

.btn:disabled {
  opacity: 0.65;
  cursor: not-allowed;
}

.search-results {
  position: absolute;
  top: 100%;
  left: 0;
  right: 0;
  z-index: 1000;
  max-height: 300px;
  overflow-y: auto;
  background: white;
  border: 1px solid #ddd;
  border-radius: 0.375rem;
  box-shadow: 0 0.5rem 1rem rgba(0, 0, 0, 0.15);
}

.list-group {
  margin: 0;
  padding: 0;
  list-style: none;
}

.list-group-item {
  padding: 0.75rem 1rem;
  border: none;
  border-bottom: 1px solid #dee2e6;
  background-color: transparent;
  cursor: pointer;
  width: 100%;
  text-align: left;
  transition: background-color 0.15s ease-in-out;
}

.list-group-item:hover {
  background-color: #f8f9fa;
}

.list-group-item:last-child {
  border-bottom: none;
}

.map-container {
  border: 1px solid #ddd;
  border-radius: 0.375rem;
  overflow: hidden;
  margin: 1rem 0;
}

.selected-address {
  border-radius: 0.375rem;
}

.alert {
  padding: 0.75rem 1.25rem;
  margin-bottom: 1rem;
  border: 1px solid transparent;
  border-radius: 0.375rem;
}

.alert-success {
  color: #0f5132;
  background-color: #d1e7dd;
  border-color: #badbcc;
}

.text-center {
  text-align: center;
}

.mt-3 {
  margin-top: 1rem;
}

.mb-3 {
  margin-bottom: 1rem;
}

.mb-1 {
  margin-bottom: 0.25rem;
}

.text-muted {
  color: #6c757d;
}

.d-flex {
  display: flex;
}

.w-100 {
  width: 100%;
}

.justify-content-between {
  justify-content: space-between;
}

.spinning {
  animation: spin 1s linear infinite;
}

@keyframes spin {
  from { transform: rotate(0deg); }
  to { transform: rotate(360deg); }
}

/* Responsive Design */
@media (max-width: 768px) {
  .input-group {
    flex-direction: column;
  }
  
  .btn {
    width: 100%;
    justify-content: center;
  }
  
  .map-container {
    margin: 0.5rem 0;
  }
  
  #map {
    height: 300px !important;
  }
}

/* Override Leaflet default styles */
:deep(.leaflet-container) {
  font-family: inherit;
}

:deep(.leaflet-popup-content) {
  margin: 8px 12px;
  font-size: 14px;
}

:deep(.leaflet-control-attribution) {
  font-size: 11px;
}

/* Custom map controls */
:deep(.leaflet-control-zoom) {
  border: 1px solid #ddd;
  border-radius: 0.375rem;
}

:deep(.leaflet-control-zoom a) {
  border-radius: 0;
  color: #333;
}

:deep(.leaflet-control-zoom a:hover) {
  background-color: #f8f9fa;
}
</style>
