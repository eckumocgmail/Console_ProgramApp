using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using static InputApplicationProgram;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class ConsoleProgram: ProgramConsoleCursor
    {
        public static void Main(params string[] args)
        {
            Host.CreateDefaultBuilder().ConfigureWebHostDefaults(b => b.UseStartup<ConsoleProgram>()).Build().RunAsync();

            var actions = new ConcurrentDictionary<string, Action<string>>();
            actions["WriteLine"] = (message) =>            
                Console.WriteLine(message);
            
            var client = new SignalRClient("https://localhost:5151/hubs/programs", "", "");
            while (client.IsConnected() == false)
            {
                Console.WriteLine("connecting");
                client.Connect(actions, async () =>
                {
                    Info(client.IsConnected());
                }).Wait();
                Thread.Sleep(1000);
            }
            while (true)
            {
                Console.WriteLine(client.IsConnected());
         
                Thread.Sleep(1000);
            }
            Thread.Sleep(Timeout.Infinite);
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
 
            app.UseRouting();

            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ProgramsHub>("/hubs/programs");
            });
        }
    }


    
    public class ProgramConsoleCursor: ProgramConsolePosition
    {
         
        public static string Clear( int x, int y, int dx, int dy )
        {
            var messages = new List<string>();
            for (int j = 0; j < dy; j++)
                messages.Add(CreateSpace(dx));
            return Write(x,y,messages.ToArray());
        }

        private static string CreateSpace(int dx)
        {
            string space = "";
            for (int i = 0; i < dx; i++)
                space += " ";
            return space;
        }

        public static string Write( int x, int y, params string[] args )
        {
            int maxLength = 0;
            for (int i=0; i<args.Length; i++ )
            {
                maxLength = Math.Max(maxLength, args[i].Length);
                SetPos(x, y+i);
                Console.WriteLine(args[i]);
            }
            return "{" + $"{x},{y}" + ";" + $"{x+maxLength},{y+args.Length}" + "}";


        }
    }
    public class ProgramConsolePosition
    {
        private static string _Pos = "[0,0]";
        public static string GetPos()
            => $"[{(Console.CursorLeft)},{(Console.CursorTop)}]";
        public static void SetPos(string pos)
            => SetPos(int.Parse(pos.Substring(1).Split(',')[0]), int.Parse(pos.Substring(1).Split(',')[1]));
        public static string SetPos(int x, int y)
            => (_Pos = $"[{(Console.CursorLeft = x)},{(Console.CursorTop = y)}]");
    }
}
