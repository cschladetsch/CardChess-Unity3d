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

        #region Public Methods
        public ModelRegistry()
        {
            Verbosity = 100;
        }

        public byte[] Write()
        {
            return null;
        }

        public bool Read(byte[] data)
        {
            return false;
        }

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
            Warn($"Failed to find targetModel with id {id}");
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
            var ity = typeof(TInterface);
            if (_bindings.ContainsKey(ity))
            {
                Warn($"Registry has already bound {ity} to {typeof(TImpl)}");
                return false;
            }

            // TODO: combine these to one lookup.
            // That is, put the TImpl into PrepareModel, and parameterise it.
            _bindings[ity] = typeof(TImpl);
            _preparers[ity] = new PrepareModel(this, typeof(TImpl));

            return true;
        }

        public TIModel New<TIModel>(params object[] args) where TIModel : class, IModel
        {
            var ty = typeof(TIModel);
            var single = GetSingle(ty);
            if (single != null)
            {
                if (args.Length != 0)
                    Error($"Attempt to get singleton {ty}, when passing arguments {ToArgTypeList(args)}");
                var result = single as TIModel;
                if (result == null)
                    Error($"Couldn't convert singleton {single.GetType()} to {typeof(TIModel)}");
                return result;
            }

            var model = NewModel(typeof(TIModel), args) as TIModel;
            if (model == null)
            {
                Warn($"Failed to make instance for interface {typeof(TIModel)}");
                return null;
            }

            // store types for persistence
            _models[model.Id] = model;
            if (!_typeToGuid.ContainsKey(ty))
            {
                _idToType[model.Id] = ty;
                _typeToGuid[ty] = model.Id;
            }

            Verbose(10, $"Made an instance of {ty} with Id={model.Id}");
            model.Registry = this;
            model.OnDestroy += ModelDestroyed;
            return model;
        }

        private static string ToArgTypeList(IEnumerable<object> args)
        {
            return string.Join(", ", args.Select(a => a.GetType().Name));
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
            var prep = new PrepareModel(this, typeof(TImpl));
            _singles[ity] = prep.Prepare(single, typeof(TInterface), single);
            return true;
        }

        public bool Resolve()
        {
            if (_resolved)
            {
                Error("Registry already resolved");
                return false;
            }
            _resolved = true;
            var pendingInjections = _pendingInjections.ToArray();
            foreach (var pi in pendingInjections)
            {
                if (pi.Single != null)
                {
                    Verbose(50, $"Setting delayed singleton for {pi.Interface}");
                    _singles[pi.Interface] = pi.Single;
                }
                else
                {
                    _bindings[pi.Interface] = pi.ModelType;
                }
            }

            foreach (var pi in pendingInjections)
            {
                var inject = pi.Injection;
                var val = GetSingle(pi.Injection.ValueType);
                if (val == null)
                {
                    val = NewModel(inject.ValueType, inject.Args);
                    if (val == null)
                    {
                        Error($"Failed to resolve deferred dependancy {pi}");
                        continue;
                    }
                }
                inject.PropertyType.SetValue(pi.TargetModel, val);
                _pendingInjections.Remove(pi);
            }
            foreach (var pi in _pendingInjections)
            {
                Warn($"Failed to resolve for {pi}");
            }

            return _pendingInjections.Count == 0;
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
                Verbose(10, "Attempt to destroy null targetModel");
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

        internal IModel NewModel(Type ity, object[] args)
        {
            Type ty;
            if (!_bindings.TryGetValue(ity, out ty))
            {
                Warn($"Registry has no binding for {ity}");
                return null;
            }
            var cons = ty.GetConstructors();
            foreach (var con in cons)
            {
                if (!MatchingConstructor(args, con))
                    continue;
                var model = con.Invoke(args) as IModel;
                if (model != null)
                    return Prepare(ity, model);
            }
            Error($"No mathching Constructor for {ty} with args {ToArgTypeList(args)}");
            return null;
        }

        private static bool MatchingConstructor(object[] args, ConstructorInfo con)
        {
            var ctorParams = con.GetParameters().ToArray();
            if (ctorParams.Length != args.Length)
                return false;
            var n = 0;
            foreach (var param in ctorParams.Select(p => p.ParameterType))
            {
                if (!param.IsInstanceOfType(args[n]))
                    break;
                ++n;
            }
            return n == args.Length;
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

        #region Private Fields

        // TODO: fast way to set properties with private setters. Not possible?
        // Current work-around is to make the setters public.
        class PrepareModel
        {
            private PropertyInfo _setRegistry;
            private PropertyInfo _setId;
            private readonly ModelRegistry _reg;
            private readonly Type _modelType;
            private readonly List<Inject> _injections = new List<Inject>();

            internal PrepareModel(ModelRegistry reg, Type ty)
            {
                _modelType = ty;
                _reg = reg;
                foreach (var prop in ty.GetProperties(
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                {
                    var inject = prop.GetCustomAttribute<Inject>();
                    if (inject == null)
                        continue;
                    inject.PropertyType = prop;
                    inject.ValueType = prop.PropertyType;
                    _injections.Add(inject);
                }
            }

            public IModel Prepare(IModel model, Type iface = null, IModel single = null)
            {
                model.Id = Guid.NewGuid();
                model.Registry = _reg;
                foreach (var inject in _injections)
                {
                    var val = _reg.GetSingle(inject.ValueType);
                    if (val == null)
                    {
                        val = _reg.NewModel(inject.ValueType, inject.Args);
                        if (val == null)
                        {
                            if (_reg._resolved)
                            {
                                _reg.Error($"Cannot resolve interface {iface}");
                                return null;
                            }
                            var pi = new PendingInjection(model, inject, model.GetType(), iface, single);
                            _reg.Warn($"Adding {pi}");
                            _reg._pendingInjections.Add(pi);
                            continue;
                        }
                    }
                    inject.PropertyType.SetValue(model, val);
                }
                return model;
            }
        }

        private class PendingInjection
        {
            internal readonly IModel TargetModel;
            internal readonly Inject Injection;
            internal readonly IModel Single;
            internal readonly Type Interface;
            internal readonly Type ModelType;

            public PendingInjection(IModel targetModel, Inject inject, Type modelType, Type iface = null, IModel single = null)
            {
                TargetModel = targetModel;
                Injection = inject;
                ModelType = modelType;
                Interface = iface;
                Single = single;
            }

            public override string ToString()
            {
                return $"PendingInject: {Injection.ValueType} into {TargetModel}";
            }
        }

        private bool _resolved;
        private readonly List<PendingInjection> _pendingInjections = new List<PendingInjection>();
        private readonly Dictionary<Guid, IModel> _models = new Dictionary<Guid, IModel>();
        private readonly Dictionary<Guid, Type> _idToType = new Dictionary<Guid, Type>();
        private readonly Dictionary<Type, Guid> _typeToGuid = new Dictionary<Type, Guid>();
        private readonly Dictionary<Type, Type> _bindings = new Dictionary<Type, Type>();
        private readonly Dictionary<Type, PrepareModel> _preparers = new Dictionary<Type, PrepareModel>();
        private readonly Dictionary<Type, IModel> _singles = new Dictionary<Type, IModel>();
        #endregion
    }
}
