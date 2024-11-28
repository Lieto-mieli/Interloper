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

public enum Origin
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
    public Origin origin;
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
    public void LoadSfx()// loads the player walksound
    {
        walksfx = Raylib.LoadSound("Sounds/P1-Metalpipe.mp3");
    }
    public void UnloadSfx()//unloads the player walksound
    {
        Raylib.UnloadSound(walksfx);
    }
    public void Move(int x_move, int y_move)//handles player movement depending on which keypad button was pressed
    {
        position.x += x_move;
        position.y += y_move;
        position.x = Math.Clamp(position.x, 0, Console.WindowWidth - 1);//keeps player within map boundaries on the x axis
        position.y = Math.Clamp(position.y, 0, Console.WindowHeight - 1);//keeps player within map boundaries on the y axis
        if (draw.MapIndex[position.x + (position.y * 200)]==2 || draw.MapIndex[position.x + (position.y * 200)] == 3 || draw.MapIndex[position.x + (position.y * 200)] == 5)//stops player from moving into tiles with impassable id types
        {
            position.x -= x_move;
            position.y -= y_move;
        }
        else if (false)//this is a placeholder "else if" that was planned for attacking enemies
        {
            // this will execute when moving to an enemy(attacking)
        }
        else
        {
            Raylib.PlaySound(walksfx);//plays the player walksound
        }
        int Y = Convert.ToInt32(draw.upgPos/200);//update internal values
        int X = draw.upgPos - (Y * 200);
        if ((AbsDiff(X,position.x)+AbsDiff(Y,position.y))<10 && draw.MapIndex[draw.upgPos]==4)//checks if player is close enough to upgrade for it to be visible, if so then draw the upgrade
        {
            Console.SetCursorPosition(X, Y);
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Write("§");
            Color tempcolor = FromColor((ConsoleColor)r.Next(1, 16));
            Raylib.BeginDrawing();
            Raylib.EndDrawing();
            Raylib.BeginDrawing();
            Raylib.DrawPoly(new Vector2((X * 10)+5,(Y * 20)+10), 6, 8, 0, tempcolor);
            Raylib.EndDrawing();
        } 
        if (position.x + (position.y * 200) == draw.upgPos)//if player is at the upgrade, show upgrade menu
        {
            draw.MapIndex[draw.upgPos] = 1;
            draw.UpgradeGet(Class, playerColor);
            draw.upgPos = 100000;
        }
        Debug.Write(position.x + (position.y * 200)," ");
    }
    public void Draw()//deprecated, doesnt do anything with raylib
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
    public void AltDraw()//draws player and powerup
    {
        Color tempcolor = FromColor(playerColor);
        Raylib.DrawRectangle(position.x * 10, position.y * 20, 10, 20, tempcolor);
        int Y = Convert.ToInt32(draw.upgPos / 200);
        int X = draw.upgPos - (Y * 200);
        if ((AbsDiff(X, position.x) + AbsDiff(Y, position.y)) < 10 && draw.MapIndex[draw.upgPos] == 4)
        {
            tempcolor = FromColor((ConsoleColor)r.Next(1, 16));
            Raylib.DrawPoly(new Vector2((X * 10) + 5, (Y * 20) + 10), 6, 8, 0, tempcolor);
        }
    }
    public static Color FromColor(ConsoleColor c)//turns console colors into raylib equivalents
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
    public Color FromColorNOBAMBOOZLE(ConsoleColor c)//turns console colors into raylib equivalents, also this one isnt static and that appeases the code gods for some reason
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