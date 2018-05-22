using System;
using App.Common;

// DI fails this inspection test
// ReSharper disable UnassignedGetOnlyAutoProperty

namespace App.Model
{
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

        public void SetPlayers(IPlayerModel w, IPlayerModel b)
        {
            WhitePlayer = w;
            BlackPlayer = b;
            _players = new[] {w, b};

            Construct(this);
        }

        public void NewGame()
        {
            Board.NewGame();
            foreach (var p in _players)
                p.NewGame();
            _currentPlayer = 0;
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
