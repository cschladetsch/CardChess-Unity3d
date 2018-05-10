using System;

namespace App.Model
{
    public interface IDeck : ICardCollection
    {
        Guid Template { get; }
        void NewGame();
        void Shuffle();
        ICardInstance Draw();
        void AddToBottom(ICardInstance card);
        void AddToRandom(ICardInstance card);
    }
}
