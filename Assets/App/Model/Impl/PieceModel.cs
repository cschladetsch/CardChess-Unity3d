using Dekuple;
using UniRx;

namespace App.Model
{
    using Common;
    using Common.Message;

    /// <inheritdoc cref="IPieceModel" />
    /// <summary>
    /// Model of a piece on the board.
    /// </summary>
    public class PieceModel
        : PlayerOwnedModelBase
        , IPieceModel
    {
        [Inject] public IBoardModel Board { get; set; }

        public ICardModel Card { get; }
        public EPieceType PieceType => Card.PieceType;
        public IReactiveProperty<Coord> Coord => _coord;
        public IReadOnlyReactiveProperty<int> Power => Card.Power;
        public IReadOnlyReactiveProperty<int> Health => Card.Health;
        public IReadOnlyReactiveProperty<bool> Dead => Card.Dead;
        public bool AttackedThisTurn { get; set; }
        public bool MovedThisTurn { get; set; }

        private readonly ReactiveProperty<Coord> _coord = new ReactiveProperty<Coord>();

        public PieceModel(IOwner player, ICardModel card)
            : base(player)
        {
            Card = card;
            Dead.Subscribe(dead => { if (dead) Died(); }).AddTo(this);
        }

        private void Died()
        {
            Verbose(5, $"{this} died");
            Board.Remove(this);
        }

        public IResponse Attack(IPieceModel defender)
        {
            var attack = defender.TakeDamage(this);
            if (attack.Failed)
                return attack;
            var defend = TakeDamage(defender);
            if (defend.Failed)
                return defend;
            if (defender.Dead.Value && !Dead.Value)
                return Board.Move(this, defender.Coord.Value);

            return Response.Ok;
        }

        public IResponse TakeDamage(IPieceModel attacker)
        {
            return Card.TakeDamage(attacker.Card);
        }

        public override string ToString()
        {
            return $"{Player}'s {PieceType} @{Coord} with {Power}/{Health}";
        }

        public void NewTurn()
        {
            AttackedThisTurn = MovedThisTurn = false;
        }
    }
}
