using System.Linq;
using Dekuple;
using Dekuple.Model;
using UniRx;

namespace App.Model.Impl
{
    using Model;

    /// <summary>
    /// Logic for state of 'end-turn' button
    /// </summary>
    public class EndTurnButtonModel
        : ModelBase
        , IEndTurnButtonModel
    {
        public IReadOnlyReactiveProperty<bool> Interactive => _isInteractive;
        public IReadOnlyReactiveProperty<bool> PlayerHasOptions => _playerHasOptions;
        public IPlayerModel PlayerModel => Owner.Value as IPlayerModel;

        [Inject] public IBoardModel _board;
        [Inject] public IArbiterModel _arbiter;

        private readonly BoolReactiveProperty _isInteractive = new BoolReactiveProperty();
        private readonly BoolReactiveProperty _playerHasOptions = new BoolReactiveProperty();

        public EndTurnButtonModel(IOwner owner) : base(owner) { }

        public override void PrepareModels()
        {
            base.PrepareModels();

            Info($"{_arbiter} {_arbiter.LastResponse} {PlayerModel}");
            Info($"{Owner} {Owner.GetType()}");
            Info($"{PlayerModel}");

            _arbiter.LastResponse.CombineLatest(PlayerModel.Mana, (p, m) =>
            {
                if (_arbiter.CurrentPlayer.Value != PlayerModel)
                    return false;
                var canPlace = PlayerModel.Hand.Cards.Any(c => c.ManaCost.Value <= m);
                var canMove = m > 1 && _board.Pieces.Where(SameOwner).Any(_board.CanMoveOrAttack);
                //Info($"*** CanMove={canMove}, canPlace={canPlace}, mana={m}, {PlayerModel}: hasOptions={_playerHasOptions.Value}");
                return canPlace || canMove;
            })
            .Subscribe(h => _playerHasOptions.Value = h)
            ;

            _arbiter.CurrentPlayer.Subscribe(p => _isInteractive.Value = p == PlayerModel);

            //    .
            //.LastResponse.Subscribe(resp =>
            //{
            //    var mana = PlayerModel.Mana.Value;
            //    var canPlace = PlayerModel.Hand.Cards.Any(c => c.ManaCost.Value <= mana);
            //    var canMove = mana > 1 && _board.Pieces.Where(SameOwner).Any(_board.CanMoveOrAttack);
            //    _playerHasOptions.Value = canPlace || canMove;
            //    Info($"CanMove={canMove}, canPlace={canPlace}, mana={mana}, {PlayerModel}: hasOptions={_playerHasOptions.Value}");
            //});//.AddTo(this); why does this remove the subscription?>??
        }
    }
}
