using System;
using System.Threading;

class Program
{
    // 0 - горизонтальні, 1 - вертикальні
    static int[] lightStates = { 0, 0 }; // кожна група має свій стан
    static int[] lightCounters = { 0, 0 };
    static int[] lightDurations = { 7, 10 }; // кожна група працює з іншою швидкістью зміни світла

    static int[] horizontalCarPositions = { 0, 20 };
    static int[] horizontalCarSpeeds = { 1, 2 };
    static int[] verticalCarPositions = { 0, 10 };
    static int[] verticalCarSpeeds = { 1, 1 };

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.CursorVisible = false;

        int roadWidth = 60;
        int roadHeight = 20;

        while (true)
        {

            // Малюємо всі дороги
            DrawRoad(roadWidth, roadHeight);

            // Оновлюємо світло кожної групи світлофорів
            for (int i = 0; i < lightStates.Length; i++)
            {
                lightCounters[i]++;
                if (lightCounters[i] >= lightDurations[i])
                {
                    lightCounters[i] = 0;
                    lightStates[i] = (lightStates[i] + 1) % 3;
                }
            }
            // Відображаємо світлофори:
            // Горизонтальні (ліво,право) — група 0
            DisplayTrafficLights(20, 9, lightStates[0]);
            DisplayTrafficLights(40, 9, lightStates[0]);
            // Вертикальні (верх,низ) — група 1
            DisplayTrafficLights(29, 5, lightStates[1]);
            DisplayTrafficLights(29, 13, lightStates[1]);

            // Рухаємо всі автомобілі
            for (int i = 0; i < horizontalCarPositions.Length; i++)
            {
                int nextPos = (horizontalCarPositions[i] + horizontalCarSpeeds[i]) % roadWidth;

                // Червоне світло справа (або зліва) світлофора?
                if (lightStates[0] == 0 && nextPos == 39) // червоне, світлофор на 40
                {
                    // зупинитися, не рухаємо
                }
                else
                {
                    horizontalCarPositions[i] = nextPos;
                }
            }

            for (int i = 0; i < verticalCarPositions.Length; i++)
            {
                int nextPos = (verticalCarPositions[i] + verticalCarSpeeds[i]) % roadHeight;

                // Червоне світло зверху світлофора?
                if (lightStates[1] == 0 && nextPos == 12) // червоне, світлофор на 13
                {
                    // зупинитися, не рухаємо
                }
                else
                {
                    verticalCarPositions[i] = nextPos;
                }
            }
            DisplayAllCars(roadWidth, roadHeight);

            Thread.Sleep(500);
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

        // Горизонтальні автомобілі
        for (int i = 0; i < horizontalCarPositions.Length; i++)
        {
            int x = horizontalCarPositions[i] % roadWidth;
            Console.SetCursorPosition(x, centerY);
            Console.Write("=>");
        }

        // Вертикальні автомобілі
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

        // Горизонтальна смуга
        for (int x = 0; x < width; x++)
        {
            Console.SetCursorPosition(x, centerY);
            Console.Write("─");
        }

        // Вертикальна смуга
        for (int y = 0; y < height; y++)
        {
            Console.SetCursorPosition(centerX, y);
            Console.Write("│");
        }
    }
}
