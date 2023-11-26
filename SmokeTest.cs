using System.Text;
using FluentAssertions;
using Xunit;

namespace CirsaStressTestingApplication
{
    /// <summary>
    /// Clase que contiene pruebas de "smoke tests" para la API de estadísticas de juegos.
    /// </summary>
    public class GameStatisticsSmokeTest
    {
        private readonly HttpClient _client;

        string id = "3423cdf8-2e47-421d-b1aa-03ebea2f5bec";

        /// <summary>
        /// Constructor de la clase. Configura el cliente HTTP para apuntar al entorno local o de prueba de la API.
        /// </summary>
        public GameStatisticsSmokeTest()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5212") // Cambia esto según la URL de tu API
            };
        }

        /// <summary>
        /// Verifica que el endpoint /gamedata responda con un código de estado OK.
        /// </summary>
        [Fact(Skip = "Esta prueba ya es correcta")]
        public async Task SmokeTest_GetAllGameData_ShouldReturnOk()
        {
            var response = await _client.GetAsync("/gamedata");
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        /// <summary>
        /// Verifica que el endpoint /gamedata/{id} responda con un código de estado OK si el juego existe.
        /// Además, verifica que el código de estado no sea NotFound para el mismo endpoint.
        /// </summary>
        [Fact(Skip = "Esta prueba ya es correcta")]
        public async Task SmokeTest_GetGameDataById_ShouldReturnOk()
        {
            var response = await _client.GetAsync($"/gamedata/{id}");

            // 200
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            // 404
            response.StatusCode.Should().NotBe(System.Net.HttpStatusCode.NotFound);
        }

        /// <summary>
        /// Verifica que el endpoint /gamedata responda con un código de estado Created después de agregar un nuevo juego.
        /// </summary>
        [Fact(Skip = "Esta prueba ya es correcta")]
        public async Task SmokeTest_CreateGameData_ShouldReturnCreated()
        {
            var requestContent = new StringContent("{ \"gameName\": \"CirsaGame\", \"category\": \"Sport(ium)\" }", Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/gamedata", requestContent);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }

        /// <summary>
        /// Verifica que el endpoint /gamedata/{id} responda con un código de estado NoContent después de actualizar los datos de un juego existente.
        /// </summary>
        [Fact(Skip = "Esta prueba ya es correcta")]
        public async Task SmokeTest_UpdateGameData_ShouldReturnNoContent()
        {
            var requestContent = new StringContent("{ \"gameName\": \"Juego Actualizado\", \"category\": \"Categoria actualizada\" }", Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"/gamedata/{id}", requestContent);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Verifica que el endpoint /gamedata/{id} responda con un código de estado NoContent después de eliminar un juego.
        /// Además, verifica que el código de estado no sea NotFound para el mismo endpoint.
        /// </summary>
        [Fact(Skip = "Esta prueba ya es correcta")]
        public async Task SmokeTest_DeleteGameData_ShouldReturnNoContent()
        {
            var response = await _client.DeleteAsync($"/gamedata/{id}");
            //204
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
            // 404
            response.StatusCode.Should().NotBe(System.Net.HttpStatusCode.NotFound);
        }
    }
}