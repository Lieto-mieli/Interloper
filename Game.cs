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
using RayGuiCreator;
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
    string inMenu = "main"; // states are "main", "character", "setting", and "none"
    int spinnerValue = 0;
    bool spinnerEditActive = false;
    TextBoxEntry playerNameEntry = new TextBoxEntry(15);
    MultipleChoiceEntry classAns = new MultipleChoiceEntry(new string[]
    {
            Convert.ToString(Class.Street_Thug),
            Convert.ToString(Class.Tinkerer),
            Convert.ToString(Class.Spellcaster) 
    });
    MultipleChoiceEntry originAns = new MultipleChoiceEntry(new string[]
    {
            Convert.ToString(Origin.Human),
            Convert.ToString(Origin.Undead),
            Convert.ToString(Origin.Elemental),
            Convert.ToString(Origin.Golem)
    });
    string mostRecentBranchMenu;
    float volume = 1.0f;
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
        if (false)
        {
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
                Console.WriteLine("Select Origin");
                foreach (int i in Enum.GetValues(typeof(Origin)))
                {
                    Console.Write($"{Enum.GetName(typeof(Origin), i)}");
                    Console.WriteLine($" {i}, ");
                }
                string originAnswer = Console.ReadLine();
                if (Enum.TryParse<Origin>(originAnswer, true, out player.origin))
                {
                    if (long.TryParse(originAnswer, out number1))
                    {
                        if (Convert.ToInt64(originAnswer) <= Enum.GetNames(typeof(Origin)).Length - 1)
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
                    player.origin = Enum.Parse<Origin>(originAnswer, true);
                    break;
                }
                Console.WriteLine("Invalid selection. Please input the name of a origin, or its order in the list");
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
            Console.WriteLine($"Name: {player.name} | Origin: {player.origin} | Class: {player.Class} | Color: {player.playerColor}");
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
        }
        Console.WindowWidth = 200;
        Console.WindowHeight = 50;
        Console.CursorVisible = false;
        player.position = new Point2D(10, 25);
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
        DrawMainMenu();
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
        if (Raylib.IsKeyDown(KeyboardKey.KEY_ENTER) && delay <= 0)
        {
            if (inMenu == "none")
            {
                inMenu = "pause";
                delay = 0.2;
            }
            else if (inMenu == "pause")
            {
                inMenu = "none";
                delay = 0.2;
            }
        }
        if (inMenu == "main")
        {
            DrawMainMenu();
        }
        else if (inMenu == "pause")
        {
            DrawPauseMenu();
        }
        else if (inMenu == "setting")
        {
            DrawSettingMenu();
            //draw settings menu
        }
        else if (inMenu == "character")
        {
            DrawCharacterCreation();
        }
        else
        {
            Raylib.BeginDrawing();
            draw.AltDrawMap();
            player.AltDraw();
            Raylib.EndDrawing();
        }
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
    public void DrawMainMenu()
    {
        // Tyhjennä ruutu ja aloita piirtäminen
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Raylib.BLACK);

        // Laske ylimmän napin paikka ruudulla.
        int button_width = 600;
        int button_height = 120;
        int button_x = Raylib.GetScreenWidth() / 2 - button_width / 2;
        int button_y = Raylib.GetScreenHeight() / 2 - button_height * 3;
        MenuCreator c = new MenuCreator(button_x, button_y, button_height, button_width, 80);

        // Piirrä pelin nimi nappien yläpuolelle
        //RayGui.GuiLabel(new Rectangle(button_x, button_y - button_height * 2, button_width, button_height), "Rogue");
        c.Label("Interloper");

        //if (RayGui.GuiButton(new Rectangle(button_x, button_y, button_width, button_height), "Start Game") == 1)
        if (c.Button("Enter"))
        {
            // Start the game
            inMenu = "character";
        }
        // Piirrä seuraava nappula edellisen alapuolelle
        //button_y += button_height * 2;

        //if (RayGui.GuiButton(new Rectangle(button_x, button_y, button_width, button_height), "Options") == 1)
        if (c.Button("Options"))
        {
            // Go to options somehow
            inMenu = "setting";
            mostRecentBranchMenu = "main";
        }

        //button_y += button_height * 2;

        //if (RayGui.GuiButton(new Rectangle(button_x, button_y, button_width, button_height), "Quit") == 1)
        if (c.Button("Leave"))
        {
            // Quit the game
            Raylib.CloseWindow();
        }
        Raylib.EndDrawing();
    }
    public void DrawCharacterCreation()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Raylib.BLACK);
        int width = Raylib.GetScreenWidth() / 2;
        // Fit 22 rows on the screen
        int rows = 22;
        int rowHeight = Raylib.GetScreenHeight() / rows;
        // Center the menu horizontally
        int x = (Raylib.GetScreenWidth() / 2) - (width / 2);
        // Center the menu vertically
        int y = (Raylib.GetScreenHeight() - (rowHeight * rows)) / 2;
        // 3 pixels between rows, text 3 pixels smaller than row height
        MenuCreator c = new MenuCreator(x, y, rowHeight, width, 10, -3);
        c.Label("Create your character");

        c.Label("Player name");
        c.TextBox(playerNameEntry);

        c.Label("Character class");
        c.DropDown(classAns);

        c.Label("Character origin");
        c.DropDown(originAns);

        c.Spinner("Player Color", ref spinnerValue, 0, 15, ref spinnerEditActive);
        Color temp = player.FromColorNOBAMBOOZLE((ConsoleColor)spinnerValue);
        Raylib.DrawCircle(1650, 420, 110, Raylib.WHITE);
        Raylib.DrawCircle(1650, 420, 100, temp);

        if (c.Button("Confirm"))
        {
            if (playerNameEntry.ToString() != null && playerNameEntry.ToString().Length > 0)
            {
                inMenu = "none";
                player.name = playerNameEntry.ToString();
                player.Class = player.Class = Enum.Parse<Class>(classAns.GetSelected(), true);
                player.origin = player.origin = Enum.Parse<Origin>(originAns.GetSelected(), true);
                player.playerColor = player.playerColor = Enum.Parse<ConsoleColor>(Convert.ToString(spinnerValue), true);
            }
            else { Console.WriteLine("Your name must have atleast one character"); }
        }
        // Draws open dropdowns over other menu items
        int menuHeight = c.EndMenu();
        int padding = 2;
        Raylib.DrawRectangleLines(
            x - padding,
            y - padding,
            width + padding * 2,
            menuHeight + padding * 2,
            MenuCreator.GetLineColor());
        Raylib.EndDrawing();
    }
    public void DrawPauseMenu()
    {
        // Tyhjennä ruutu ja aloita piirtäminen
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Raylib.BLACK);
        int button_width = 600;
        int button_height = 120;
        int button_x = Raylib.GetScreenWidth() / 2 - button_width / 2;
        int button_y = Raylib.GetScreenHeight() / 2 - button_height * 3;
        MenuCreator c = new MenuCreator(button_x, button_y, button_height, button_width, 80);
        c.Label("Game Paused");
        if (c.Button("Resume"))
        {
            inMenu = "none";
        }
        if (c.Button("Options"))
        {
            inMenu = "setting";
            mostRecentBranchMenu = "pause";
        }
        if (c.Button("Main menu"))
        {
            inMenu = "main";
        }
        Raylib.EndDrawing();
    }
    public void DrawSettingMenu()
    {
        // Tyhjennä ruutu ja aloita piirtäminen
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Raylib.BLACK);
        int button_width = Raylib.GetScreenWidth() / 2;
        int button_height = 120;
        int button_x = Raylib.GetScreenWidth() / 2 - button_width / 2;
        int button_y = Raylib.GetScreenHeight() / 2 - button_height * 3;
        MenuCreator c = new MenuCreator(button_x, button_y, button_height, button_width, 30);
        c.Label("Settings");
        c.Label("Volume");
        c.Slider("Quiet", "Max", ref volume, 0.0f, 1.0f);
        c.Spinner("Player Color", ref spinnerValue, 0, 15, ref spinnerEditActive);
        Color temp = player.FromColorNOBAMBOOZLE((ConsoleColor)spinnerValue);
        Raylib.DrawCircle(1650, 420, 110, Raylib.WHITE);
        Raylib.DrawCircle(1650, 420, 100, temp);
        if (c.Button("Save & Exit"))
        {
            Raylib.SetMasterVolume(volume);
            inMenu = mostRecentBranchMenu;
        }
        Raylib.EndDrawing();
    }
}
