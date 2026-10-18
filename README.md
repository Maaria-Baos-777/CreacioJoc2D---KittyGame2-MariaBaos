# 🐱 Kitty Platformer 2D - Sistema de Control y Mecánicas

Este repositorio contiene el núcleo de scripts en C# desarrollados para un videojuego de plataformas en 2D en Unity. El proyecto incluye un sistema completo de control de personaje con físicas, mecánicas de combate por salto/ataque frontal, interacciones con elementos destruibles, recogida de ítems y una interfaz de usuario dinámica y persistente.

## 📁 Estructura de Scripts

El proyecto se divide en 5 componentes principales bien estructurados:

1. **`KittyController.cs`**: El controlador principal del jugador. Gestiona las entradas del teclado/ratón, la velocidad del Rigidbody2D, los rebotes con enemigos, las colisiones con trampas y la reproducción segura de efectos de sonido a través de *Null Checks*.
2. **`EnemyController.cs`**: Inteligencia artificial básica para patrullas enemigas (ratas). Cambian de dirección al chocar con un trigger limitador y gestionan su propia salud y animación de muerte diferida.
3. **`CanvaManager.cs`**: Administrador de la interfaz gráfica de usuario en tiempo real (HUD). Actualiza de forma segura los textos de monedas, pociones y contenedores visuales de vida (corazones) mediante TextMeshPro.
4. **`UIManager.cs`**: Gestor de flujo de escenas dedicado al control de menús, botones de inicio y reintentos de pantalla.
5. **`Persistance.cs`**: Implementación básica del patrón *Singleton* encargado de mantener la música o sistemas globales vivos entre los cambios de escena sin duplicar objetos en el motor.

---

## 🛠️ Detalles de Implementación Técnica

### 1. Mecánicas del Jugador (`KittyController`)
- **Movimiento adaptativo:** El personaje lee los ejes horizontales nativos y adapta automáticamente el tamaño de su `CapsuleCollider2D` según las dimensiones exactas del sprite de la animación en curso.
- **Combate multidireccional:** El jugador puede dañar a los enemigos mediante un ataque frontal activado por corrutina (clic izquierdo) o cayendo sobre ellos, lo que calcula la normal del contacto y genera un rebote físico vertical.
- **Condición de Victoria Blindada:** Al recoger las 2 pociones necesarias y tocar el pez final, el colisionador del objetivo se apaga inmediatamente en el primer fotograma para evitar bucles infinitos de recarga antes de saltar a la escena de victoria.

### 2. Prevención de Errores y Seguridad (Anti-Crashes)
- **Escudos de Audio (Null Checks):** Todas las ejecuciones de `audioSource.PlayOneShot()` están protegidas ante variables vacías en el Inspector. Si falta un archivo de audio, el juego continúa su flujo lógico de vidas o monedas en lugar de congelar el hilo de ejecución por una excepción de referencia nula (`NullReferenceException`).
- **Control de Rangos:** Las funciones que modifican los corazones de vida y la UI verifican que los índices y límites de los arrays sean válidos antes de intentar sustituir los sprites en la pantalla.

---

## 📝 Buenas Prácticas Aplicadas
- **Código DRY:** Reutilización de lógicas físicas y funciones centralizadas para el reinicio de posición y el cálculo de daño.
- **Nombres autoexplicativos:** Variables y métodos tipados adecuadamente en inglés técnico para estandarizar el desarrollo.
- **Comentarios en español:** Documentación exhaustiva paso a paso integrada en el propio código para facilitar la comprensión personal y el mantenimiento del proyecto.
