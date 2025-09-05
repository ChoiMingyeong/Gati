using CommonLib;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DummyClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var serverUrl = "https://localhost:8001/game";

            var connection = new HubConnectionBuilder()
                .WithUrl(serverUrl)
                .AddMessagePackProtocol()
                .WithAutomaticReconnect()
                .Build();

            connection.Reconnecting += error =>
            {
                Console.WriteLine($"Reconnecting: {error?.Message}");
                return Task.CompletedTask;
            };
            connection.Reconnected += connectionId =>
            {
                Console.WriteLine($"Reconnected: {connectionId}");
                return Task.CompletedTask;
            };
            connection.Closed += async error =>
            {
                Console.WriteLine($"Closed: {error?.Message}");
                await Task.Delay(TimeSpan.FromSeconds(3));
                try
                {
                    await connection.StartAsync();
                }
                catch { }
            };

            try
            {
                await connection.StartAsync();
                Console.WriteLine($"Connected. ConnectionId = {connection.ConnectionId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to connect: {ex}");
                return;
            }
            var res = await connection.InvokeAsync<Response.UserInfo>("Login", new Request.LoginInfo()
            {
                Id = connection.ConnectionId
            });

            Console.WriteLine($"Login Response Name : {res.Name} Level : {res.Level}");

            Console.ReadLine();
        }
    }
}
