using System;

namespace App.Service
{
	using Model;
	using Common;
	using Registry;

    public interface ICardTemplateService
		: IKnown, IHasDestroyHandler<ICardTemplateService>
    {
		ICardTemplate GetCard(EPieceType pieceType);
		ICardTemplate GetCard(Guid id);
    }
}
