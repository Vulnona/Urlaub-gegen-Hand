<template>
    <div class="relative w-64">
      <!-- Dropdown Toggle Button -->
      <button
        @click="toggleDropdown"
        class="w-full bg-white border border-gray-300 rounded-lg px-4 py-2 text-left focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent flex justify-between items-center"
      >
        {{ selectedItem || 'Select an option' }}
        <svg
          :class="{ 'transform rotate-180': isOpen }"
          class="w-5 h-5 transition-transform duration-200"
          xmlns="http://www.w3.org/2000/svg"
          viewBox="0 0 20 20"
          fill="currentColor"
        >
          <path fill-rule="evenodd" d="M5.293 7.293a1 1 0 011.414 0L10 10.586l3.293-3.293a1 1 0 111.414 1.414l-4 4a1 1 0 01-1.414 0l-4-4a1 1 0 010-1.414z" clip-rule="evenodd" />
        </svg>
      </button>
  
      <!-- Dropdown Menu -->
      <div
        v-if="isOpen"
        class="absolute z-10 w-full mt-1 bg-white border border-gray-300 rounded-lg shadow-lg"
      >
        <ul class="py-1 max-h-60 overflow-auto">
          <!-- Visible Items -->
          <li
            v-for="(item, index) in items.slice(0, currentVisibleCount)"
            :key="item.id"
            @click="selectItem(item)"
            class="px-4 py-2 hover:bg-blue-50 cursor-pointer transition-colors duration-150"
            :class="{ 'bg-blue-50': item.value === selectedItem }"
          >
            {{ item.value }}
          </li>
  
          <!-- Show More Button -->
          <li
            v-if="hasMoreItems"
            @click.stop="loadMore"
            class="px-4 py-2 text-blue-500 hover:bg-blue-50 cursor-pointer text-center font-medium border-t"
          >
            Show More ({{ remainingCount }} more)
          </li>
        </ul>
      </div>
    </div>
  </template>
  
  <script>
  export default {
    name: 'DropdownAppendMore',
    data() {
      return {
        isOpen: false,
        selectedItem: null,
        currentVisibleCount: 5,
        itemsPerLoad: 5,
        // Example items - replace with your actual data
        items: Array.from({ length: 20 }, (_, i) => ({
          id: i + 1,
          value: `Option ${i + 1}`
        }))
      }
    },
    computed: {
      hasMoreItems() {
        return this.currentVisibleCount < this.items.length
      },
      remainingCount() {
        return this.items.length - this.currentVisibleCount
      }
    },
    methods: {
      toggleDropdown() {
        this.isOpen = !this.isOpen
        // Reset visible count when closing
        if (!this.isOpen) {
          this.currentVisibleCount = 5
        }
      },
      selectItem(item) {
        this.selectedItem = item.value
        this.$emit('item-selected', item)
      },
      loadMore(event) {
        // Prevent event bubbling
        event.stopPropagation()
        
        // Append more items
        this.currentVisibleCount += this.itemsPerLoad
        
        // Emit the load more event
        this.$emit('load-more', this.currentVisibleCount)
      },
      closeDropdown(e) {
        if (!this.$el.contains(e.target)) {
          this.isOpen = false
          this.currentVisibleCount = 5 // Reset when closing
        }
      }
    },
    mounted() {
      // Close dropdown when clicking outside
      document.addEventListener('click', this.closeDropdown)
    },
    beforeDestroy() {
      document.removeEventListener('click', this.closeDropdown)
    }
  }
  </script>