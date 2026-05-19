# 🐱 Kitty Platformer 2D - Sistema de Control i Mecàniques

Aquest repositori conté el nucli de scripts en C# desenvolupats per a un videojoc de plataformes en 2D a Unity. El projecte inclou un sistema complet de control de personatge amb físiques, mecàniques de combat per salt/atac frontal, interaccions amb elements destruïbles, recollida d'ítems i una interfície d'usuari dinàmica i persistent.

## 📁 Estructura de Scripts

El projecte es divideix en 5 components principals ben estructurats:

1. **`KittyController.cs`**: El controlador principal del jugador. Gestiona les entrades del teclat/ratolí, la velocitat del Rigidbody2D, els rebots amb enemics, les col·lisions amb trampes i la reproducció segura d'efectes de so a través de *Null Checks*.
2. **`EnemyController.cs`**: Intel·ligència artificial bàsica per a patrulles enemigues (rates). Canvien de direcció en xocar amb un trigger limitador i gestionen la seva pròpia salut i animació de mort diferida.
3. **`CanvaManager.cs`**: Administrador de la interfície gràfica d'usuari en temps real (HUD). Actualitza de forma segura els textos de monedes, pocions i contenidors visuals de vida (cors) mitjançant TextMeshPro.
4. **`UIManager.cs`**: Gestor de flux de escenes dedicat al control de menús, botons d'inici i reintents de pantalla.
5. **`Persistance.cs`**: Implementació bàsica del patró *Singleton* encarregat de mantenir la música o sistemes globals vius entre els canvis d'escena sense duplicar objectes al motor.

---

## 🛠️ Detalls d'Implementació Tècnica

### 1. Mecàniques del Jugador (`KittyController`)
* **Moviment adaptatiu:** El personatge llegeix els eixos horitzontals natius i adapta automàticament la mida del seu `CapsuleCollider2D` segons les dimensions exactes del sprite de l'animació en curs.
* **Combat multidireccional:** El jugador pot danyar els enemics mitjançant un atac frontal activat per corrutina (clic esquerre) o caient sobre ells, fet que calcula la normal del contacte i genera un rebot físic vertical.
* **Condició de Victòria Blindada:** En recollir les 2 pocions necessàries i tocar el peix final, el col·lisionador de l'objectiu s'apaga immediatament en el primer fotograma per evitar bucles infinits de recàrrega abans de saltar a l'escena de victòria.

### 2. Prevenció d'Errors i Seguretat (Anti-Crashes)
* **Escuts d'Àudio (Null Checks):** Totes les execucions de `audioSource.PlayOneShot()` estan protegides davant de variables buides a l'Inspector. Si falta un arxiu d'àudio, el joc continua el seu flux lògic de vides o monedes en lloc de congelar el fil d'execució per una excepció de referència nul·la (`NullReferenceException`).
* **Control de Rangs:** Les funcions que modifiquen els cors de vida i la UI verifiquen que els índexs i límits dels arrays siguin vàlids abans d'intentar substituir els sprites a la pantalla.

---

## 📝 Bones Pràctiques Aplicades
* **Codi DRY:** Reutilització de lògiques físiques i funcions centralitzades per al reinici de posició i el càlcul de dany.
* **Noms auto-explicatius:** Variables i mètodes tipats adequadament en anglès tècnic per estandarditzar el desenvolupament.
* **Comentaris en espanyol:** Documentació exhaustiva pas a pas integrada en el propi codi per facilitar la comprensió personal i el manteniment del projecte.
