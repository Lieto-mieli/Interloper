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