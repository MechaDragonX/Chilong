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
            if(Process.GetProcesses().ToList().Where(x => x.ProcessName.StartsWith("SumatraPDF")).Count() == 0)
            {
                Console.WriteLine("SumatraPDF was not found! Is it open?");
                return;
            }

            Process sumatra = Process.GetProcesses().ToList().Where(x => x.ProcessName.StartsWith("SumatraPDF")).ToList()[0];
            using(DiscordRpcClient client = new DiscordRpcClient("692113594623721514"))
            {
                if(!client.Initialize())
                {
                    Console.WriteLine("Connection to client was not successful!");
                    return;
                }

                Console.WriteLine("Successfully connected to client!");
                client.SetPresence(new RichPresence
                {
                    Details = sumatra.MainWindowTitle.TrimEnd(" - SumatraPDF".ToCharArray()),
                    State = "State Placeholder",
                    Timestamps = new Timestamps(DateTime.Now),
                    Assets = new Assets()
                    {
                        LargeImageKey = "sumatra",
                        LargeImageText = "SumatraPDF"
                    }
                });
                if(client.CurrentPresence == null)
                {
                    Console.WriteLine("Setting presence was not successful!");
                    return;
                }
                Console.WriteLine("Presence successfully set!");
                
                while(true)
                {
                    if(Process.GetProcesses().ToList().Where(x => x.ProcessName.StartsWith("SumatraPDF")).Count() == 0)
                    {
                        // client.UpdateEndTime();
                        client.ClearPresence();
                        return;
                    }
                }
            }
        }
    }
}
