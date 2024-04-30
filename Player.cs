// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ZeroElectric.Vinculum;
using static Game;

public enum Race
{
    Human,    //gains 1 extra upgrade
    Undead,   //has 2 extra hitpoints
    Elemental,//immune to most natural element attacks
    Golem,    //deals double melee damage
}
public enum Class 
{
    Street_Thug,//melee-only with passive upgrades
    Tinkerer,   //ranged-focus with some abilities
    Spellcaster,//ability-focus with a limited manapool
}
internal class Player
{
    static Random r = new Random();
    public string name = "";
    public Race race;
    public Class Class;
    public ConsoleColor playerColor;
    public Point2D position;
    private char image = '@';
    public int hitpoints = 1;
    public int meleedmgmult = 1;
    public int cooldownmult = 1;
    Draw draw = null;
    Sound walksfx;
    public Player (Draw draw) 
    {
    this.draw = draw;
    }
    public static int AbsDiff(int a, int b)
    {
        if (a > b)
        {
            // If 'a' is greater than 'b', return the difference of 'a' and 'b'
            return a - b;
        }
        // If 'a' is not greater than 'b', return the difference of 'b' and 'a'
        return b - a;
    }
    public void LoadSfx()
    {
        walksfx = Raylib.LoadSound("Sounds/P1-Metalpipe.mp3");
    }
    public void UnloadSfx()
    {
        Raylib.UnloadSound(walksfx);
    }
    public void Move(int x_move, int y_move)
    {
        position.x += x_move;
        position.y += y_move;
        position.x = Math.Clamp(position.x, 0, Console.WindowWidth - 1);
        position.y = Math.Clamp(position.y, 0, Console.WindowHeight - 1);
        if (draw.MapIndex[position.x + (position.y * 200)]==2 || draw.MapIndex[position.x + (position.y * 200)] == 3 || draw.MapIndex[position.x + (position.y * 200)] == 5)
        {
            position.x -= x_move;
            position.y -= y_move;
            // dont spend turn!
        }
        else if (false)
        {
            // this will execute when moving to an enemy(attacking)
        }
        else
        {
            Raylib.PlaySound(walksfx);
            //spend turn on movement
            Console.SetCursorPosition(position.x-x_move, position.y-y_move);
            Raylib.BeginDrawing();
            Raylib.EndDrawing();
            Raylib.BeginDrawing();
            Color tempcolor = FromColor(playerColor);
            Raylib.DrawRectangle(position.x * 10, position.y * 20, 10, 20, tempcolor);
            switch (draw.MapIndex[position.x-x_move + ((position.y-y_move) * 200)])
            {
                    case 0:
                        Raylib.DrawRectangle((position.x - x_move) * 10, (position.y - y_move) * 20, 10, 20, Raylib.BLACK);
                    Debug.WriteLine("on nolla");
                    break;
                    case 1:
                        Raylib.DrawRectangle((position.x - x_move) * 10, (position.y - y_move) * 20, 10, 20, Raylib.GRAY);
                    Debug.WriteLine("on yksi");
                    break;
                    default:
                    Debug.WriteLine("ei vapaa");
                    break;
            }
            Debug.WriteLine($"{position.x - x_move} {position.y - y_move}");
            Debug.WriteLine(draw.MapIndex[position.x - x_move + ((position.y - y_move) * 200)]);
            Raylib.EndDrawing();
        }
        int Y = Convert.ToInt32(draw.upgPos/200);
        int X = draw.upgPos - (Y * 200);
        if ((AbsDiff(X,position.x)+AbsDiff(Y,position.y))<10 && draw.MapIndex[draw.upgPos]==4)
        {
            Console.SetCursorPosition(X, Y);
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Write("§");
            Color tempcolor = FromColor((ConsoleColor)r.Next(1, 16));
            Raylib.BeginDrawing();
            Raylib.EndDrawing();
            Raylib.BeginDrawing();
            //Raylib.DrawTriangle(new Vector2(X * 10,Y * 20), new Vector2((X * 10)+5, (Y * 20)-5), new Vector2((X * 10)+10, Y * 20), tempcolor);
            Raylib.DrawPoly(new Vector2((X * 10)+5,(Y * 20)+10), 6, 8, 0, tempcolor);
            Raylib.EndDrawing();
        } 
        if (position.x + (position.y * 200) == draw.upgPos)
        {
            draw.MapIndex[draw.upgPos] = 1;
            draw.UpgradeGet(Class, playerColor);
            draw.upgPos = 100000;
        }
        Debug.Write(position.x + (position.y * 200)," ");
    }
    public void Draw()
    {
        Console.ForegroundColor = playerColor;
        if (draw.MapIndex[position.x + (position.y * 200)] == 1)
        {
            Console.BackgroundColor = ConsoleColor.Gray;
        }
        else
        {
            Console.BackgroundColor = ConsoleColor.Black;
        }
        Console.SetCursorPosition(position.x, position.y);
        Console.Write(image);
        Console.CursorVisible = false;
    }
    public void AltDraw()
    {
        Raylib.BeginDrawing();
        Raylib.EndDrawing();
        Raylib.BeginDrawing();
        Raylib.EndDrawing();
    }
    public static Color FromColor(ConsoleColor c)
    {
        Color tempcol = Raylib.BLACK;
        switch (c)
        {
            case ConsoleColor.Black:
                tempcol = Raylib.BLACK;
                break;
            case ConsoleColor.DarkBlue:
                tempcol = Raylib.DARKBLUE;
                break;
            case ConsoleColor.DarkGreen:
                tempcol = Raylib.DARKGREEN;
                break;
            case ConsoleColor.DarkYellow:
                tempcol = Raylib.GOLD;
                break;
            case ConsoleColor.DarkGray:
                tempcol = Raylib.DARKGRAY;
                break;
            case ConsoleColor.DarkCyan:
                tempcol = Raylib.SKYBLUE;
                break;
            case ConsoleColor.DarkMagenta:
                tempcol = Raylib.VIOLET;
                break;
            case ConsoleColor.White:
                tempcol = Raylib.WHITE;
                break;
            case ConsoleColor.Red:
                tempcol = Raylib.RED;
                break;
            case ConsoleColor.Green:
                tempcol = Raylib.GREEN;
                break;
            case ConsoleColor.Yellow:
                tempcol = Raylib.YELLOW;
                break;
            case ConsoleColor.Cyan:
                tempcol = Raylib.SKYBLUE;
                break;
            case ConsoleColor.Blue:
                tempcol = Raylib.BLUE;
                break;
            case ConsoleColor.Magenta:
                tempcol = Raylib.MAGENTA;
                break;
            case ConsoleColor.Gray: 
                tempcol = Raylib.GRAY;
                break;
            case ConsoleColor.DarkRed:
                tempcol = Raylib.MAROON;
                break;
        }
        return tempcol;
    }
}