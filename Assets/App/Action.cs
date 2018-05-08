namespace App
{
    public interface IAction
    {
        IPlayer Player { get; }
    }

    public class ActionBase : IAction
    {
        public IPlayer Player { get; private set; }
    }

    public class PlayCard : ActionBase
    {
        public ICard Card;
        public Coord Coord;
    }

    public class Pass : ActionBase
    {
    }

    public class MovePiece : ActionBase
    {
        public IInstance Instance;
        public Coord Target;
    }
}
