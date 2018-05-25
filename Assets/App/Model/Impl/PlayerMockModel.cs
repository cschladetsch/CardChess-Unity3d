using System;

using App.Common;
using App.Action;

namespace App.Model
{
    public class PlayerMockModel
		: PlayerModelBase
    {
		public PlayerMockModel(EColor color) : base(color)
		{
		}

		public override IAction Mulligan()
		{
			throw new NotImplementedException();
		}

		public override IAction PlaceKing()
		{
			throw new NotImplementedException();
		}

		public override IAction StartTurn()
		{
			throw new NotImplementedException();
		}

		public override IAction NextAction()
		{
			throw new NotImplementedException();
		}
	}
}
