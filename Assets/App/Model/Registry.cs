using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UniRx.Triggers;

namespace App.Model
{
    using Common;

    public class Registry
        : ModelBase
    {
        public byte[] Write()
        {
            return null;
        }

        public bool Read(byte[] data)
        {
            return false;
        }

        public IModel Get(Guid id)
        {
            IModel model;
            if (Models.TryGetValue(id, out model))
                return model;
            Warn($"Failed to find model with id {id}");
            return null;
        }

        public TModel New<TModel, A0, A1>(A0 a0, A1 a1) where TModel : class, IModel, ICreateWith<A0, A1>, new()
        {
            var model = New<TModel>();
            if (model.Create(a0, a1))
                return model;
            Error($"Failed to create instance of {typeof(TModel)} with args {a0}, {a1}");
            Destroy(model);
            return null;
        }

        public TModel New<TModel, A0>(A0 a0) where TModel : class, IModel, ICreateWith<A0>, new()
        {
            var model = New<TModel>();
            if (model.Create(a0))
                return model;
            Error($"Failed to create instance of {typeof(TModel)} with arg {a0}");
            Destroy(model);
            return null;
        }

        public TModel New<TModel>() where TModel : class, IModel, new()
        {
            var model = new TModel();
            Models[model.Id] = model;
            var ty = typeof(TModel);
            if (!KnownTypes.Contains(ty))
            {
                KnownTypes.Add(ty);
                Types[Guid.NewGuid()] = ty;
            }

            model.Registry = this;
            return model;
        }

        public void Destroy(IModel model)
        {
            if (model == null)
            {
                Verbose(10, "Attempt to destroy null model");
                return;
            }

            model.Destroy();
            if (!Models.ContainsKey(model.Id))
                Warn($"Attempt to destroy unknown {model.GetType()} named {model.Name}");

            Models.Remove(model.Id);
        }

        private Dictionary<Guid, IModel> Models = new Dictionary<Guid, IModel>();
        private Dictionary<Guid, Type> Types = new Dictionary<Guid, Type>();
        private HashSet<Type> KnownTypes = new HashSet<Type>();
    }
}
