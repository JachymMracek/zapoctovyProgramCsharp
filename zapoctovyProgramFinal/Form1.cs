using Microsoft.Win32;
using Microsoft.AspNetCore.SignalR.Client;
using System.ComponentModel.DataAnnotations;
using System;
using System.IO;
using static zapoctovyProgramFinal.Form1;
using System.Security.AccessControl;
using System.ComponentModel.Design;
using System.DirectoryServices;
using System.Reflection;
using System.Windows.Forms;
using System.Security;
using System.Security.Permissions;
using System.Text.Json.Serialization.Metadata;
using System.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;
using System.Reflection.Metadata.Ecma335;

//

namespace zapoctovyProgramFinal
{
    public partial class Form1 : Form
    {
        public UserInformation userInformation = new UserInformation();

        public SignalR signalR = new SignalR();

        public List<Page> pages = new List<Page>();

        public SignalCommands signalCommands;

        public System.Windows.Forms.Timer timer1 = new System.Windows.Forms.Timer();

        public int timeLeft = 0;
        public int plusMove = 0;
        public bool classical = true;

        public TournamentPiskvorky tournamentPiskvorky = new TournamentPiskvorky();
        public Form1()
        {
            InitializeComponent();
            ServerRecieve();

            signalCommands = new SignalCommands(this);

            Page1 page1 = new Page1(this);
            Page2 page2 = new Page2(this);
            Page3 page3 = new Page3(this);
            Page4 page4 = new Page4(this);
            Page5 page5 = new Page5(this);
            Page6 page6 = new Page6(this);
            Page7 page7 = new Page7(this);
            Page8 page8 = new Page8(this);
            Page9 page9 = new Page9(this);
            Page10 page10 = new Page10(this);
            Page11 page11 = new Page11(this);

            pages.Add(page1);
            pages.Add(page2);
            pages.Add(page3);
            pages.Add(page4);
            pages.Add(page5);
            pages.Add(page6);
            pages.Add(page7);
            pages.Add(page8);
            pages.Add(page9);
            pages.Add(page10);
            pages.Add(page11);

            timer1.Interval = 1000;
            timer1.Tick += Timer1_Tick;

            new Page0(this);
        }
        public async void Timer1_Tick(object sender, EventArgs e)
        {
            if (((Page11)pages[10]).turnButton.BackColor != Color.LightGreen) { return; }

            if (timeLeft > 0) { timeLeft--; ((Page11)this.pages[10]).myTimeButton.Text = $"{timeLeft / 60}:{timeLeft % 60}"; }

            else { if (!classical) { await signalR.Connection.InvokeAsync("EndGame", userInformation.name, userInformation.nameOpponent); timer1.Stop(); } }
        }
        private void ServerRecieve()
        {
            signalR.Connection.StartAsync().ContinueWith(task =>
            { });
            signalR.Connection.On<string>("DeleteCancelGame", (name) =>
            {
                signalCommands.DeleteCancelGame(name);
            });
            signalR.Connection.On<int>("RestartGame", (indexPage) =>
            {
                signalCommands.RestartGame(indexPage);
            });
            signalR.Connection.On<string, string>("LineGameSet", (TypeOfGame, nameOfSetter) =>
            {
                signalCommands.LineGameSet(TypeOfGame, nameOfSetter);
            });
            signalR.Connection.On<int, string?, string>("DeleteLine", (index, nameDelete, namePressed) =>
            {
                signalCommands.DeleteLine(index, nameDelete, namePressed);
            });
            signalR.Connection.On<string>("OpponentConnected", (opponentName) =>
            {
                signalCommands.OpponentConnected(opponentName);
            });
            signalR.Connection.On<string>("startGameWithGreenButton", (gameType) =>
            {
                signalCommands.StartGameWithGreenButton(gameType);
            });
            signalR.Connection.On<byte[], string>("opponentStartImage", (image, name) =>
            {
                signalCommands.OpponentStartImage(image, name);
            });
            signalR.Connection.On<byte[], string>("ChangeDeckStart", (imageBytes, nameDeck) =>
            {
                signalCommands.ChangeDeckStart(imageBytes, nameDeck);
            });
            signalR.Connection.On<byte[], string, int>("ChangeDeck", (imageBytes, nameDeck, takeCards) =>
            {
                signalCommands.ChangeDeck(imageBytes, nameDeck, takeCards);
            });
            signalR.Connection.On("TahnoutButtonPressed", () =>
            {
                signalCommands.TahnoutButtonPressed();
            });
            signalR.Connection.On("ChangePlay", () =>
            {
                signalCommands.ChangePlay();
            });
            signalR.Connection.On("ChangePlayBoardGame", () =>
            {
                signalCommands.ChangePlayBoardGame();
            });
            signalR.Connection.On("EndPage", () =>
            {
                signalCommands.EndPage();
            });
            signalR.Connection.On("ZalozitPage", () =>
            {
                signalCommands.ZalozitPage();
            });
            signalR.Connection.On<int>("EndGameByUser", (indexPage) =>
            {
                signalCommands.EndGameByUser(indexPage);
            });
            signalR.Connection.On<int, byte[]>("PiskvorkyPlayed", (index, image) =>
            {
                signalCommands.PiskvorkyPlayed(index, image);
            });
            signalR.Connection.On("ChooseColourPiskvorky", () =>
            {
                signalCommands.ChooseColourPiskvorky();
            });
            signalR.Connection.On("Blue", () =>
            {
                signalCommands.Blue();
            });
            signalR.Connection.On("Red", () =>
            {
                signalCommands.Red();
            });
            signalR.Connection.On<int, byte[]>("MoveFigure", (indexTo, imageData) =>
            {
                signalCommands.MoveFigure(indexTo, imageData);
            });
            signalR.Connection.On<int>("DelteFigureFromBoard", (index) =>
            {
                signalCommands.DelteFigureFromBoard(index);
            });
            signalR.Connection.On<string>("ChangeDamaPlay", (textTime) =>
            {
                signalCommands.ChangeDamaPlay(textTime);
            });
            signalR.Connection.On<int, int>("SetTimerInfo", (timeLeftSignal, plusMoveSignal) =>
            {
                signalCommands.SetTimerInfo(timeLeftSignal, plusMoveSignal);
            });
            signalR.Connection.On<string>("Message", (message) =>
            {
                signalCommands.Message(message);
            });
            signalR.Connection.On<bool, string>("Pass", (notTaken, namePlayed) =>
            {
                signalCommands.Pass(notTaken, namePlayed);
            });
        }
    }
}
