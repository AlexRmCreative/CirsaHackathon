using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Collections.Generic;
using FluentAssertions.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using GameStatistics.Test.Models;
using Newtonsoft.Json;
using System;
using System.Text;

namespace SmokeTests
{
    [TestFixture]
    public class SmokeTests
    {
        private HttpClient _client;
        private int _times;


        [SetUp]
        public void Setup()
        {
            // Inicializar el cliente HTTP con la URL base de la API y las veces que se llamará
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://localhost:7170"); //Change this to your api URL
            _times = 100;
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
        public async Task GetGameData_ShouldReturnOkUnderMinimalLoadAsync()
        {
            // ---- ARRANGE ---
            // Crear una lista para almacenar las tareas
            var tasks = new ConcurrentBag<Task<HttpResponseMessage>>();
            string endpointToCall = "/gamedata";

            // ---- ACT ----
            // Enviar tantas peticiones como _times diga al endpoint /gamedata en paralelo
            await Parallel.ForEachAsync(Enumerable.Range(0, _times), async (i, ct) =>
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
                    Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), $"ERR: Respondió correctamente {totalAnswers} de {_times} Requests");
                    // Verificar que todas las respuestas tengan el contenido JSON válido
                    Assert.That(response.Content.Headers.ContentType.MediaType, Is.EqualTo("application/json"));
                    // Verificamos que el contenido de la respuesta sea una lista de objetos 
                    // JSON válidos y tenga la estructura esperada usando FluentAssertions
                    var content = await response.Content.ReadAsStringAsync();
                    var parseContentToJson = JToken.Parse(content);
                    foreach (var item in parseContentToJson)
                    {
                        item.Should().HaveElement("id")
                        .And.HaveElement("gameName")
                        .And.HaveElement("category")
                        .And.HaveElement("totalBets")
                        .And.HaveElement("totalWins")
                        .And.HaveElement("averageBetAmount")
                        .And.HaveElement("popularityScore");
                    }
                }
            });
        }


        [NonParallelizable]
        [Order(2)]
        [Test]
        public async Task GetValidGameDataByID_ShouldReturnOkUnderMinimalLoadAsync()
        {
            // ---- ARRANGE ---
            // Creamos una lista para almacenar las tareas
            var tasks = new ConcurrentBag<Task<HttpResponseMessage>>();
            //Creamos una lista con algunos IDs
            var arrayOfIDs = new String[]
            {
                "19e44f74-fa25-4284-90ea-e6f02be75613", "1cfffeb9-66a3-4ab2-b1b3-208610be7a73", "27896d44-dbca-48bd-9b14-50ac09e5b0a4"
            };
            Random random = new Random();
            string endpointToCall = $"/gamedata/ID";
            var rIndex = 0;

            // ---- ACT ----
            // Enviar tantas peticiones como _times diga al endpoint /gamedata en paralelo
            await Parallel.ForEachAsync(Enumerable.Range(0, _times), async (i, ct) =>
            {
                //Enviamos un ID válido aleatorio
                rIndex = random.Next(0, arrayOfIDs.Length);
                endpointToCall = $"/gamedata/{arrayOfIDs[rIndex]}";
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
                    Assert.That(response.StatusCode, Is.EqualTo(expected: HttpStatusCode.OK), $"ERR: Respondió correctamente {totalAnswers} de {_times} Requests");
                    // Verificar que todas las respuestas tengan el contenido JSON válido
                    Assert.That(response.Content.Headers.ContentType.MediaType, Is.EqualTo(expected: "application/json"));
                    // Verificamos que el contenido de la respuesta sea una lista de objetos 
                    // JSON válidos y tenga la estructura esperada usando FluentAssertions
                    var content = await response.Content.ReadAsStringAsync();
                    var parseContentToJson = JToken.Parse(content);
                    parseContentToJson.Should().HaveElement("id")
                        .And.HaveElement("gameName")
                        .And.HaveElement("category")
                        .And.HaveElement("totalBets")
                        .And.HaveElement("totalWins")
                        .And.HaveElement("averageBetAmount")
                        .And.HaveElement("popularityScore");
                }
            });
        }


        [NonParallelizable]
        [Order(3)]
        [Test]
        public async Task PostValidGameData_ShouldReturnCreatedUnderMinimalLoadAsync()
        {
            // ---- ARRANGE ---
            var tasks = new ConcurrentBag<Task<HttpResponseMessage>>();
            Random random = new Random();
            string endpointToCall = "/gamedata";
            //Creamos una lista de objetos GameData
            var gameDataList = new List<GameData>();
            gameDataList.Add(new GameData
            {
                GameName = "Peck a Bo",
                Category = "Dice Game",
                TotalBets = 6000,
            });
            gameDataList.Add(new GameData
            {
                GameName = "Gin Poker-Texas Hold-Em",
                Category = "Card Game",
                TotalBets = 5000,
            });
            gameDataList.Add(new GameData
            {
                GameName = "Chinchon",
                Category = "Card Game",
                TotalBets = 2000,
            });
            var rIndex = 0;
            string gameDataJson ;
            string responseContent;
            GameData gameData;
            StringContent content;

            // ---- ACT ----
            await Parallel.ForEachAsync(Enumerable.Range(0, _times), async (i, ct) =>
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
            Assert.Multiple(async () =>
            {
                // Verificar que todas las respuestas tengan el código de estado 201 (Created)
                foreach (var response in responses)
                {
                    totalAnswers++;
                    Assert.That(response.StatusCode, Is.EqualTo(expected: HttpStatusCode.Created), $"ERR: Respondió correctamente {totalAnswers} de {_times} Requests");
                    //responseContent = await response.Content.ReadAsStringAsync();
                    //gameData = JsonConvert.DeserializeObject<GameData>(responseContent);
                    var content = await response.Content.ReadAsStringAsync();
                    var parseContentToJson = JToken.Parse(content);
                    parseContentToJson.Should().HaveElement("id")
                        .And.HaveElement("gameName")
                        .And.HaveElement("category")
                        .And.HaveElement("totalBets")
                        .And.HaveElement("totalWins")
                        .And.HaveElement("averageBetAmount")
                        .And.HaveElement("popularityScore");
                }
            });
        }


        [NonParallelizable]
        [Order(4)]
        [Test]
        public async Task PutValidGameData_ShouldReturnNoContentUnderMinimalLoadAsync()
        {
            // ---- ARRANGE ---
            // Creamos una lista para almacenar las tareas
            var tasks = new ConcurrentBag<Task<HttpResponseMessage>>();
            var gameDataList = new List<GameData>();
            gameDataList.Add(new GameData
            {
                Id = "bb2fd604-cf76-457b-9f00-f0ab8c892781",
                GameName = "Volleyball Game",
                Category = "Recreational machines",
            });
            gameDataList.Add(new GameData
            {
                Id = "1cfffeb9-66a3-4ab2-b1b3-208610be7a73",
                GameName = "Caribbean Stud Poker",
                Category = "Card Game",
            });
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
            await Parallel.ForEachAsync(Enumerable.Range(0, _times), async (i, ct) =>
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
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent), $"ERR: Respondió correctamente {totalAnswers} de {_times} Requests");
            }
        }


        [NonParallelizable]
        [Test]
        public async Task DeleteUnvalidGameDataByID_ShouldReturnNotFoundUnderMinimalLoadAsync()
        {
            // ---- ARRANGE ---
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
            await Parallel.ForEachAsync(Enumerable.Range(0, _times), async (i, ct) =>
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
                    Assert.That(response.StatusCode, Is.EqualTo(expected: HttpStatusCode.NotFound), $"ERR: Respondió correctamente {total_answer} de {_times} Requests");
                    // Verificamos que el contenido de la respuesta sea una lista de objetos 
                    // JSON válidos y tenga la estructura esperada usando FluentAssertions
                    content = await response.Content.ReadAsStringAsync();
                }
            });
        }
    }
}
