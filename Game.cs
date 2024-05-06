using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ZeroElectric.Vinculum;
using static System.Net.Mime.MediaTypeNames;
internal class Game
{
    Random r = new Random();
    static Enemies enemies = new();
    static Draw draw = new(enemies);
    static Player player = new(draw);
    static Refresh refresh = new(player);
    long number1 = 0;
    public int pos_x;
    public int pos_y;
    Music background;
    double delay = 0;
    DateTime start = DateTime.Now;
    DateTime check;
    bool go = true;
    public void UnloadAll()
    {
        player.UnloadSfx();
        UnloadMusic();
    }
    public void Run()
    {
        Console.WriteLine(Console.LargestWindowWidth);
        Console.WriteLine(Console.LargestWindowHeight);

        while (true)
        {
            if (Console.LargestWindowWidth >= 200 || Console.LargestWindowHeight >= 50) { break; }
            else
            {
                Console.WriteLine("Not enough space for game window. Display resolution should be atleast 1920x1080.");
                Console.WriteLine($"Current possible console window width: {Console.LargestWindowWidth} | Required: 200");
                Console.WriteLine($"Current possible console window height: {Console.LargestWindowHeight} | Required: 50");
                Console.WriteLine("Please press Enter when ready");
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.Enter:
                        break;
                }
            }
        }
        bool valid = false;
        Console.CursorVisible = true;
        Console.Clear();
        Console.WriteLine("Monkeys Spinning Monkeys Kevin MacLeod (incompetech.com) and Fluffing a Duck Kevin MacLeod (incompetech.com) Both licensed under Creative Commons: By Attribution 3.0 License http://creativecommons.org/licenses/by/3.0/");
        Console.WindowWidth = 60;
        Console.WindowHeight = 25;
        while (true)
        {
            Console.WriteLine("What do people call you?:");
            player.name = Console.ReadLine();
            if (player.name != null & player.name.Length > 0)
            {
                break;
            }
            Console.WriteLine("Your name must have atleast one character");
        }
        while (true)
        {
            Console.WriteLine("Select Race");
            foreach (int i in Enum.GetValues(typeof(Race)))
            {
                Console.Write($"{Enum.GetName(typeof(Race), i)}");
                Console.WriteLine($" {i}, ");
            }
            string raceAnswer = Console.ReadLine();
            if (Enum.TryParse<Race>(raceAnswer, true, out player.race))
            {
                if (long.TryParse(raceAnswer, out number1))
                {
                    if (Convert.ToInt64(raceAnswer) <= Enum.GetNames(typeof(Race)).Length - 1)
                    {
                        valid = true;
                    }
                }
                else
                {
                    valid = true;
                }
            }
            if (valid)
            {
                player.race = Enum.Parse<Race>(raceAnswer, true);
                break;
            }
            Console.WriteLine("Invalid selection. Please input the name of a race, or its order in the list");
        }
        valid = false;
        while (true)
        {
            Console.WriteLine("Select Class");
            foreach (int i in Enum.GetValues(typeof(Class)))
            {
                Console.Write($"{Enum.GetName(typeof(Class), i)}");
                Console.WriteLine($" {i}, ");
            }
            string classAnswer = Console.ReadLine();
            if (Enum.TryParse<Class>(classAnswer, true, out player.Class))
            {
                if (long.TryParse(classAnswer, out number1))
                {
                    if (Convert.ToInt64(classAnswer) <= Enum.GetNames(typeof(Class)).Length - 1)
                    {
                        valid = true;
                    }
                }
                else
                {
                    valid = true;
                }
            }
            if (valid)
            {
                player.Class = Enum.Parse<Class>(classAnswer, true);
                break;
            }
            Console.WriteLine("Invalid selection. Please input the name of a class, or its order in the list");
        }
        valid = false;
        while (true)
        {
            Console.WriteLine("Choose player color. Choices are:");
            foreach (int i in Enum.GetValues(typeof(ConsoleColor)))
            {
                Console.Write($"{Enum.GetName(typeof(ConsoleColor), i)}");
                Console.WriteLine($" {i}, ");
            }
            string colorAnswer = Console.ReadLine();
            if (Enum.TryParse<ConsoleColor>(colorAnswer, true, out player.playerColor))
            {
                if (long.TryParse(colorAnswer, out number1))
                {
                    if (Convert.ToInt64(colorAnswer) < Enum.GetNames(typeof(ConsoleColor)).Length)
                    {
                        valid = true;
                    }
                }
                else
                {
                    valid = true;
                }
            }
            if (valid)
            {
                player.playerColor = Enum.Parse<ConsoleColor>(colorAnswer, true);
                break;
            }
            Console.WriteLine("Invalid selection. Please input the name of a color, or its order in the list");
        }
        Console.WindowWidth = 200;
        Console.WindowHeight = 50;
        Console.WriteLine($"Name: {player.name} | Race: {player.race} | Class: {player.Class} | Color: {player.playerColor}");
        Console.WriteLine("Countless have ventured into this place over the years. Few have made it out with their lives. None have gotten to it's elusive center, that which lies to the west.");
        Console.WriteLine("Today you are to become one of many in your attempt to explore this cursed city. And it is unlikely you will be the last. Take your first steps and accept your impending fate.");
        Console.CursorVisible = false;
        player.position = new Point2D(10, 25);
        player.Draw();
        bool startmove = true;
        while (startmove)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            switch (key.Key)
            {
                default:
                    startmove = false;
                    break;
                    //case ConsoleKey.NumPad9:
                    //    startmove= false;
                    //    break;
                    //case ConsoleKey.NumPad8:
                    //    startmove = false;
                    //    break;
                    //case ConsoleKey.NumPad7:
                    //    startmove = false;
                    //    break;
                    //case ConsoleKey.NumPad6:
                    //    startmove = false;
                    //    break;
                    //case ConsoleKey.NumPad4:
                    //    startmove = false;
                    //    break;
                    //case ConsoleKey.NumPad3:
                    //    startmove = false;
                    //    break;
                    //case ConsoleKey.NumPad2:
                    //    startmove = false;
                    //    break;
                    //case ConsoleKey.NumPad1:
                    //    startmove = false;
                    //    break;
            }
        }
        Raylib.InitWindow(2000, 1000, "Raylib");
        Raylib.SetTargetFPS(30);
        Raylib.InitAudioDevice();
        player.LoadSfx();
        Console.Clear();
        draw.GenerateMap();
        draw.DrawMap();
        draw.AltDrawMap();
        player.Draw();
        player.AltDraw();
        bool game_running = true;
        if (r.Next(1, 3) == 1)
        {
            background = Raylib.LoadMusicStream("Sounds/E1-Fluffing-a-Duck.mp3");
        }
        else
        {
            background = Raylib.LoadMusicStream("Sounds/E2-Monkeys-Spinning-Monkeys.mp3");
        }
        background.looping = true;
        Raylib.PlayMusicStream(background);
        while (!Raylib.WindowShouldClose())
        {
            Update();
        }
        UnloadAll();
    }
    public void UnloadMusic()
    {
        Raylib.UnloadMusicStream(background);
    }
    public void Update()
    {
            Raylib.UpdateMusicStream(background);
            if (delay > 0) { delay -= 0.1; }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_KP_9) && delay <= 0)
            {
                player.Move(1, -1);
                delay = 0.2;
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_KP_8) && delay <= 0)
            {
                player.Move(0, -1);
                delay = 0.2;
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_KP_7) && delay <= 0)
            {
                player.Move(-1, -1);
                delay = 0.2;
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_KP_6) && delay <= 0)
            {
                player.Move(1, 0);
                delay = 0.2;
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_KP_4) && delay <= 0)
            {
                player.Move(-1, 0);
                delay = 0.2;
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_KP_3) && delay <= 0)
            {
                player.Move(1, 1);
                delay = 0.2;
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_KP_2) && delay <= 0)
            {
                player.Move(0, 1);
                delay = 0.2;
            }
            if (Raylib.IsKeyDown(KeyboardKey.KEY_KP_1) && delay <= 0)
            {
                player.Move(-1, 1);
                delay = 0.2;
            }
        Raylib.BeginDrawing();
        draw.AltDrawMap();
        player.AltDraw();
        Raylib.EndDrawing();
            //if (go)
            //{
            //    Raylib.BeginDrawing();

            //    Raylib.EndDrawing();
            //    start = DateTime.Now;
            //    go = false;
            //}
            //check = DateTime.Now;
            //if (check.Subtract(start).TotalMilliseconds >= 80)
            //{
            //    go = true;

            //}
    }
}
