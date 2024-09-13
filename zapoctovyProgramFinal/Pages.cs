using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using static zapoctovyProgramFinal.Form1;

namespace zapoctovyProgramFinal
{
    public class Page : UsesOfProject
    {
        public List<Button> buttons = new List<Button>();
        public List<Label> labels = new List<Label>();

        public Page(Form1 project) : base(project)
        {
            this.project = project;
        }
        public void SetPage()
        {
            if (project.InvokeRequired) { project.Invoke(new Action(SetPage)); }

            else
            {
                project.Controls.Clear();

                foreach (Button button in buttons) { project.Controls.Add(button); }
                foreach (Label label in labels) { project.Controls.Add(label); }
            }
        }
        public void SeePage(int pageIndex)
        {
            if (pageIndex >= 0 && pageIndex < project.pages.Count) { project.pages[pageIndex].SetPage(); }
        }
        public virtual void ZpetButton_Click(object sender, EventArgs e) { }
    }
    public class Page0 : Page
    {
        private TextBox textBox = new TextBox { Location = new System.Drawing.Point(300, 200), Width = 200 };
        public Page0(Form1 Project) : base(Project)
        {
            textBox.KeyPress += TextBox_KeyPress;

            project.Controls.Add(textBox);

            this.project = Project;
        }
        private async void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                await project.signalR.Connection.InvokeAsync("Pass", textBox.Text);
            }
        }
    }
    public class Page1 : Page
    {
        public Page1(Form1 project) : base(project)
        {
            buttons.Add(new Button { Location = new Point(310, 140), Size = new Size(150, 50), Text = "VSTOUPIT", BackColor = Color.LightBlue });
            buttons.Add(new Button { Location = new Point(310, 200), Size = new Size(150, 50), Text = "ODEJÍT", BackColor = Color.LightCoral });

            buttons[0].Click += new EventHandler(VstoupitButton_Click);
            buttons[1].Click += new EventHandler(OdejitButton_Click);
        }
        private void VstoupitButton_Click(object sender, EventArgs e)
        {
            SeePage(1);
        }
        private void OdejitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
    public class Page2 : Page
    {
        public List<LineOfTheGame> lineOfTheGames = new List<LineOfTheGame>();
        public Page2(Form1 parentForm) : base(parentForm)
        {
            buttons.Add(new Button { Location = new Point(310, 140), Size = new Size(150, 50), Text = "ZALOŽIT", BackColor = Color.LightBlue });
            buttons.Add(new Button { Location = new Point(310, 60), Size = new Size(150, 50), Text = "ZPĚT", BackColor = Color.LightCoral });

            labels.Add(new Label { Text = "typ hry                                            jméno                                                        stav ", Location = new Point(85, 200), Font = new Font("Arial", 10, FontStyle.Bold | FontStyle.Underline), AutoSize = true });

            buttons[0].Click += new EventHandler(ZalozitButton_Click);
            buttons[1].Click += new EventHandler(ZpetButton_Click);
        }
        private void ZalozitButton_Click(object sender, EventArgs e)
        {
            SeePage(2);
        }
        public override void ZpetButton_Click(object sender, EventArgs e)
        {
            SeePage(0);
        }
        public void AddToButtonsAndLabels(LineOfTheGame lineOfTheGame)
        {
            lineOfTheGames.Add(lineOfTheGame);

            lineOfTheGame.greenButtons.button.Top = 225 + (((labels.Count - 1) / 2)) * 30;

            buttons.Add(lineOfTheGame.greenButtons.button);

            foreach (Label label in lineOfTheGame.typeGamelabels)
            {
                label.Top = 225 + ((labels.Count - 1) / 2) * 30;

                labels.Add(label);
            }
        }
        public void DeleteLabelesButtons(int index)
        {
            foreach (Label label in lineOfTheGames[index].typeGamelabels) { labels.Remove(label); }

            buttons.Remove(lineOfTheGames[index].greenButtons.button);

            lineOfTheGames.RemoveAt(index);

            for (int i = 1; i < labels.Count; i++)
            {
                Label label = labels[i];

                if (label.InvokeRequired) { label.Invoke(new Action(() => { label.Top = 225 + (i - 1) / 2 * 30;}));}

                else { label.Top = 225 + (i - 1) / 2 * 30;}
            }

            for (int i = 2; i < buttons.Count; i++)
            {
                Button button = buttons[i];

                if (button.InvokeRequired)
                {
                    button.Invoke(new Action(() => { button.Top = 225 + (i - 2) * 30;}));
                }
                else
                {
                    button.Top = 225 + (i - 2) * 30;
                }
            }

        }
    }
    public class Page3 : Page
    {
        public Page3(Form1 project) : base(project)
        {

            buttons.Add(new Button { Location = new Point(310, 120), Size = new Size(150, 50), Text = "ŠACHY", BackColor = Color.LightBlue });
            buttons.Add(new Button { Location = new Point(310, 180), Size = new Size(150, 50), Text = "DÁMA", BackColor = Color.LightBlue });
            buttons.Add(new Button { Location = new Point(310, 240), Size = new Size(150, 50), Text = "PIŠKVORKY", BackColor = Color.LightBlue });
            buttons.Add(new Button { Location = new Point(310, 300), Size = new Size(150, 50), Text = "PRŠÍ", BackColor = Color.LightBlue });
            buttons.Add(new Button { Location = new Point(310, 60), Size = new Size(150, 50), Text = "ZPĚT", BackColor = Color.LightCoral });


            buttons[0].Click += new EventHandler(ChessButton_Click);
            buttons[1].Click += new EventHandler(CheckerButton_Click);
            buttons[2].Click += new EventHandler(PiskvorkyButton_Click);
            buttons[3].Click += new EventHandler(PrsiButton_Click);
            buttons[4].Click += new EventHandler(ZpetButton_Click);
        }
        private void ChessButton_Click(object sender, EventArgs e)
        {
            SeePage(3);
        }
        private void CheckerButton_Click(Object sender, EventArgs e)
        {
            SeePage(3);
        }
        private void PiskvorkyButton_Click(Object sender, EventArgs e)
        {
            SeePage(4);
        }
        private void PrsiButton_Click(Object sender, EventArgs e)
        {
            SeePage(5);
        }
        public override void ZpetButton_Click(object sender, EventArgs e)
        {
            SeePage(1);
        }
    }
    public class Page4 : Page
    {
        public Page4(Form1 project) : base(project)
        {
            buttons.Add(new Button { Location = new Point(200, 70), Size = new Size(150, 50), Text = "BULLET 1 MINUTA", BackColor = Color.LightBlue });
            buttons.Add(new Button { Location = new Point(200, 130), Size = new Size(150, 50), Text = "BULLET 1 MINUTA + 2 SEKUNDY", BackColor = Color.LightBlue });
            buttons.Add(new Button { Location = new Point(200, 190), Size = new Size(150, 50), Text = "BLITZ 3 MINUTY", BackColor = Color.LightBlue });
            buttons.Add(new Button { Location = new Point(325, 250), Size = new Size(150, 50), Text = "CLASSICAL", BackColor = Color.LightBlue });
            buttons.Add(new Button { Location = new Point(450, 70), Size = new Size(150, 50), Text = "BLITZ 3 MINUTY + 2 SEKUNDY", BackColor = Color.LightBlue });
            buttons.Add(new Button { Location = new Point(450, 130), Size = new Size(150, 50), Text = "RAPID 10 MINUT", BackColor = Color.LightBlue });
            buttons.Add(new Button { Location = new Point(450, 190), Size = new Size(150, 50), Text = "RAPID 15 MINUT + 15 SEKUND", BackColor = Color.LightBlue });
            buttons.Add(new Button { Location = new Point(325, 10), Size = new Size(150, 50), Text = "ZPĚT", BackColor = Color.LightCoral });


            buttons[0].Click += new EventHandler(Bullet1Button_Click);
            buttons[1].Click += new EventHandler(Bullet12Button_Click);
            buttons[2].Click += new EventHandler(Blitz3Button_Click);
            buttons[3].Click += new EventHandler(ClassicalButton_Click);
            buttons[4].Click += new EventHandler(Blitz32Button_Click);
            buttons[5].Click += new EventHandler(Rapid10Button_Click);
            buttons[6].Click += new EventHandler(Rapid1515Button_Click);
            buttons[7].Click += new EventHandler(ZpetButton_Click);
        }
        private async void Bullet1Button_Click(object sender, EventArgs e)
        {
            ClassicalButton_Click(sender, e);

            project.timeLeft = 60;

            project.userInformation.timeSet = 60;

            project.classical = false;

            project.pages[10].SeePage(10);
        }
        private async void Bullet12Button_Click(object sender, EventArgs e)
        {
            ClassicalButton_Click(sender, e);

            project.timeLeft = 60;

            project.userInformation.timeSet = 60;

            project.classical = false;

            project.plusMove = 2;

            project.pages[10].SeePage(10);
        }
        private async void Blitz3Button_Click(Object sender, EventArgs e)
        {
            ClassicalButton_Click(sender, e);

            project.timeLeft = 180;

            project.userInformation.timeSet = 180;

            project.classical = false;

            project.pages[10].SeePage(10);
        }
        private async void ClassicalButton_Click(Object sender, EventArgs e)
        {
            project.userInformation.GameTypePage = 10;

            project.userInformation.make = true;

            project.timeLeft = 0;

            project.plusMove = 0;

            project.classical = true;

            foreach (Button button in ((Page11)project.pages[10]).boardFigure.buttons)
            {
                if (button.Name == "whiteFigure")
                {
                    button.Image = Image.FromFile(@"C:\Users\Admin\Documents\ZAPOCTOVY_PROGRAM_Csharp\obrazky1\blueBird.png");
                }
                else if (button.Name == "blackFigure")
                {
                    button.Image = Image.FromFile(@"C:\Users\Admin\Pictures\Screenshots\Snímek obrazovky 2024-08-19 000024.png");
                }
            }

            await new DamaGame(project).StartGame("dáma");

            project.pages[10].SeePage(10);
        }
        private void Blitz32Button_Click(Object sender, EventArgs e)
        {
            ClassicalButton_Click(sender, e);

            project.timeLeft = 180;

            project.userInformation.timeSet = 180;

            project.classical = false;

            project.plusMove = 2;

            project.pages[10].SeePage(10);
        }
        private void Rapid10Button_Click(Object sender, EventArgs e)
        {
            ClassicalButton_Click(sender, e);

            project.timeLeft = 600;

            project.userInformation.timeSet = 600;

            project.classical = false;

            project.pages[10].SeePage(10);
        }
        private void Rapid1515Button_Click(Object sender, EventArgs e)
        {
            ClassicalButton_Click(sender, e);

            project.timeLeft = 900;

            project.userInformation.timeSet = 900;

            project.classical = false;

            project.plusMove = 15;

            project.pages[10].SeePage(10);
        }
        public override void ZpetButton_Click(object sender, EventArgs e)
        {
            SeePage(2);
        }
    }
    public class Page5 : Page
    {
        public Page5(Form1 project) : base(project)
        {
            buttons.Add(new Button { Location = new Point(310, 120), Size = new Size(150, 50), Text = "CLASSICAL", BackColor = Color.LightBlue });
            buttons.Add(new Button { Location = new Point(310, 170), Size = new Size(150, 50), Text = "TURNAJOVÁ HRA", BackColor = Color.LightBlue });
            buttons.Add(new Button { Location = new Point(310, 60), Size = new Size(150, 50), Text = "ZPĚT", BackColor = Color.LightCoral });

            buttons[0].Click += new EventHandler(Classical_Click);
            buttons[1].Click += new EventHandler(Tournament_Click);
            buttons[2].Click += new EventHandler(ZpetButton_Click);
        }
        private async void Classical_Click(Object sender, EventArgs e)
        {
            project.userInformation.make = true;

            project.userInformation.GameTypePage = 9;

            await new PiskvorkyGame(project).StartGame("piškvorky");

            project.pages[9].SeePage(9);
        }
        private async void Tournament_Click(Object sender, EventArgs e)
        {
            project.userInformation.make = true;

            project.userInformation.GameTypePage = 9;

            project.userInformation.threes = true;

            await new PiskvorkyGame(project).StartGame("piškvorky");

            project.tournamentPiskvorky.ThreeStartMoves();

            project.pages[9].SeePage(9);
        }
        public override void ZpetButton_Click(object sender, EventArgs e)
        {
            SeePage(2);
        }
    }
    public class Page6 : Page
    {
        public Page6(Form1 parentForm) : base(parentForm)
        {
            buttons.Add(new Button { Location = new Point(310, 180), Size = new Size(150, 50), Text = "HRA S HRÁČEM", BackColor = Color.LightBlue });
            buttons.Add(new Button { Location = new Point(310, 100), Size = new Size(150, 50), Text = "ZPĚT", BackColor = Color.LightCoral });

            buttons[0].Click += new EventHandler(HrasHracem_Click);
            buttons[1].Click += new EventHandler(ZpetButton_Click);
        }
        public async void HrasHracem_Click(Object sender, EventArgs e)
        {
            project.userInformation.GameTypePage = 6;

            project.userInformation.make = true;

            await new PrsiGame(project).StartGame("prší");

            SeePage(6);
        }
        public override void ZpetButton_Click(object sender, EventArgs e)
        {
            SeePage(2);
        }
    }
    public class Page7 : Page
    {
        public PrsiTable prsiTable { get; set; }
        public PrsiPlayer prsiPlayer { get; set; }
        public Page7(Form1 project) : base(project)
        {
            this.prsiTable = new PrsiTable(project);
            this.prsiPlayer = new PrsiPlayer(prsiTable, project);

            buttons.Add(prsiTable.cardTable.button);
            buttons.Add(prsiTable.deckButton.button);
            buttons.Add(prsiTable.turnButton);
            buttons.Add(prsiTable.ZpetButton);

            foreach (Card card in prsiPlayer.cardsInHand)
            {
                buttons.Add(card.ButtonCard);
            }
        }
        public void AddToButtonsCrdsInHand(Card card)
        {
            this.buttons.Add(card.ButtonCard);
        }
    }
    public class Page8 : Page
    {
        public Blue blue;
        public Black black;
        public Red red;
        public Yellow yellow;

        public Page8(Form1 project) : base(project)
        {
            this.project = project;

            blue = new Blue(project);
            black = new Black(project);
            red = new Red(project);
            yellow = new Yellow(project);

            buttons.Add(blue.button);
            buttons.Add(black.button);
            buttons.Add(red.button);
            buttons.Add(yellow.button);
        }
    }
    public class Page9 : Page
    {
        public Page9(Form1 project) : base(project)
        {
            buttons.Add(new Button { Location = new Point(310, 120), Size = new Size(150, 50), Text = "Konec", BackColor = Color.LightBlue, });
            buttons.Add(new Button { Location = new Point(310, 170), Size = new Size(150, 50), Text = "Opakovat", BackColor = Color.LightBlue, });

            buttons[0].Click += new EventHandler(Konec_Click);
            buttons[1].Click += new EventHandler(Opakovat_Click);
        }
        private async void Konec_Click(Object sender, EventArgs e)
        {
            project.tournamentPiskvorky = new TournamentPiskvorky();

            await Resetconnection();
        }
        private async void Opakovat_Click(Object sender, EventArgs e)
        {
            await OpakovatSend();
        }
        private async Task Resetconnection()
        {
            await project.signalR.Connection.InvokeAsync("ResetGameConnection", project.userInformation.name, project.userInformation.nameOpponent);
        }
        private async Task OpakovatSend()
        {
            await project.signalR.Connection.InvokeAsync("RestartGame", project.userInformation.name, project.userInformation.nameOpponent, project.userInformation.GameTypePage);
        }
    }
    public class Page10 : Page
    {
        public Board board;

        public Button turnButton = new Button { Location = new Point(850, 200), Size = new Size(100, 100), Text = "", BackColor = Color.LightCoral };
        public Page10(Form1 project) : base(project)
        {
            board = new Board(project, 14);

            foreach (Button button in board.buttons)
            {
                buttons.Add(button);
            }

            buttons.Add(turnButton);
        }
    }
    public class Page11 : Page
    {
        public BoardFigure boardFigure { get; set; }

        public Button turnButton = new Button { Location = new Point(550, 200), Size = new Size(100, 100), Text = "", BackColor = Color.LightCoral };

        public Button myTimeButton = new Button { Location = new Point(550, 330), Size = new Size(100, 50), Text = "" };

        public Button OpponentTimeButton = new Button { Location = new Point(550, 70), Size = new Size(100, 50), Text = "" };
        public Page11(Form1 project) : base(project)
        {
            boardFigure = new BoardFigure(project, 8);

            foreach (Button button in boardFigure.buttons)
            {
                buttons.Add(button);
            }

            buttons.Add(turnButton);

            buttons.Add(myTimeButton);

            buttons.Add(OpponentTimeButton);
        }
    }
}
