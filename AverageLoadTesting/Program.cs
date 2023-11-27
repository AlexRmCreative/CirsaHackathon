using GameStatistics.Test.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static int nRequest = 0;
    static int iRequest = 5;
    static double elapsedSeconds = 0;
    static List<DateTime> startTimes = new List<DateTime>();
    static string apiUrl = "http://localhost:5212/gamedata";
    static string id = "004d5d26-5724-43e7-b68f-c0e1061a5e50"; // ID para utilizar en las llamadas
    static int selectedEndpointCall = -1; // Variable para almacenar el índice del endpoint seleccionado

    static async Task SelectEndpointCall()
    {
        string[] options = { "(GET)Get all games", "(GET/id)Get game by his id", "(POST)Create a new game", "(PUT/id)Update a game data", "(DELETE/id)Delete a game data" };

        ConsoleKey key;
        do
        {
            Console.Clear();
            Console.WriteLine("Presiona la flecha hacia arriba o abajo para seleccionar una opción. Presiona Esc para cerrar la aplicación.");

            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine(i == selectedEndpointCall ? $"=> {options[i]}" : $"   {options[i]}");
            }

            key = Console.ReadKey().Key;

            if (key == ConsoleKey.Enter)
            {
                selectedEndpointCall = selectedEndpointCall != -1 ? selectedEndpointCall : 0; // Almacena el índice solo si no se ha seleccionado previamente
                await SelectTestType();
            }
            else if (key == ConsoleKey.UpArrow && selectedEndpointCall > 0)
            {
                selectedEndpointCall--;
            }
            else if (key == ConsoleKey.DownArrow && selectedEndpointCall < options.Length - 1)
            {
                selectedEndpointCall++;
            }
        } while (key != ConsoleKey.Escape);
    }

    static async Task SelectTestType()
    {
        int selectedIndex = 0;
        string[] options = { "Manual Load Test", "Automatic Load Test" };

        ConsoleKey key;
        do
        {
            Console.Clear();
            Console.WriteLine("Presiona Esc para salir de la aplicación");
            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine(i == selectedIndex ? $"=> {options[i]}" : $"   {options[i]}");
            }

            key = Console.ReadKey().Key;

            if (key == ConsoleKey.Enter)
            {
                Console.Clear();
                if (selectedIndex != -1)
                {
                    if (selectedIndex == 0)
                    {
                        await RunLoadTest();
                    }
                    else if (selectedIndex == 1)
                    {
                        await RunAutomaticLoadTest();
                    }
                }
            }
            else if (key == ConsoleKey.UpArrow && selectedIndex > 0)
            {
                selectedIndex--;
            }
            else if (key == ConsoleKey.DownArrow && selectedIndex < options.Length - 1)
            {
                selectedIndex++;
            }
        } while (key != ConsoleKey.Escape);
    }

    static async Task RunLoadTest()
    {
        Console.Clear();
        Console.WriteLine("Presiona la flecha hacia arriba para aumentar o hacia abajo para disminuir las peticiones por segundo. Presiona Esc para volver al menú principal.");

        while (true)
        {
            var key = Console.ReadKey().Key;

            if (key != ConsoleKey.Escape)
            {
                await RunLoadTestIteration();
                if (key == ConsoleKey.UpArrow)
                {
                    nRequest += iRequest;
                }
                else if (key == ConsoleKey.DownArrow && nRequest > iRequest)
                {
                    nRequest -= iRequest;
                }
                UpdateConsole();
                await CallSelectedEndpoint(); // Llama al endpoint seleccionado
                Console.WriteLine($"Respuesta recibida en {elapsedSeconds} segundos");
            }
            else
            {
                break;
            }
        }
    }

    static async Task RunAutomaticLoadTest()
    {
        Console.Clear();
        Console.WriteLine("Presiona la flecha hacia arriba para aumentar o hacia abajo para disminuir las peticiones por segundo. Presiona Esc para volver al menú principal.");

        while (true)
        {
            await RunLoadTestIteration();

            if (Console.KeyAvailable)
            {
                var key = Console.ReadKey().Key;

                if (key != ConsoleKey.Escape)
                {
                    if (key == ConsoleKey.UpArrow)
                    {
                        nRequest += iRequest;
                    }
                    else if (key == ConsoleKey.DownArrow && nRequest > iRequest)
                    {
                        nRequest -= iRequest;
                    }

                    UpdateConsole();
                    Console.WriteLine($"Respuesta recibida en {elapsedSeconds} segundos");
                    Console.WriteLine($"Se están enviando {nRequest} peticiones a la API POR SEGUNDO...");

                    await CallSelectedEndpoint(); // Llama al endpoint seleccionado
                }
                else
                {
                    break;
                }
            }
        }
    }

    static async Task CallSelectedEndpoint()
    {
        var newGameData = new GameData
        {
            GameName = "Example Game",
            Category = "Example Category",
            TotalBets = 100,  // Asigna el valor necesario
            LastUpdated = DateTime.Now
        };
        switch (selectedEndpointCall)
        {
            case 0:
                await CallGetApiEndpoint();
                break;
            case 1:
                // Lógica para llamar al endpoint (GET/id)Get game by his id
                await CallGetByIdApiEndpoint(id);
                break;
            case 2:
                // Lógica para llamar al endpoint (POST)Create a new game
                await CallPostApiEndpoint(newGameData);
                break;
            case 3:
                // Lógica para llamar al endpoint (PUT/id)Update a game data
                await CallPutApiEndpoint(id, newGameData);

                break;
            case 4:
                // Lógica para llamar al endpoint (DELETE/id)Delete a game data
                await CallDeleteApiEndpoint(id);
                break;
            default:
                Console.WriteLine("Opción no válida");
                break;
        }
    }

    static void UpdateConsole()
    {
        Console.Clear();
        Console.WriteLine("Presiona la flecha hacia arriba para aumentar o hacia abajo para disminuir las peticiones enviadas");
        Console.WriteLine("Presiona Esc para volver al menú principal.");
        Console.WriteLine();
        Console.WriteLine($"Número de solicitudes enviadas: {nRequest}");
        EvaluateResponseTime(elapsedSeconds);
    }

    static async Task RunLoadTestIteration()
    {
        var requestTasks = new List<Task>();

        using (var httpClient = new HttpClient())
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                for (int i = 0; i < nRequest; i++)
                {
                    var startTime = DateTime.Now;
                    startTimes.Add(startTime);
                    requestTasks.Add(MakeRequest(httpClient, apiUrl, startTime));
                }

                await Task.WhenAll(requestTasks);
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }

            stopwatch.Stop();
            elapsedSeconds = stopwatch.Elapsed.TotalSeconds;
        }
    }

    static async Task MakeRequest(HttpClient httpClient, string apiUrl, DateTime startTime)
    {
        try
        {
            // Puedes cambiar la llamada según el endpoint que quieras probar
            // Ejemplo: await httpClient.GetAsync($"{apiUrl}/{id}");
            var response = await httpClient.GetAsync(apiUrl);
            var endTime = DateTime.Now;
            elapsedSeconds = (endTime - startTime).TotalSeconds;
        }
        catch (Exception ex)
        {
            HandleError(ex);
        }
    }

    static async Task CallGetApiEndpoint()
    {
        using (var httpClient = new HttpClient())
        {
            try
            {
                var response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("GET request successful.");
                }
                else
                {
                    Console.WriteLine($"GET request failed with status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }
    }

    static async Task CallGetByIdApiEndpoint(string id)
    {
        using (var httpClient = new HttpClient())
        {
            try
            {
                var response = await httpClient.GetAsync($"{apiUrl}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"GET by ID request for {id} successful.");
                }
                else
                {
                    Console.WriteLine($"GET by ID request for {id} failed with status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }
    }

    static async Task CallPostApiEndpoint(object requestData)
    {
        using (var httpClient = new HttpClient())
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("POST request successful.");
                }
                else
                {
                    Console.WriteLine($"POST request failed with status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }
    }

    static async Task CallPutApiEndpoint(string id, object requestData)
    {
        using (var httpClient = new HttpClient())
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

                var response = await httpClient.PutAsync($"{apiUrl}/{id}", content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"PUT request for {id} successful.");
                }
                else
                {
                    Console.WriteLine($"PUT request for {id} failed with status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }
    }

    static async Task CallDeleteApiEndpoint(string id)
    {
        using (var httpClient = new HttpClient())
        {
            try
            {
                var response = await httpClient.DeleteAsync($"{apiUrl}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"DELETE request for {id} successful.");
                }
                else
                {
                    Console.WriteLine($"DELETE request for {id} failed with status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                HandleError(ex);
            }
        }
    }

    static void HandleError(Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
        Task.Delay(15000).Wait();
        Environment.Exit(1);
    }

    static void EvaluateResponseTime(double responseTimeSeconds)
    {
        double goodThreshold = 0.375; // Tiempo de respuesta considerado "bueno" en segundos
        double badThreshold = 0.625;  // Tiempo de respuesta considerado "malo" en segundos

        Console.Write("Calidad del tiempo de respuesta: ");

        if (responseTimeSeconds <= goodThreshold)
        {
            Console.WriteLine("Bueno");
        }
        else if (responseTimeSeconds <= badThreshold)
        {
            Console.WriteLine("Malo");
        }
        else
        {
            Console.WriteLine("Muy Malo");
        }
    }

    static async Task Main()
    {
        await SelectEndpointCall();
    }
}
