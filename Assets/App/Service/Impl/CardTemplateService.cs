using System;
using System.Linq;

namespace App.Service.Impl
{
	using Model;
	using Common;
	using Registry;
    
	public class CardTemplateService
		: ModelBase
	    , ICardTemplateService
	    , IKnown
    {
		public ICardTemplate GetCardTemplate(EPieceType pieceType)
		{
			return Database.CardTemplates.OfType(pieceType).First();
		}

        public ICardTemplate GetCardTemplate(Guid id)
		{
			return Database.CardTemplates.Get(id);
		}

		public ICardModel NewCardModel(IPlayerModel owner, ICardTemplate tmpl)
		{
			return Registry.New<ICardModel>(tmpl, owner);
		}

		public ICardModel NewCardModel(IPlayerModel owner, EPieceType type)
		{
			return Registry.New<ICardModel>(GetCardTemplate(type), owner);
		}
	}
}
