using GameStatistics.Test.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System.Collections.Concurrent;
using System.Net;
using System.Text;

namespace SpykeTests
{
    [TestFixture]
    public class SpikeTest
    {
        private HttpClient _client;


        [SetUp]
        public void Setup()
        {
            // Inicializar el cliente HTTP con la URL base de la API y las veces que se llamará
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://localhost:7170"); //Change this to your api URL
        }


        [OneTimeSetUp]
        public async Task CheckApiAvailability()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7170"); //Change this to your api URL
            // Enviar una petición GET al endpoint /gamedata de la API
            var response = await client.GetAsync("/gamedata");
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [TearDown]
        public void TearDown()
        {
            // Liberar los recursos del cliente HTTP
            _client.Dispose();
        }

        [NonParallelizable]
        [Order(1)]
        [Test]
        public async Task GetValidGameData_ShouldReturnOkUnderPikeLoadAsync()
        {
            // ---- ARRANGE ---
            int timesToCall = 750;
            var tasks = new ConcurrentBag<Task<HttpResponseMessage>>();
            string endpointToCall = "/gamedata";

            // ---- ACT ----
            // Enviar tantas peticiones como _times diga al endpoint /gamedata en paralelo
            await Parallel.ForEachAsync(Enumerable.Range(0, timesToCall), async (i, ct) =>
            {
                var response = _client.GetAsync(endpointToCall, ct);
                tasks.Add(response);
            });

            // Esperar a que todas las peticiones se completen
            Task.WaitAll(tasks.ToArray());
            // Obtener las respuestas de las tareas
            var responses = tasks.Select(t => t.Result).ToList();

            // ---- ASSERT ----
            int totalAnswers = 0;
            Assert.Multiple(async () =>
            {
                // Verificar que todas las respuestas tengan el código de estado 200 (Ok)
                foreach (var response in responses)
                {
                    totalAnswers++;
                    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), $"ERR: Respondió correctamente {totalAnswers} de {timesToCall} Requests");
                    Assert.That(response.Content.Headers.ContentType.MediaType, Is.EqualTo("application/json"));
                }
            });

        }


        [NonParallelizable]
        [Order(2)]
        [Test]
        public async Task GetValidGameDataByID_ShouldReturnOkUnderPikeLoadAsync()
        {
            // ---- ARRANGE ---
            int timesToCall = 600;
            var tasks = new ConcurrentBag<Task<HttpResponseMessage>>();
            var arrayOfIDs = new String[]
            {
                "febe6431-b71e-4571-b7e4-8fdab85fc98e"
            };
            Random random = new Random();
            string endpointToCall = $"/gamedata/ID";
            var rIndex = 0;

            // ---- ACT ----
            // Enviar tantas peticiones como _times diga al endpoint /gamedata en paralelo
            await Parallel.ForEachAsync(Enumerable.Range(0, timesToCall), async (i, ct) =>
            {
                //Enviamos un ID válido aleatorio
                //rIndex = random.Next(0, arrayOfIDs.Length);
                //endpointToCall = $"/gamedata/{arrayOfIDs[rIndex]}";
                endpointToCall = $"/gamedata/{arrayOfIDs[0]}";
                var response = _client.GetAsync(endpointToCall, ct);
                tasks.Add(response);
            });

            // Esperamos a que todas las peticiones se completen
            Task.WaitAll(tasks.ToArray());
            // Obtenemos las respuestas de las tareas
            var responses = tasks.Select(t => t.Result).ToList();

            // ---- ASSERT ----
            int totalAnswers = 0;
            Assert.Multiple(async () =>
            {
                // Verificar que todas las respuestas tengan el código de estado 200 (Ok)
                foreach (var response in responses)
                {
                    totalAnswers++;
                    Assert.That(response.StatusCode, Is.EqualTo(expected: HttpStatusCode.OK), $"ERR: Respondió correctamente {totalAnswers} de {timesToCall} Requests");
                    Assert.That(response.Content.Headers.ContentType.MediaType, Is.EqualTo(expected: "application/json"));
                }
            });

        }

        [NonParallelizable]
        [Order(3)]
        [Test]
        public async Task PutValidGameData_ShouldReturnNoContentUnderPikeLoadAsync()
        {
            // ---- ARRANGE ---
            int timesToCall = 350;
            var tasks = new ConcurrentBag<Task<HttpResponseMessage>>();
            var gameDataList = new List<GameData>()
            {
                new GameData
            {
                Id = "febe6431-b71e-4571-b7e4-8fdab85fc98e",
                GameName = "Volleyball Game",
                Category = "Recreational machines",
                TotalBets = 0,
            }
            };
            var rIndex = 0;
            var rTotalBets = 0;
            Random random = new Random();
            string gameDataJson;
            string responseContent;
            GameData gameData;
            StringContent content;
            string endpointToCall = $"/gamedata/ID";

            // ---- ACT ----
            // Enviar tantas peticiones como _times diga al endpoint /gamedata en paralelo
            await Parallel.ForEachAsync(Enumerable.Range(0, timesToCall), async (i, ct) =>
            {
                //Enviamos un GameData Object aleatorio
                rIndex = random.Next(0, gameDataList.Count);
                rTotalBets = random.Next(10, 10000);
                gameData = gameDataList[rIndex];
                gameData.TotalBets = rTotalBets;
                gameDataJson = JsonConvert.SerializeObject(gameDataList[rIndex], Formatting.Indented);
                content = new StringContent(gameDataJson, Encoding.UTF8, "application/json");
                endpointToCall = $"/gamedata/{gameData.Id}";
                var response = _client.PutAsync(endpointToCall, content, ct);
                tasks.Add(response);
            });

            // Esperamos a que todas las peticiones se completen
            Task.WaitAll(tasks.ToArray());
            // Obtenemos las respuestas de las tareas
            var responses = tasks.Select(t => t.Result).ToList();

            // ---- ASSERT ----
            // Verificar que todas las respuestas tengan el código de estado 204 (NoContents)
            int totalAnswers = 0;
            foreach (var response in responses)
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent), $"ERR: Respondió correctamente {totalAnswers} de {timesToCall} Requests");
            }

        }


        [NonParallelizable]
        [Order(4)]
        [Test]
        public async Task DeleteUnvalidGameDataByID_ShouldReturnNotFoundUnderPikeLoadAsync()
        {
            // ---- ARRANGE ---
            int timesToCall = 4000;
            // Creamos una lista para almacenar las tareas
            var tasks = new ConcurrentBag<Task<HttpResponseMessage>>();
            //Creamos una lista con algunos IDs
            var arrayOfIDs = new String[]
            {
                "19e44f74-x-4284-90ea-e6f02be75613", "1cfffeb9-66a3-x-b1b3-208610be7a73", "27896d44-x-48bd-9b14-50ac09e5b0a4"
            };
            Random random = new Random();
            string endpointToCall = $"/gamedata/ID";
            var rIndex = 0;

            // ---- ACT ----
            // Enviar tantas peticiones como _times diga al endpoint /gamedata en paralelo
            await Parallel.ForEachAsync(Enumerable.Range(0, timesToCall), async (i, ct) =>
            {
                //Enviamos un ID válido aleatorio
                rIndex = random.Next(0, arrayOfIDs.Length);
                endpointToCall = $"/gamedata/{arrayOfIDs[rIndex]}";
                var response = _client.DeleteAsync(endpointToCall, ct);
                tasks.Add(response);
            });

            // Esperamos a que todas las peticiones se completen
            Task.WaitAll(tasks.ToArray());
            // Obtenemos las respuestas de las tareas
            var responses = tasks.Select(t => t.Result).ToList();

            // ---- ASSERT ----
            Assert.Multiple(async () =>
            {
                // Verificar que todas las respuestas tengan el código de estado 404 (NotFound)
                string content;
                JToken parseContentToJson;
                int total_answer = 0;
                foreach (var response in responses)
                {
                    total_answer += 1;
                    Assert.That(response.StatusCode, Is.EqualTo(expected: HttpStatusCode.NotFound), $"ERR: Respondió correctamente {total_answer} de {timesToCall} Requests");
                    // Verificamos que el contenido de la respuesta sea una lista de objetos 
                    // JSON válidos y tenga la estructura esperada usando FluentAssertions
                    content = await response.Content.ReadAsStringAsync();
                }
            });

        }

        [NonParallelizable]
        [Order(5)]
        [Test]
        public async Task PostValidGameData_ShouldReturnCreatedUnderPikeLoadAsync()
        {
            // ---- ARRANGE ---
            int timesToCall = 300;
            var client = new HttpClient();
            var tasks = new ConcurrentBag<Task<HttpResponseMessage>>();
            Random random = new Random();
            string endpointToCall = "/gamedata";
            //Creamos una lista de objetos GameData
            var gameDataList = new List<GameData>()
            {
                new GameData
            {
                GameName = "Peck a Bo",
                Category = "Dice Game",
                TotalBets = 6000,
            },new GameData
            {
                GameName = "Gin Poker-Texas Hold-Em",
                Category = "Card Game",
                TotalBets = 5000,
            },new GameData
            {
                GameName = "Chinchon",
                Category = "Card Game",
                TotalBets = 2000,
            }
            };
            var rIndex = 0;
            string gameDataJson;
            string responseContent;
            GameData gameData;
            StringContent content;

            // ---- ACT ----
            await Parallel.ForEachAsync(Enumerable.Range(0, timesToCall), async (i, ct) =>
            {
                rIndex = random.Next(0, gameDataList.Count);
                gameDataJson = JsonConvert.SerializeObject(gameDataList[rIndex], Formatting.Indented);
                content = new StringContent(gameDataJson, Encoding.UTF8, "application/json");
                var response = _client.PostAsync(endpointToCall, content, ct);
                tasks.Add(response);
            });
            Task.WaitAll(tasks.ToArray());
            // Obtener las respuestas de las tareas
            var responses = tasks.Select(t => t.Result).ToList();

            // ---- ASSERT ----
            int totalAnswers = 0;

            // Verificar que todas las respuestas tengan el código de estado 201 (Created)
            foreach (var response in responses)
            {
                totalAnswers++;
                Assert.That(response.StatusCode, Is.EqualTo(expected: HttpStatusCode.Created), $"ERR: Respondió correctamente {totalAnswers} de {timesToCall} Requests");
            }

        }
    }
}
