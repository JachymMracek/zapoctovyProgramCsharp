using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using static zapoctovyProgramFinal.Form1;

namespace zapoctovyProgramFinal
{
    public class StartEndGame
    {
        protected Form1 project;
        public virtual async Task EndGame() { }
        public async Task StartGame(string typeGame)
        {
            await project.signalR.Connection.InvokeAsync("SetGame", typeGame, project.userInformation.name);
        }
        public byte[]? GetImageInBytes(Image? image)
        {
            using (var memo = new MemoryStream())
            {
                if (image == null) {return null;}

                image.Save(memo, image.RawFormat);

                return memo.ToArray();
            }
        }
        public StartEndGame(Form1 project)
        {
            this.project = project;
        }
    }
    public class PrsiGame : StartEndGame
    {
        public PrsiGame(Form1 project) : base(project) { }
        public async Task Play(string? nameButton, Image? image, int takeCards)
        {
            await project.signalR.Connection.InvokeAsync("PlayedPrsi", project.userInformation.name, project.userInformation.nameOpponent, GetImageInBytes(image), nameButton, takeCards);
        }
    }
    public class PiskvorkyGame : StartEndGame
    {
        public PiskvorkyGame(Form1 project) : base(project) { }
        public async Task Play(Image image, int index)
        {
            if (((Page10)project.pages[9]).buttons[((Page10)project.pages[9]).buttons.Count - 1].BackColor == Color.LightCoral) { return; }

            byte[] img = GetImageInBytes(image);

            await project.signalR.Connection.InvokeAsync("playedPiskvorky", project.userInformation.name, project.userInformation.nameOpponent, img, index);
        }
    }
    public class DamaGame : StartEndGame
    {
        public DamaGame(Form1 project) : base(project) { }
        public async Task Play(int indexTo, Image image)
        {
            byte[] img = GetImageInBytes(image);

            await project.signalR.Connection.InvokeAsync("PlayDama", project.userInformation.name, project.userInformation.nameOpponent, img, indexTo);
        }
    }

    public class CheckerGame : UsesOfProject
    {
        public Board Board { get; set; }
        public CheckerGame(Form1 project) : base(project)
        {
            Board = new Board(project, 14);
        }
    }
    public class ButtonPressed : UsesOfProject
    {
        public List<Button> buttons;

        public List<bool> hasJumped = new List<bool> { false };
        public ButtonPressed(Form1 project, List<Button> buttons) : base(project)
        {
            this.buttons = buttons;
        }
    }
}
