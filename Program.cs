using DiscordRPC;
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

            using(DiscordRpcClient client = new DiscordRpcClient("692113594623721514"))
            {
                client.Initialize();
                Console.WriteLine("Client successfully started!");
                client.SetPresence(new RichPresence
                {
                    Details = sumatra.MainWindowTitle.TrimEnd(" - SumatraPDF".ToCharArray()),
                    State = "State Placeholder",
                    Timestamps = new Timestamps
                    {
                        Start = sumatra.StartTime
                    }
                });
                Console.WriteLine("Presence successfully set!");
                Console.WriteLine("Details: {0}\nState: {1}\nStartTime: {2}",
                    client.CurrentPresence.Details, client.CurrentPresence.State, client.CurrentPresence.Timestamps.Start);
                Console.ReadKey();
            }
        }
    }
}
