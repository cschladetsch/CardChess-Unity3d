using System;
using System.Linq;

namespace App.Service.Impl
{
	using App.Model;
	using App.Common;
	using App.Registry;
    
	internal class CardTemplateService
		: ICardTemplateService
	    , IKnown, IHasDestroyHandler<ICardTemplateService>
    {
		public Guid Id { get; set; }
		public event DestroyedHandler<ICardTemplateService> OnDestroy;

		public ICardTemplate GetCard(EPieceType pieceType)
		{
			return Database.CardTemplates.OfType(pieceType).First();
		}

        public ICardTemplate GetCard(Guid id)
		{
			return Database.CardTemplates.Get(id);
		}
    }
}
