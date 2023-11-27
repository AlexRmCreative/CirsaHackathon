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
### Resumen:
The `Program` class represents a C# console application that performs load testing on a specified API. It provides a menu with options for manual or automatic load testing, allowing the user to increase or decrease the number of requests per second. The class also evaluates the response time of the API and displays the quality of the response.

## Example Usage
```csharp
// Create an instance of the Program class
Program program = new Program();

// Start the menu
await program.StartMenu();
```

## Code Analysis
### Main functionalities
- Provides a menu for selecting manual or automatic load testing
- Allows the user to increase or decrease the number of requests per second
- Performs load testing by sending multiple HTTP requests to the specified API
- Evaluates the response time of the API and displays the quality of the response
___
### Methods
- `StartMenu()`: Displays the menu options and handles user input to start manual or automatic load testing.
- `RunLoadTest()`: Performs manual load testing by allowing the user to increase or decrease the number of requests per second.
- `RunAutomaticLoadTest()`: Performs automatic load testing by continuously increasing or decreasing the number of requests per second.
- `UpdateConsole()`: Updates the console with the current number of requests and the quality of the response time.
- `RunLoadTestIteration()`: Performs a single iteration of load testing by sending multiple HTTP requests to the API.
- `MakeRequest()`: Sends an HTTP request to the API and calculates the response time.
- `HandleError()`: Handles any errors that occur during load testing and displays an error message.
- `EvaluateResponseTime()`: Evaluates the response time of the API and displays the quality of the response.
___
### Fields
- `nRequest`: The number of requests per second.
- `iRequest`: The increment or decrement value for the number of requests per second.
- `elapsedSeconds`: The elapsed time in seconds for the last load testing iteration.
- `startTimes`: A list of start times for each request in the current load testing iteration.
- `ErrorDelayMilliseconds`: The delay in milliseconds before the application exits after an error occurs.
- `GoodThreshold`: The threshold value for a good response time.
- `BadThreshold`: The threshold value for a bad response time.
- `apiUrl`: The URL of the API to be tested.
___


## Spike testing:
### Resumen:
The `SpikeTest` class is a test fixture for testing the behavior of an API endpoint related to game data. It contains several test methods that send HTTP requests to the API endpoint and assert the expected responses.

## Example Usage
```csharp
// Create an instance of the `SpikeTest` class
var spikeTest = new SpikeTest();

// Set up the HTTP client with the base URL of the API
spikeTest.Setup();

// Check the availability of the API by sending a GET request to the /gamedata endpoint
spikeTest.CheckApiAvailability();

// Send multiple parallel GET requests to the /gamedata endpoint and assert the responses
spikeTest.GetValidGameData_ShouldReturnOkUnderPikeLoadAsync();

// Send multiple parallel GET requests to the /gamedata/{ID} endpoint and assert the responses
spikeTest.GetValidGameDataByID_ShouldReturnOkUnderPikeLoadAsync();

// Send multiple parallel PUT requests to the /gamedata/{ID} endpoint and assert the responses
spikeTest.PutValidGameData_ShouldReturnNoContentUnderPikeLoadAsync();

// Send multiple parallel DELETE requests to the /gamedata/{ID} endpoint and assert the responses
spikeTest.DeleteUnvalidGameDataByID_ShouldReturnNotFoundUnderPikeLoadAsync();

// Send multiple parallel POST requests to the /gamedata endpoint and assert the responses
spikeTest.PostValidGameData_ShouldReturnCreatedUnderPikeLoadAsync();

// Clean up the resources used by the HTTP client
spikeTest.TearDown();
```

## Code Analysis
### Main functionalities
- Set up the HTTP client with the base URL of the API
- Check the availability of the API by sending a GET request to the /gamedata endpoint
- Send multiple parallel HTTP requests to the API endpoints and assert the expected responses
- Clean up the resources used by the HTTP client
___
### Methods
- `Setup()`: Sets up the HTTP client with the base URL of the API.
- `CheckApiAvailability()`: Checks the availability of the API by sending a GET request to the /gamedata endpoint and asserts that the response status code is OK.
- `TearDown()`: Cleans up the resources used by the HTTP client.
- `GetValidGameData_ShouldReturnOkUnderPikeLoadAsync()`: Sends multiple parallel GET requests to the /gamedata endpoint and asserts that the response status code is OK for each request.
- `GetValidGameDataByID_ShouldReturnOkUnderPikeLoadAsync()`: Sends multiple parallel GET requests to the /gamedata/{ID} endpoint and asserts that the response status code is OK for each request.
- `PutValidGameData_ShouldReturnNoContentUnderPikeLoadAsync()`: Sends multiple parallel PUT requests to the /gamedata/{ID} endpoint and asserts that the response status code is NoContent for each request.
- `DeleteUnvalidGameDataByID_ShouldReturnNotFoundUnderPikeLoadAsync()`: Sends multiple parallel DELETE requests to the /gamedata/{ID} endpoint and asserts that the response status code is NotFound for each request.
- `PostValidGameData_ShouldReturnCreatedUnderPikeLoadAsync()`: Sends multiple parallel POST requests to the /gamedata endpoint and asserts that the response status code is Created for each request.
___
### Fields
- `_client`: An instance of the `HttpClient` class used to send HTTP requests to the API.
___









## Instalación del Proyecto

...

## Decisiones Tomadas

...
