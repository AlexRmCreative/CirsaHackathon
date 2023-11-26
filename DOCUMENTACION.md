# Documentación del Proyecto de Stress Testing con xUnit

## Background del Proyecto

El proyecto de Stress Testing tiene como objetivo evaluar el rendimiento y la estabilidad de la API de estadísticas de juegos.

## Uso de la Aplicación de Stress Testing

1. **Requisitos Previos:**
   - Microsoft Visual Studio 2022 o una versión compatible.
   - Paquetes NuGet instalados: Xunit, FluentAssertions, Moq.

2. **Configuración del Entorno:**
   - Clonar el repositorio del proyecto.
   - Abrir la solución en Visual Studio.
   - Instalar los paquetes NuGet mencionados anteriormente.

3. **Ejecución de las Pruebas:**
   - Ejecutar las pruebas unitarias desde el explorador de pruebas de Visual Studio.

## Instalación del Proyecto

...

## Decisiones Tomadas

1. **Selección del Framework de Pruebas:**
   - Se optó por utilizar xUnit como el framework de pruebas debido a su simplicidad, flexibilidad y legibilidad en el codigo junto a los demás paquetes mencionados.

2. **Estructura del Código:**
   - Se ha estructurado el código de las pruebas para que sea legible, fácil de entender y mantener. Cada prueba está documentada utilizando la etiqueta `<summary>` para proporcionar información sobre su propósito y comportamiento.

## Decisiones de Diseño

### `GameStatisticsSmokeTests` Clase de Pruebas

La clase `GameStatisticsSmokeTests` contiene pruebas de "smoke tests" para la API de estadísticas de juegos. A continuación, se describen las pruebas realizadas:

1. **`SmokeTest_GetAllGameData_ShouldReturnOk`:**
   - Verifica que el endpoint `/gamedata` responda con un código de estado OK.

2. **`SmokeTest_GetGameDataById_ShouldReturnOk`:**
   - Verifica que el endpoint `/gamedata/{id}` responda con un código de estado OK si el juego existe. Además, verifica que el código de estado no sea NotFound para el mismo endpoint.

3. **`SmokeTest_CreateGameData_ShouldReturnCreated`:**
   - Verifica que el endpoint `/gamedata` responda con un código de estado Created después de agregar un nuevo juego.

4. **`SmokeTest_UpdateGameData_ShouldReturnNoContent`:**
   - Verifica que el endpoint `/gamedata/{id}` responda con un código de estado NoContent después de actualizar los datos de un juego existente.

5. **`SmokeTest_DeleteGameData_ShouldReturnNoContent`:**
   - Verifica que el endpoint `/gamedata/{id}` responda con un código de estado NoContent después de eliminar un juego. Además, verifica que el código de estado no sea NotFound para el mismo endpoint.
