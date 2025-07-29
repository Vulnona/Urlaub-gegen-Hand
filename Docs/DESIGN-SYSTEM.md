# UGH Design System

## Farben

### Primärfarben
- **Primary Yellow**: `#ffc107` - Hauptakzentfarbe für Buttons, Icons und Highlights
- **Primary Blue**: `#001f3f` - Haupttextfarbe und dunkle Buttons
- **Secondary Blue**: `#004080` - Hover-Zustand für blaue Buttons

### Textfarben
- **Text Dark**: `#333333` - Haupttextfarbe
- **Text Light**: `#666666` - Sekundärer Text
- **Text White**: `#ffffff` - Weißer Text auf dunklen Hintergründen

### Hintergrundfarben
- **Background White**: `#ffffff` - Weißer Hintergrund
- **Background Light**: `#f9f9f9` - Heller Hintergrund
- **Background Gradient**: `linear-gradient(135deg, #ff9a9e, #fad0c4)` - Gradient für E-Mails

### Schatten
- **Shadow Light**: `rgba(0, 0, 0, 0.1)` - Leichte Schatten
- **Shadow Medium**: `rgba(0, 0, 0, 0.2)` - Mittlere Schatten
- **Shadow Dark**: `rgba(0, 0, 0, 0.3)` - Dunkle Schatten

### Statusfarben
- **Success Green**: `#4CAF50` - Erfolg
- **Error Red**: `#e53935` - Fehler
- **Warning Orange**: `#ff9800` - Warnung
- **Info Blue**: `#2196F3` - Information

## Verwendung

### CSS-Variablen
Alle Farben sind als CSS-Variablen definiert und können in Komponenten verwendet werden:

```scss
.my-component {
  color: var(--primary-blue);
  background-color: var(--primary-yellow);
  box-shadow: 0 4px 8px var(--shadow-medium);
}
```

### SCSS-Variablen
Für SCSS-Komponenten stehen auch SCSS-Variablen zur Verfügung:

```scss
@import '@/styles/colors.scss';

.my-component {
  color: $primary-blue;
  background-color: $primary-yellow;
  box-shadow: 0 4px 8px $shadow-medium;
}
```

### Utility-Klassen
Vordefinierte Utility-Klassen für schnelle Anwendung:

```html
<div class="text-primary-yellow">Gelber Text</div>
<div class="bg-primary-blue">Blauer Hintergrund</div>
<div class="bg-gradient">Gradient Hintergrund</div>
```

### Button-Styles
Vordefinierte Button-Styles:

```html
<button class="btn-primary-ugh">Primärer Button</button>
<button class="btn-secondary-ugh">Sekundärer Button</button>
```

### Card-Styles
Vordefinierte Card-Styles:

```html
<div class="card-ugh">
  <h3>Card Titel</h3>
  <p>Card Inhalt</p>
</div>
```

### Coupon-Display
Spezieller Style für Coupon-Anzeige:

```html
<div class="coupon-display">COUPON123</div>
```

## E-Mail-Templates

Die E-Mail-Templates verwenden das gleiche Design-System:

- **Icons**: 🎉 für erhaltene Coupons, 🎊 für gekaufte Coupons
- **Farben**: Gelb/Orange und Dunkelblau
- **Layout**: Zentriert mit abgerundeten Ecken und Schatten
- **Sprache**: Deutsch

## Best Practices

1. **Konsistenz**: Verwende immer die definierten Farben und Variablen
2. **Kontrast**: Stelle sicher, dass Text gut lesbar ist
3. **Zugänglichkeit**: Verwende ausreichende Kontraste für Barrierefreiheit
4. **Responsive**: Alle Styles sind responsive und funktionieren auf allen Geräten

## Dateien

- **Farben**: `Frontend-Vuetify/src/styles/colors.scss`
- **Haupt-Styles**: `Frontend-Vuetify/src/styles/settings.scss`
- **E-Mail-Templates**: `Backend/Services/HtmlTemplate/HtmlTemplateService.cs` 