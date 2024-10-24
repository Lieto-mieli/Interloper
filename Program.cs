using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroElectric.Vinculum;
internal class Program
{
    public static void Main()
    {
        while (true)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Game interloper = new Game();
            interloper.Run();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Start anew? Y/N");
            interloper.UnloadAll(); 
            if (Console.ReadLine() == "N")
            {
                break;
            }
        }
    }
}