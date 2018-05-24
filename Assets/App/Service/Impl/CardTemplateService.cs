using System;

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

		public ICardModel GetCard(EPieceType pieceType)
		{
			return null;
		}

        public ICardModel GetCard(Guid id)
		{
			return null;
		}

		static void Register<T>(IRegistry<T> registry) where T : class, IKnown, IHasDestroyHandler<T>
		{
			//registry.Bind<ICardTemplateService, CardTemplateService>(new CardTemplateService());
		}
    }
}
