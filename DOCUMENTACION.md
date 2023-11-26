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
   - Instalar los paquetes mencionados anteriormente.

3. **Ejecución de las Pruebas:**
## Smoke testing:
### Resumen:
La clase `GameStatisticsSmokeTest` es un conjunto de pruebas (fixture) que contiene un conjunto de métodos de prueba para evaluar la funcionalidad de una API de estadísticas de juegos. Utiliza el marco de pruebas xUnit y la biblioteca FluentAssertions para las afirmaciones. La clase incluye métodos para verificar la disponibilidad de la API, recuperar datos del juego, publicar nuevos datos del juego, actualizar datos del juego y eliminar datos del juego.

## Ejemplo de Uso

### (Ejecutar las pruebas unitarias desde el explorador de pruebas de Visual Studio)
```csharp
// Configurar la prueba
var smokeTest = new GameStatisticsSmokeTest();
smokeTest.Setup();

// Verificar la disponibilidad de la API
await smokeTest.CheckApiAvailability();

// Obtener datos del juego y afirmar que devuelve OK
await smokeTest.GetGameData_ShouldReturnOkUnderMinimalLoadAsync();

// Obtener datos del juego por ID y afirmar que devuelve OK
await smokeTest.GetValidGameDataByID_ShouldReturnOkUnderMinimalLoadAsync();

// Publicar nuevos datos del juego y afirmar que devuelve Creado
await smokeTest.PostValidGameData_ShouldReturnCreatedUnderMinimalLoadAsync();

// Actualizar datos del juego y afirmar que devuelve NoContent
await smokeTest.PutValidGameData_ShouldReturnNoContentUnderMinimalLoadAsync();

// Eliminar datos del juego por ID y afirmar que devuelve NoEncontrado (NotFound)
await smokeTest.DeleteUnvalidGameDataByID_ShouldReturnNotFoundUnderMinimalLoadAsync();

// Desmontar la prueba
smokeTest.TearDown();
```

## Análisis de Código

### Funcionalidades

- Verificar la disponibilidad de la API
- Recuperar datos del juego y afirmar que devuelve OK
- Recuperar datos del juego por ID y afirmar que devuelve OK
- Publicar nuevos datos del juego y afirmar que devuelve Creado
- Actualizar datos del juego y afirmar que devuelve NoContent
- Eliminar datos del juego por ID y afirmar que devuelve NoEncontrado (NotFound)

### Métodos

- `Setup()`: Inicializa el cliente HTTP con la URL base de la API y el número de veces que se llamará a la API durante las pruebas.
- `CheckApiAvailability()`: Envía una solicitud GET al endpoint `/gamedata` de la API y verifica que el código de estado de la respuesta sea OK.
- `TearDown()`: Libera los recursos del cliente HTTP.
- `GetGameData_ShouldReturnOkUnderMinimalLoadAsync()`: Envía múltiples solicitudes GET al endpoint `/gamedata` en paralelo y verifica que el código de estado de cada respuesta sea OK y el contenido de la respuesta sea JSON válido.
- `GetValidGameDataByID_ShouldReturnOkUnderMinimalLoadAsync()`: Envía múltiples solicitudes GET al endpoint `/gamedata/{ID}` con IDs válidos en paralelo y verifica que el código de estado de cada respuesta sea OK y el contenido de la respuesta sea JSON válido.
- `PostValidGameData_ShouldReturnCreatedUnderMinimalLoadAsync()`: Envía múltiples solicitudes POST al endpoint `/gamedata` con datos de juego válidos en paralelo y verifica que el código de estado de cada respuesta sea Creado y el contenido de la respuesta sea JSON válido.
- `PutValidGameData_ShouldReturnNoContentUnderMinimalLoadAsync()`: Envía múltiples solicitudes PUT al endpoint `/gamedata/{ID}` con datos de juego válidos en paralelo y verifica que el código de estado de cada respuesta sea NoContent.
- `DeleteUnvalidGameDataByID_ShouldReturnNotFoundUnderMinimalLoadAsync()`: Envía múltiples solicitudes DELETE al endpoint `/gamedata/{ID}` con IDs no válidos en paralelo y verifica que el código de estado de cada respuesta sea NotFound.

### Campos

- `_client`: Una instancia de la clase `HttpClient` utilizada para enviar solicitudes HTTP a la API.
- `_times`: El número de veces que se llamará a la API durante las pruebas.

---

## Instalación del Proyecto

...

## Decisiones Tomadas

...
