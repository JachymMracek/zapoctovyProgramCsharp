using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using static zapoctovyProgramFinal.Form1;

namespace zapoctovyProgramFinal
{
    public class ColourCard : UsesOfProject
    {
        public Button button { get; set; }
        public bool choose = false;
        public ColourCard(Form1 project) : base(project) { }
        public async void Symbol_Click(object sender, EventArgs e)
        {
            await new PrsiGame(project).Play(((Button)sender).Name, ((Button)sender).Image, ((Page7)project.pages[6]).prsiTable.cardTable.takeCards);

            project.pages[6].SeePage(6);
        }
    }
    public class Blue : ColourCard
    {
        public Blue(Form1 project) : base(project)
        {
            button = new Button { Size = new Size(100, 150), Location = new Point(310, 130), Image = Image.FromFile("C:\\Users\\Admin\\Documents\\ZAPOCTOVY_PROGRAM_Csharp\\barva\\b.png"), ImageAlign = ContentAlignment.MiddleCenter, Name = Path.GetFileNameWithoutExtension("C:\\Users\\Admin\\Documents\\ZAPOCTOVY_PROGRAM_Csharp\\barva\\b.png") };

            button.Click += new EventHandler(Symbol_Click);
        }
    }
    public class Black : ColourCard
    {
        public Black(Form1 project) : base(project)
        {
            button = new Button { Size = new Size(100, 150), Location = new Point(370, 130), Image = Image.FromFile("C:\\Users\\Admin\\Documents\\ZAPOCTOVY_PROGRAM_Csharp\\barva\\c.png"), ImageAlign = ContentAlignment.MiddleCenter, Name = Path.GetFileNameWithoutExtension("C:\\Users\\Admin\\Documents\\ZAPOCTOVY_PROGRAM_Csharp\\barva\\c.png") };

            button.Click += new EventHandler(Symbol_Click);
        }
    }
    public class Red : ColourCard
    {
        public Red(Form1 project) : base(project)
        {
            button = new Button { Size = new Size(100, 150), Location = new Point(430, 130), Image = Image.FromFile("C:\\Users\\Admin\\Documents\\ZAPOCTOVY_PROGRAM_Csharp\\barva\\r.png"), ImageAlign = ContentAlignment.MiddleCenter, Name = Path.GetFileNameWithoutExtension("C:\\Users\\Admin\\Documents\\ZAPOCTOVY_PROGRAM_Csharp\\barva\\r.png") };

            button.Click += new EventHandler(Symbol_Click);
        }
    }
    public class Yellow : ColourCard
    {
        public Yellow(Form1 project) : base(project)
        {
            button = new Button { Size = new Size(100, 150), Location = new Point(490, 130), Image = Image.FromFile("C:\\Users\\Admin\\Documents\\ZAPOCTOVY_PROGRAM_Csharp\\barva\\y.png"), ImageAlign = ContentAlignment.MiddleCenter, Name = Path.GetFileNameWithoutExtension("C:\\Users\\Admin\\Documents\\ZAPOCTOVY_PROGRAM_Csharp\\barva\\y.png") };

            button.Click += new EventHandler(Symbol_Click);
        }
    }
    public class DeckButton : UsesOfProject
    {
        public Button button = new Button { Location = new Point(500, 130), Size = new Size(150, 150), Text = "táhnout" };
        public bool firstMove = true;
        public DeckButton(Form1 project) : base(project)
        {
            button.Click += new EventHandler(DeckButton_Click);
        }
        public async void DeckButton_Click(object sender, EventArgs e)
        {
            if (project.userInformation.nameOpponent == null || ((Page7)project.pages[6]).prsiTable.turnButton.BackColor != Color.LightGreen) { return; }

            for (int i = 0; i < ((Page7)project.pages[6]).prsiTable.cardTable.takeCards; i++)
            {
                PickCard();
            }

            ((Page7)project.pages[6]).SeePage(6);

            if (firstMove) { await project.signalR.Connection.InvokeAsync("FirstImageDeck", ((Page7)project.pages[6]).prsiTable.cardTable.button.Name, new PrsiGame(project).GetImageInBytes(((Page7)project.pages[6]).prsiTable.cardTable.button.Image), project.userInformation.name, project.userInformation.nameOpponent); firstMove = false; }

            await new PrsiGame(project).Play(null, null, 0);
        }
        private void PickCard()
        {
            ((Page7)project.pages[6]).prsiPlayer.AddToHand(((Page7)project.pages[6]).prsiTable, project);

            ((Page7)project.pages[6]).AddToButtonsCrdsInHand(((Page7)project.pages[6]).prsiPlayer.cardsInHand[((Page7)project.pages[6]).prsiPlayer.cardsInHand.Count - 1]);
        }
    }
    public class CardTable
    {
        public Button button = new Button { Location = new Point(310, 130), Size = new Size(150, 150), Text = "", BackColor = Color.LightGray };

        public bool valid = false;

        public int takeCards = 1;
        public CardTable() { }
    }
    public class PrsiTable : EndGameButton
    {
        public CardTable cardTable = new CardTable();
        public Button turnButton = new Button { Location = new Point(530, 30), Size = new Size(100, 100), Text = "", BackColor = Color.LightCoral };
        public DeckButton deckButton { get; set; }
        public List<Card> cardsInDeck = new List<Card>();
        public PrsiTable(Form1 project) : base(project)
        {
            this.deckButton = new DeckButton(project);

            string directoryPath = @"C:\Users\Admin\Documents\ZAPOCTOVY_PROGRAM_Csharp\obrazky";

            string[] files = Directory.GetFiles(directoryPath);

            foreach (string file in files) { AddCardToDeck(file); }

            cardsInDeck = cardsInDeck.OrderBy(x => new Random().Next()).ToList();
        }
        private void AddCardToDeck(string pathImage)
        {
            Card card = new Card(pathImage, project);

            if (card.ButtonCard.Name.Substring(1) == "7") { card = new SevenCard(pathImage, project); }

            else if (card.ButtonCard.Name.Substring(1) == "12") { card = new SvrsekCard(pathImage, project); }

            else if (card.ButtonCard.Name.Substring(1) == "14") { card = new EsoCard(pathImage, project); }

            else { card = new NormalCard(pathImage, project); }

            cardsInDeck.Add(card);
        }
    }
    public class PrsiPlayer : UsesOfProject
    {
        public List<Card> cardsInHand = new List<Card>();
        public PrsiTable prsiTable { get; set; }
        public PrsiPlayer(PrsiTable prsiTable, Form1 project) : base(project)
        {
            for (int i = 0; i < 4; i++)
            {
                this.prsiTable = prsiTable;

                Card card = prsiTable.cardsInDeck[0];

                card.ButtonCard.Location = new Point(card.ButtonCard.Location.X + (i * 100), card.ButtonCard.Location.Y);

                cardsInHand.Add(card);

                prsiTable.cardsInDeck.RemoveAt(0);
            }
        }
        public void AddToHand(PrsiTable prsiTable, Form1 project)
        {
            Card card = prsiTable.cardsInDeck[0];

            card.ButtonCard.Location = new Point(200 + ((cardsInHand.Count) * 100), card.ButtonCard.Location.Y);

            cardsInHand.Add(card);

            prsiTable.cardsInDeck.RemoveAt(0);
        }
        public void MatchCards()
        {
            for (int i = 0; i < cardsInHand.Count; i++)
            {
                int index = project.Controls.IndexOf(((Page7)project.pages[6]).prsiPlayer.cardsInHand[i].ButtonCard);

                Point LocationChange = new Point(200 + (i * 100), 290);

                project.Controls[index].Location = LocationChange;

                ((Page7)project.pages[6]).prsiPlayer.cardsInHand[i].ButtonCard.Location = LocationChange;
            }
        }
    }
    public class GreenButtonSetPlayable : UsesOfProject
    {
        public GreenButtonSetPlayable(Form1 project) : base(project) { }
        public bool GreenButtonSetProject()
        {
            if (((Page7)project.pages[6]).prsiTable.turnButton.BackColor == Color.LightGreen) { return true; }

            return false;
        }
    }
    public class Card : GreenButtonSetPlayable
    {
        public Button ButtonCard { get; set; }
        public Card(string pathImage, Form1 project) : base(project)
        {
            ButtonCard = new Button { Size = new Size(100, 150), Location = new Point(200, 290), Image = Image.FromFile(pathImage), ImageAlign = ContentAlignment.MiddleCenter, Name = Path.GetFileNameWithoutExtension(pathImage) };
            ButtonCard.Click += new EventHandler(CardButton_Click);
        }
        public bool Playable(Button sender)
        {
            if ((((Button)sender).Name[0] == ((Page7)project.pages[6]).prsiTable.cardTable.button.Name[0] || ((Button)sender).Name.Substring(1) == ((Page7)project.pages[6]).prsiTable.cardTable.button.Name.Substring(1)) && GreenButtonSetProject())
            {
                return true;
            }
            return false;
        }
        public bool EsoPlayed()
        {
            if (((Page7)project.pages[6]).prsiTable.cardTable.button.Name.Substring(1) == "14" && ((Page7)project.pages[6]).prsiTable.cardTable.valid)
            {
                return true;
            }
            return false;
        }
        public bool SevenPlayed()
        {
            if (((Page7)project.pages[6]).prsiTable.cardTable.button.Name.Substring(1) == "7" && ((Page7)project.pages[6]).prsiTable.cardTable.valid)
            {
                return true;
            }
            return false;
        }
        public void Delete(Button sender)
        {
            Card cardToRemove = ((Page7)project.pages[6]).prsiPlayer.cardsInHand.FirstOrDefault(c => c.ButtonCard.Name == ((Button)sender).Name);

            ((Page7)project.pages[6]).prsiPlayer.cardsInHand.Remove(cardToRemove);

            project.Controls.Remove((Button)sender);

            ((Page7)project.pages[6]).buttons.Remove((Button)sender);

            ((Page7)project.pages[6]).prsiPlayer.MatchCards();

            ((Page7)project.pages[6]).prsiTable.deckButton.firstMove = false;
        }
        public async Task PlayCard(Button sender)
        {
            Delete(sender);

            if (((Page7)project.pages[6]).prsiTable.turnButton.BackColor == Color.LightCoral) { return; }

            await new PrsiGame(project).Play(((Button)sender).Name, ((Button)sender).Image, ((Page7)project.pages[6]).prsiTable.cardTable.takeCards);
        }
        public virtual void CardButton_Click(object sender, EventArgs e) { }
        public virtual bool Stop(object sender) { return true; }
    }
    public class SpecialCard : Card
    {
        public SpecialCard(string pathImage, Form1 project) : base(pathImage, project) { }
    }
    public class SevenCard : SpecialCard
    {
        public SevenCard(string pathImage, Form1 project) : base(pathImage, project) { }
        public override void CardButton_Click(object sender, EventArgs e)
        {
            if (Stop(sender)) { return; }

            else if (((Page7)project.pages[6]).prsiTable.cardTable.takeCards == 1) { ((Page7)project.pages[6]).prsiTable.cardTable.takeCards++; }

            else { ((Page7)project.pages[6]).prsiTable.cardTable.takeCards += 2; }

            PlayCard((Button)sender);
        }
        public override bool Stop(object sender)
        {
            if (project.userInformation.nameOpponent == null) { return true; }

            if (!Playable((Button)sender) || EsoPlayed()) { return true; }

            return false;
        }
    }
    public class NormalCard : Card
    {
        public NormalCard(string pathImage, Form1 project) : base(pathImage, project) { }
        public override async void CardButton_Click(object sender, EventArgs e)
        {
            if (Stop(sender)) { return; }

            PlayCard((Button)sender);
        }
        public override bool Stop(object sender)
        {
            if (project.userInformation.nameOpponent == null) { return true; }

            if (!Playable((Button)sender) || EsoPlayed() || SevenPlayed()) { return true; }

            return false;
        }
    }
    public class SvrsekCard : SpecialCard
    {
        public bool svrsekCard = true;
        public SvrsekCard(string pathImage, Form1 project) : base(pathImage, project) { }
        public override void CardButton_Click(object sender, EventArgs e)
        {
            if (Stop(sender)) { return; }

            Delete((Button)sender);

            project.pages[7].SeePage(7);
        }
        public override bool Stop(object sender)
        {
            if (EsoPlayed() || SevenPlayed()) { return true; }

            return false;
        }
    }
    public class EsoCard : SpecialCard
    {
        public EsoCard(string pathImage, Form1 project) : base(pathImage, project) { }
        public override void CardButton_Click(object sender, EventArgs e)
        {
            if (Stop(sender)) { return; }

            ((Page7)project.pages[6]).prsiTable.cardTable.takeCards = 0;

            PlayCard((Button)sender);
        }
        public override bool Stop(object sender)
        {
            if (!Playable((Button)sender) || SevenPlayed()) { return true; }

            return false;
        }
    }
    public class MovePrsi : UsesOfProject
    {
        public MovePrsi(Form1 project) : base(project) { }

        public void ChangeDeckNameImage(byte[] imageBytes, string nameDeck)
        {
            using (var ms = new MemoryStream(imageBytes))
            {
                ((Page7)project.pages[6]).prsiTable.cardTable.button.Image = Image.FromStream(ms);
            }

            ((Page7)project.pages[6]).prsiTable.cardTable.button.Name = nameDeck;
        }
        public void ChangePlay()
        {
            if (((Page7)project.pages[6]).prsiTable.turnButton.BackColor == Color.LightCoral) { ((Page7)project.pages[6]).prsiTable.turnButton.BackColor = Color.LightGreen; }

            else { ((Page7)project.pages[6]).prsiTable.turnButton.BackColor = Color.LightCoral; }

            if (((Page7)project.pages[6]).prsiPlayer.cardsInHand.Count == 0) { ShowEndPage(); }
        }
        public async Task ShowEndPage()
        {
            await project.signalR.Connection.InvokeAsync("ShowEndPage", project.userInformation.name, project.userInformation.nameOpponent);
        }
    }
}
