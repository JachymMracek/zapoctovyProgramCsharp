using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.AspNetCore.SignalR.Client;
using static zapoctovyProgramFinal.Form1;

namespace zapoctovyProgramFinal
{
    public class SignalR
    {
        public HubConnection Connection { get; }
        public SignalR() { Connection = new HubConnectionBuilder().WithUrl("https://localhost:7139/Hub").Build(); }
    }
    public class SignalCommands : UsesOfProject
    {
        public SignalCommands(Form1 project) : base(project) { }
        public void LineGameSet(string TypeOfGame, string nameOfSetter)
        {
            new LineOfTheGame(TypeOfGame, nameOfSetter, project);
        }
        public void DeleteLine(int index, string? nameDelete, string namePressed)
        {
            ((Page2)project.pages[1]).DeleteLabelesButtons(index);

        }
        public async Task OpponentConnected(string opponentName)
        {
            project.userInformation.nameOpponent = opponentName;

            await project.signalR.Connection.InvokeAsync("SetTimerInfo", project.userInformation.name, project.userInformation.nameOpponent, project.timeLeft, project.plusMove);
        }
        public void StartGameWithGreenButton(string gameType)
        {
            new ConnectedToGame(project).StartGreenButtonGame(gameType);
        }
        public void OpponentStartImage(byte[] image, string name)
        {
            new ConnectedToGame(project).StartGreenButtonGame("prší");
        }
        public void ChangeDeckStart(byte[] imageBytes, string nameDeck)
        {
            new MovePrsi(project).ChangeDeckNameImage(imageBytes, nameDeck);
        }
        public void ChangeDeck(byte[] imageBytes, string nameDeck, int takeCards)
        {
            ((Page7)project.pages[6]).prsiTable.cardTable.valid = true;

            ((Page7)project.pages[6]).prsiTable.cardTable.takeCards = takeCards;

            new MovePrsi(project).ChangeDeckNameImage(imageBytes, nameDeck);
        }
        public void TahnoutButtonPressed()
        {
            ((Page7)project.pages[6]).prsiTable.cardTable.valid = false;
            ((Page7)project.pages[6]).prsiTable.cardTable.takeCards = 1;
        }
        public void ChangePlay()
        {
            ((Page7)project.pages[6]).prsiTable.deckButton.firstMove = false;
            new MovePrsi(project).ChangePlay();
        }
        public void ChangePlayBoardGame()
        {
            if (((Page10)project.pages[9]).buttons[((Page10)project.pages[9]).buttons.Count - 1].BackColor == Color.LightCoral)
            {
                ((Page10)project.pages[9]).buttons[((Page10)project.pages[9]).buttons.Count - 1].BackColor = Color.LightGreen;
            }
            else
            {
                ((Page10)project.pages[9]).buttons[((Page10)project.pages[9]).buttons.Count - 1].BackColor = Color.LightCoral;
            }
        }
        public void EndPage()
        {
            project.pages[8].SeePage(8);
        }
        public void ZalozitPage()
        {
            project.userInformation.opakovat = 0;

            project.userInformation.make = false;

            project.pages[6] = new Page7(project);

            project.pages[9] = new Page10(project);

            project.pages[10] = new Page11(project);

            project.pages[1].SeePage(1);
        }
        public void EndGameByUser(int indexPage)
        {
            if (indexPage == 6) { project.pages[6] = new Page7(project); }

            else if (indexPage == 10) { project.pages[10] = new Page11(project); }

            else if (indexPage == 9) { project.pages[9] = new Page10(project); }

            project.pages[1].SeePage(1);
        }
        public void PiskvorkyPlayed(int index, byte[] image)
        {
            ((Page10)project.pages[9]).board.buttons[index].Image = ChangeImage(image);
            ((Page10)project.pages[9]).board.buttons[index].Name = image.Length.ToString();

            new CheckPiskvorkyWinner(project).FindFifts(index, image.Length.ToString());
        }
        private Image ChangeImage(byte[] imageBytes)
        {
            using (MemoryStream ms = new MemoryStream(imageBytes)) { return Image.FromStream(ms); }
        }
        public void ChooseColourPiskvorky()
        {
            Action newButtons = () =>
            {
                Button button1 = new Button { Location = new Point(850, 400), Size = new Size(100, 100), Name = "REDBUTTON", Image = Image.FromFile(@"C:\Users\Admin\Pictures\Screenshots\Snímek obrazovky 2024-08-19 000024.png") };

                Button button2 = new Button { Location = new Point(850, 550), Size = new Size(100, 100), Name = "", Image = Image.FromFile(@"C:\Users\Admin\Documents\ZAPOCTOVY_PROGRAM_Csharp\obrazky1\blueBird.png") };

                button1.Click += new EventHandler(new AddButtons(project).RedButtonPressed);

                button2.Click += new EventHandler(new AddButtons(project).BlueButtonPressed);

                this.project.Controls.Add(button1);

                this.project.Controls.Add(button2);
            };

            if (project.InvokeRequired) { project.Invoke(newButtons); }

            else { newButtons(); }
        }
        public void Blue()
        {
            ((Page10)project.pages[9]).board.image = Image.FromFile(@"C:\Users\Admin\Documents\ZAPOCTOVY_PROGRAM_Csharp\obrazky1\blueBird.png");

            project.pages[9].SeePage(9);
        }
        public void Red()
        {
            ((Page10)project.pages[9]).board.image = Image.FromFile(@"C:\Users\Admin\Pictures\Screenshots\Snímek obrazovky 2024-08-19 000024.png");

            project.pages[9].SeePage(9);
        }
        public void MoveFigure(int indexTo, byte[] imageData)
        {
            using (var ms = new MemoryStream(imageData)) { ((Page11)project.pages[10]).boardFigure.buttons[63 - indexTo].Image = Image.FromStream(ms); }

                ((Page11)project.pages[10]).boardFigure.buttons[63 - indexTo].Name = "";
        }
        public void DelteFigureFromBoard(int index)
        {
            ((Page11)project.pages[10]).boardFigure.buttons[63 - index].Image = null;
            ((Page11)project.pages[10]).boardFigure.buttons[63 - index].Name = "";
            ((Page11)project.pages[10]).boardFigure.buttons[63 - index].Tag = "";

            project.pages[10].SeePage(10);
        }
        public void ChangeDamaPlay(string textTime)
        {
            Action changeDama = () => { ((Page11)project.pages[10]).OpponentTimeButton.Text = textTime; project.timer1.Start(); ((Page11)project.pages[10]).turnButton.BackColor = Color.LightGreen; };

            if (project.InvokeRequired) { project.Invoke(changeDama); }

            else { changeDama(); }

            if (textTime == "0:0") { EndGameDama(); return; }
        }
        private async Task EndGameDama()
        {
            await project.signalR.Connection.InvokeAsync("EndGame");
        }
        public void SetTimerInfo(int timeLeftSignal, int plusMoveSignal)
        {
            project.timeLeft = timeLeftSignal;

            project.userInformation.timeSet = project.timeLeft;

            project.plusMove = plusMoveSignal;

            if (!(project.timeLeft == 0 && project.plusMove == 0)) { project.classical = false; }

            else { project.classical = true; }
        }
        public void Message(string message)
        {
            if (project.InvokeRequired) { project.Invoke(new Action<string>(Message), message); return; }

            var labelToRemove = project.Controls.OfType<Label>().FirstOrDefault(lbl => lbl.Name == "message");

            if (labelToRemove != null) { project.Controls.Remove(labelToRemove); }

            project.Controls.Add(new Label { Text = message, Size = new Size(150, 20), Location = new Point(85, 225), Font = new Font(FontFamily.GenericSansSerif, 10), Name = "message" });
        }
        public void DeleteCancelGame(string name)
        {
            foreach (LineOfTheGame lineOfTheGame in ((Page2)project.pages[1]).lineOfTheGames)
            {
                if (lineOfTheGame.typeGamelabels[1].Text == name)
                {
                    ((Page2)project.pages[1]).DeleteLabelesButtons(((Page2)project.pages[1]).lineOfTheGames.IndexOf(lineOfTheGame));

                    break;
                }
            }
        }
        public void RestartGame(int indexPage)
        {
            project.userInformation.opakovat++;

            if (project.userInformation.opakovat != 2) { return; }

            else if (indexPage == 6) { Page6Restart(indexPage); }

            else if (indexPage == 10) { Page10Restart(indexPage); }

            else if (indexPage == 9) { Page9Restart(indexPage); }

        }
        private void Page6Restart(int indexPage)
        {
            project.pages[indexPage] = new Page7(project); project.pages[indexPage].SeePage(indexPage); project.userInformation.opakovat = 0;

            if (project.userInformation.make) { new ConnectedToGame(project).StartGreenButtonGame("prší"); project.userInformation.make = true; }
        }
        private void Page10Restart(int indexPage)
        {
            project.pages[indexPage] = new Page11(project); project.pages[indexPage].SeePage(indexPage); project.userInformation.opakovat = 0;

            if (project.userInformation.timeSet != 0) { project.timeLeft = project.userInformation.timeSet; }

            if (project.userInformation.make)
            {
                new ConnectedToGame(project).StartGreenButtonGame("dáma"); project.userInformation.make = true;

                foreach (Button button in ((Page11)project.pages[10]).boardFigure.buttons)
                {
                    if (button.Name == "whiteFigure") { button.Image = Image.FromFile(@"C:\Users\Admin\Documents\ZAPOCTOVY_PROGRAM_Csharp\obrazky1\blueBird.png"); }

                    else if (button.Name == "blackFigure") { button.Image = Image.FromFile(@"C:\Users\Admin\Pictures\Screenshots\Snímek obrazovky 2024-08-19 000024.png"); }
                }
            }
        }
        private void Page9Restart(int indexPage)
        {
            project.tournamentPiskvorky = new TournamentPiskvorky();

            project.pages[indexPage] = new Page10(project); project.pages[indexPage].SeePage(indexPage); project.userInformation.opakovat = 0;

            if (project.userInformation.make)
            {
                if (project.userInformation.threes) { project.tournamentPiskvorky.ThreeStartMoves(); project.userInformation.threes = true; }

                else { project.userInformation.threes = false; }

                new ConnectedToGame(project).StartGreenButtonGame("piškvorky");
            }
        }
        public void Pass(bool notTaken, string nameWritten)
        {
            if (!notTaken) { return; }

            project.userInformation.name = nameWritten;

            Action action = () =>
            {
                TextBox textBox = project.Controls.OfType<TextBox>().FirstOrDefault();

                if (textBox != null) { project.Controls.Remove(textBox); }

                project.pages[0].SeePage(0);
            };

            if (project.InvokeRequired) { project.Invoke(action);}

            else { action();}
        }

    }
}
