
---
# Konvertera GPS-koordinater till Win XY-koordinater

Att konvertera en rektangel definierad i GPS-koordinater till ett koordinatsystem anpassat för ett Windows-fönster innebär att man skalar och översätter GPS-koordinaterna till pixelvärden i fönstret. 

---

## 1. **Definiera indata:**
   - **GPS-rektangeln**: Beskrivs av två koordinater:
     - $$(lat_{min}, lon_{min})$$: Nedre vänstra hörnet.
     - $$(lat_{max}, lon_{max})$$: Övre högra hörnet.
           
       Vi kommer att konvertera GPS-koordinaterna till pixelkoordinater i fönstret för rektangeln som definerar kartan över sverige.

       ```json
         {
        "Rectangle": {
          "NorthWest": {
            "Latitude": 69.0600,
            "Longitude": 10.9300
          },
          "NorthEast": {
            "Latitude": 69.0600,
            "Longitude": 24.1600
          },
          "SouthWest": {
            "Latitude": 55.2000,
            "Longitude": 10.9300
          },
          "SouthEast": {
            "Latitude": 55.2000,
            "Longitude": 24.1600
          }
        },

       ```

   - **Fönsterstorlek**: Bredd och höjd i pixlar, t.ex. $$(width, height)$$.

        För att beräkna det maximala värden på bredd och höjd i pixlar, används de maximala värdena på GPS-koordinaterna.
        

    

---

## 2. **Skapa en linjär transformation för skalning:**

Du behöver en linjär funktion som mappar GPS-koordinaterna till pixelkoordinater:

- Latitude (lat) → y-koordinater (pixlar):
  - Latitude minskar när man rör sig neråt i kartan. I ett Windows-fönster minskar y från toppen.
  - Formeln blir:

      $$y = height \cdot \frac{lat_{max} - lat}{lat_{max} - lat_{min}}$$

    

- Longitude (lon) → x-koordinater (pixlar):
  - Longitude ökar från vänster till höger i både GPS och pixelvärden.
  - Formeln blir:
    
    $$x = width \cdot \frac{lon - lon_{min}}{lon_{max} - lon_{min}}$$

---

## 3. **Konvertera varje hörn av rektangeln:**

Använd ovanstående formler på rektangelns hörn:
- Nedre vänstra hörnet:
  
  $$x_{min} = 0, \quad y_{min} = height$$

- Övre högra hörnet:
  
  $$x_{max} = width, \quad y_{max} = 0$$

---

## 4. **Exempel:**

### Indata:

- GPS-rektangel:
  $$(lat_{min}, lon_{min}) = (55.2, 10.9), \quad (lat_{max}, lon_{max}) = (69.0, 24.1)$$

- Fönsterstorlek: 
  $$( width = 800 ), ( height = 600 )$$.

### Konvertering:

För en godtycklig GPS-koordinat $$( (lat, lon) = (59.5, 18.5) )$$:
- Beräkna $$( x )$$:
  
  $$x = 800 \cdot \frac{18.5 - 18.0}{19.0 - 18.0} = 800 \cdot 0.5 = 400$$

- Beräkna $$( y )$$:
  
  $$y = 600 \cdot \frac{60.0 - 59.5}{60.0 - 59.0} = 600 \cdot 0.5 = 300$$

Pixelkoordinaten blir $$( (x, y) = (400, 300) )$$.

```csharp
        public (int x, int y) TransformToScreenPosition(double latitude, double longitude)
        {
            var x = (int)((longitude - _minLongitude) / (_maxLongitude - _minLongitude) * _windowWidth);
            var y = (int)((_maxLatitude - latitude) / (_maxLatitude - _minLatitude) * _windowHeight);
            return (x, y);
        }
```

Note: Ingen skalfaktor nödvändigt i Sveriges fall ~(520, 1540). Värden på Höjd och Bredd är acceptabla m.a.p. fönstrets storlek på skärmen.
Kan läggas som ett extra krav (Addera skalfaktor för fönstret).

---

### 5. **Storleken på Windows fönstret:**
- GPS-koordinater är i decimalgrader, medan pixelkoordinater är heltal.
- För att kunna ha samma proportionella rektanglar (GPS - XY)
  - räkna avståndet mellan 

  $$Width_{max} = distance((lat_{min},lon_{min}), (lat_{max}, lon_{min}))$$

  $$Heigh_{max} = distance((lat_{min},lon_{min}), (lat_{min}, lon_{max}))$$

  - Vid behov skala och anpassa maximala Width och Heigh till aktuell storlek på fönstret.

---

            //          NorthWest( Lat, Lon )  ---------------------------- NorhtEast( Lat, Lon )
            //             (69.0, 11.0)                                        (69.0, 24.0 )
            //                  ^                                                   |
            //                  ^                                                   |
            //                  |                                                   |
            //                  |                                                   |
            //                  |                                                   |
            //                  |                                                   |
            //             (55.0, 11.0)                                        (55.0, 24.0)
            //          SouthWest( Lat, Lon )  --------------------------->> SouthEast( Lat, Lon )


  | $$lat_{min} = 55.0$$ | $$lat_{max} = 69.0$$ |
  |----------------------|----------------------|
  | $$lon_{min} = 11.0$$ | $$lon_{max} = 24.0$$ |

---

            //          TopLeft( X, Y )  ------------------------------------ TopRight( X, Y )
            //             (Xmin, Ymin)                                        (Xmax, Ymin)
            //                  ^                                                   |
            //                  ^                                                   |
            //                  |                                                   |
            //                  |                                                   |
            //                  |                                                   |
            //                  |                                                   |
            //             (Xmin, Ymax)                                        (Xmax, Ymax)
            //          BottonLeft( X, Y )  --------------------------->> BottonRight( X, Y )


| $$X_{min} = 0$$ | $$Y_{min} = 0$$ |
|-----------------|-----------------|
|$$X_{max} = dist((lat_{min},lon_{min}), (lat_{min},lon_{max})) * normFactor$$ | $$Y_{max} = dist((lat_{min},lon_{min}), (lat_{max},lon_{min})) * normFactor$$ |

