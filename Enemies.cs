using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
internal class Enemies
{
    Enemy enemy = new();
    int nro = 0;
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
    public void GenerateEnemies(int[] mapindex, int levelnum)
    {
        for (int i = 0; i < 10000; i++)
        {
            if (mapindex[i] == 1)
            {
                if(BinaryRandom(1, 9, 100, levelnum + 4) == 9)
                {
                    enemy.CreateEnemy(nro, i);
                    nro ++;
                }
            }
        }
    }
}