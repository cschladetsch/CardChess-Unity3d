using System;

namespace App.Service
{
	using Model;
	using Common;
	using Registry;

    public interface ICardTemplateService
		: IKnown, IHasDestroyHandler<ICardTemplateService>
    {
		ICardModel GetCard(EPieceType pieceType);
		ICardModel GetCard(Guid id);
    }
}
