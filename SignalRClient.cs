using Microsoft.AspNetCore.SignalR.Client;
 

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

internal class SignalRClient
{
    private string url;
    private string username;
    private string password;
    private HubConnection Connection;

    public SignalRClient(string url, string username, string password)
    {
        this.url = url;
        this.username = username;
        this.password = password;
    }


    public void Connect<TService>( IServiceProvider provider ) where TService : class
    {
        var actions = new ConcurrentDictionary<string, Action<string>>();
        var names = typeof(TService).GetMethods().Select(method => method.Name).Distinct();
        foreach (var name in names)
        {

            actions[name] = (message) => 
            {
                var availableMethods = typeof(TService).GetMethods().Where(m => m.Name == name);
            };
            
        }
    }

    public bool IsConnected()
    {
        return Connection != null && Connection.State == HubConnectionState.Connected;
    }

    public async Task Connect( 
        IDictionary<string, Action<string>> PublicApi, 
        Action OnStart )
    {
        Console.WriteLine($"Connect( ... )");
        
        this.Connection = new HubConnectionBuilder()
            .WithUrl(url)
            .Build();
          
        
        await Connection.StartAsync().ContinueWith(async (result) =>
        {
          
            result.Wait();
            if (result.Exception!=null)
            {
                this.Error(result.Exception);
            }

            foreach(var kv in PublicApi)
            {
                string EventType = kv.Key;
                var ActionEvent = kv.Value;
                Connection.On<string>(kv.Key, (response) => {
                    this.Info(EventType);
                    try
                    {
                        ActionEvent(response);
                    }
                    catch (Exception ex)
                    {
                        this.Error("Ошибка при обработки события "+EventType, ex);
                    }
                    return Task.CompletedTask;
                });

            }
            OnStart();
        });

    }

    internal async Task<string> Signin(string username, string password)
    {
        this.Info(this.Connection.State);
        Thread.Sleep(1000);
        this.Info(this.Connection.State);

        return "123";
    }
}