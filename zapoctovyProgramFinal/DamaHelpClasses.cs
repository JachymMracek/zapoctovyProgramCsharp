using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using static zapoctovyProgramFinal.Form1;

namespace zapoctovyProgramFinal
{
    public class FigurePressed : ButtonPressed
    {
        public FigurePressed(Form1 project, List<Button> buttons) : base(project, buttons) { }
        public virtual void SetLeftGreenButton(int index, bool mustJump) { }
        public virtual void SetRightGreenButton(int index, bool mustJump) { }
        public void FigureGamePressed(object sender, bool mustJump)
        {
            int index = buttons.IndexOf((Button)sender);

            SetLeftGreenButton(index, mustJump);

            SetRightGreenButton(index, mustJump);
        }
    }
    public class GreenGameButtonShort : FigurePressed
    {
        public int lastPressed = 0;
        public GreenGameButtonShort(Form1 project, List<Button> buttons) : base(project, buttons) { }
        public override void SetLeftGreenButton(int index, bool mustJump)
        {
            lastPressed = index;

            if (mustJump) { return; }

            if (index - 9 >= 0 && buttons[index - 9].Image == null) { buttons[index - 9].BackColor = Color.Green;}
        }
        public override void SetRightGreenButton(int index, bool mustJump)
        {
            if (mustJump) { return; }

            if (index + 7 < buttons.Count && buttons[index + 7].Image == null) {buttons[index + 7].BackColor = Color.Green;}
        }
    }
    public class GreenGameButtonLong : FigurePressed
    {
        public GreenGameButtonLong(Form1 project, List<Button> buttons) : base(project, buttons) { }
        public override void SetLeftGreenButton(int index, bool mustJump)
        {
            if (index - 18 >= 0 && buttons[index - 18].Image == null && index % 8 > (index - 18) % 8 && buttons[index - 9].Image != null && buttons[index - 9].Name != "blackFigure")
            {
                buttons[index - 18].BackColor = Color.Green;
            }
        }
        public override void SetRightGreenButton(int index, bool mustJUmp)
        {
            if (index + 14 < buttons.Count && index % 8 > (index + 14) % 8 && buttons[index + 14].Image == null && buttons[index + 7].Name != "blackFigure" && buttons[index + 7].Image != null)
            {
                buttons[index + 14].BackColor = Color.Green;
            }
        }
    }
    public class EmptyGameButton : ButtonPressed
    {
        public EmptyGameButton(Form1 project, List<Button> buttons) : base(project, buttons) { }
        public void EmptyGamePressed()
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                Button button = buttons[i];

                if (button.BackColor == Color.Green) { SetBackColour(button);}
            }
        }
        private void SetBackColour(Button button)
        {
            if ((button.Location.X / 50 + button.Location.Y / 50) % 2 == 0) { button.BackColor = Color.White; }

            else { button.BackColor = Color.Black; }
        }
        public void QueenDeleteGreen()
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                Button button = buttons[i];

                if (button.BackColor == Color.Green && (button.Name != "takeLD" && button.Name != "takeRD" && button.Name != "takeRU" && button.Name != "takeLU")) {SetBackColour(button);}
            }
        }
    }
    public class PressGreenButtonGame : ButtonPressed
    {
        public List<bool> hasJumped = new List<bool> { false };
        public PressGreenButtonGame(Form1 project, List<Button> buttons) : base(project, buttons) { }
        private async Task DeleteRightFigure(int indexFrom, int indexTo)
        {
            if (indexTo - indexFrom == 14)
            {
                buttons[indexFrom + 7].Image = null;
                buttons[indexFrom + 7].Name = "";

                hasJumped[0] = true;

                await project.signalR.Connection.InvokeAsync("DelteFigureFromBoard", indexFrom + 7, project.userInformation.name, project.userInformation.nameOpponent);
            }
        }
        private async Task DeleteLeftFigure(int indexFrom, int indexTo)
        {
            if (indexTo - indexFrom == -18)
            {
                buttons[indexFrom - 9].Image = null;
                buttons[indexFrom - 9].Name = "";

                hasJumped[0] = true;

                await project.signalR.Connection.InvokeAsync("DelteFigureFromBoard", indexFrom - 9, project.userInformation.name, project.userInformation.nameOpponent);
            }
        }
        private void NextJump(int indexTo)
        {
            new GreenGameButtonLong(project, buttons).SetLeftGreenButton(indexTo, false);
            new GreenGameButtonLong(project, buttons).SetRightGreenButton(indexTo, false);
        }
        public async Task Jump(int indexFrom, int indexTo, object button)
        {
            new EmptyGameButton(project, buttons).EmptyGamePressed();

            buttons[indexTo].Image = buttons[indexFrom].Image;
            buttons[indexTo].Tag = buttons[indexFrom].Tag;

            buttons[indexFrom].Image = null;
            buttons[indexFrom].Name = "";
            buttons[indexFrom].Tag = "";

            DeleteQueenJump deleteQueenJump = new DeleteQueenJump(project, buttons);

            if (((Button)button).Name == "takeLD")
            {
                hasJumped[0] = true;

                deleteQueenJump.DeleteLD(indexTo);
            }
            else if (((Button)button).Name == "takeRD")
            {
                hasJumped[0] = true;

                deleteQueenJump.DeleteRD(indexTo);
            }
            else if (((Button)button).Name == "takeLU")
            {
                hasJumped[0] = true;

                deleteQueenJump.DeleteLU(indexTo);
            }
            else if (((Button)button).Name == "takeRU")
            {
                hasJumped[0] = true;

                deleteQueenJump.DeleteRU(indexTo);
            }
            else
            {
                DeleteRightFigure(indexFrom, indexTo);
                DeleteLeftFigure(indexFrom, indexTo);

                NextJump(indexTo);
            }

            buttons[indexTo].Name = "blackFigure";

            await project.signalR.Connection.InvokeAsync("DelteFigureFromBoard", indexFrom, project.userInformation.name, project.userInformation.nameOpponent);

            if (indexTo % 8 == 0)
            {
                if (project.userInformation.make) { buttons[indexTo].Image = Image.FromFile(@"C:\Users\Admin\Documents\ZAPOCTOVY_PROGRAM_Csharp\obrazky1\redQueen.png"); }

                else { buttons[indexTo].Image = Image.FromFile(@"C:\Users\Admin\Documents\ZAPOCTOVY_PROGRAM_Csharp\obrazky1\blueQueen.png"); }

                buttons[indexTo].Tag = "queen";
            }

            await new DamaGame(project).Play(indexTo, buttons[indexTo].Image);
        }
    }
    public class QuennGreen : ButtonPressed
    {
        public int lastPressed = 0;
        public QuennGreen(Form1 project, List<Button> buttons) : base(project, buttons) { }

        private void DiagonalRightUp(int index)
        {
            int diagonalIndex = index + 7;
            bool opponent = false;

            while (true)
            {
                if (diagonalIndex < buttons.Count && (diagonalIndex - 7) % 8 > (diagonalIndex) % 8 && buttons[diagonalIndex].Image == null)
                {
                    buttons[diagonalIndex].BackColor = Color.Green;

                    if (opponent) {buttons[diagonalIndex].Name = "takeRU";break;}
                }
                else if (opponent) {break;}

                else if (diagonalIndex < buttons.Count && (diagonalIndex - 7) % 8 > (diagonalIndex) % 8 && buttons[diagonalIndex].Image != null && buttons[diagonalIndex].Name != "blackFigure") {opponent = true;}

                else if (diagonalIndex < buttons.Count && (diagonalIndex - 7) % 8 > (diagonalIndex) % 8 && buttons[diagonalIndex].Image != null && buttons[diagonalIndex].Name == "blackFigure") {break;}

                else if (diagonalIndex >= buttons.Count || (diagonalIndex - 7) % 8 <= (diagonalIndex) % 8) {break;}

                diagonalIndex += 7;
            }
        }
        private void DiagonalLefttUp(int index)
        {
            int diagonalIndex = index - 9;
            bool opponent = false;

            while (true)
            {
                if (diagonalIndex >= 0 && (diagonalIndex + 9) % 8 > (diagonalIndex) % 8 && buttons[diagonalIndex].Image == null)
                {
                    buttons[diagonalIndex].BackColor = Color.Green;

                    if (opponent) {buttons[diagonalIndex].Name = "takeLU";break;}
                }
                else if (opponent) { break; }

                else if (diagonalIndex >= 0 && (diagonalIndex + 9) % 8 > (diagonalIndex) % 8 && buttons[diagonalIndex].Image != null && buttons[diagonalIndex].Name != "blackFigure") {opponent = true;}

                else if (diagonalIndex >= 0 && (diagonalIndex + 9) % 8 > (diagonalIndex) % 8 && buttons[diagonalIndex].Image != null && buttons[diagonalIndex].Name == "blackFigure") {break;}

                else if (diagonalIndex < 0 || (diagonalIndex + 9) % 8 <= (diagonalIndex) % 8) {break;}

                diagonalIndex -= 9;
            }
        }
        private void DiagonalRightDown(int index)
        {
            int diagonalIndex = index + 9;
            bool opponent = false;

            while (true)
            {
                if (diagonalIndex < buttons.Count && (diagonalIndex - 9) % 8 < (diagonalIndex) % 8 && buttons[diagonalIndex].Image == null)
                {
                    buttons[diagonalIndex].BackColor = Color.Green;

                    if (opponent) {buttons[diagonalIndex].Name = "takeRD";break;}
                }
                else if (opponent) {break;}

                else if (diagonalIndex < buttons.Count && (diagonalIndex - 9) % 8 < (diagonalIndex) % 8 && buttons[diagonalIndex].Image != null && buttons[diagonalIndex].Name != "blackFigure") {opponent = true;}

                else if (diagonalIndex < buttons.Count && (diagonalIndex - 9) % 8 < (diagonalIndex) % 8 && buttons[diagonalIndex].Image != null && buttons[diagonalIndex].Name == "blackFigure") {break;}

                else if (diagonalIndex >= buttons.Count || (diagonalIndex - 9) % 8 >= (diagonalIndex) % 8) {break;}

                diagonalIndex += 9;
            }
        }
        private void DiagonalLeftDown(int index)
        {
            int diagonalIndex = index - 7;
            bool opponent = false;

            while (true)
            {
                if (diagonalIndex >= 0 && (diagonalIndex + 7) % 8 < (diagonalIndex) % 8 && buttons[diagonalIndex].Image == null)
                {
                    buttons[diagonalIndex].BackColor = Color.Green;

                    if (opponent) { buttons[diagonalIndex].Name = "takeLD";break;}
                }
                else if (opponent) { break;}

                else if (diagonalIndex >= 0 && (diagonalIndex + 7) % 8 < (diagonalIndex) % 8 && buttons[diagonalIndex].Image != null && buttons[diagonalIndex].Name != "blackFigure") {opponent = true;}

                else if (diagonalIndex >= 0 && (diagonalIndex + 7) % 8 < (diagonalIndex) % 8 && buttons[diagonalIndex].Image != null && buttons[diagonalIndex].Name == "blackFigure") { break;}

                else if (diagonalIndex < 0 || (diagonalIndex + 7) % 8 >= (diagonalIndex) % 8) {break;}

                diagonalIndex -= 7;
            }
        }
        public void MakeGreen(int index)
        {
            lastPressed = index;

            DiagonalLeftDown(index);

            DiagonalRightDown(index);

            DiagonalRightUp(index);

            DiagonalLefttUp(index);
        }
    }
    public class DeleteQueenJump : ButtonPressed
    {
        public DeleteQueenJump(Form1 project, List<Button> buttons) : base(project, buttons) { }

        private void MakeGreenSquaresAfterJump(int diagonalIndex)
        {
            new EmptyGameButton(project, buttons).EmptyGamePressed();

            QuennGreen quennGreen = new QuennGreen(project, buttons);

            quennGreen.MakeGreen(diagonalIndex);

            new EmptyGameButton(project, buttons).QueenDeleteGreen();
        }
        public async Task DeleteLD(int diagonalIndex)
        {
            buttons[diagonalIndex + 7].Image = null;
            buttons[diagonalIndex + 7].Name = "";
            buttons[diagonalIndex + 7].Tag = "";

            await project.signalR.Connection.InvokeAsync("DelteFigureFromBoard", diagonalIndex + 7, project.userInformation.name, project.userInformation.nameOpponent);

            MakeGreenSquaresAfterJump(diagonalIndex);
        }
        public async Task DeleteRD(int diagonalIndex)
        {
            buttons[diagonalIndex - 9].Image = null;
            buttons[diagonalIndex - 9].Name = "";
            buttons[diagonalIndex - 9].Tag = "";

            await project.signalR.Connection.InvokeAsync("DelteFigureFromBoard", diagonalIndex - 9, project.userInformation.name, project.userInformation.nameOpponent);

            MakeGreenSquaresAfterJump(diagonalIndex);
        }
        public async Task DeleteLU(int diagonalIndex)
        {
            buttons[diagonalIndex + 9].Image = null;
            buttons[diagonalIndex + 9].Name = "";
            buttons[diagonalIndex + 9].Tag = "";

            await project.signalR.Connection.InvokeAsync("DelteFigureFromBoard", diagonalIndex + 9, project.userInformation.name, project.userInformation.nameOpponent);

            MakeGreenSquaresAfterJump(diagonalIndex);
        }
        public async Task DeleteRU(int diagonalIndex)
        {
            buttons[diagonalIndex - 7].Image = null;
            buttons[diagonalIndex - 7].Name = "";
            buttons[diagonalIndex - 7].Tag = "";

            await project.signalR.Connection.InvokeAsync("DelteFigureFromBoard", diagonalIndex - 7, project.userInformation.name, project.userInformation.nameOpponent);

            MakeGreenSquaresAfterJump(diagonalIndex);
        }
    }

    public class BoardFigure : UsesOfProject
    {
        public List<Button> buttons = new List<Button>();
        public int indexOfLastPressed = -1;

        public bool mustjump = false;
        public bool jumped = false;

        public int lastPlayed = -1;

        public Image image1 = Image.FromFile(@"C:\Users\Admin\Documents\ZAPOCTOVY_PROGRAM_Csharp\obrazky1\blueBird.png");
        public Image image2 = Image.FromFile(@"C:\Users\Admin\Pictures\Screenshots\Snímek obrazovky 2024-08-19 000024.png");
        public BoardFigure(Form1 project, int size) : base(project)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Button button = new Button();button.Size = new Size(50, 50);button.Location = new Point(50 + 50 * i, 50 + 50 * j);buttons.Add(button);

                    if ((i + j) % 2 == 0) { button.BackColor = Color.White;}

                    else { button.BackColor = Color.Black;

                        if (j < 3) { button.Image = image2;button.Name = "whiteFigure";}

                        else if (j > 4) { button.Image = image1;button.Name = "blackFigure";}
                    }

                    button.Click += new EventHandler(BoardButton_Click);
                }
            }
        }
        private void AfterJump(object sender, bool hasJumped)
        {
            Application.DoEvents();

            jumped = false;

            if (buttons.Any(button => button.BackColor == Color.Green) && hasJumped){jumped = true;indexOfLastPressed = buttons.IndexOf((Button)sender);}
        }
        private async Task ChangePlayDama()
        {
            if (!jumped)
            {
                ((Page11)project.pages[10]).turnButton.BackColor = Color.LightCoral;

                new EmptyGameButton(project, buttons).EmptyGamePressed();

                project.timeLeft += project.plusMove;

                if (project.InvokeRequired) {project.Invoke(new Action(() => project.timer1.Stop()));}

                else {project.timer1.Stop();}

                await project.signalR.Connection.InvokeAsync("ChangeDamaPlay", project.userInformation.name, project.userInformation.nameOpponent, ((Page11)project.pages[10]).myTimeButton.Text);
            }
        }
        private void MustPlay()
        {
            mustjump = false;

            foreach (var button in buttons)
            {
                if (button.Name != "blackFigure") { continue; }

                else if (button.Tag == "queen") { new QuennGreen(project, buttons).MakeGreen(buttons.IndexOf((Button)button)); new EmptyGameButton(project, buttons).QueenDeleteGreen(); }

                new GreenGameButtonLong(project, buttons).FigureGamePressed(button, false);
            }

            if (buttons.Any(button => button.BackColor == Color.Green)) { mustjump = true;}

            new EmptyGameButton(project, buttons).EmptyGamePressed();
        }
        private async Task EndOpponent()
        {
            foreach (var button in buttons)
            {
                if (button.Image != null && button.Name != "blackFigure") { return; }
            }
            await project.signalR.Connection.InvokeAsync("EndGame", project.userInformation.name, project.userInformation.nameOpponent);
        }
        private void BoardButton_Click(object sender, EventArgs e)
        {
            if (((Page11)project.pages[10]).turnButton.BackColor != Color.LightGreen) { return; }

            if (((Button)sender).Name == "blackFigure" && ((Button)sender).Tag == "queen" && !jumped)
            {
                new EmptyGameButton(project, buttons).EmptyGamePressed();

                MustPlay();

                QuennGreen quennGreen = new QuennGreen(project, buttons);

                quennGreen.MakeGreen(buttons.IndexOf((Button)sender));

                if (buttons.Any(button => button.Name == "takeLD" || button.Name == "takeRD" || button.Name == "takeRU" || button.Name == "takeLU")) { new EmptyGameButton(project, buttons).QueenDeleteGreen(); }

                indexOfLastPressed = quennGreen.lastPressed;
            }
            else if (((Button)sender).Name == "blackFigure" && !jumped)
            {
                new EmptyGameButton(project, buttons).EmptyGamePressed();

                MustPlay();

                GreenGameButtonShort greenGameButtonShort = new GreenGameButtonShort(project, buttons);

                new GreenGameButtonLong(project, buttons).FigureGamePressed(sender, false);

                greenGameButtonShort.FigureGamePressed(sender, mustjump);

                indexOfLastPressed = greenGameButtonShort.lastPressed;
            }
            else if (((Button)sender).BackColor != Color.Green && !jumped)
            {
                new EmptyGameButton(project, buttons).EmptyGamePressed();
            }
            else if (((Button)sender).BackColor == Color.Green)
            {
                PressGreenButtonGame pressGreenButtonGame = new PressGreenButtonGame(project, buttons);

                pressGreenButtonGame.Jump(indexOfLastPressed, buttons.IndexOf((Button)sender), sender);

                AfterJump(sender, pressGreenButtonGame.hasJumped[0]);

                ChangePlayDama();
            }
            EndOpponent();
        }
    }
}
