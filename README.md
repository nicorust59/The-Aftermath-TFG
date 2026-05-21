#  The Aftermath

**Trabajo de Fin de Grado - Desarrollo de Aplicaciones Multiplataforma (DAM)**

**The Aftermath** es un videojuego de *survival horror* centrado en el terror psicológico y la indefensión del jugador. Ambientado en el opresivo entorno del Puerto de Arkham (1860), el proyecto prescinde del combate tradicional para enfocarse en la tensión atmosférica, la exploración y la resolución de puzles bajo presión constante.

## ⚙️ Características Técnicas Principales

* **IA de Percepción Inversa ("El Vacío"):** Sistema de inteligencia artificial enemiga que utiliza el Frustum de la cámara del jugador. La entidad persigue al usuario mediante *NavMeshAgent* únicamente cuando este no la está observando directamente, generando una mecánica constante de tensión psicológica.
* **Interacción Dinámica (Raycasting):** Implementación de un sistema de detección mediante proyección de rayos (Raycast) desde el centro de la cámara. Este enfoque optimiza el rendimiento del motor de físicas al eliminar la necesidad de colisionadores complejos (Triggers) para leer notas o recoger objetos clave.
* **Arquitectura Centralizada:** Uso del patrón `GameManager` para la gestión global de estados de partida, control de inventario (llaves), flujos de interfaz de usuario y transiciones cinemáticas asíncronas (Corrutinas).
* **Diseño Atmosférico:** Aprovechamiento del Universal Render Pipeline (URP) de Unity para la generación de niebla volumétrica, iluminación puntual (Point Lights) y configuración de audio espacial 3D inmersivo.

## 🛠️ Stack Tecnológico
* **Motor Gráfico:** Unity Engine
* **Lenguaje de Programación:** C# (Programación Orientada a Objetos)
* **Entorno de Desarrollo:** Visual Studio Code
* **Control de Versiones:** Git / GitHub

---
*Desarrollado por Nicolás Lage Mora (Curso 2025-2026).*
