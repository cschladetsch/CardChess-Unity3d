using System.Text;
using App.Common.Message;
using Flow;
using UniRx;

namespace App.Agent
{
    using Common;
    using Model;

    /// <summary>
    /// Active agent for dealing with the BoardModel. This will be eventually networked,
    /// and the Agent will be local and the Model remote (on server).
    /// </summary>
    public class BoardAgent
        : AgentBaseCoro<IBoardModel>
        , IBoardAgent
    {
        public IReadOnlyReactiveProperty<int> Width => _width;
        public IReadOnlyReactiveProperty<int> Height => _height;
        public IReadOnlyReactiveDictionary<Coord, IPieceAgent> Pieces => _pieces;

        public override bool IsValid
        {
            get
            {
                Info("Test Valid BoardAgent");
                if (!base.IsValid)
                    return false;
                foreach (var kv in _pieces)
                {
                    IPieceAgent agent;
                    var model = Model.At(kv.Key);
                    if (model == null)
                        Assert.IsFalse(_pieces.TryGetValue(kv.Key, out agent));
                    else
                    {
                        Assert.IsTrue(_pieces.TryGetValue(kv.Key, out agent));
                        Assert.AreEqual(agent.Coord.Value, model.Coord.Value);
                    }
                }
                return true;
            }
        }

        public BoardAgent(IBoardModel model)
            : base(model)
        {
            Assert.IsNotNull(model);
            model.Pieces.ObserveAdd().Subscribe(PieceAdded);
            model.Pieces.ObserveRemove().Subscribe(PieceRemoved);
        }

        private void PieceAdded(DictionaryAddEvent<Coord, IPieceModel> add)
        {
            Assert.IsTrue(!_pieces.ContainsKey(add.Key));
            var pieceAgent = Registry.New<IPieceAgent>(add.Value);
            pieceAgent.SetOwner(add.Value.Owner.Value);
            _pieces[add.Key] = pieceAgent;
        }

        private void PieceRemoved(DictionaryRemoveEvent<Coord, IPieceModel> add)
        {
            Assert.IsTrue(_pieces.ContainsKey(add.Key));
            IPieceAgent agent;
            _pieces.TryGetValue(add.Key, out agent);
            Assert.IsNotNull(agent);
            _pieces.Remove(add.Key);
            agent.Destroy();
        }

        public string Print()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"BoardAgent: {_pieces.Count} pieces:");
            foreach (var kv in _pieces)
            {
                sb.AppendLine($"\t{kv.Key} -> {kv.Value}");
            }
            return sb.ToString();
        }

        public override void StartGame()
        {
            base.StartGame();
            Model.StartGame();
            _pieces.Clear();
        }

        public override void EndGame()
        {
            Info($"BoardAgent EndGame");
            _pieces.Clear();
        }

        public IResponse Move(IPieceAgent agent, Coord coord)
        {
            return Model.Move(agent.Model, coord);
        }

        public IResponse Remove(IPieceAgent agent)
        {
            Assert.IsTrue(_pieces.ContainsKey(agent.Coord.Value));
            return Model.Remove(agent.Model);
        }

        public IResponse Add(IPieceAgent agent)
        {
            Assert.IsFalse(_pieces.ContainsKey(agent.Coord.Value));
            return Model.Add(agent.Model);
        }

        public IPieceAgent At(Coord coord)
        {
            IPieceAgent agent;
            return _pieces.TryGetValue(coord, out agent) ? agent : null;
        }

        public ITransient PerformNewGame()
        {
            // TODO: animation
            _pieces.Clear();
            return null;
        }

        private readonly ReactiveDictionary<Coord, IPieceAgent> _pieces = new ReactiveDictionary<Coord, IPieceAgent>();
        private readonly IntReactiveProperty _width = new IntReactiveProperty(8);
        private readonly IntReactiveProperty _height = new IntReactiveProperty(8);
    }
}
