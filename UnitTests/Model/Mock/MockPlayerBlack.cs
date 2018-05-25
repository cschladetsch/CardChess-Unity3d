
using System.Collections.Generic;

namespace App.Model.Test
{
    using App.Action;
    using Common;
    using Model;

    public interface IWhitePlayer : IPlayerModel { }
    public interface IBlackPlayer : IPlayerModel { }

    public class MockPlayerBlack
        : PlayerModelBase, IBlackPlayer
    {
        public MockPlayerBlack()
            : base(EColor.Black)
        {
            _pass = new Action.Pass(this);

            // play king, accept

            _actions = new List<IAction>()
            {
                new PlayCard(King, new Coord(4, 5)),
                new AcceptCards(this),


            };
        }

        public override IAction Mulligan()
        {
            return _pass;
        }

        public override IAction NextAction()
        {
            // first action is to play King
            return null;
        }



        public override IAction PlaceKing()
        {
            return new Action.PlayCard(King, new Coord(4, 2));
        }

        public override IAction StartTurn()
        {
            return new Action.Pass(this);
        }

        private IAction _pass;
        private List<IAction> _actions = new List<IAction>();

    }
}

