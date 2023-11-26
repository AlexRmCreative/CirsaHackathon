using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Timers;
using System.Collections.Generic;
using FluentAssertions.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using GameStatistics.Test.Models;
using Newtonsoft.Json;
using System;
using System.Text;

namespace SpikeTests
{
    [TestFixture]
    public class SpikeTests
    {
        private HttpClient _client;
        private int _times;


        [SetUp]
        public void Setup()
        {
            // Inicializar el cliente HTTP con la URL base de la API y las veces que se llamará
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://localhost:7170"); //Change this to your api URL
            _times = 1000; // Cambiar este valor a un número más alto para generar más carga
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
        [Order(2)]
        [Test]
        public async Task GetValidGameDataByID_ShouldReturnOkUnderSpikeLoadAsync()
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

            // Añadimos un Synchronizing Timer al Thread Group
            var syncTimer = new SynchronizingTimer();
            syncTimer.SyncCount = 100; // Configuramos el número de hilos sincronizados
            syncTimer.AddTestElement(_client);

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
                        .And.HaveElement("category").And.HaveElement("totalBets")
                             .And.HaveElement("totalWins")
                             .And.HaveElement("averageBetAmount")
                             .And.HaveElement("popularityScore");
                }
            });
        }
    }
}