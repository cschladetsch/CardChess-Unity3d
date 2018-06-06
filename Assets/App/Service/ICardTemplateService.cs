using System;

namespace App.Service
{
    using Model;
    using Agent;
    using Common;

    public interface ICardTemplateService
        : IModel
    {
        ICardTemplate GetCardTemplate(EPieceType pieceType);
        ICardTemplate GetCardTemplate(Guid id);

        Model.ICardModel NewCardModel(IPlayerModel owner, ICardTemplate tmpl);
        Model.ICardModel NewCardModel(IPlayerModel owner, EPieceType type);
    }

}
