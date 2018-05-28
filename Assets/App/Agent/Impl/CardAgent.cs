using System.Collections;
using System.Collections.Generic;
using App.Common.Message;
using App.Model;
using Flow;
using UniRx;

namespace App.Agent
{
    using Common;

    public abstract class CardAgent :
        AgentBaseCoro<Model.ICardModel>,
        ICardAgent
    {
        private ReactiveProperty<IPlayerModel> _player;

        #region Public Fields
        public int ManaCost => Model.ManaCost;
        public int Power => Model.Power;
        public int Health => Model.Health;
        public string Description => Model.Description;
        public ECardType Type => Model.Type;
        public EPieceType PieceType => Model.PieceType;

        public ICardTemplate Template => Model.Template;

        public ReactiveProperty<IPlayerModel> Player { get; }

        public ReactiveCollection<IEnumerable<IEffect>> Effects { get; }
        public ReactiveCollection<IEnumerable<ItemModel>> Items { get; }
        public ReactiveProperty<IEnumerable<EAbility>> Abilities { get; }
        #endregion

        #region Public Methods

        //public IFuture<Action.MovePiece> Move()
        //{
        //    throw new System.NotImplementedException();
        //}

        //#region Abstract Methods
        ///// <summary>
        ///// Where this card can move to
        ///// </summary>
        //public abstract IEnumerable<Coord> PotentialCoords();

        ///// <summary>
        ///// What attacks this card from the given coord
        ///// </summary>
        ///// <param name="coord">From this coord</param>
        //public abstract IEnumerable<ICardAgent> Attackers(Coord coord);

        ///// <summary>
        ///// What this card can attack
        ///// </summary>
        ///// <param name="coord"></param>
        //public abstract IEnumerable<ICardAgent> Defenders(Coord coord);

        ///// <summary>
        ///// Guards defending this piece from the given coord
        ///// </summary>
        ///// <param name="coord"></param>
        ///// <returns></returns>
        //public abstract IEnumerable<ICardAgent> Guards(Coord coord);
        //#endregion

        #endregion

        #region Protected Methods
        protected override IEnumerator Next(IGenerator self)
        {
            yield break;
        }
        #endregion

        #region Private Fields
        #endregion

        public ITransient TakeDamage(IPieceAgent self, IPieceAgent attacker)
        {
            throw new System.NotImplementedException();
        }
    }
}
