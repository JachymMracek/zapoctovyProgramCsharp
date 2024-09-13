using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using static zapoctovyProgramFinal.Form1;

namespace zapoctovyProgramFinal
{
    public class Board : UsesOfProject
    {
        public List<Button> buttons = new List<Button>();

        public Image image = Image.FromFile(@"C:\Users\Admin\Documents\ZAPOCTOVY_PROGRAM_Csharp\obrazky1\blueBird.png");
        public Board(Form1 project, int size) : base(project)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Button button = new Button();
                    button.Size = new Size(50, 50);
                    button.Location = new Point(50 + 50 * i, 50 + 50 * j);
                    buttons.Add(button);

                    button.Click += new EventHandler(BoardButton_Click);
                }
            }
        }
        public async Task StartTournamentThreePlay()
        {
            int i = 0;

            foreach (int index in project.tournamentPiskvorky.RedSymbols)
            {
                new PiskvorkyGame(project).Play(Image.FromFile(@"C:\Users\Admin\Pictures\Screenshots\Snímek obrazovky 2024-08-19 000024.png"), index);

            }
            foreach (int index in project.tournamentPiskvorky.BlueSymbols)
            {
                Application.DoEvents();
                new PiskvorkyGame(project).Play(Image.FromFile(@"C:\Users\Admin\Documents\ZAPOCTOVY_PROGRAM_Csharp\obrazky1\blueBird.png"), index);
            }

            await project.signalR.Connection.InvokeAsync("Threes", project.userInformation.name, project.userInformation.nameOpponent);
        }
        public async void BoardButton_Click(object sender, EventArgs e)
        {
            if (project.Controls[project.Controls.Count - 1] is Button button)
            {
                if (button.Name == "REDBUTTON") { return; }
            }
            if (project.tournamentPiskvorky.Played > 1)
            {
                project.tournamentPiskvorky.RedSymbols.Add(buttons.IndexOf((Button)sender));

                project.tournamentPiskvorky.Played--;

                return;
            }
            else if (project.tournamentPiskvorky.Played == 1)
            {
                project.tournamentPiskvorky.BlueSymbols.Add(buttons.IndexOf((Button)sender));

                project.tournamentPiskvorky.Played--;

                StartTournamentThreePlay();

                return;
            }

            await new PiskvorkyGame(project).Play(image, buttons.IndexOf((Button)sender));
        }
    }
    public class CheckPiskvorkyWinner : UsesOfProject
    {
        public CheckPiskvorkyWinner(Form1 project) : base(project) { }

        public async Task Row(int index, string tag)
        {
            int count = 0;

            for (int i = -4; i <= 4; i++)
            {
                if (((Page10)project.pages[9]).buttons.Count <= index + i * 14 || index + i * 14 < 0) { continue; }

                if (((Page10)project.pages[9]).buttons[index + i * 14].Name == tag) {count++;}

                else { count = 0;}

                if (count == 5){await project.signalR.Connection.InvokeAsync("EndGame", project.userInformation.name, project.userInformation.nameOpponent);break;}
            }
        }
        public async Task Diagonal1(int index, string tag)
        {
            int count = 0;

            for (int i = -4; i <= 4; i++)
            {
                if (((Page10)project.pages[9]).buttons.Count <= index + (i * 14 - i) || index + (i * 14 - i) < 0) { continue; }

                if (((Page10)project.pages[9]).buttons[index + (i * 14 - i)].Name == tag) {count++;}

                else { count = 0;}

                if (count == 5) { await project.signalR.Connection.InvokeAsync("EndGame", project.userInformation.name, project.userInformation.nameOpponent);break;}
            }
        }
        public async Task Diagonal2(int index, string tag)
        {
            int count = 0;

            for (int i = -4; i <= 4; i++)
            {
                if (((Page10)project.pages[9]).buttons.Count <= index + (-i * 14 - i) || index + (-i * 14 - i) < 0) { continue; }

                if (((Page10)project.pages[9]).buttons[index + (-i * 14 - i)].Name == tag) { count++;}

                else {count = 0;}

                if (count == 5) {await project.signalR.Connection.InvokeAsync("EndGame", project.userInformation.name, project.userInformation.nameOpponent);break;}
            }
        }
        public async Task Column(int index, string tag)
        {
            int count = 0;

            for (int i = -4; i <= 4; i++)
            {
                if (((Page10)project.pages[9]).buttons.Count <= index + i * 14 || index + i * 14 < 0) { continue; }

                if (((Page10)project.pages[9]).buttons[index + i].Name == tag) {count++;}

                else { count = 0;}

                if (count == 5) { await project.signalR.Connection.InvokeAsync("EndGame", project.userInformation.name, project.userInformation.nameOpponent);break;}
            }
        }
        public void FindFifts(int index, string tag)
        {
            Column(index, tag);

            Row(index, tag);

            Diagonal1(index, tag);

            Diagonal2(index, tag);
        }
    }
    public class TournamentPiskvorky
    {
        public int Played = 0;

        public List<int> RedSymbols = new List<int>();

        public List<int> BlueSymbols = new List<int>();
        public TournamentPiskvorky() { }
        public void ThreeStartMoves() { Played = 3; }
    }
}
