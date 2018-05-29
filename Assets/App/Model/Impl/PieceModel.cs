using UniRx;

namespace App.Model
{
    using Common;
    using Common.Message;
    using Registry;

    /// <summary>
    /// Model of a piece on the board.
    /// </summary>
    public class PieceModel
        : PlayerOwnedModelBase
        , IPieceModel
    {
        public ICardModel Card { get; private set; }
        public EPieceType Type => Card.PieceType;
        [Inject] public IBoardModel Board { get; set; }
        public IReactiveProperty<Coord> Coord => _coord;
        public IReactiveProperty<int> Power => Card.Power;
        public IReactiveProperty<int> Health => Card.Health;
        public IReactiveProperty<bool> Dead => _dead;

        public bool Construct(IPlayerModel player, ICardModel card)
        {
            if (!base.Construct(player))
                return false;

            Card = card;
            Health.Subscribe(h => Dead.Value = h <= 0).AddTo(this);
            Dead.Subscribe(d => Died()).AddTo(this);
            return true;
        }

        void Died()
        {
            Info($"{this} died");
            Board.Remove(this);
        }

        public Response Attack(IPieceModel defender)
        {
            var attack = defender.TakeDamage(this);
            if (attack.Failed)
                return attack;
            var defend = TakeDamage(defender);
            if (defend.Failed)
                return defend;
            if (defender.Dead.Value && !Dead.Value)
            {
                return Board.TryMovePiece(
                    new MovePiece(Player, this, defender.Coord.Value));
            }

            return Response.Ok;
        }

        public Response TakeDamage(IPieceModel defender)
        {
            return Card.TakeDamage(this, defender);
        }

        public override string ToString()
        {
            return $"{Player}'s {Type} @{Coord} with {Power}/{Health}";
        }

        private readonly ReactiveProperty<Coord> _coord = new ReactiveProperty<Coord>();
        readonly BoolReactiveProperty _dead = new BoolReactiveProperty(false);
    }
}
