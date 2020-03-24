using System;
using System.Diagnostics;

namespace Chilong
{
    class Program
    {
        static void Main(string[] args)
        {
            Process sumatra = Process.GetProcessesByName("SumatraPDF")[0];
            Console.WriteLine("Id: {0}, MainWindowTitle: {1}", sumatra.Id, sumatra.MainWindowTitle);
        }
    }
}
