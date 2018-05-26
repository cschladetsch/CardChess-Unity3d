namespace App.Model
{
    using Common;
    using Common.Message;
    using Registry;

    /// <summary>
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
        public Coord Coord { get; set; }
        public int Power => Card.Power;
        public int Health => Card.Health;
        public bool Dead => Health <= 0;
        public bool Alive => !Dead;

        public PieceModel(IPlayerModel a0, ICardModel a1)
        {
            Construct(a0, a1);
        }

        public bool Construct(IPlayerModel a0, ICardModel a1)
        {
            base.Construct(a0);
            Card = a1;
            return true;
        }

        public Response Attack(IPieceModel defender)
        {
            var attack = defender.TakeDamage(this);
            if (attack.Failed)
                return attack;
            var defend = TakeDamage(defender);
            if (defend.Failed)
                return defend;
            if (Dead)
                return Board.Remove(this);
            return Board.TryMovePiece(new MovePiece(Player, this, defender.Coord));
        }

        public Response TakeDamage(IPieceModel defender)
        {
            var resp = Card.TakeDamage(this, defender);
            if (resp.Failed)
                return resp;
            if (Health <= 0)
                Board.Remove(this);
            return Response.Ok;
        }

        public override string ToString()
        {
            return $"{Player}'s {Type} @{Coord} with {Power}/{Health}";
        }
    }
}
