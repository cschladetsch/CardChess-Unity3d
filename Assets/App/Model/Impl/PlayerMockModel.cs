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

		public override IRequest Mulligan()
		{
			throw new NotImplementedException();
		}

		public override IRequest PlaceKing()
		{
			throw new NotImplementedException();
		}

		public override IRequest StartTurn()
		{
			throw new NotImplementedException();
		}

		public override IRequest NextAction()
		{
			throw new NotImplementedException();
		}
	}
}
