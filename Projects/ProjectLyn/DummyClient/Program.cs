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

            while (true)
            {
                Console.WriteLine("=== DummyClient Login ===");
                Console.Write("이메일을 입력하세요 (종료하려면 'exit' 입력): ");
                var userInput = Console.ReadLine();

                if (string.IsNullOrEmpty(userInput) || userInput.ToLower() == "exit")
                {
                    Console.WriteLine("프로그램을 종료합니다");
                    break;
                }

                try
                {
                    Console.WriteLine($"로그인 시도 중... [{userInput}]");
                    
                    var loginRequest = new Request.LoginInfo()
                    {
                        ConnectionId = connection.ConnectionId,
                        UserEmail = userInput
                    };

                    var res = await connection.InvokeAsync<Response.UserInfo>("Login", loginRequest);

                    Console.WriteLine("로그인 성공");
                    Console.WriteLine($"이름: {res.Name}");
                    Console.WriteLine($"레벨: {res.Level}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"로그인 실패: {ex.Message}");
                }
            }

            await connection.DisposeAsync();
        }
    }
}
