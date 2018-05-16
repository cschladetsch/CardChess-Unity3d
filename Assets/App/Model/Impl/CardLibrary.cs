using System;

namespace App.Model
{
    using Common;

    public class CardLibrary :
        CardCollection<ICardTemplate>,
        ICreateWith<Guid>
    {
        public override int MaxCards => 1000;

        public CardLibrary()
        {
        }

        public bool Create(Guid id)
        {
            // TODO: lookup library using guid
            for (var n = 0; n < Parameters.MinCardsInDeck; ++n)
                Cards.Add(Database.CardTemplates.GetRandom());

            return true;
        }
    }
}
