using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class Server
    {
        public static ConcurrentDictionary<string, string> tables = new ConcurrentDictionary<string, string>();
        public static List<string> nameServer = new List<string>();
        public static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        public WebApplicationBuilder applicationBuilder { get; set; }
        public WebApplication application { get; set; }
        public Server(string[] args)
        {
            this.applicationBuilder = WebApplication.CreateBuilder(args);
            this.applicationBuilder.Services.AddSignalR();
            application = this.applicationBuilder.Build();
            BuildApp();
        }
        private void WebApplicationWork() { }
        private void BuildApp()
        {
            application.UseRouting();

            application.UseEndpoints(endpoints => { endpoints.MapHub<ServerRequests>("/Hub");endpoints.MapGet("/", WebApplicationWork);});

            application.Run();
        }
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            Server server = new Server(args);
        }
    }
    public class ServerRequests : Hub
    {
        public async void SetGame(string typeOfGame, string nameSetter)
        {
            new SetGameClass(nameSetter, Context, Clients, typeOfGame);
        }
        public async void GreenPressed(string namePressed, string nameSetter, int index, string gameType)
        {
            new GreenPressedClass(nameSetter, Context, Clients, gameType, namePressed, index);
        }
        public async void ChangeDamaPlay(string namePlayed, string nameSecond, string time)
        {
            new ChangeDamaPlayClass(namePlayed, nameSecond, time, Context, Clients);
        }
        public async Task SetTimerInfo(string namePlayed, string nameSecond, int timeLeft, int plusMove)
        {
            new SetTimerInfoClass(namePlayed, nameSecond, timeLeft, plusMove, Context, Clients);
        }
        public async void DeleteLineInMenu(string name)
        {
            await Clients.All.SendAsync("DeleteLine", 0, name);
        }
        public async void FirstImageDeck(string nameImage, byte[] Image, string namePlayed, string nameSecond)
        {
            new FirstImageDeckClass(nameImage, Image, namePlayed, nameSecond, Context, Clients);
        }
        public async void AgainPrsi(string name)
        {
            new AgainPrsiClass(name, Context, Clients);
        }
        public async Task PlayedPrsi(string namePlayed, string nameSecond, byte[]? image, string? nameImage, int takeCards)
        {
            new PlayedPrsiClass(namePlayed, nameSecond, image, nameImage, takeCards, Context, Clients);
        }
        public async void Threes(string namePlayed, string nameSecond)
        {
            new ThreesClass(namePlayed, nameSecond, Context, Clients);
        }
        public async void RedColour(string namePlayed, string nameSecond)
        {
            new RedColourClass(namePlayed, nameSecond, Context, Clients);
        }
        public async Task BlueColour(string namePlayed, string nameSecond)
        {
            new BlueColourClass(namePlayed, nameSecond, Context, Clients);
        }
        public async Task playedPiskvorky(string namePlayed, string nameSecond, byte[]? image, int index)
        {
            new playedPiskvorkyClass(namePlayed, nameSecond, image, index, Context, Clients);
        }
        public async Task PlayDama(string namePlayed, string nameSecond, byte[]? image, int moveTo)
        {
            new PlayDamaClass(namePlayed, nameSecond, image, moveTo, Context, Clients);
        }
        public async Task DelteFigureFromBoard(int index, string namePlayed, string nameSecond)
        {
            new DelteFigureFromBoardClass(index, namePlayed, nameSecond, Context, Clients);
        }
        public async Task EndGame(string namePlayed, string nameSecond)
        {
            new EndGameClass(namePlayed, nameSecond, Context, Clients);
        }
        public async Task DeleteFromPage1(string namePlayed, string? nameSecond, int indexPage)
        {
            new DeleteFromPage1Class(namePlayed, nameSecond, indexPage, Context, Clients);
        }
        public async Task RestartGame(string namePlayed, string nameSecond, int indexPage)
        {
            new RestartGameClass(namePlayed, nameSecond, indexPage, Context, Clients);
        }
        public async Task ResetGameConnection(string namePlayed, string? nameSecond)
        {
            new ResetGameConnectionClass(namePlayed, nameSecond, Context, Clients);
        }
        public async Task ShowEndPage(string namePlayed, string nameSecond)
        {
            new ShowEndPageClass(namePlayed, nameSecond, Context, Clients);
        }
        public async Task SendMessage(string namePlayed, string? nameSecond, string message)
        {
            new SendMessageClass(namePlayed, nameSecond, message, Context, Clients);
        }
        public async Task Pass(string namePlayed)
        {
            await Server._semaphore.WaitAsync();

            try { new Pass(namePlayed, Context, Clients);}

            finally {Server._semaphore.Release();}
        }
    }
}
