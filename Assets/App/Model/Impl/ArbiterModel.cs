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
        [Inject] public IBoardModel Board { get; }
        public EColor Color => EColor.Neutral;
        public IPlayerModel WhitePlayer { get; private set; }
        public IPlayerModel BlackPlayer { get; private set; }

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
