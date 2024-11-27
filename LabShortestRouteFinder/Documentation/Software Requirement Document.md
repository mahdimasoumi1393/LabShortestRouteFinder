
---

# Software Requirement Document for the Application *Shortest Route Finder*

---

## 1. **Introduktion**
Detta dokument beskriver kraven för utvecklingen av en WPF-applikation kallad **Shortest Route Finder**. 

Applikationen hjälper användare att hitta den snabbaste vägen mellan två städer i ett definierat vägnät baserat på data från JSON-filer. 

Applikationen följer **MVVM (Model-View-ViewModel)**-mönstret för att säkerställa en tydlig separation av ansvar, underhållbarhet och skalbarhet.

---

## 2. **Syfte med Systemet**
**Shortest Route Finder** syftar till att:
1. Visa och hantera ett vägnät mellan städer.

2. Låta användare söka efter den snabbaste vägen mellan två städer baserat på avstånd och/eller kostnad.

3. Möjliggöra redigering av rutt- och stadsinformation.

4. Sortera och filtrera rutterna baserat på olika parametrar.

5. Visualisera vägnätet grafiskt och markera den snabbaste vägen.

---

## 3. **Funktionella Krav**

### 3.1 **Måste-krav**  
Applikationen **måste**:

1. **Hantering av indata**:
   - Importera ruttdata från en JSON-fil med följande struktur:
   
     ```json
     [
       { "Startpunkt": "StadA", "Destination": "StadB", "Avstånd": 120, "Kostnad": 15 }
     ]
     ```

   - Importera stadsdata från en JSON-fil med följande struktur:
     
     ```json
     [
       { "Namn": "StadA", "Latitud": 59.3293, "Longitud": 18.0686 }
     ]
     ```

2. **Visa rutter**:
   - Visa en lista över alla rutter med kolumner för startpunkt, destination, avstånd och kostnad.
   - Tillåta sortering av listan baserat på valfri kolumn (exempelvis startpunkt eller avstånd).
3. **Redigera rutter**:
   - Möjliggöra redigering av startpunkt, destination, avstånd och kostnad för valfri rutt.
   - Spara ändringar tillbaka till JSON-filen.
4. **Hitta snabbaste vägen**:
   - Låta användare specificera en startstad och en destinationstad.
   - Beräkna och visa den snabbaste vägen baserat på:
     - Avstånd.
     - Kostnad.
5. **Grafisk visualisering**:
   - Visa vägnätet som en graf, där noder representerar städer och kanter representerar rutter.
   - Markera den snabbaste vägen i grafen.

### 3.2 **Bra-att-ha-krav**  
Applikationen **bör**:
1. **Dynamiska uppdateringar**:
   - Dynamiskt uppdatera grafen vid förändringar i rutt- eller stadsdata.
2. **Sökmöjligheter**:
   - Låta användare söka efter specifika rutter eller städer i listan.

### 3.3 **Bonuskrav**  
Applikationen **kan**:
1. Tillåta export av uppdaterade rutt- och stadsdata till JSON-filer.
2. Erbjuda olika visuella teman för grafen och WPF-gränssnittet.

---

## 4. **Icke-funktionella Krav**

1. **Användarvänlighet**:
   - Intuitivt gränssnitt med tydliga instruktioner och hjälptexter.
2. **Prestanda**:
   - Hantera vägnät med upp till 50 städer och 100 rutter utan märkbar fördröjning.
3. **Skalbarhet**:
   - Möjlighet att enkelt lägga till nya funktioner, som fler sökkriterier.
4. **Tillförlitlighet**:
   - Validera JSON-strukturen vid inläsning för att undvika krascher.

---

## 5. **Systemarkitektur**

### 5.1 **Designmönster**  
Applikationen kommer att följa **MVVM**-mönstret:
1. **Model**:
   - Representerar städer och rutter.
   - Implementerar logik för beräkning av snabbaste vägen (t.ex. Dijkstras algoritm).
2. **ViewModel**:
   - Kopplar Model till View.
   - Exponerar kommandon och egenskaper för databindning.
3. **View**:
   - Tillhandahåller ett grafiskt gränssnitt med WPF.

### 5.2 **Moduler**:
1. **Ruttmodul**:
   - Läser in och sparar ruttdata från JSON.
   - Möjliggör sortering och redigering av rutter.
2. **Stadsmodul**:
   - Läser in och visar stadsdata.
3. **Vägvalsmodul**:
   - Implementerar algoritmer för att hitta snabbaste vägen.
4. **Grafmodul**:
   - Renderar vägnätet och markerar snabbaste vägen.

---

## 6. **Gränssnittsdesign**

### 6.1 **Huvudfönster**:
- **Flikar**:
  - Ruttlista (sorterbar och redigerbar).
  - Grafisk visualisering.
- **Kontroller**:
  - Knapp för att läsa in data, hitta snabbaste väg och spara ändringar.
  - Dropdown-menyer för att välja start- och destinationstad.
  - Sorterings- och filtreringsalternativ.

### 6.2 **Grafisk visualisering**:
- Visa städer som noder med deras namn.
- Visa rutter som kanter med ev. etiketter för avstånd och kostnad.
- Markera snabbaste vägen med en tydlig färg (*Bonus krav**).

---

## 7. **Exempel på Indata och Utdata**

### Exempel på indata:
**Rutter JSON**:
```json
[
  { "Startpunkt": "StadA", "Destination": "StadB", "Avstånd": 120, "Kostnad": 15 },
  { "Startpunkt": "StadB", "Destination": "StadC", "Avstånd": 100, "Kostnad": 20 }
]
```

**Städer JSON**:
```json
[
  { "Namn": "StadA", "Latitud": 59.3293, "Longitud": 18.0686 },
  { "Namn": "StadB", "Latitud": 57.7089, "Longitud": 11.9746 },
  { "Namn": "StadC", "Latitud": 55.6049, "Longitud": 13.0038 }
]
```

### Exempel på utdata:
1. **Ruttlista**:
   ```
   Startpunkt   Destination   Avstånd (km)   Kostnad
   -------------------------------------------------
   StadA        StadB         120            15
   StadB        StadC         100            20
   ```

2. **Graf**:
   - Noder: StadA, StadB, StadC.
   - Kanter: StadA → StadB (120km, 15), StadB → StadC (100km, 20).

3. **Snabbaste väg**:
   ```
   Kortaste väg (avstånd): StadA → StadB → StadC (220km)
   Kortaste väg (kostnad): StadA → StadB → StadC (35)
   ```

---

## 8. **Utvärderingskriterier**
1. **Korrekthet**:
   - Möjlighet att läsa in och manipulera JSON-data.
   - Korrekt beräkning av snabbaste vägen.
2. **Kodkvalitet**:
   - Rätt implementering av MVVM-principer.
   - Ren, modulär och välkommenterad kod.
3. **Användarupplevelse**:
   - Smidigt och responsivt WPF-gränssnitt.
   - Intuitiv grafisk visualisering.
4. **Kreativitet**:
   - Implementering av valfria funktioner (exempelvis export av data, avancerad grafstil).

---

## 9. **Inlämningsdetaljer**
- **Leverans**: Kompletta projektfiler inklusive källkod, JSON-filer och dokumentation.
- **Deadline**: 2024-11-28.
- **Inlämningsplattform**: Elevens GitHub repo.

---

## 10. **Slutsats**
Detta slutprojekt är en praktisk övning i att utveckla WPF-applikationer med MVVM-mönstret.
Ett delmål är även att arbeta i projektform, arbeta med gemensamnkod-bas och ansvarsfördelning. 
Använda versionshanteringssystem som ett hjälpmedel. 

Genom att integrera filhantering, datavisualisering och algoritmisk problemlösning erbjuder det en omfattande programmeringsupplevelse.