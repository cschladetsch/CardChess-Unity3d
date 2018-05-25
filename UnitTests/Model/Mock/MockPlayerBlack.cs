
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
		}

		public override IAction Mulligan()
		{
			return new Action.Pass(this);
		}

		public override IAction NextAction()
		{
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
	}		
}

