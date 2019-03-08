using System;
using Dekuple.Model;

namespace App.Service
{
    using Model;
    using Common;

    public interface ICardTemplateService
        : IModel
    {
        ICardTemplate GetCardTemplate(EPieceType pieceType);
        ICardTemplate GetCardTemplate(Guid id);

        ICardModel NewCardModel(IPlayerModel owner, ICardTemplate tmpl);
        ICardModel NewCardModel(IPlayerModel owner, EPieceType type);
    }
}
