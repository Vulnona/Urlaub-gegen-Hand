// Utility functions for address handling

export const formatAddressDisplay = (address) => {
  if (!address) return 'Keine Adresse'
  
  const parts = []
  
  if (address.houseNumber) parts.push(address.houseNumber)
  if (address.road) parts.push(address.road)
  if (address.city) parts.push(address.city)
  if (address.postcode) parts.push(address.postcode)
  if (address.country) parts.push(address.country)
  
  return parts.join(', ') || address.displayName || 'Unbekannte Adresse'
}

export const formatAddressShort = (address) => {
  if (!address) return 'Keine Adresse'
  
  const parts = []
  
  if (address.city) parts.push(address.city)
  if (address.postcode) parts.push(address.postcode)
  if (address.country) parts.push(address.country)
  
  return parts.join(', ') || address.displayName || 'Unbekannte Adresse'
}

export const getCoordinatesString = (address) => {
  if (!address || !address.latitude || !address.longitude) {
    return 'Keine Koordinaten'
  }
  
  return `${address.latitude.toFixed(6)}, ${address.longitude.toFixed(6)}`
}

export const getOpenStreetMapUrl = (address) => {
  if (!address || !address.latitude || !address.longitude) {
    return null
  }
  
  return `https://www.openstreetmap.org/?mlat=${address.latitude}&mlon=${address.longitude}&zoom=15&layers=M`
}

export const validateAddress = (address) => {
  if (!address) return false
  
  // Minimum requirements: coordinates and display name or basic address components
  const hasCoordinates = address.latitude && address.longitude
  const hasDisplayName = address.displayName && address.displayName.trim().length > 0
  const hasBasicAddress = address.city || address.road
  
  return hasCoordinates && (hasDisplayName || hasBasicAddress)
}

export const createAddressFromNominatim = (nominatimData) => {
  if (!nominatimData) return null
  
  return {
    latitude: parseFloat(nominatimData.lat),
    longitude: parseFloat(nominatimData.lon),
    displayName: nominatimData.display_name,
    houseNumber: nominatimData.address?.house_number || '',
    road: nominatimData.address?.road || '',
    city: nominatimData.address?.city || nominatimData.address?.town || nominatimData.address?.village || '',
    postcode: nominatimData.address?.postcode || '',
    country: nominatimData.address?.country || '',
    addressType: 'Location'
  }
}
