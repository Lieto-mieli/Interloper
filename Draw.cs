using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Formats.Asn1;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZeroElectric.Vinculum;
using static System.Net.Mime.MediaTypeNames;

internal class Draw
{
    public int option1;
    public int option2;
    public int option3;
    public int upgPos;
    public int levelNum = 0;
    static Random r = new Random();
    static int[] houseLengths = new int[99];
    static int[] Xpos = new int[99];
    static int[] Ypos = new int[99];
    public int[] MapIndex = new int[10000];
    Enemies enemies = null;
    public int nroOfEnemies = 0;
    Font font;
    public Draw(Enemies enemies)
    {
        this.enemies = enemies;
    }
    Player player = null;
    public Draw(Player player)
    {
        this.player = player;
    }
    public string FirstCharSubstring(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }
        return $"{input[0].ToString().ToUpper()}{input.Substring(1)}";
        //capitalizes first letter of string
    }
    public static int BinaryRandom(int resultA, int resultB, int resultAchance, int resultBchance)//returns 'resultA' with 'resultAchance' weight and 'resultB' with 'resultBchance' weight
    {
        int returner = 1;
        WeightedChanceExecutor weightedChanceExecutor = new WeightedChanceExecutor
        (
        new WeightedChanceParam(() =>
            {
                returner = resultA;
            }, resultAchance),
        new WeightedChanceParam(() =>
            {
                returner = resultB;
            }, resultBchance)
        );
        weightedChanceExecutor.Execute();
        return returner;
    }
    public static int[][] GenerateHouses(int amount)//generates a set of buildings to make navigation more interesting for the player
    {
        int houseAmount = r.Next(amount - 1, amount + 2);
        int[][] temparr = new int[houseAmount][];
        for (int h = 0; h < houseAmount; h++)
        {
            int hL = r.Next(10, 29);
            int[] temphouse = new int[hL * r.Next(4, hL / 2)];
            houseLengths[h] = hL;
            Xpos[h] = r.Next(10, 170);
            Ypos[h] = r.Next(10, 36);
            for (int i = 0; i < temphouse.Length; i++)
            {
                temphouse[i] = BinaryRandom(1, 2, 2, 1);
            }
            temparr[h] = temphouse;
        }
        return temparr;

    }
    public void GenerateMap()
    {
        int[][] Houses = GenerateHouses(levelNum + 5);
        int treeDensity = r.Next(12 - levelNum, 16 - levelNum);
        for (int i = 0; i < 10000; i++)//generates trees based on how close the player has gotten to the center
        {
            MapIndex[i] = BinaryRandom(0, 3, 90, treeDensity);

        }
        int currenthousepos;
        for (int i = 0; i < Houses.Length; i++)//inserts generated buildings onto level
        {
            currenthousepos = Xpos[i] + (200 * Ypos[i]);
            for (int row = 0; row + 1 < Houses[i].Length / houseLengths[i]; row++)
            {

                for (int n = 0; n < houseLengths[i]; n++)
                {
                    MapIndex[currenthousepos + (row * 200) + n] = Houses[i][n + houseLengths[i] * (row + 1)];
                }
            }
        }
        bool upgDone = true;
        while (upgDone)//places upgrade onto a random indoor tile
        {
            int randoSpot = r.Next(0, 10000);
            if (MapIndex[randoSpot] == 1)
            {
                MapIndex[randoSpot] = 4;
                upgPos = randoSpot;
                upgDone = false;
            }
        }
        nroOfEnemies = enemies.GenerateEnemies(MapIndex, levelNum);
    }
    //here is a list of all upgrades for all classes.
    static string[] strength = new string[]
    {
        "┌──────────┐",
        "│          │",
        "│  ┎┮┯┯┓   │",
        "│  ┠┴┴┴╊┓  │",
        "│  ┗┓  ╋╹  │",
        "│          │",
        "└──────────┘",
        "Strength",
        "Gray"
        //deal more damage
    };
    public void Strength()
    {
        //this should call player class and tell them to increase their damage by a bit
    }
    static string[] constitution = new string[]
    {
        "┌──────────┐",
        "│ ┏┓    ┏┓ │",
        "│┌┨┃    ┃┠┐│",
        "┝┿╋╋━━━━╋╋┿┥",
        "│└┨┃    ┃┠┘│",
        "│ ┗┛    ┗┛ │",
        "└──────────┘",
        "Constitution",
        "DarkGray"
        //get an extra life
    };
    public void Constitution()
    {
        //this should call player class and tell them to increase hp by 1
    }
    static string[] refuse = new string[]
    {
        "┌──────────┐",
        "│  ╲   ╱   │",
        "│   ╲ ╱    │",
        "│    ╳     │",
        "│   ╱ ╲    │",
        "│  ╱   ╲   │",
        "└──────────┘",
        "Refuse",
        "Red"
        //do nothing
    };
    public void Refuse()
    {
        //this upgrade does nothing, method exists to avoid null reference
    }
    static string[][] class0lists = new string[][]
    {
        strength,
        constitution,
        refuse
    };
    static string[] engineer = new string[]
    {
        "┌──────────┐",
        "│          │",
        "│          │",
        "│ {▐###█══ │",
        "│  [███]   │",
        "│  ╱   ╲   │",
        "└──────────┘",
        "Engineer",
        "DarkYellow"
        //gives access to/improves ability to summon turret
    };
    public void Engineer()
    {
        //this upgrade flips a bool to allow the ability to be used, and if this has already been done it increases its stats (extra hitpoint and maybe more damage?)
    }
    static string[] buckshot = new string[]
    {
        "┌──────────┐",
        "│  ╭────╮  │",
        "│  │‾‾‾‾│  │",
        "│  │    │  │",
        "│  │____│  │",
        "│  ┴────┴  │",
        "└──────────┘",
        "Buckshot",
        "Red"
        //gives access to/improves shotgun
    };
    public void Buckshot()
    {
        //this upgrade flips a bool to allows ranged attacks to be made in the 8 possible directions, does very high damage and even higher damage when chosen again. drawback is that it needs to be used once again to reload so you can fire it again
    }
    static string[] theory = new string[]
    {
        "┌──────────┐",
        "│  ╱‾‾‾‾╲/ │",
        "│ ╱     ╱╲ │",
        "│|    ╱   |│",
        "│ ╲ ╱    ╱ │",
        "│ /╲____╱  │",
        "└──────────┘",
        "Theory",
        "Cyan"
        //lowers cooldowns and improves effect slightly on abilities
    };
    public void Theory()
    {
        //this upgrade decreases a universal ability cooldown (and potency?) modifier
    }
    static string[][] class1lists = new string[][]
    {
        engineer,
        buckshot,
        theory
    };
    static string[] dagaz = new string[]
    {
        "┌──────────┐",
        "│ |╲   ╱|  │",
        "│ | ╲ ╱ |  │",
        "│ |  ╳  |  │",
        "│ | ╱ ╲ |  │",
        "│ |╱   ╲|  │",
        "└──────────┘",
        "Dagaz",
        "Yellow"
        //orbital solar destruction ray
    };
    public void Dagaz()
    {
        //this upgrade flips a bool to allow the ability to be used, and if this has already been done it increases its damage. at 5 copies its cooldown is removed and mana-cost halved
    }
    static string[] algiz = new string[]
    {
        "┌──────────┐",
        "│ \\  |  /  │",
        "│   \\|/    │",
        "│    |     │",
        "│    |     │",
        "│    |     │",
        "└──────────┘",
        "Algiz",
        "Magenta"
    //teleport enemy to a random spot atleast _ tiles away from the player
    };
    public void Algiz()
    {
        //this upgrade flips a bool to allow the ability to be used, min distance starts at 20 and each copy increases it by 10 up to 100, copies also decrease mana-cost significantly.
    }
    static string[] fehu = new string[]
{
        "┌──────────┐",
        "│   | ╱    │",
        "│   |╱╱    │",
        "│   |╱     │",
        "│   |      │",
        "│   |      │",
        "└──────────┘",
        "Fehu",
        "DarkBlue"
    //increase maximum mana and mana regen
};
    public void Fehu()
    {
        //this upgrade increases the players maximum mana capacity and also increases how fast their mana regenerates
    }
    static string[][] class2lists = new string[][]
    {
        dagaz,
        algiz,
        fehu
    };
    static string[][][] upgClassLists = new string[][][]
    {
        class0lists,
        class1lists,
        class2lists
    };
    static Texture constitutioN;
    static Texture strengtH;
    static Texture refusE;
    static Texture[] class0pngs = new Texture[]
    {
        strengtH,
        constitutioN,
        refusE,
    };
    static Texture buckshoT;
    static Texture engineeR;
    static Texture theorY;
    static Texture[] class1pngs = new Texture[]
    {
        engineeR,
        buckshoT,
        theorY,
    };
    static Texture dagaZ;
    static Texture algiZ;
    static Texture fehU;
    static Texture[] class2pngs = new Texture[]
{
        dagaZ,
        algiZ,
        fehU,
};
    static Texture[][] upgClassPngs = new Texture[][]
    {
        class0pngs,
        class1pngs,
        class2pngs
    };

    //here ends the lists of upgrades
    int outTest;
    public void UpgradeGet(Class Pclass, ConsoleColor Pcolor)//draw the upgrade menu, has old worthless code and raylib code interspersed but i dont want to risk breaking it
    {
        // lataan tekstuurit tässä koska se pitää tehdä window initialisaation jälkeen ja oon laiska
        upgClassPngs[0][0] = Raylib.LoadTexture("Textures/0-Strength.png");
        upgClassPngs[0][1] = Raylib.LoadTexture("Textures/0-Constitution.png");
        upgClassPngs[0][2] = Raylib.LoadTexture("Textures/0-Refuse.png");
        upgClassPngs[1][0] = Raylib.LoadTexture("Textures/1-Engineer.png");
        upgClassPngs[1][1] = Raylib.LoadTexture("Textures/1-Buckshot.png");
        upgClassPngs[1][2] = Raylib.LoadTexture("Textures/1-Theory.png");
        upgClassPngs[2][0] = Raylib.LoadTexture("Textures/2-Dagaz.png");
        upgClassPngs[2][1] = Raylib.LoadTexture("Textures/2-Algiz.png");
        upgClassPngs[2][2] = Raylib.LoadTexture("Textures/2-Constitution.png");
        font = Raylib.LoadFont("Fonts/ArialUnicodeFont.ttf");
        //font = Raylib.LoadFontEx("Fonts/ArialUnicodeFont.ttf", 20, 2580);
        ZeroElectric.Vinculum.Color tempcolor = Player.FromColor(Pcolor);
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = Pcolor;
        Console.SetCursorPosition(50, 30);
        Console.Write("╔");
        for (int i = 0; i < 100; i++)
        {
            Console.Write("═");
        }
        Console.Write("╗");
        for (int y = 0; y < 20; y++)
        {
            Console.SetCursorPosition(50, 31 + y);
            Console.Write("║");
            for (int i = 0; i < 100; i++)
            {
                Console.Write(" ");
            }
            Console.Write("║");
        }
        Raylib.BeginDrawing();
        Raylib.EndDrawing();
        Raylib.BeginDrawing();
        // *Raylib rant removed*
        Debug.WriteLine("");
        Debug.WriteLine(Raylib.GetCodepoint('═', out outTest));
        Debug.WriteLine(Char.ConvertFromUtf32(Raylib.GetCodepoint('═', out outTest)));
        Debug.WriteLine(Char.ConvertFromUtf32(outTest));
        Debug.WriteLine("");
        Raylib.DrawRectangle(50 * 10, 30 * 20, 10, 21 * 20, tempcolor);
        Raylib.DrawRectangle(51 * 10, 30 * 20, 10 * 100, 20, tempcolor);
        Raylib.DrawRectangle(150 * 10, 30 * 20, 10, 21 * 20, tempcolor);
        Raylib.DrawRectangle(51 * 10, 31 * 20, 10 * 99, 20 * 20, Raylib.BLACK);
        Raylib.DrawTexture(upgClassPngs[0][0], 60*10, 35*20, Raylib.WHITE);
        Raylib.DrawRectangle((71 * 10)+5, (37 * 20)+10, 30, 30, Raylib.BLACK);
        Debug.WriteLine(upgClassPngs[0][0]);
        string upg1temp = upgClassLists[(int)Pclass][option1][7];
        string upg2temp = upgClassLists[(int)Pclass][option2][7];
        string upg3temp = upgClassLists[(int)Pclass][option3][7];
        UpgChoice(60, 35, Pclass, 1);
        UpgChoice(94, 35, Pclass, 2);
        UpgChoice(128,35, Pclass, 3);
        Raylib.EndDrawing();
        bool upgChoosing = true;
        while (upgChoosing)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    // choose first upgrade
                    System.Reflection.MethodInfo info1 =
                    GetType().GetMethod(upg1temp);
                    info1.Invoke(this, null);
                    upgChoosing = false;
                    break;
                case ConsoleKey.D2:
                case ConsoleKey.NumPad2:
                    // choose second upgrade
                    System.Reflection.MethodInfo info2 =
                    GetType().GetMethod(upg2temp);
                    info2.Invoke(this, null);
                    upgChoosing = false;
                    break;
                case ConsoleKey.D3:
                case ConsoleKey.NumPad3:
                    // choose third upgrade
                    System.Reflection.MethodInfo info3 =
                    GetType().GetMethod(upg3temp);
                    info3.Invoke(this, null);
                    upgChoosing = false;
                    break;
            }
        }
        Console.Clear();
        DrawMap();
    }
    public void UpgChoice(int X, int Y, Class upgClass, int order)//im gonna be real, i have no idea what the bozo was on while writing this
    {
        int currentoption = 0;
        bool tempdone = true;
        if (order == 1)
        {
            option1 = r.Next(0, upgClassLists[(int)upgClass].Length);
            currentoption = option1;
        }
        else if (order == 2)
        {
            while (tempdone)
            {
                option2 = r.Next(0, upgClassLists[(int)upgClass].Length);
                if (option2 != option1)
                {
                    currentoption = option2;
                    tempdone = false;
                }
            }
        }
        else if (order == 3)
        {
            tempdone = true;
            while (tempdone)
            {
                option3 = r.Next(0, upgClassLists[(int)upgClass].Length);
                if (option3 != option1 && option3 != option2)
                {
                    currentoption = option3;
                    tempdone = false;
                }
            }
        }
        Debug.Write(X);
        Console.ForegroundColor = Enum.Parse<ConsoleColor>(upgClassLists[(int)upgClass][currentoption][8]);
        for (int i = 0; i < 8; i++)
        {
            Console.SetCursorPosition(X + ((12 - upgClassLists[(int)upgClass][currentoption][i].Length) / 2), Y + i);
            if (i == 3)
            {
                Console.Write($"{upgClassLists[(int)upgClass][currentoption][i]} ({order})");
            }
            else
            {
                Console.Write($"{upgClassLists[(int)upgClass][currentoption][i]}");
            }
        }
        Raylib.DrawTexture(upgClassPngs[(int)upgClass][currentoption], X * 10, Y * 20, Raylib.WHITE);
        Raylib.DrawRectangle(((X + 11) * 10) + 5, ((Y + 2) * 20) + 10, 30, 30, Raylib.BLACK);
        Raylib.DrawTextEx(font, Convert.ToString($"({order})"), new Vector2(((X + 12) * 10) + 5, ((Y + 2) * 20) + 15), 20, 20, Player.FromColor(Enum.Parse<ConsoleColor>(upgClassLists[(int)upgClass][currentoption][8])));
    }
    // 0 = " " = nothing
    // 1 = "-" = floor
    // 2 = "#" = wall
    // 3 = "ƒ" = tree
    // 4 = "-" | "§" when close = upgrade
    // 5 = "%" = enemy
    public void DrawMap()//old method, now useless
    {
        for (int i = 0; i < 10000; i++)
        {
            switch (MapIndex[i])
            {
                case 0:
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write(" ");
                    break;
                case 1:
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("-");
                    break;
                case 2:
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("#");
                    break;
                case 3:
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.Write("ƒ");
                    break;
                case 4:
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("-");
                    break;
            }
        }
    }
    public void AltDrawMap()//draws map
    {
        Raylib.ClearBackground(Raylib.BLACK);
        for (int i = 0; i < 10000; i++)
        {
            switch (MapIndex[i])
            {
                case 0:
                    Raylib.DrawRectangle((i - ((Convert.ToInt32(i / 200)) * 200)) * 10, (Convert.ToInt32(i / 200)) * 20, 10, 20, Raylib.BLACK);
                    break;
                case 1:
                    Raylib.DrawRectangle((i - ((Convert.ToInt32(i / 200)) * 200)) * 10, (Convert.ToInt32(i / 200)) * 20, 10, 20, Raylib.GRAY);
                    break;
                case 2:
                    Raylib.DrawRectangle((i - ((Convert.ToInt32(i / 200)) * 200)) * 10, (Convert.ToInt32(i / 200)) * 20, 10, 20, Raylib.DARKGRAY);
                    break;
                case 3:
                    Raylib.DrawRectangle(((i - ((Convert.ToInt32(i / 200)) * 200)) * 10)+2, ((Convert.ToInt32(i / 200)) * 20)+10, 6, 10, Raylib.DARKBROWN);
                    Raylib.DrawTriangle(new Vector2(((i - ((Convert.ToInt32(i / 200)) * 200)) * 10) + 10, ((Convert.ToInt32(i / 200)) * 20) + 12), new Vector2(((i - ((Convert.ToInt32(i / 200)) * 200)) * 10)+5, ((Convert.ToInt32(i / 200)) * 20) - 5), new Vector2(((i - ((Convert.ToInt32(i / 200)) * 200)) * 10), ((Convert.ToInt32(i / 200)) * 20) + 12), Raylib.DARKGREEN);
                    break;
                case 4:
                    Raylib.DrawRectangle((i - ((Convert.ToInt32(i / 200)) * 200)) * 10, (Convert.ToInt32(i / 200)) * 20, 10, 20, Raylib.GRAY);
                    break;
            }
        }
        for (int i = 0; i < nroOfEnemies; i++)
        {
            using (StreamReader outputFile = new StreamReader(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $"Enemy{i}.json")))
            {
                Enemy testoutput = JsonConvert.DeserializeObject<Enemy>(outputFile.ReadToEnd());
                Raylib.DrawRectangle((testoutput.position - ((Convert.ToInt32(testoutput.position / 200)) * 200)) * 10, (Convert.ToInt32(testoutput.position / 200)) * 20, 10, 20, Raylib.RED);
                MapIndex[testoutput.position] = 5;
            }
        }
    }
}