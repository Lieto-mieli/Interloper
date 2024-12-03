using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
public enum TileState
{
    Inactive,
    Open,
    Closed,
}
internal class Pathfind//This whole class is never getting finished lol
{
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

    public int[] CalcPath(int[] index,int start, int goal)
    {
        TileState[] state = new TileState[index.Length];
        int[] parent = new int[index.Length];
        bool[] newindex = new bool[index.Length];
        bool ben;
        int goaly = Convert.ToInt32(goal / 200);
        int goalx = goal - (goaly * 200);
        int starty = Convert.ToInt32(start / 200);
        int startx = start - (starty * 200);
        int iy;
        int ix;
        for (int i = 0;i < index.Length; i++)
        {
            state[i] = TileState.Inactive;
            if (index[i] == 0 || index[i] == 1) 
            {
                ben = true;
            }
            else
            {
                ben = false;
            }
            newindex[i] = ben;
        }
        double[] distindex = new double[index.Length];
        for (int i = 0; i < index.Length; i++)
        {
            if (newindex[i] == true)
            {
                iy = Convert.ToInt32(i / 200);
                ix = i - (iy * 200);
                distindex[i] = Math.Sqrt((AbsDiff(goalx, ix) * AbsDiff(goalx, ix)) + (AbsDiff(goaly, iy) * AbsDiff(goaly, iy))) + Math.Sqrt((AbsDiff(startx, ix) * AbsDiff(startx, ix)) + (AbsDiff(starty, iy) * AbsDiff(starty, iy)));
            }
            else
            {
                distindex[i] = double.PositiveInfinity;
            }
        }
        state[startx+(starty*200)] = TileState.Closed;
        OpenAdjacent(startx, starty);
        double[] openlist = new double[index.Length];
        while (true)
        {
            for (int i = 0; i < index.Length; i++)
            {
                if (state[i] == TileState.Open)
                {
                    openlist[i] = distindex[i];
                }
            }
            openlist.Min();
            break;
        }
        
        int[] pathtoplayer = new int[500];
        return pathtoplayer;

        void OpenAdjacent(int x, int y)
        {
            int iy;
            int ix;
            for (int i = 0; i < 10000; i++)
            {
                iy = Convert.ToInt32(i / 200);
                ix = i - (iy * 200);
                if (Math.Sqrt((AbsDiff(x, ix) * AbsDiff(x, ix)) + (AbsDiff(y, iy) * AbsDiff(y, iy))) < 1.5)
                {
                    if (state[i] == TileState.Inactive)
                    {
                        state[i] = TileState.Open;
                    }
                }
            }
        }
    }

}