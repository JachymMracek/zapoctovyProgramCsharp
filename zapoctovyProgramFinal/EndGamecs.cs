using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using static zapoctovyProgramFinal.Form1;

namespace zapoctovyProgramFinal
{
    public class EndGameButton : UsesOfProject
    {
        public Button ZpetButton { get; set; }
        public EndGameButton(Form1 project) : base(project)
        {
            this.ZpetButton = new Button { Location = new Point(350, 50), Size = new Size(90, 50), Text = "Ukončit hru" };

            ZpetButton.Click += new EventHandler(ZpetButton_Click);
        }
        private async void ZpetButton_Click(Object sender, EventArgs e)
        {
            new EndUserGame(project).EndGame(project.userInformation.GameTypePage);
        }
    }
    public class EndUserGame : UsesOfProject
    {
        public EndUserGame(Form1 project) : base(project) { }
        private async Task DeleteFromPage1(int index)
        {
            await project.signalR.Connection.InvokeAsync("DeleteFromPage1",project.userInformation.name, project.userInformation.nameOpponent, index);
        }
        public void EndGame(int index)
        {
            DeleteFromPage1(index);
        }
    }
}
