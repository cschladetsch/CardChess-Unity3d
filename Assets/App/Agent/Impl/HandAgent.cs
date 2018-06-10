using UniRx;

namespace App.Agent
{
    using Model;

    public class HandAgent
        : AgentBaseCoro<IHandModel>
        , IHandAgent
    {
        public IReadOnlyReactiveCollection<ICardAgent> Cards => _cards;

        public HandAgent(IHandModel model)
            : base(model)
        {
            model.Cards.ObserveAdd().Subscribe(Add).AddTo(Model);
            model.Cards.ObserveRemove().Subscribe(Remove).AddTo(Model);
        }

        public void StartGame()
        {
            Model.StartGame();
        }

        void Remove(CollectionRemoveEvent<ICardModel> remove)
        {
            Info($"HandAgent: Remove {remove.Value} @{remove.Index}");
            var index = remove.Index;
            var card = _cards[index];
            card.Destroy();
            _cards.RemoveAt(index);
        }

        void Add(CollectionAddEvent<ICardModel> add)
        {
            Info($"HandAgent: Add {add.Value} @{add.Index}");
            _cards.Insert(add.Index, Registry.New<ICardAgent>(add.Index));
        }

        private readonly ReactiveCollection<ICardAgent> _cards = new ReactiveCollection<ICardAgent>();
    }
}
