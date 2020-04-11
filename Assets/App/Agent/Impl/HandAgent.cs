namespace App.Agent
{
    using UniRx;
    using Dekuple.Agent;
    using Model;

    /// <inheritdoc cref="AgentBaseCoro{TModel}" />
    /// <summary>
    /// Representative for the Hand owned by the player.
    /// </summary>
    public class HandAgent
        : AgentBaseCoro<IHandModel>
        , IHandAgent
    {
        public IReadOnlyReactiveCollection<ICardAgent> Cards => _cards;
        private readonly ReactiveCollection<ICardAgent> _cards = new ReactiveCollection<ICardAgent>();

        public HandAgent(IHandModel model)
            : base(model)
        {
        }

        public void StartGame()
        {
            Model.StartGame();
            Verbose(10, $"{this} created");
            foreach (var c in Model.Cards)
                _cards.Add(Registry.Get<ICardAgent>(c));

            Model.Cards.ObserveAdd().Subscribe(Add).AddTo(this);
            Model.Cards.ObserveRemove().Subscribe(Remove);
        }

        public void EndGame()
        {
            throw new System.NotImplementedException();
        }

        private void Remove(CollectionRemoveEvent<ICardModel> remove)
        {
            Verbose(10, $"HandAgent: Remove {remove.Value} @{remove.Index}");
            var index = remove.Index;
            _cards.RemoveAt(index);
        }

        private void Add(CollectionAddEvent<ICardModel> add)
        {
            Verbose(10, $"HandAgent: Add {add.Value} @{add.Index}");
            _cards.Insert(add.Index, Registry.Get<ICardAgent>(add.Value));
        }
    }
}
