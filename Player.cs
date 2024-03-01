// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
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
    public Player (Draw draw) 
    {
    this.draw = draw;
    }
    public static int AbsDiff(int a, int b)
    {
        if (a > b)
        {
            // If 'a' is greater than 'b', return the difference of 'a' and 'b' multiplied by 2
            return a - b;
        }
        // If 'a' is not greater than 'b', return the difference of 'b' and 'a'
        return b - a;
    }
    public void Move(int x_move, int y_move)
    {
        position.x += x_move;
        position.y += y_move;
        position.x = Math.Clamp(position.x, 0, Console.WindowWidth - 1);
        position.y = Math.Clamp(position.y, 0, Console.WindowHeight - 1);
        if (draw.MapIndex[position.x + (position.y * 200)]==2 || draw.MapIndex[position.x + (position.y * 200)] == 3)
        {
            //this if isnt working for some reason!!!
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
            //spend turn on movement
            Console.SetCursorPosition(position.x-x_move, position.y-y_move);
            switch (draw.MapIndex[position.x-x_move + ((position.y-y_move) * 200)])
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
                default:
                    break;
            }
        }
        int Y = Convert.ToInt32(draw.upgPos/200);
        int X = draw.upgPos - (Y * 200);
        if ((AbsDiff(X,position.x)+AbsDiff(Y,position.y))<10 && draw.MapIndex[draw.upgPos]==4)
        {
            Console.SetCursorPosition(X, Y);
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = (ConsoleColor)r.Next(1,16);
            Console.Write("§");
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
}