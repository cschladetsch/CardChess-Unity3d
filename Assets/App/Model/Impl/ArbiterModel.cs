using System;
using App.Common;

// DI fails this inspection test
// ReSharper disable UnassignedGetOnlyAutoProperty

namespace App.Model
{
    using Registry;

    public class ArbiterModel
        : ModelBase
        , IOwner
        , IArbiterModel
    {
        #region Public Properties

        public EGameState GameState { get; private set;}
        [Inject] public IBoardModel Board { get; set;}
        public EColor Color => EColor.Neutral;
        public IPlayerModel WhitePlayer { get; private set; }
        public IPlayerModel BlackPlayer { get; private set; }
        public IPlayerModel CurrentPlayer => _players[_currentPlayer];
        public IPlayerModel OtherPlayer => _players[(_currentPlayer + 1)%2];
        #endregion

        #region Public Methods
        public ArbiterModel()
        {
        }

        public void NewGame(IPlayerModel w, IPlayerModel b)
        {
            WhitePlayer = w;
            BlackPlayer = b;
            _players = new[] {w, b};
            _currentPlayer = 0;

            Construct(this);
            Board.NewGame();
            foreach (var p in _players)
                p.NewGame();

            GameState = EGameState.Ready;
        }

        public void Endame()
        {
            GameState = EGameState.Completed;
        }

        #endregion

        #region Private Fields
        private IPlayerModel[] _players;
        private int _currentPlayer;

        #endregion
    }
}
