using System;

namespace App.Model
{
    public class Deck :
        CardCollection<ICardInstance>,
        IDeck,
        ICreateWith<Guid, IOwner>
    {
        public override int MaxCards => 50;
        public IOwner Owner { get; private set; }

        public bool Create(Guid a0, IOwner owner)
        {
            Owner = owner;
            for (var n = 0; n < 50; ++n)
            {
                var tmpl = Database.CardTemplates.GetRandom();
                var card = Main.Arbiter.Instance.NewCardModel(tmpl, Owner);
                Cards.Add(card);
            }
            return true;
        }

        public bool Create(ITemplateDeck a0)
        {
            throw new NotImplementedException();
        }

        public void NewGame()
        {
            Info("Deck.NewGame: TODO");
        }

        public void Shuffle()
        {
            throw new NotImplementedException();
        }

        public ICardInstance Draw()
        {
            throw new NotImplementedException();
        }

        public void AddToBottom(ICardInstance card)
        {
            throw new NotImplementedException();
        }

        public void AddToRandom(ICardInstance card)
        {
            throw new NotImplementedException();
        }

    }
}
