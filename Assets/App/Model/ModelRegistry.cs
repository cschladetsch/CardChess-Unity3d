using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace App.Model
{
    using Common;

    public class ModelRegistry
        : ModelBase, IModelRegistry
    {
        #region Public Properties
        public IEnumerable<IModel> Models => _models.Values;
        public int NumModels => _models.Count;
        #endregion

        public byte[] Write()
        {
            return null;
        }

        public bool Read(byte[] data)
        {
            return false;
        }

        #region Public Methods
        public bool HasModel(IModel model)
        {
            return Models.Contains(model);
        }

        public bool HasModel(Guid id)
        {
            return Models.Any(m => m.Id == id);
        }

        public IModel Get(Guid id)
        {
            IModel model;
            if (_models.TryGetValue(id, out model))
                return model;
            Warn($"Failed to find model with id {id}");
            return null;
        }

        public TModel New<TModel, A0, A1>(A0 a0, A1 a1) where TModel : class, IModel, IConstructWith<A0, A1>, new()
        {
            var model = New<TModel>();
            if (model.Construct(a0, a1))
                return model;
            Error($"Failed to create instance of {typeof(TModel)} with args {a0}, {a1}");
            Remove(model);
            return null;
        }

        public TModel New<TModel, A0>(A0 a0) where TModel : class, IModel, IConstructWith<A0>, new()
        {
            var model = New<TModel>();
            if (model.Construct(a0))
                return model;
            Error($"Failed to create instance of {typeof(TModel)} with arg {a0}");
            Remove(model);
            return null;
        }

        public bool Bind<TInterface, TImpl>() where TInterface : IModel where TImpl : TInterface
        {
            if (_bindings.ContainsKey(typeof(TInterface)))
            {
                Warn($"Registry has already bound {typeof(TInterface)} to {typeof(TImpl)}");
                return false;
            }

            // TODO: combine these to one lookup.
            // That is, put the TImpl into PrepareModel, and parameterise it.
            _bindings[typeof(TInterface)] = typeof(TImpl);
            _preparers[typeof(TInterface)] = new PrepareModel(this, typeof(TImpl));

            return true;
        }

        public TModel New<TModel>(params object[] args) where TModel : class, IModel
        {
            var ty = typeof(TModel);
            var single = GetSingle(ty);
            if (single != null)
            {
                if (args.Length != 0)
                    Error($"Attempt to get singleton {ty}, when passing arguments {ToArgList(args)}");
                var result = single as TModel;
                if (result == null)
                    Error($"Couldn't convert singleton {single.GetType()} to {typeof(TModel)}");
                return result;
            }

            var model = NewModel(typeof(TModel), args) as TModel;
            if (model == null)
            {
                Warn($"Failed to make instance for interface {typeof(TModel)}");
                return null;
            }
            _models[model.Id] = model;
            if (!_typeToGuid.ContainsKey(ty))
            {
                var id = Guid.NewGuid();
                _idToType[id] = ty;
                _typeToGuid[ty] = id;
            }

            Verbose(10, $"Made an instance of {ty} with Id={model.Id}");
            model.Registry = this;
            model.OnDestroy += ModelDestroyed;
            return model;
        }

        private static string ToArgList(IEnumerable<object> args)
        {
            return string.Join(", ",  args.Select(a => a.ToString()));
        }

        public bool Bind<TInterface, TImpl>(Func<TImpl> creator) where TInterface : IModel where TImpl : TInterface
        {
            throw new NotImplementedException();
        }

        public bool Bind<TInterface, TImpl, A0>(Func<A0, TImpl> creator) where TInterface : IModel where TImpl : TInterface
        {
            throw new NotImplementedException();
        }

        public bool Bind<TInterface, TImpl, A0, A1>(Func<A0, A1, TImpl> creator) where TInterface : IModel where TImpl : TInterface
        {
            throw new NotImplementedException();
        }

        public bool Bind<TInterface, TImpl>(TImpl single) where TInterface : IModel where TImpl  : TInterface
        {
            var ity = typeof(TInterface);
            if (_singles.ContainsKey(ity))
            {
                Warn($"Already have singleton valuye for {ity}");
                return false;
            }
            _singles[ity] = single;
            return true;
        }

        #endregion

        #region Private Methods
        private void Remove(IModel model)
        {
            if (!_models.ContainsKey(model.Id))
                Warn($"Attempt to destroy unknown {model.GetType()} named {model.Name}");
            else
                _models.Remove(model.Id);
        }

        private void ModelDestroyed(object sender, IModel model, object[] context)
        {
            if (model == null)
            {
                Verbose(10, "Attempt to destroy null model");
                return;
            }

            model.OnDestroy -= ModelDestroyed;
            Remove(model);
        }

        private IModel GetSingle(Type ty)
        {
            IModel single;
            if (_singles.TryGetValue(ty, out single))
            {
                return single;
            }
            return null;
        }

        /// <summary>
        /// All Models start their life here
        /// </summary>
        /// <param name="ity">the interface type given as input</param>
        /// <param name="args">construction args if any</param>
        /// <returns>a prepared model</returns>
        private IModel NewModel(Type ity, object[] args)
        {
            Type ty;
            if (!_bindings.TryGetValue(ity, out ty))
            {
                Warn($"Registry has no binding for {ity}");
                return null;
            }
            var cons = ty.GetConstructors();//BindingFlags.NonPublic);// | BindingFlags.Public);
            var argTypes = args.Select(a => a.GetType()).ToArray();
            foreach (var con in cons)
            {
                var paramTypes = con.GetParameters().Select(p => p.ParameterType).ToArray();
                int n = 0;
                foreach (var param in paramTypes)
                {
                    if (!param.IsInstanceOfType(args[n]))
                        break;
                    ++n;
                }
                if (n != args.Length)
                    continue;
                var model = con.Invoke(args) as IModel;
                if (model != null)
                    return Prepare(ity, model);
            }
            Error($"Couldn't create type {ty} with args {ToArgList(args)}");
            return null;
        }

        IModel Prepare(Type ity, IModel model)
        {
            PrepareModel prep;
            if (!_preparers.TryGetValue(ity, out prep))
            {
                throw new Exception($"No preparer for type {ity}");
            }
            return prep.Prepare(model);
        }

        #endregion

        #region IPrintable
        public string Print()
        {
            var sb = new StringBuilder();
            sb.Append($"{NumModels} Models:\n");
            foreach (var kv in _models)
            {
                sb.Append($"\t{kv.Key} -> {kv.Value}\n");
            }
            sb.Append($"\n{_idToType.Count} Types:");
            foreach (var kv in _idToType)
            {
                sb.Append($"\t{kv.Value}");
            }
            return sb.ToString();
        }
        #endregion

        // TODO: fast way to set properties with private setters
        // Current work-around is to make the setters public.
        class PrepareModel
        {
            private PropertyInfo _setRegistry;
            private PropertyInfo _setId;
            private IModelRegistry _reg;
            private Type _modelType;

            internal PrepareModel(IModelRegistry reg, Type ty)
            {
                _modelType = ty;
                _reg = reg;
                //foreach (var prop in ty.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                //{
                //    if (typeof(Guid) == prop.PropertyType && "Id" == prop.Name)
                //    {
                //        _setId = prop;
                //    }
                //    if (typeof(IModelRegistry) == prop.PropertyType && "Registry" == prop.Name)
                //    {
                //        _setRegistry = prop;
                //    }
                //}

                //Assert.IsNotNull(_setRegistry);
                //Assert.IsNotNull(_setId);
            }

            public IModel Prepare(IModel model)
            {
                // TODO: cache these
                //_modelType.InvokeMember("Id", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty | BindingFlags.Instance,
                //    null, model, new object[] { Guid.NewGuid() });
                //_modelType.InvokeMember("Registry", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty | BindingFlags.Instance,
                //    null, model, new object[] { _reg });
                //return model;
                model.Id = Guid.NewGuid();
                model.Registry = _reg;
                return model;
            }
        }

        #region Private Fields
        private readonly Dictionary<Guid, IModel> _models = new Dictionary<Guid, IModel>();
        private readonly Dictionary<Guid, Type> _idToType = new Dictionary<Guid, Type>();
        private readonly Dictionary<Type, Guid> _typeToGuid = new Dictionary<Type, Guid>();
        private readonly Dictionary<Type, Type> _bindings = new Dictionary<Type, Type>();
        private readonly Dictionary<Type, PrepareModel> _preparers = new Dictionary<Type, PrepareModel>();
        private readonly Dictionary<Type, IModel> _singles = new Dictionary<Type, IModel>();
        #endregion
    }
}
