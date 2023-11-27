using System;
using System.Diagnostics;

class Program
{
    static int nRequest = 0;
    static int iRequest = 5;
    static double elapsedSeconds = 0;
    static List<DateTime> startTimes = new List<DateTime>();
    static string apiUrl = "http://localhost:5212/gamedata";

    static async Task StartMenu()
    {
        string[] options = { "Manual Load Test", "Automatic Load Test" };
        int selectedIndex = 0;
        Console.Clear();
        ConsoleKey key = new ConsoleKey();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Presiona Esc para salir de la aplicación");
            for (int i = 0; i < options.Length; i++)
            {
                if (i == selectedIndex)
                {
                    Console.WriteLine("=> " + options[i]);
                }
                else
                {
                    Console.WriteLine("   " + options[i]);
                }
            }

            key = Console.ReadKey().Key;

            if (key == ConsoleKey.Enter)
            {
                Console.Clear();
                if (selectedIndex == 0)
                {
                    await RunLoadTest();
                }
                else if (selectedIndex == 1)
                {
                    await RunAutomaticLoadTest();
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
            else if (key == ConsoleKey.Escape)
            {
                if (selectedIndex == 0)
                {
                    Environment.Exit(0); // Cierra la aplicación si se presiona "Esc" en el menú principal
                }
                else
                {
                    selectedIndex = 0; // Vuelve al menú principal si se presiona "Esc" en el menú secundario
                }
            }
        }
    }

    static async Task RunLoadTest()
    {
        Console.Clear();
        Console.WriteLine("Presiona la flecha hacia arriba para aumentar o hacia abajo para disminuir las peticiones por segundo. Presiona Esc para volver al menú principal.");

        while (true)
        {
            if (Console.KeyAvailable)
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
                    Console.WriteLine($"Respuesta recibida en {elapsedSeconds} segundos");
                }
                else
                {
                    break;
                }
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
                    Console.WriteLine($"Se están enviando {nRequest} peticiones a la API constantemente...");
                }
                else
                {
                    break;
                }
            }
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

            for (int i = 0; i < nRequest; i++)
            {
                var startTime = DateTime.Now;
                startTimes.Add(startTime);
                requestTasks.Add(MakeRequest(httpClient, apiUrl, startTime));
            }

            await Task.WhenAll(requestTasks);

            stopwatch.Stop();
            elapsedSeconds = stopwatch.Elapsed.TotalSeconds;
        }
    }

    static async Task MakeRequest(HttpClient httpClient, string apiUrl, DateTime startTime)
    {
        var response = await httpClient.GetAsync(apiUrl);
        var endTime = DateTime.Now;
        elapsedSeconds = (endTime - startTime).TotalSeconds;
    }

    static void EvaluateResponseTime(double responseTimeSeconds)
    {
        double goodThreshold = 0.3; // Tiempo de respuesta considerado "bueno" en segundos
        double badThreshold = 0.525;  // Tiempo de respuesta considerado "malo" en segundos

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
        await StartMenu();
    }
}
