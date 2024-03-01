using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
internal class Program
{
    public static void Main()
    {
        while (true)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Enemy enemy = new Enemy();
            Game interloper = new Game();
            interloper.Run();
            Console.WriteLine("Start anew? Y/N");
            if (Console.ReadLine() == "N")
            {
                break;
            }
        }
    }
}