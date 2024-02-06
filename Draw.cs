using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

internal class Draw
{
    static Random r = new Random();
    int[][] Houses = GenerateHouses(r.Next(60, 80));
    static int[] houseLengths = new int[99];
    static int[] Xpos = new int[99];
    static int[] Ypos = new int[99];
    public int[] MapIndex = new int[10000];
    public static int BinaryRandom(int resultA, int resultB, int resultAchance, int resultBchance)
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
    public static int[][] GenerateHouses(int width)
    {
        int houseAmount = r.Next(width/20, width/10);
        int[][] temparr = new int[houseAmount][];
        for (int h = 0;h<houseAmount ;h++)
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
        int treeDensity = r.Next(2, 15);
        for (int i = 0; i < 10000; i++) 
        {
            MapIndex[i] = BinaryRandom(0, 3, 90, treeDensity);
        }
        int currenthousepos;
        for (int i = 0; i < Houses.Length; i++)
        {
            currenthousepos = Xpos[i] + (200 * Ypos[i]);
            for (int row = 0; row + 1 < Houses[i].Length / houseLengths[i]; row++)
            {
                
                for (int n = 0; n < houseLengths[i]; n++)
                {
                    MapIndex[currenthousepos+(row*200)+n] = Houses[i][n+ houseLengths[i]*(row+1)];
                }
            }
        }
    }
    // 0 = " " = nothing
    // 1 = "-" = floor
    // 2 = "#" = wall
    // 3 = "ƒ" = tree
    //

    public void DrawMap()
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
            }
        }
    }
}