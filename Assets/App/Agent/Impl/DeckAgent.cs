using System;
using System.Collections;
using Dekuple.Agent;
using Flow;
using UniRx;

namespace App.Agent
{
    using Model;

    /// <summary>
    /// Representative for the deck of cards owned by the player
    /// </summary>
    public class DeckAgent
        : AgentBaseCoro<IDeckModel>
        , IDeckAgent
    {
        public event Action<ICardAgent> OnDraw;
        public int MaxCards => Parameters.MinCardsInDeck;
        public IReadOnlyReactiveCollection<ICardAgent> Cards => _cards;

        public DeckAgent(IDeckModel model)
            : base(model)
        {
        }

        public override void StartGame()
        {
            Model.StartGame();
            foreach (var c in Model.Cards)
                _cards.Add(Registry.New<ICardAgent>(c));
            Model.Cards.ObserveAdd().Subscribe(Add);
            Model.Cards.ObserveRemove().Subscribe(RemoveCard);
        }

        public IChannel<ICardAgent> DrawCards(uint n)
        {
            var channel = New.Channel<ICardAgent>();
            _Node.Add(New.Coroutine(DrawCardsCoro, n, channel));
            return channel;
        }

        public IEnumerator DrawCardsCoro(IGenerator self, uint n, IChannel<ICardAgent> channel)
        {
            while (n-- > 0)
            {
                var card = Draw();
                yield return self.After(card);
                if (!card.Available)
                    yield break;
                channel.Insert(card.Value);

                yield return self.ResumeAfter(TimeSpan.FromSeconds(0.020));
            }
        }

        public IFuture<ICardAgent> Draw()
        {
            var cardModel = Model.Draw();
            var futureAgent = New.Future<ICardAgent>();
            var agent = Registry.New<ICardAgent>(cardModel);
            OnDraw?.Invoke(agent);
            futureAgent.Value = agent;
            return futureAgent;
        }

        private void Add(CollectionAddEvent<ICardModel> add)
        {
            Verbose(6, $"DeckAgent: Add {add.Value} @{add.Index}");
            _cards.Insert(add.Index, Registry.New<ICardAgent>(add.Value));
        }

        private void RemoveCard(CollectionRemoveEvent<ICardModel> remove)
        {
            Verbose(6, $"DeckAgent: Remove {remove.Value} @{remove.Index}");
            var index = remove.Index;
            //var card = _cards[index];
            //card.Destroy();
            _cards.RemoveAt(index);
        }

        private readonly ReactiveCollection<ICardAgent> _cards = new ReactiveCollection<ICardAgent>();
    }
}
