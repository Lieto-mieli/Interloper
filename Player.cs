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
    Human,
    Undead,
    Elemental,
    Golem,
}
public enum Class 
{
    StreetThug,
    Engineer,
    MagicUser,
    
}
internal class Player
{
    public string name = "";
    public Race race;
    public Class Class;
    public ConsoleColor playerColor;
    public Point2D position;
    private char image = '@';
    Draw draw = null;
    public Player (Draw draw) 
    {
    this.draw = draw;
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
                    Debug.Write("paskat menee |");
                    break;
                case 1:
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("-");
                    Debug.Write("saatanan vitun paskat menee nyt |");
                    break;
                default:
                    Debug.Write("nyt meni ihan hyllynmyllyn |");
                    break;
            }
        }
        Debug.Write(position.x + (position.y * 200));
    }
    public void Draw()
    {
        Console.ForegroundColor = playerColor;
        Console.SetCursorPosition(position.x, position.y);
        Console.Write(image);
        Console.CursorVisible = false;
    }
}