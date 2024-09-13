using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using static zapoctovyProgramFinal.Form1;

namespace zapoctovyProgramFinal
{
    public class GreenButton : UsesOfProject
    {
        public Button button { get; set; }
        public GreenButton(Form1 project) : base(project)
        {
            button = new Button { Size = new Size(20, 20), BackColor = Color.Green, Location = new Point(700, 225) };

            button.Click += new EventHandler(GreenButton_Click);
        }
        private async void GreenButton_Click(object sender, EventArgs e)
        {
            int i = 0;

            foreach (LineOfTheGame lineOfTheGame in ((Page2)project.pages[1]).lineOfTheGames)
            {
                if (lineOfTheGame.greenButtons.button == button) { break; }

                i++;
            }

            await new ConnectedToGame(project).ConnectToGame(project.userInformation.name, ((Page2)project.pages[1]).lineOfTheGames[i].typeGamelabels[1].Text, i, ((Page2)project.pages[1]).lineOfTheGames[i].typeGamelabels[0].Text);
        }
    }
    public class LineOfTheGame
    {
        public GreenButton greenButtons { get; set; }
        public List<Label> typeGamelabels = new List<Label>();
        public LineOfTheGame(string TypeOfGame, string nameOfSetter, Form1 project)
        {
            greenButtons = new GreenButton(project);

            typeGamelabels.Add(new Label { Text = TypeOfGame, Size = new Size(150, 20), Location = new Point(85, 225), Font = new Font(FontFamily.GenericSansSerif, 10) });

            typeGamelabels.Add(new Label { Text = nameOfSetter, Size = new Size(150, 20), Location = new Point(350, 225), Font = new Font(FontFamily.GenericSansSerif, 10) });

            ((Page2)project.pages[1]).AddToButtonsAndLabels(this);
        }
    }
    public class AddButtons : UsesOfProject
    {
        public AddButtons(Form1 project) : base(project) { }
        private void DeleteChooseButtons()
        {
            project.Controls.RemoveAt(project.Controls.Count - 1);
            project.Controls.RemoveAt(project.Controls.Count - 1);
        }
        public async void RedButtonPressed(object sender, EventArgs e)
        {
            DeleteChooseButtons();

            await project.signalR.Connection.InvokeAsync("RedColour", project.userInformation.name, project.userInformation.nameOpponent);
        }
        public async void BlueButtonPressed(object sender, EventArgs e)
        {
            DeleteChooseButtons();

            await project.signalR.Connection.InvokeAsync("BlueColour", project.userInformation.name, project.userInformation.nameOpponent);
        }
    }
    public class ConnectedToGame : UsesOfProject
    {
        public ConnectedToGame(Form1 project) : base(project) { }
        public async Task ConnectToGame(string namePressed, string nameSetter, int index, string gameType)
        {
            await project.signalR.Connection.InvokeAsync("GreenPressed", namePressed, nameSetter, index, gameType);

            if (gameType == "prší") { project.pages[6].SeePage(6); project.userInformation.GameTypePage = 6; }

            else if (gameType == "piškvorky") { project.pages[9].SeePage(9); project.userInformation.GameTypePage = 9; }

            else if (gameType == "dáma") { project.pages[10].SeePage(10); project.userInformation.GameTypePage = 10; }
        }
        public void StartGreenButtonGame(string gameType)
        {
            if (gameType == "prší")
            {
                ((Page7)project.pages[6]).prsiTable.turnButton.BackColor = Color.LightGreen;

                ((Page7)project.pages[6]).prsiTable.cardTable.button.Image = ((Page7)project.pages[6]).prsiTable.cardsInDeck[0].ButtonCard.Image;

                ((Page7)project.pages[6]).prsiTable.cardTable.button.Name = ((Page7)project.pages[6]).prsiTable.cardsInDeck[0].ButtonCard.Name;

                ((Page7)project.pages[6]).prsiTable.cardsInDeck.RemoveAt(0);
            }
            else if (gameType == "dáma")
            {
                ((Page11)project.pages[10]).turnButton.BackColor = Color.LightGreen;

                if (project.InvokeRequired)
                {
                    project.Invoke(new Action(() => project.timer1.Start()));
                }
                else
                {
                    project.timer1.Start();
                }

            }
            else if (gameType == "piškvorky")
            {
                ((Page10)project.pages[9]).turnButton.BackColor = Color.LightGreen;

                ((Page10)project.pages[9]).board.image = Image.FromFile(@"C:\Users\Admin\Pictures\Screenshots\Snímek obrazovky 2024-08-19 000024.png");
            }
        }
    }
}
