using System;

namespace App.Service
{
	using Model;
	using Common;
	using Registry;

    public interface ICardTemplateService
		: IKnown
	    , IModel
    {
		ICardTemplate GetCardTemplate(EPieceType pieceType);
		ICardTemplate GetCardTemplate(Guid id);

		ICardModel NewCardModel(IPlayerModel owner, ICardTemplate tmpl);
		ICardModel NewCardModel(IPlayerModel owner, EPieceType type);
    }
}
