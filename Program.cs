using DiscordRPC;
using System;
using System.Diagnostics;
using System.Linq;

namespace Chilong
{
    class Program
    {
        static void Main(string[] args)
        {
            if(Process.GetProcesses().Where(x => x.ProcessName.StartsWith("SumatraPDF")).Count() == 0)
            {
                Console.WriteLine("SumatraPDF was not found! Is it open?");
                return;
            }

            Process sumatra = Process.GetProcesses().Where(x => x.ProcessName.StartsWith("SumatraPDF")).ToList()[0];
            using(DiscordRpcClient client = new DiscordRpcClient("692113594623721514"))
            {
                try
                {
                    client.Initialize();
                    Console.WriteLine("Successfully connected to client!");
                }
                catch(Exception e)
                {
                    Console.WriteLine($"Connection to client was not successful!\nERROR: {e.Message}");
                    return;
                }

                try
                {
                    client.SetPresence(new RichPresence
                    {
                        Details = sumatra.MainWindowTitle.TrimEnd(" - SumatraPDF".ToCharArray()),
                        State = "State Placeholder",
                        Timestamps = new Timestamps(DateTime.UtcNow),
                        Assets = new Assets()
                        {
                            LargeImageKey = "sumatra",
                            LargeImageText = "SumatraPDF"
                        }
                    });
                    Console.WriteLine("Presence successfully set!");
                }
                catch(Exception e)
                {
                    Console.WriteLine($"Setting presence was not successful!\nERROR: {e.Message}");
                    return;
                }
                
                while(true)
                {
                    if(Process.GetProcesses().Where(x => x.ProcessName.StartsWith("SumatraPDF")).Count() == 0)
                    {
                        client.ClearPresence();
                        Console.WriteLine("Thanks for using Chilong!");
                        return;
                    }
                }
            }
        }
    }
}
