using DiscordRPC;
using System;
using System.Diagnostics;
using System.Linq;

namespace Chilong
{
    class Program
    {
        private static DiscordRpcClient client;
        private static Process sumatra;
        private static string windowTitle;

        static void Main(string[] args)
        {
            Initialize();
            if (Process.GetProcesses().Where(x => x.ProcessName.StartsWith("SumatraPDF")).Count() == 0)
                return;

            while (true)
            {
                Update();
                if (Process.GetProcesses().Where(x => x.ProcessName.StartsWith("SumatraPDF")).Count() == 0)
                {
                    Deinitialize();
                    Console.WriteLine("Thanks for using Chilong!");
                    return;
                }
            }
        }

        private static void Initialize()
        {
            client = new DiscordRpcClient("692113594623721514");

            if(Process.GetProcesses().Where(x => x.ProcessName.StartsWith("SumatraPDF")).Count() == 0)
            {
                Console.WriteLine("SumatraPDF was not found! Is it open?");
                return;
            }
            sumatra = Process.GetProcesses().Where(x => x.ProcessName.StartsWith("SumatraPDF")).ToList()[0];
            windowTitle = sumatra.MainWindowTitle;

            client.OnReady += (sender, e) =>
            {
                Console.WriteLine("Received Ready from user {0}", e.User.Username);
            };
            client.OnPresenceUpdate += (sender, e) =>
            {
                Console.WriteLine("Received Update! {0}", e.Presence);
            };

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

            try { SetNewPresence(); }
            catch(Exception e)
            {
                Console.WriteLine($"Setting presence was not successful!\nERROR: {e.Message}");
                return;
            }
        }
        private static void Update()
        {
            client.OnPresenceUpdate += (sender, e) =>
            {
                Console.WriteLine("Received Update! {0}", e.Presence);
            };
            client.Invoke();
            OnUpdate();
        }
        private static void Deinitialize()
        {
            client.ClearPresence();
            client.Dispose();
        }

        private static void OnUpdate()
        {
            Process process;
            try
            {
                process = Process.GetProcesses().Where(x => x.ProcessName.StartsWith("SumatraPDF")).ToList()[0];
            }
            catch(Exception) { return; }

            if (process.MainWindowTitle != windowTitle)
            {
                sumatra = process;
                windowTitle = sumatra.MainWindowTitle;
                SetNewPresence();
            }
        }
        private static void SetNewPresence()
        {
            string details = windowTitle.TrimEnd(" - SumatraPDF".ToCharArray());
            if(details == " " || details == "")
                details = "Picking a New File";

            client.SetPresence(new RichPresence
            {
                Details = details,
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
    }
}
