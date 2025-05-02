declare module 'vue-multiselect' {
    import { DefineComponent } from 'vue'
    
    const Multiselect: DefineComponent<{
      options: any[]
      value: any
      multiple?: boolean
      trackBy?: string
      label?: string
      searchable?: boolean
      clearOnSelect?: boolean
      hideSelected?: boolean
      placeholder?: string
      tagPlaceholder?: string
      taggable?: boolean
      allowEmpty?: boolean
      onChange?: () => void
      onSelect?: () => void
      onRemove?: () => void
      onSearchChange?: () => void
      onTag?: () => void
      onClear?: () => void
      onOpen?: () => void
      onClose?: () => void
    }, {}, any>
    
    export default Multiselect
  }