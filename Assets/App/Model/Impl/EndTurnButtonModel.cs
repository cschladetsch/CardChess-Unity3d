using System.Linq;
using UniRx;

namespace App.Model.Impl
{
    using Common;
    using Model;
    using Registry;

    public class EndTurnButtonModel
        : ModelBase
        , IEndTurnButtonModel
    {
        public IReadOnlyReactiveProperty<bool> Interactive => _isInteractive;
        public IReadOnlyReactiveProperty<bool> PlayerHasOptions => _playerHasOptions;

        [Inject] public IBoardModel _board;
        [Inject] public IArbiterModel _arbiter;

        public EndTurnButtonModel(IOwner owner) : base(owner) { }

        public override void PrepareModels()
        {
            base.PrepareModels();

            _arbiter.CurrentPlayer.Subscribe(p => _isInteractive.Value = p == PlayerModel);

            var canPlay = PlayerModel.Hand.Cards
                .Select(c => c.ManaCost.Value <= PlayerModel.Mana.Value)
                .ToReactiveCollection()
                .ObserveCountChanged()
                .Where(n => n > 0)
                .ToReactiveProperty()
                .AddTo(this);
            var canMove = _board.Pieces
                .Where(p => p.SameOwner(this))
                .Select(p => !p.MovedThisTurn && !p.AttackedThisTurn)
                .ToReactiveCollection()
                .ObserveCountChanged()
                .Where(n => n > 0)
                .ToReactiveProperty()
                .AddTo(this);

            canPlay
                .CombineLatest(canMove, (play, move) => play > 0 || move > 0)
                .Subscribe(any => _playerHasOptions.Value = any)
                .AddTo(this);
        }

        private readonly BoolReactiveProperty _isInteractive = new BoolReactiveProperty();
        private readonly BoolReactiveProperty _playerHasOptions = new BoolReactiveProperty();
    }
}
