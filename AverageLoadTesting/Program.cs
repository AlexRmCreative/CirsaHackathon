using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;

namespace AverageLoadTesting
{
    class Program
    {
        static async Task Main()
        {
            string apiUrl = "http://localhost:5212/gamedata";

            // Número inicial de solicitudes concurrentes
            int initialNumberOfRequests = 1;

            // Incremento en el número de solicitudes concurrentes en cada iteración
            int increment = 5;

            // Lista para almacenar los tiempos de inicio de las solicitudes
            var startTimes = new List<DateTime>();

            while (true) // bucle infinito para esperar la entrada del usuario
            {
                // Lista para almacenar las tareas de solicitud
                var requestTasks = new List<Task>();

                // Configura el cliente HTTP
                using (var httpClient = new HttpClient())
                {
                    // Inicia el cronómetro para medir el tiempo total
                    var stopwatch = Stopwatch.StartNew();

                    // Realiza las solicitudes concurrentes
                    for (int i = 0; i < initialNumberOfRequests; i++)
                    {
                        startTimes.Add(DateTime.Now); // Registra el tiempo de inicio
                        requestTasks.Add(MakeRequest(httpClient, apiUrl));
                    }

                    // Espera a que todas las tareas de solicitud se completen
                    await Task.WhenAll(requestTasks);

                    // Detiene el cronómetro
                    stopwatch.Stop();

                    // Calcula y muestra el tiempo total en segundos
                    double totalTimeInSeconds = stopwatch.Elapsed.TotalSeconds;
                    Console.WriteLine($"Número de solicitudes concurrentes: {initialNumberOfRequests}");
                    Console.WriteLine($"Tiempo total: {totalTimeInSeconds} segundos");

                    // Calcula y muestra el tiempo de respuesta promedio en segundos
                    double averageResponseTimeInSeconds = totalTimeInSeconds / initialNumberOfRequests;
                    Console.WriteLine($"Tiempo de respuesta promedio: {averageResponseTimeInSeconds} segundos");

                    // Evalúa y muestra la calidad del tiempo de respuesta
                    EvaluateResponseTime(totalTimeInSeconds);
                    Console.WriteLine();

                    // Espera la entrada del usuario
                    Console.WriteLine("Presiona la flecha hacia arriba para aumentar, hacia abajo para disminuir. Presiona Esc para salir.");
                    var key = Console.ReadKey().Key;

                    // Ajusta el número de solicitudes concurrentes según la tecla presionada
                    if (key == ConsoleKey.UpArrow)
                    {
                        initialNumberOfRequests += increment;
                    }
                    else if (key == ConsoleKey.DownArrow && initialNumberOfRequests > increment)
                    {
                        initialNumberOfRequests -= increment;
                    }
                    else if (key == ConsoleKey.Escape)
                    {
                        break; // Sal del bucle si se presiona la tecla Esc
                    }

                    Console.Clear(); // Limpia la consola para mostrar la nueva información
                }
            }
        }

        static async Task MakeRequest(HttpClient httpClient, string apiUrl)
        {
            // Realiza una solicitud GET a la API
            var response = await httpClient.GetAsync(apiUrl);

            // Por ahora, simplemente consumimos el contenido de la respuesta
            var content = await response.Content.ReadAsStringAsync();
        }

        static void EvaluateResponseTime(double averageResponseTimeInSeconds)
        {
            // Define los umbrales para evaluar la calidad del tiempo de respuesta
            double goodThreshold = 0.2;
            double badThreshold = 0.75;

            string response = "Calidad del tiempo de respuesta total: ";

            Console.Write(response);

            // Evalúa y muestra la calidad del tiempo de respuesta
            if (averageResponseTimeInSeconds <= goodThreshold)
            {
                Console.Write("Bueno");
            }
            else if (averageResponseTimeInSeconds <= badThreshold)
            {
                Console.WriteLine("Malo");
            }
            else
            {
                Console.WriteLine("Muy Malo");
            }
        }
    }
}
