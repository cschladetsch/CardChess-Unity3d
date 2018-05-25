
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

        }

        public override Response NewGame()
        {
            base.NewGame();

            _actions = new List<IRequest>()
            {
                new PlayCard(this, King, new Coord(4, 5)),
                new AcceptCards(this),
            };
            return Response.Ok;
        }

        public override IRequest Mulligan()
        {
            return _pass;
        }

        public override IRequest NextAction()
        {
            return null;
        }

        public override IRequest StartTurn()
        {
            return new Action.Pass(this);
        }

        private IRequest _pass;
        private List<IRequest> _actions = new List<IRequest>();

    }
}

