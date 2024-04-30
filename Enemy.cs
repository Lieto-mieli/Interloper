using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
internal class Enemy
{
    Random r = new Random();
    public int health;
    public string[] attackStyles = new string[10];
    public string vanity;
    public int position;
    public ConsoleColor color;
    public string[] Q = new string[]
    {
            "beam",
            "widemelee",
            "melee",
    };
    public void CreateEnemy(int nro, int pos)
    {
        // makes new json file representing an enemy on the map
        Enemy enemy = new Enemy();
        enemy.position = pos;
        enemy.health = 12;
        enemy.vanity = "Q";
        switch(vanity)
        {
            case "Q":
                for (int i = 0; i < Q.Length; i++)
                {
                    enemy.attackStyles[i] = Q[i];
                }
                break;
        }
        enemy.color = (ConsoleColor)health;
        string output = JsonConvert.SerializeObject(enemy);
        using (StreamWriter outputFile = new StreamWriter(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $"Enemy{nro}.json"))) 
        {
            outputFile.Write(output);
            //Debug.Write(output);
        }
        // reads a json file which represents an enemy, allowing it to interact with the game
        using (StreamReader outputFile = new StreamReader(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $"Enemy{nro}.json")))
        {
            Enemy testoutput = JsonConvert.DeserializeObject<Enemy>(outputFile.ReadToEnd());
            Debug.Write(testoutput.health);
        }
    }
}
