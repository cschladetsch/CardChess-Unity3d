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
        }

        public override void StartGame()
        {
            Model.StartGame();
            Info($"HandAgent {PlayerModel} created");
            foreach (var c in Model.Cards)
                _cards.Add(Registry.New<ICardAgent>(c));

            Model.Cards.ObserveAdd().Subscribe(Add);
            Model.Cards.ObserveRemove().Subscribe(Remove);
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
            _cards.Insert(add.Index, Registry.New<ICardAgent>(add.Value));
        }

        private readonly ReactiveCollection<ICardAgent> _cards = new ReactiveCollection<ICardAgent>();
    }
}
