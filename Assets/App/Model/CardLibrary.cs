using System;

namespace App.Model
{
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
            for (var n = 0; n < 50; ++n)
                Add(Database.CardTemplates.GetRandom());

            return true;
        }
    }
}
