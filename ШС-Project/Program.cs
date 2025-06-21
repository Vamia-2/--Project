using System;
using System.Threading;

class Program
{
    static int[] lightStates = { 0, 0 };
    static int[] lightCounters = { 50, 0 };
    static int[] lightDurations = { 10, 10 };

    static int[] horizontalCarPositions = { 0, 20 };
    static int[] horizontalCarSpeeds = { 1, 2 };
    static int[] verticalCarPositions = { 0, 10 };
    static int[] verticalCarSpeeds = { 1, 1 };

    static int mode = 1; // 1 - звичайний, 2 - жовтий блимає, 3 - стоп

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.CursorVisible = false;

        int roadWidth = 60;
        int roadHeight = 20;

        // Окремий потік для читання клавіш
        new Thread(ReadKeys) { IsBackground = true }.Start();

        while (true)
        {

            DrawRoad(roadWidth, roadHeight);

            // Режим 1 — нормальний: оновлюємо світлофори
            if (mode == 1)
            {
                for (int i = 0; i < lightStates.Length; i++)
                {
                    lightCounters[i]++;
                    if (lightCounters[i] >= lightDurations[i])
                    {
                        lightCounters[i] = 0;
                        lightStates[i] = (lightStates[i] + 1) % 3;
                    }
                }
            }

            // Відображення світлофорів
            if (mode == 2)
            {
                // блимає жовтий (стан = 1)
                DisplayTrafficLights(20, 9, 1);
                DisplayTrafficLights(40, 9, 1);
                DisplayTrafficLights(29, 5, 1);
                DisplayTrafficLights(29, 13, 1);
            }
            else
            {
                DisplayTrafficLights(20, 9, lightStates[0]);
                DisplayTrafficLights(40, 9, lightStates[0]);
                DisplayTrafficLights(29, 5, lightStates[1]);
                DisplayTrafficLights(29, 13, lightStates[1]);
            }

            // Рух автомобілів
            for (int i = 0; i < horizontalCarPositions.Length; i++)
            {
                int nextPos = (horizontalCarPositions[i] + horizontalCarSpeeds[i]) % roadWidth;

                bool canMove = true;
                if (mode != 2) // якщо не жовтий блимає
                {
                    if ((mode == 1 || mode == 3) && lightStates[0] == 0 && nextPos == 27)
                    {
                        canMove = false;
                    }
                }

                if (canMove)
                    horizontalCarPositions[i] = nextPos;
            }

            for (int i = 0; i < verticalCarPositions.Length; i++)
            {
                int nextPos = (verticalCarPositions[i] + verticalCarSpeeds[i]) % roadHeight;

                bool canMove = true;
                if (mode != 2)
                {
                    if ((mode == 1 || mode == 3) && lightStates[1] == 0 && nextPos == 10)
                    {
                        canMove = false;
                    }
                }

                if (canMove)
                    verticalCarPositions[i] = nextPos;
            }

            DisplayAllCars(roadWidth, roadHeight);

            // Назва режиму
            Console.SetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Режим: ");
            Console.ForegroundColor = ConsoleColor.White;
            switch (mode)
            {
                case 1: Console.Write("Звичайний"); break;
                case 2: Console.Write("Блимання жовтим"); break;
                case 3: Console.Write("Стоп"); break;
            }

            Thread.Sleep(500);
        }
    }

    static void ReadKeys()
    {
        while (true)
        {
            var key = Console.ReadKey(true).Key;
            if (key == ConsoleKey.D1) mode = 1;
            else if (key == ConsoleKey.D2) mode = 2;
            else if (key == ConsoleKey.D3) mode = 3;
        }
    }

    static void DisplayTrafficLights(int x, int y, int state)
    {
        Console.SetCursorPosition(x, y);
        Console.Write("(");
        Console.ForegroundColor = state == 0 ? ConsoleColor.Red : ConsoleColor.White;
        Console.Write("●");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(")");

        Console.SetCursorPosition(x, y + 1);
        Console.Write("(");
        Console.ForegroundColor = state == 1 ? ConsoleColor.Yellow : ConsoleColor.White;
        Console.Write("●");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(")");

        Console.SetCursorPosition(x, y + 2);
        Console.Write("(");
        Console.ForegroundColor = state == 2 ? ConsoleColor.Green : ConsoleColor.White;
        Console.Write("●");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(")");
    }

    static void DisplayAllCars(int roadWidth, int roadHeight)
    {
        int centerY = roadHeight / 2;
        int centerX = roadWidth / 2;

        for (int i = 0; i < horizontalCarPositions.Length; i++)
        {
            int x = horizontalCarPositions[i] % roadWidth;
            Console.SetCursorPosition(x, centerY);
            Console.Write("=>");
        }

        for (int i = 0; i < verticalCarPositions.Length; i++)
        {
            int y = verticalCarPositions[i] % roadHeight;
            Console.SetCursorPosition(centerX, y);
            Console.Write("*");
        }
    }

    static void DrawRoad(int width, int height)
    {
        int centerY = height / 2;
        int centerX = width / 2;

        for (int x = 0; x < width; x++)
        {
            Console.SetCursorPosition(x, centerY);
            Console.Write("─");
        }

        for (int y = 0; y < height; y++)
        {
            Console.SetCursorPosition(centerX, y);
            Console.Write("|");
        }
    }
}
