using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zapoctovyProgramFinal
{
    public class UsesOfProject
    {
        protected Form1 project { get; set; }
        public UsesOfProject(Form1 project)
        {
            this.project = project;
        }
    }
    public class UserInformation
    {
        public string name = "";
        public string nameOpponent { get; set; }

        public int GameTypePage = 0;

        public bool make = false;

        public int opakovat = 0;

        public int timeSet = 0;

        public bool threes = false;

        public UserInformation() { }
    }
}
