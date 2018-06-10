using System;
using System.Collections;

using Flow;

namespace App.Agent
{
    using Model;
    using Common.Message;

    public class DeckAgent
        //: CardCollection
        : AgentBaseCoro<IDeckModel>
        , IDeckAgent
    {
        public event Action<ICardAgent> OnDraw;

        public int MaxCards => Parameters.MinCardsInDeck;
        //public IEnumerable<ICardAgent> Cards => null;//base.Cards.OfType<ICardAgent>();

        public DeckAgent(IDeckModel model) : base(model)
        {
        }

        public IChannel<ICardAgent> DrawCards(uint n)
        {
            var channel = New.Channel<ICardAgent>();
            //Node.Add(New.Coroutine(DrawCardsCoro, n, channel));
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
                yield return card;
            }
        }

        public IFuture<Response> NewGame()
        {
            Model.Prepare();
            return null;
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

        public void AddToBottom(ICardAgent card)
        {
            Model.AddToBottom(card.Model);
        }

        public void Remove(ICardAgent card)
        {
            Model.Remove(card.Model);
        }

        //protected override IEnumerator Next(IGenerator self)
        //{
        //    yield return null;
        //}

    }
}
