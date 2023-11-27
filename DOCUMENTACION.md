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




## Average Load testing:
## Summary
The `Program` class is a C# console application that allows the user to select and call different endpoints of a game data API. It provides options for manual and automatic load testing of the API endpoints.

## Example Usage
```csharp
// Select an endpoint to call
// Press the up or down arrow to navigate through the options
// Press Enter to select an option
// Select a test type (manual or automatic)
// Press the up or down arrow to navigate through the options
// Press Enter to select a test type
// For manual load testing:
// Press the up or down arrow to increase or decrease the number of requests per second
// Press Enter to send the requests
// For automatic load testing:
// Press the up or down arrow to increase or decrease the number of requests per second
// Requests will be automatically sent without pressing Enter
// Press Esc to exit the application
```

## Code Analysis
### Main functionalities
- Allows the user to select and call different endpoints of a game data API
- Provides options for manual and automatic load testing of the API endpoints
___
### Methods
- `SelectEndpointCall()`: Allows the user to select an endpoint to call
- `SelectTestType()`: Allows the user to select a test type (manual or automatic)
- `RunLoadTest()`: Runs a manual load test by sending requests to the selected endpoint
- `RunAutomaticLoadTest()`: Runs an automatic load test by continuously sending requests to the selected endpoint
- `CallSelectedEndpoint()`: Calls the selected endpoint based on the user's choice
- `UpdateConsole()`: Updates the console with the current number of requests and response time
- `RunLoadTestIteration()`: Runs a single iteration of the load test by sending multiple requests to the API
- `MakeRequest()`: Sends a single request to the API
- `CallGetApiEndpoint()`: Calls the GET endpoint of the API
- `CallGetByIdApiEndpoint()`: Calls the GET by ID endpoint of the API
- `CallPostApiEndpoint()`: Calls the POST endpoint of the API
- `CallPutApiEndpoint()`: Calls the PUT endpoint of the API
- `CallDeleteApiEndpoint()`: Calls the DELETE endpoint of the API
- `HandleError()`: Handles any errors that occur during the API calls
- `EvaluateResponseTime()`: Evaluates the response time and determines its quality
___
### Fields
- `nRequest`: The number of requests to be sent per second
- `iRequest`: The increment or decrement value for the number of requests
- `elapsedSeconds`: The elapsed time in seconds for each request
- `startTimes`: A list of start times for each request
- `apiUrl`: The URL of the game data API
- `id`: The ID to be used in the API calls
- `selectedEndpointCall`: The index of the selected endpoint
___

## SpikeTest:

### Resumen:

La clase `SpikeTest` es un conjunto de pruebas diseñado para evaluar el comportamiento de un punto final de API relacionado con datos de juegos. Contiene varios métodos de prueba que envían solicitudes HTTP al punto final de la API y verifican las respuestas esperadas.

### Ejemplo de Uso

```csharp
// Crea una instancia de la clase `SpikeTest`
var spikeTest = new SpikeTest();

// Configura el cliente HTTP con la URL base de la API
spikeTest.Setup();

// Verifica la disponibilidad de la API enviando una solicitud GET al punto final /gamedata
spikeTest.CheckApiAvailability();

// Envía múltiples solicitudes GET en paralelo al punto final /gamedata y verifica las respuestas
spikeTest.GetValidGameData_ShouldReturnOkUnderPikeLoadAsync();

// Envía múltiples solicitudes GET en paralelo al punto final /gamedata/{ID} y verifica las respuestas
spikeTest.GetValidGameDataByID_ShouldReturnOkUnderPikeLoadAsync();

// Envía múltiples solicitudes PUT en paralelo al punto final /gamedata/{ID} y verifica las respuestas
spikeTest.PutValidGameData_ShouldReturnNoContentUnderPikeLoadAsync();

// Envía múltiples solicitudes DELETE en paralelo al punto final /gamedata/{ID} y verifica las respuestas
spikeTest.DeleteUnvalidGameDataByID_ShouldReturnNotFoundUnderPikeLoadAsync();

// Envía múltiples solicitudes POST en paralelo al punto final /gamedata y verifica las respuestas
spikeTest.PostValidGameData_ShouldReturnCreatedUnderPikeLoadAsync();

// Limpia los recursos utilizados por el cliente HTTP
spikeTest.TearDown();
```
## Análisis de Código

### Funcionalidades Principales
- Configurar el cliente HTTP con la URL base de la API.
- Verificar la disponibilidad de la API enviando una solicitud GET al punto final /gamedata.
- Enviar múltiples solicitudes HTTP en paralelo a los puntos finales de la API y verificar las respuestas esperadas.
- Limpiar los recursos utilizados por el cliente HTTP.
___
### Métodos
- `Setup()`: Configura el cliente HTTP con la URL base de la API.
- `CheckApiAvailability()`: Verifica la disponibilidad de la API enviando una solicitud GET al punto final /gamedata y verifica que el código de estado de la respuesta sea OK.
- `TearDown()`: Limpia los recursos utilizados por el cliente HTTP.
- `GetValidGameData_ShouldReturnOkUnderPikeLoadAsync()`: Envía múltiples solicitudes GET en paralelo al punto final /gamedata y verifica que el código de estado de la respuesta sea OK para cada solicitud.
- `GetValidGameDataByID_ShouldReturnOkUnderPikeLoadAsync()`: Envía múltiples solicitudes GET en paralelo al punto final /gamedata/{ID} y verifica que el código de estado de la respuesta sea OK para cada solicitud.
- `PutValidGameData_ShouldReturnNoContentUnderPikeLoadAsync()`: Envía múltiples solicitudes PUT en paralelo al punto final /gamedata/{ID} y verifica que el código de estado de la respuesta sea NoContent para cada solicitud.
- `DeleteUnvalidGameDataByID_ShouldReturnNotFoundUnderPikeLoadAsync()`: Envía múltiples solicitudes DELETE en paralelo al punto final /gamedata/{ID} y verifica que el código de estado de la respuesta sea NotFound para cada solicitud.
- `PostValidGameData_ShouldReturnCreatedUnderPikeLoadAsync()`: Envía múltiples solicitudes POST en paralelo al punto final /gamedata y verifica que el código de estado de la respuesta sea Created para cada solicitud.
___
### Campos
- `_client`: Una instancia de la clase `HttpClient` utilizada para enviar solicitudes HTTP a la API.

## Instalación del Proyecto

Para instalar y ejecutar este proyecto, debes seguir los siguientes pasos:

1. Clona o descarga el repositorio de GitHub en tu computadora. Puedes usar el botón verde "Code" que aparece en la parte superior derecha de la página del repositorio, o puedes usar el siguiente comando en una terminal:

```bash
git clone https://github.com/AlexRmCreative/CirsaHackathon.git
```

2. Instalar los siguientes paquetes en cada solución:

```Average Load
Install-Package System.Net.Http
```
```
Install-Package Newtonsoft.Json
```
```Smoke Test
coverlet.collector
FluentAssertions.Json
FluentAssertions
NUnit
```
```Spike Test
FluentAssertions.Json
NUnit
```

3. Seguir el paso a paso de uso de cada solución que se encuentra en la parte superior
