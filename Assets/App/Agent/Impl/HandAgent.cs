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
            Verbose(30, $"HandAgent {PlayerModel} created");
            foreach (var c in Model.Cards)
                _cards.Add(Registry.New<ICardAgent>(c));

            Model.Cards.ObserveAdd().Subscribe(Add);//.AddTo(this);
            Model.Cards.ObserveRemove().Subscribe(Remove);
        }

        private void Remove(CollectionRemoveEvent<ICardModel> remove)
        {
            Verbose(0, $"HandAgent: Remove {remove.Value} @{remove.Index}");
            var index = remove.Index;
            var card = _cards[index];
            //card.Destroy();
            _cards.RemoveAt(index);
        }

        private void Add(CollectionAddEvent<ICardModel> add)
        {
            Verbose(0, $"HandAgent: Add {add.Value} @{add.Index}");
            _cards.Insert(add.Index, Registry.New<ICardAgent>(add.Value));
        }

        private readonly ReactiveCollection<ICardAgent> _cards = new ReactiveCollection<ICardAgent>();
    }
}
