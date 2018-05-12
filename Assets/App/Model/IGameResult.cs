using System;

namespace App.Model
{
    public interface IGameResult
    {
        Guid WhitePlayer { get; }
        Guid BlackPlayer { get; }
        Guid BlackDeck { get; }
        Guid WhiteDeck { get; }
        DateTime Played { get; }
        TimeSpan Duration { get; }
        EGameResult Result { get; }
    }
}
