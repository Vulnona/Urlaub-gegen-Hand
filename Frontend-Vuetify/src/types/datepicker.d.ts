declare module 'vue3-datepicker' {
    import { DefineComponent } from 'vue'
    import { Locale } from 'date-fns'
    
    const component: DefineComponent<{
      modelValue?: Date
      locale?: Locale
      // Add other props as needed
    }, {}, any>
    
    export default component
  }