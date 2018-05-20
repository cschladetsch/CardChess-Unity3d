using System;
using App.Common;

namespace App.Model
{
    public class ArbiterModel
        : ModelBase,
        IOwner,
        IArbiterModel
    {
        public EColor Color => EColor.Neutral;
        public IBoardModel Board { get; }
        public IPlayerModel WhitePlayer { get; }
        public IPlayerModel BlackPlayer { get; }

        public ArbiterModel()
        {
        }

        public ArbiterModel(IBoardModel board, IPlayerModel w, IPlayerModel b)
        {
            Construct(this);
            Board = board;
            WhitePlayer = w;
            BlackPlayer = b;
            _players = new[] {w, b};
        }

        public void NewGame()
        {
            Board.NewGame(this);
            foreach (var p in _players)
                p.NewGame();
        }

        public void Endame()
        {
            throw new System.NotImplementedException();
        }

        public void PrepareDecks()
        {
            throw new System.NotImplementedException();
        }

        public void DrawCards()
        {
            throw new System.NotImplementedException();
        }

        public void PlayerTurn()
        {
            throw new System.NotImplementedException();
        }

        private IPlayerModel[] _players;
    }
}
