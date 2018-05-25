
namespace App.Model.Test
{
	using App.Action;
	using Common;
	using Model;

	public class MockPlayerWhite
		: PlayerModelBase, IWhitePlayer
	{
		public MockPlayerWhite()
			: base(EColor.White)
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

