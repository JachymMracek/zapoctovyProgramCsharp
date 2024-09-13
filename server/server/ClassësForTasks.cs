using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Server
{
    public class PLayerInformation
    {
        public string? SetterOfGameKey = null;

        public string? value = null;

        public bool notNull = false;

        public HubCallerContext Context;

        public IHubCallerClients Clients;
        protected PLayerInformation(string nameSetter, HubCallerContext context, IHubCallerClients Clients)
        {
            SetterOfGameKey = Server.tables.Keys.FirstOrDefault(key => key.Contains(nameSetter));

            if (SetterOfGameKey != null) { notNull = true; }

            this.Context = context;

            this.Clients = Clients;
        }
        public void ChangingName(ref string? SetterOfGameKey, ref string? value, string namePlayed, string nameSecond)
        {
            SetterOfGameKey = new IPchange().ChangeIdAdress(SetterOfGameKey, namePlayed, nameSecond);

            value = new IPchange().ChangeIdAdress(value, namePlayed, nameSecond);
        }
    }
    public class SetGameClass : PLayerInformation
    {
        public SetGameClass(string nameSetter, HubCallerContext Context, IHubCallerClients Clients, string typeOfGame) : base(nameSetter, Context, Clients)
        {
            SetGame(nameSetter, typeOfGame);
        }
        public async Task SetGame(string nameSetter, string typeOfGame)
        {
            Server.tables[Context.ConnectionId + nameSetter] = "";

            await Clients.All.SendAsync("LineGameSet", typeOfGame, nameSetter);
        }
    }
    public class GreenPressedClass : PLayerInformation
    {
        public GreenPressedClass(string nameSetter, HubCallerContext Context, IHubCallerClients Clients, string typeOfGame, string namePressed, int index) : base(nameSetter, Context, Clients)
        {
            GreenPressed(namePressed, nameSetter, index, typeOfGame);
        }
        public async Task GreenPressed(string namePressed, string nameSetter, int index, string gameType)
        {
            Server.tables[SetterOfGameKey] = Context.ConnectionId + namePressed;

            SetterOfGameKey = new IPchange().ChangeIdAdress(SetterOfGameKey, namePressed, nameSetter);

            await Clients.All.SendAsync("DeleteLine", index, nameSetter, namePressed);

            await Clients.Clients(SetterOfGameKey).SendAsync("OpponentConnected", namePressed);

            await Clients.Clients(SetterOfGameKey).SendAsync("startGameWithGreenButton", gameType);

            await Clients.Caller.SendAsync("OpponentConnected", nameSetter);
        }
    }

    public class ChangeDamaPlayClass : PLayerInformation
    {
        public ChangeDamaPlayClass(string namePlayed, string nameSecond, string time, HubCallerContext Context, IHubCallerClients Clients) : base(namePlayed, Context, Clients)
        {
            ChangeDamaPlay(namePlayed, nameSecond, time);
        }
        private async Task NotNullChangeDama(string namePlayed, string nameSecond, string time)
        {
            value = Server.tables[SetterOfGameKey];

            ChangingName(ref SetterOfGameKey, ref value, namePlayed, nameSecond);

            await Clients.Client(value).SendAsync("ChangeDamaPlay", time);
        }
        private async Task NullChangeDama(string namePlayed, string nameSecond, string time)
        {
            SetterOfGameKey = Server.tables.Keys.FirstOrDefault(key => key.Contains(nameSecond));

            value = Server.tables[SetterOfGameKey];

            ChangingName(ref SetterOfGameKey, ref value, namePlayed, nameSecond);

            await Clients.Client(SetterOfGameKey).SendAsync("ChangeDamaPlay", time);
        }
        public void ChangeDamaPlay(string namePlayed, string nameSecond, string time)
        {
            if (notNull) { NotNullChangeDama(namePlayed, nameSecond, time); }

            else { NullChangeDama(namePlayed, nameSecond, time); }
        }
    }

    public class SetTimerInfoClass : PLayerInformation
    {
        public SetTimerInfoClass(string namePlayed, string nameSecond, int timeLeft, int plusMove, HubCallerContext Context, IHubCallerClients Clients) : base(namePlayed, Context, Clients)
        {
            SetTimerInfo(namePlayed, nameSecond, timeLeft, plusMove);
        }
        public async Task SetTimerInfo(string namePlayed, string nameSecond, int timeLeft, int plusMove)
        {
            string? SetterOfGameKey = Server.tables.Keys.FirstOrDefault(key => key.Contains(namePlayed));

            string? value = null;

            if (SetterOfGameKey != null)
            {
                value = Server.tables[SetterOfGameKey];

                ChangingName(ref SetterOfGameKey, ref value, namePlayed, nameSecond);

                await Clients.Client(value).SendAsync("SetTimerInfo", timeLeft, plusMove);
            }
        }
    }
    public class FirstImageDeckClass : PLayerInformation
    {
        public FirstImageDeckClass(string nameImage, byte[] Image, string namePlayed, string nameSecond, HubCallerContext Context, IHubCallerClients Clients) : base(namePlayed, Context, Clients)
        {
            FirstImageDeck(nameImage, Image, namePlayed, nameSecond);
        }
        public async Task FirstImageDeck(string nameImage, byte[] Image, string namePlayed, string nameSecond)
        {
            string? SetterOfGameKey = Server.tables.Keys.FirstOrDefault(key => key.Contains(namePlayed));

            string? value = Server.tables[SetterOfGameKey];

            value = new IPchange().ChangeIdAdress(value, namePlayed, nameSecond);

            await Clients.Client(value).SendAsync("ChangeDeckStart", Image, nameImage);
        }
    }
    public class AgainPrsiClass : PLayerInformation
    {
        public AgainPrsiClass(string name, HubCallerContext Context, IHubCallerClients Clients) : base(name, Context, Clients)
        {
            AgainPrsi(name);
        }
        public async Task AgainPrsi(string name)
        {
            string? SetterOfGameKey = Server.tables.Keys.FirstOrDefault(key => key.Contains(name));

            if (SetterOfGameKey == null) return;

            string nameW = Server.tables[SetterOfGameKey];

            nameW = new IPchange().ChangeIdAdress(nameW, name, name);

            await Clients.Caller.SendAsync("GreenButton");

            await Clients.Caller.SendAsync("OpponentConnected", nameW);
        }
    }
    public class PlayedPrsiClass : PLayerInformation
    {
        public PlayedPrsiClass(string namePlayed, string nameSecond, byte[]? image, string? nameImage, int takeCards, HubCallerContext Context, IHubCallerClients Clients) : base(namePlayed, Context, Clients)
        {
            PlayedPrsi(namePlayed, nameSecond, image, nameImage, takeCards);
        }
        public async Task PlayedPrsi(string namePlayed, string nameSecond, byte[]? image, string? nameImage, int takeCards)
        {
            if (SetterOfGameKey != null)
            {
                value = Server.tables[SetterOfGameKey];

                ChangingName(ref SetterOfGameKey, ref value, namePlayed, nameSecond);
            }
            else
            {
                SetterOfGameKey = Server.tables.Keys.FirstOrDefault(key => key.Contains(nameSecond));

                value = Server.tables[SetterOfGameKey];

                ChangingName(ref SetterOfGameKey, ref value, namePlayed, nameSecond);
            }

            if (image != null)
            {
                await Clients.Client(value).SendAsync("ChangeDeck", image, nameImage, takeCards);

                await Clients.Client(SetterOfGameKey).SendAsync("ChangeDeck", image, nameImage, takeCards);
            }
            else
            {
                await Clients.Client(value).SendAsync("TahnoutButtonPressed");

                await Clients.Client(SetterOfGameKey).SendAsync("TahnoutButtonPressed");
            }

            await Clients.Client(SetterOfGameKey).SendAsync("ChangePlay");

            await Clients.Client(value).SendAsync("ChangePlay");
        }
    }
    public class ThreesClass : PLayerInformation
    {
        public ThreesClass(string namePlayed, string nameSecond, HubCallerContext Context, IHubCallerClients Clients) : base(namePlayed, Context, Clients)
        {
            Threes(namePlayed, nameSecond);
        }
        public async Task Threes(string namePlayed, string nameSecond)
        {
            string? SetterOfGameKey = Server.tables.Keys.FirstOrDefault(key => key.Contains(namePlayed));

            string? value = null;

            if (SetterOfGameKey != null)
            {
                value = Server.tables[SetterOfGameKey];

                ChangingName(ref SetterOfGameKey, ref value, namePlayed, nameSecond);
            }
            else
            {
                SetterOfGameKey = Server.tables.Keys.FirstOrDefault(key => key.Contains(nameSecond));

                value = Server.tables[SetterOfGameKey];

                ChangingName(ref SetterOfGameKey, ref value, namePlayed, nameSecond);
            }

            await Clients.Client(value).SendAsync("ChooseColourPiskvorky");
        }
    }
    public class RedColourClass : PLayerInformation
    {
        public RedColourClass(string namePlayed, string nameSecond, HubCallerContext Context, IHubCallerClients Clients) : base(namePlayed, Context, Clients)
        {
            RedColour(namePlayed, nameSecond);
        }
        public async Task RedColour(string namePlayed, string nameSecond)
        {
            string? SetterOfGameKey = Server.tables.Keys.FirstOrDefault(key => key.Contains(namePlayed));

            string? value = null;

            if (SetterOfGameKey != null)
            {
                value = Server.tables[SetterOfGameKey];

                ChangingName(ref SetterOfGameKey, ref value, namePlayed, nameSecond);
            }
            else
            {
                SetterOfGameKey = Server.tables.Keys.FirstOrDefault(key => key.Contains(nameSecond));

                value = Server.tables[SetterOfGameKey];

                ChangingName(ref SetterOfGameKey, ref value, namePlayed, nameSecond);
            }

            await Clients.Client(SetterOfGameKey).SendAsync("ChangePlayBoardGame");

            await Clients.Client(value).SendAsync("ChangePlayBoardGame");

            await Clients.Client(value).SendAsync("Red");

            await Clients.Client(SetterOfGameKey).SendAsync("Blue");
        }
    }
    public class BlueColourClass : PLayerInformation
    {
        public BlueColourClass(string namePlayed, string nameSecond, HubCallerContext Context, IHubCallerClients Clients) : base(namePlayed, Context, Clients)
        {
            BlueColour(namePlayed, nameSecond);
        }
        public async Task BlueColour(string namePlayed, string nameSecond)
        {
            if (SetterOfGameKey != null)
            {
                value = Server.tables[SetterOfGameKey];

                ChangingName(ref SetterOfGameKey, ref value, namePlayed, nameSecond);
            }
            else
            {
                SetterOfGameKey = Server.tables.Keys.FirstOrDefault(key => key.Contains(nameSecond));

                value = Server.tables[SetterOfGameKey];

                SetterOfGameKey = new IPchange().ChangeIdAdress(SetterOfGameKey, namePlayed, nameSecond);

                value = new IPchange().ChangeIdAdress(value, namePlayed, nameSecond);
            }

            await Clients.Client(value).SendAsync("Blue");

            await Clients.Client(SetterOfGameKey).SendAsync("Red");
        }
    }
    public class playedPiskvorkyClass : PLayerInformation
    {
        public playedPiskvorkyClass(string namePlayed, string nameSecond, byte[]? image, int index, HubCallerContext Context, IHubCallerClients Clients) : base(namePlayed, Context, Clients)
        {
            playedPiskvorky(namePlayed, nameSecond, image, index);
        }
        public async Task playedPiskvorky(string namePlayed, string nameSecond, byte[]? image, int index)
        {
            string? SetterOfGameKey = Server.tables.Keys.FirstOrDefault(key => key.Contains(namePlayed));

            string? value = null;

            if (SetterOfGameKey != null)
            {
                value = Server.tables[SetterOfGameKey];

                ChangingName(ref SetterOfGameKey, ref value, namePlayed, nameSecond);
            }
            else
            {
                SetterOfGameKey = Server.tables.Keys.FirstOrDefault(key => key.Contains(nameSecond));

                value = Server.tables[SetterOfGameKey];

                ChangingName(ref SetterOfGameKey, ref value, namePlayed, nameSecond);
            }

            await Clients.Client(value).SendAsync("PiskvorkyPlayed", index, image);

            await Clients.Client(SetterOfGameKey).SendAsync("PiskvorkyPlayed", index, image);

            await Clients.Client(SetterOfGameKey).SendAsync("ChangePlayBoardGame");

            await Clients.Client(value).SendAsync("ChangePlayBoardGame");
        }
    }
    public class PlayDamaClass : PLayerInformation
    {
        public PlayDamaClass(string namePlayed, string nameSecond, byte[]? image, int moveTo, HubCallerContext Context, IHubCallerClients Clients) : base(namePlayed, Context, Clients)
        {
            PlayDama(namePlayed, nameSecond, image, moveTo);
        }
        public async Task PlayDama(string namePlayed, string nameSecond, byte[]? image, int moveTo)
        {
            string? SetterOfGameKey = Server.tables.Keys.FirstOrDefault(key => key.Contains(namePlayed));

            string? value = null;

            if (SetterOfGameKey != null)
            {
                value = Server.tables[SetterOfGameKey];

                ChangingName(ref SetterOfGameKey, ref value, namePlayed, nameSecond);

                await Clients.Client(value).SendAsync("MoveFigure", moveTo, image);
            }
            else if (SetterOfGameKey == null)
            {
                SetterOfGameKey = Server.tables.Keys.FirstOrDefault(key => key.Contains(nameSecond));

                value = Server.tables[SetterOfGameKey];

                ChangingName(ref SetterOfGameKey, ref value, namePlayed, nameSecond);

                await Clients.Client(SetterOfGameKey).SendAsync("MoveFigure", moveTo, image);
            }
        }
    }
    public class DelteFigureFromBoardClass : PLayerInformation
    {
        public DelteFigureFromBoardClass(int index, string namePlayed, string nameSecond, HubCallerContext Context, IHubCallerClients Clients) : base(namePlayed, Context, Clients)
        {
            DelteFigureFromBoard(index, namePlayed, nameSecond);
        }
        public async Task DelteFigureFromBoard(int index, string namePlayed, string nameSecond)
        {
            string? SetterOfGameKey = Server.tables.Keys.FirstOrDefault(key => key.Contains(namePlayed));

            string? value = null;

            if (SetterOfGameKey != null)
            {
                value = Server.tables[SetterOfGameKey];

                ChangingName(ref SetterOfGameKey, ref value, namePlayed, nameSecond);

                await Clients.Client(value).SendAsync("DelteFigureFromBoard", index);
            }
            else
            {
                SetterOfGameKey = Server.tables.Keys.FirstOrDefault(key => key.Contains(nameSecond));

                value = Server.tables[SetterOfGameKey];

                ChangingName(ref SetterOfGameKey, ref value, namePlayed, nameSecond);

                await Clients.Client(SetterOfGameKey).SendAsync("DelteFigureFromBoard", index);
            }
        }
    }
    public class EndGameClass : PLayerInformation
    {
        public EndGameClass(string namePlayed, string nameSecond, HubCallerContext Context, IHubCallerClients Clients) : base(namePlayed, Context, Clients)
        {
            EndGame(namePlayed, nameSecond);
        }
        public async Task EndGame(string namePlayed, string nameSecond)
        {
            string? SetterOfGameKey = Server.tables.Keys.FirstOrDefault(key => key.Contains(namePlayed));

            string? value = null;

            if (SetterOfGameKey != null)
            {
                value = Server.tables[SetterOfGameKey];

                ChangingName(ref SetterOfGameKey, ref value, namePlayed, nameSecond);
            }
            else if (SetterOfGameKey == null)
            {
                SetterOfGameKey = Server.tables.Keys.FirstOrDefault(key => key.Contains(nameSecond));

                value = Server.tables[SetterOfGameKey];

                ChangingName(ref SetterOfGameKey, ref value, namePlayed, nameSecond);
            }

            await Clients.Client(SetterOfGameKey).SendAsync("EndPage");

            await Clients.Client(value).SendAsync("EndPage");
        }
    }
    public class DeleteFromPage1Class : PLayerInformation
    {
        public DeleteFromPage1Class(string namePlayed, string? nameSecond, int indexPage, HubCallerContext Context, IHubCallerClients Clients) : base(namePlayed, Context, Clients)
        {
            DeleteFromPage1(namePlayed, nameSecond, indexPage);
        }
        public async Task DeleteFromPage1(string namePlayed, string? nameSecond, int indexPage)
        {
            await Clients.All.SendAsync("DeleteCancelGame", namePlayed);

            if (SetterOfGameKey != null)
            {
                value = Server.tables[SetterOfGameKey];
                Server.tables.TryRemove(SetterOfGameKey, out _);
                ChangingName(ref SetterOfGameKey, ref value, namePlayed, nameSecond);
            }
            else if (SetterOfGameKey == null)
            {
                SetterOfGameKey = Server.tables.Keys.FirstOrDefault(key => key.Contains(nameSecond));

                value = Server.tables[SetterOfGameKey];

                Server.tables.TryRemove(SetterOfGameKey, out _);

                ChangingName(ref SetterOfGameKey, ref value, namePlayed, nameSecond);
            }
            await Clients.Client(SetterOfGameKey).SendAsync("EndGameByUser", indexPage);

            await Clients.Client(value).SendAsync("EndGameByUser", indexPage);
        }
    }
    public class RestartGameClass : PLayerInformation
    {
        public RestartGameClass(string namePlayed, string nameSecond, int indexPage, HubCallerContext Context, IHubCallerClients Clients) : base(namePlayed, Context, Clients)
        {
            RestartGame(namePlayed, nameSecond, indexPage);
        }
        public async Task RestartGame(string namePlayed, string nameSecond, int indexPage)
        {
            string? SetterOfGameKey = Server.tables.Keys.FirstOrDefault(key => key.Contains(namePlayed));

            string? value = null;

            if (SetterOfGameKey != null)
            {
                value = Server.tables[SetterOfGameKey];

                ChangingName(ref SetterOfGameKey, ref value, namePlayed, nameSecond);
            }
            else if (SetterOfGameKey == null)
            {
                SetterOfGameKey = Server.tables.Keys.FirstOrDefault(key => key.Contains(nameSecond));

                value = Server.tables[SetterOfGameKey];

                ChangingName(ref SetterOfGameKey, ref value, namePlayed, nameSecond);
            }

            await Clients.Client(SetterOfGameKey).SendAsync("RestartGame", indexPage);

            await Clients.Client(value).SendAsync("RestartGame", indexPage);
        }
    }
    public class ResetGameConnectionClass : PLayerInformation
    {
        public ResetGameConnectionClass(string namePlayed, string? nameSecond, HubCallerContext Context, IHubCallerClients Clients) : base(namePlayed, Context, Clients)
        {
            ResetGameConnection(namePlayed, nameSecond);
        }
        public async Task ResetGameConnection(string namePlayed, string? nameSecond)
        {
            if (SetterOfGameKey != null)
            {
                value = Server.tables[SetterOfGameKey];
                Server.tables.TryRemove(SetterOfGameKey, out _);
                ChangingName(ref SetterOfGameKey, ref value, namePlayed, nameSecond);
            }
            else if (SetterOfGameKey == null)
            {
                SetterOfGameKey = Server.tables.Keys.FirstOrDefault(key => key.Contains(nameSecond));

                value = Server.tables[SetterOfGameKey];

                Server.tables.TryRemove(SetterOfGameKey, out _);

                ChangingName(ref SetterOfGameKey, ref value, namePlayed, nameSecond);
            }

            await Clients.Client(SetterOfGameKey).SendAsync("ZalozitPage");

            await Clients.Client(value).SendAsync("ZalozitPage");
        }
    }
    public class ShowEndPageClass : PLayerInformation
    {
        public ShowEndPageClass(string namePlayed, string nameSecond, HubCallerContext Context, IHubCallerClients Clients) : base(namePlayed, Context, Clients)
        {
            ShowEndPage(namePlayed, nameSecond);
        }
        public async Task ShowEndPage(string namePlayed, string nameSecond)
        {
            if (SetterOfGameKey != null)
            {
                value = Server.tables[SetterOfGameKey];

                ChangingName(ref SetterOfGameKey, ref value, namePlayed, nameSecond);

                await Clients.Client(SetterOfGameKey).SendAsync("EndPage");

                await Clients.Client(value).SendAsync("EndPage");

                return;
            }

            SetterOfGameKey = Server.tables.Keys.FirstOrDefault(key => key.Contains(nameSecond));

            if (SetterOfGameKey != null)
            {
                value = Server.tables[SetterOfGameKey];

                ChangingName(ref SetterOfGameKey, ref value, namePlayed, nameSecond);

                await Clients.Client(SetterOfGameKey).SendAsync("EndPage");

                await Clients.Client(value).SendAsync("EndPage");
            }
        }
    }
    public class SendMessageClass : PLayerInformation
    {
        public SendMessageClass(string namePlayed, string? nameSecond, string message, HubCallerContext Context, IHubCallerClients Clients) : base(namePlayed, Context, Clients)
        {
            SendMessage(namePlayed, nameSecond, message);
        }
        public async Task SendMessage(string namePlayed, string? nameSecond, string message)
        {
            string? SetterOfGameKey = Server.tables.Keys.FirstOrDefault(key => key.Contains(namePlayed));

            string? value = null;

            if (SetterOfGameKey != null)
            {
                value = Server.tables[SetterOfGameKey];

                ChangingName(ref SetterOfGameKey, ref value, namePlayed, nameSecond);
            }
            else if (SetterOfGameKey == null)
            {
                SetterOfGameKey = Server.tables.Keys.FirstOrDefault(key => key.Contains(nameSecond));

                value = Server.tables[SetterOfGameKey];

                ChangingName(ref SetterOfGameKey, ref value, namePlayed, nameSecond);
            }

            await Clients.Client(SetterOfGameKey).SendAsync("Message", message);

            await Clients.Client(value).SendAsync("Message", message);
        }
    }
    public class Pass : PLayerInformation
    {
        public Pass(string namePlayed,HubCallerContext Context, IHubCallerClients Clients) : base(namePlayed, Context, Clients)
        {
            NameSave(namePlayed);
        }
        public async Task NameSave(string namePlayed)
        {
            if (Server.nameServer.Contains(namePlayed)) { ; await Clients.Caller.SendAsync("Pass", false,namePlayed); }

            else { Server.nameServer.Add(namePlayed); await Clients.Caller.SendAsync("Pass", true,namePlayed); }
        }
    }
    public class IPchange
    {
        public string ChangeIdAdress(string Id, string? name1, string? name2)
        {
            if (name1 != null && Id.EndsWith(name1)) { Id = Id.Substring(0, Id.Length - name1.Length); }

            if (name2 != null && Id.EndsWith(name2)) { Id = Id.Substring(0, Id.Length - name2.Length); }

            return Id;
        }
    }
}
