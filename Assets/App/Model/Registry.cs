using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
            if (_models.TryGetValue(id, out model))
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

        public TModel New<TModel>(params object[] args) where TModel : class, IModel, new()
        {
            var model = NewModel<TModel>(args);
            _models[model.Id] = model;
            var ty = typeof(TModel);
            if (!_typeToGuid.ContainsKey(ty))
            {
                var id = Guid.NewGuid();
                _idToType[id] = ty;
                _typeToGuid[ty] = id;
            }

            Verbose(10, $"Made an instance of {ty} with Id={model.Id}");
            model.Registry = this;
            return model;
        }

        private TModel NewModel<TModel>(object[] args) where TModel : class, IModel, new()
        {
            var ty = typeof(TModel);
            var cons = ty.GetConstructors();//BindingFlags.NonPublic | BindingFlags.Public);
            var argTypes = args.Select(a => a.GetType()).ToArray();
            foreach (var con in cons)
            {
                var paramTypes = con.GetParameters().Select(p => p.ParameterType);
                if (!argTypes.SequenceEqual(paramTypes))
                    continue;
                var model = con.Invoke(args) as TModel;
                if (model != null)
                    return model;
                Error($"Couldn't create type {ty} with args {args}");
                return null;
            }
            Error($"Couldn't create type {ty} with args {args}");
            return null;
        }

        public void Destroy(IModel model)
        {
            if (model == null)
            {
                Verbose(10, "Attempt to destroy null model");
                return;
            }

            model.Destroy();
            if (!_models.ContainsKey(model.Id))
                Warn($"Attempt to destroy unknown {model.GetType()} named {model.Name}");

            _models.Remove(model.Id);
        }

        private readonly Dictionary<Guid, IModel> _models = new Dictionary<Guid, IModel>();
        private readonly Dictionary<Guid, Type> _idToType = new Dictionary<Guid, Type>();
        private readonly Dictionary<Type, Guid> _typeToGuid = new Dictionary<Type, Guid>();
    }
}
