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
- NOTA: En caso que algún test salte una excepción de Object.Reference ejecuta el test en modo prueba/depuración, esta excepción suponemos es por los threadings

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




## Average Load Testing:
## Resumen
La clase `Program` es una aplicación de consola en C# que permite al usuario seleccionar y llamar a diferentes puntos finales de una API de datos de juegos. Ofrece opciones para realizar pruebas de carga manuales y automáticas en los puntos finales de la API.

## Uso Ejemplar
```csharp
// Selecciona un punto final para llamar
// Presiona las flechas arriba o abajo para navegar por las opciones
// Presiona Enter para seleccionar una opción
// Selecciona un tipo de prueba (manual o automática)
// Presiona las flechas arriba o abajo para navegar por las opciones
// Presiona Enter para seleccionar un tipo de prueba
// Para pruebas manuales de carga:
// Presiona las flechas arriba o abajo para aumentar o disminuir el número de solicitudes por segundo
// Presiona Enter para enviar las solicitudes
// Para pruebas automáticas de carga:
// Presiona las flechas arriba o abajo para aumentar o disminuir el número de solicitudes por segundo
// Las solicitudes se enviarán automáticamente sin presionar Enter
// Presiona Esc para salir de la aplicación
```
## Análisis de Código
### Funcionalidades Principales
- Permite al usuario seleccionar y llamar a diferentes puntos finales de una API de datos de juegos
- Ofrece opciones para pruebas de carga manuales y automáticas en los puntos finales de la API
___
### Métodos
- `SelectEndpointCall()`: Permite al usuario seleccionar un punto final para llamar
- `SelectTestType()`: Permite al usuario seleccionar un tipo de prueba (manual o automática)
- `RunLoadTest()`: Ejecuta una prueba de carga manual enviando solicitudes al punto final seleccionado
- `RunAutomaticLoadTest()`: Ejecuta una prueba de carga automática enviando continuamente solicitudes al punto final seleccionado
- `CallSelectedEndpoint()`: Llama al punto final seleccionado según la elección del usuario
- `UpdateConsole()`: Actualiza la consola con el número actual de solicitudes y el tiempo de respuesta
- `RunLoadTestIteration()`: Ejecuta una única iteración de la prueba de carga enviando múltiples solicitudes a la API
- `MakeRequest()`: Envía una única solicitud a la API
- `CallGetApiEndpoint()`: Llama al punto final GET de la API
- `CallGetByIdApiEndpoint()`: Llama al punto final GET por ID de la API
- `CallPostApiEndpoint()`: Llama al punto final POST de la API
- `CallPutApiEndpoint()`: Llama al punto final PUT de la API
- `CallDeleteApiEndpoint()`: Llama al punto final DELETE de la API
- `HandleError()`: Maneja cualquier error que ocurra durante las llamadas a la API
- `EvaluateResponseTime()`: Evalúa el tiempo de respuesta y determina su calidad
___

### Campos
- `nRequest`: El número de solicitudes que se enviarán por segundo
- `iRequest`: El valor de incremento o decremento para el número de solicitudes
- `elapsedSeconds`: El tiempo transcurrido en segundos para cada solicitud
- `startTimes`: Una lista de tiempos de inicio para cada solicitud
- `apiUrl`: La URL de la API de datos de juegos
- `id`: El ID a utilizar en las llamadas a la API
- `selectedEndpointCall`: El índice del punto final seleccionado
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
- NOTA: En caso que algún test salte una excepción de Object.Reference ejecuta el test en modo prueba/depuración, esta excepción suponemos es por los threadings
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
