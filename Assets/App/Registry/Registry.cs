using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace App.Registry
{
    using Common;
    using Model;

    public class Registry<IBase>
        : ModelBase
        , IRegistry<IBase>
        where IBase
            : class
            , IKnown
            , IHasRegistry<IBase>
            , IHasDestroyHandler<IBase>
    {
        #region Public Properties
        public IEnumerable<IBase> Instances => _models.Values;
        public int NumInstances => _models.Count;
        #endregion

        #region Public Methods
        public Registry()
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

        public bool Has(IBase instance)
        {
            return Instances.Contains(instance);
        }

        public bool Has(Guid id)
        {
            return Instances.Any(m => m.Id == id);
        }

        public IBase Get(Guid id)
        {
            IBase model;
            if (_models.TryGetValue(id, out model))
                return model;
            Warn($"Failed to find targetModel with id {id}");
            return null;
        }

        public TModel New<TModel, A0, A1>(A0 a0, A1 a1)
            where TModel
            : class, IBase, IHasRegistry<TModel>,
            IConstructWith<A0, A1>,
            IHasDestroyHandler<TModel>, new()
        {
            var model = New<TModel>();
            if (model.Construct(a0, a1))
                return model;
            Error($"Failed to create instance of {typeof(TModel)} with args {a0}, {a1}");
            Remove(model);
            return null;
        }

        public TModel New<TModel, A0>(A0 a0)
            where TModel
            : class, IBase, IConstructWith<A0>, IHasRegistry<TModel>,
            IHasDestroyHandler<TModel>, new()
        {
            var model = New<TModel>();
            if (model.Construct(a0))
                return model;
            Error($"Failed to create instance of {typeof(TModel)} with arg {a0}");
            Remove(model);
            return null;
        }

        public bool Bind<TInterface, TImpl>() where TInterface : IBase where TImpl : TInterface
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

        public TIBase New<TIBase>(params object[] args)
            where TIBase
            : class, IBase, IHasRegistry<IBase>, IHasDestroyHandler<IBase>
        {
            var type = typeof(TIBase);
            var single = GetSingle(type);
            if (single != null)
            {
                if (args.Length != 0)
                    Error($"Attempt to get singleton {type}, when passing arguments {ToArgTypeList(args)}");
                var result = single as TIBase;
                if (result == null)
                    Error($"Couldn't convert singleton {single.GetType()} to {type}");
                return result;
            }

            var model = NewModel(type, args) as TIBase;
            if (model == null)
            {
                Error($"Failed to make instance for interface {type}");
                return null;
            }

            // store types for persistence
            _models[model.Id] = model;
            if (!_typeToGuid.ContainsKey(type))
            {
                _idToType[model.Id] = type;
                _typeToGuid[type] = model.Id;
            }

            Verbose(10, $"Made a {model} of {type} with Id={model.Id}");
            model.Registry = this;
            model.OnDestroy += ModelDestroyed;
            return model;
        }

        private static string ToArgTypeList(IEnumerable<object> args)
        {
            return args == null ? "" : string.Join(", ", args.Select(a => a.GetType().Name));
        }

        private static string ToArgList(IEnumerable<object> args)
        {
            return args == null ? "" : string.Join(", ",  args.Select(a => a.ToString()));
        }

        public bool Bind<TInterface, TImpl>(Func<TImpl> creator) where TInterface : IBase where TImpl : TInterface
        {
            throw new NotImplementedException();
        }

        public bool Bind<TInterface, TImpl, A0>(Func<A0, TImpl> creator) where TInterface : IBase where TImpl : TInterface
        {
            throw new NotImplementedException();
        }

        public bool Bind<TInterface, TImpl, A0, A1>(Func<A0, A1, TImpl> creator) where TInterface : IBase where TImpl : TInterface
        {
            throw new NotImplementedException();
        }

        public bool Bind<TInterface, TImpl>(TImpl single) where TInterface : IBase where TImpl  : TInterface
        {
            var ity = typeof(TInterface);
            if (_singles.ContainsKey(ity))
            {
                Warn($"Already have singleton value for {ity}");
                return false;
            }
            var prep = new PrepareModel(this, typeof(TImpl));
            _singles[ity] = prep.Prepare(single, ity, single);
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

            var pi = _pendingInjections.ToArray();
            AddPendingBindings(pi);
            ApplyInjections(pi);

            foreach (var p in _pendingInjections)
            {
                Warn($"Failed to resolve for {p}");
            }

            return _pendingInjections.Count == 0;
        }

        private void AddPendingBindings(PendingInjection[] pendingInjections)
        {
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
        }

        private void ApplyInjections(PendingInjection[] pendingInjections)
        {
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
        }

        #endregion

        #region Private Methods
        private void Remove(IBase model)
        {
            if (!_models.ContainsKey(model.Id))
                Warn($"Attempt to destroy unknown {model.GetType()} named {model.Name}");
            else
                _models.Remove(model.Id);
        }

        private void ModelDestroyed<TIBase>(TIBase model)
            where TIBase
            : class, IBase,
            IHasRegistry<IBase>,
            IHasDestroyHandler<IBase>
        {
            if (model == null)
            {
                Verbose(10, "Attempt to destroy null targetModel");
                return;
            }
            model.OnDestroy -= ModelDestroyed;
            Remove(model);
        }

        private IBase GetSingle(Type ty)
        {
            IBase single;
            if (_singles.TryGetValue(ty, out single))
            {
                return single;
            }
            return null;
        }

        internal IBase NewModel(Type ity, object[] args)
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
                var model = con.Invoke(args) as IBase;
                if (model != null)
                    return Prepare(ity, model);
            }
            Error($"No mathching Constructor for {ty} with args {ToArgTypeList(args)}");
            return null;
        }

        private static bool MatchingConstructor(object[] args, ConstructorInfo con)
        {
            var ctorParams = con.GetParameters().ToArray();
            if (args == null)
                return ctorParams.Length == 0;
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

        private IBase Prepare(Type ity, IBase model)
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
            sb.Append($"{_singles.Count} Singletons:\n");
            foreach (var s in _singles)
            {
                sb.Append($"\t{s.Key} -> {s.Value}\n");
            }
            sb.Append($"{NumInstances} Instances:\n");
            foreach (var kv in _models)
            {
                sb.Append($"\t{kv.Key} -> {kv.Value}\n");
            }

            sb.Append($"\n{_idToType.Count} Types:\n");
            foreach (var kv in _idToType)
            {
                sb.Append($"\t{kv.Value}\n");
            }
            return sb.ToString();
        }
        #endregion

        #region Private Fields

        class PrepareModel
        {
            private PropertyInfo _setRegistry;
            private PropertyInfo _setId;
            private readonly IRegistry<IBase> _reg;
            private readonly Type _modelType;
            private readonly List<Inject> _injections = new List<Inject>();

            internal PrepareModel(IRegistry<IBase> reg, Type ty)
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

            //public IBase Prepare(IBase model, Type iface = null, IBase single = null)
            public IBase Prepare(IBase model, Type iface = null, IBase single = null)
            {
                model.Id = Guid.NewGuid();
                model.Registry = _reg;
                foreach (var inject in _injections)
                {
                    _reg.Inject(model, inject, iface, single);
                }
                return model;
            }
        }

        public IBase Inject(IBase model, Inject inject, Type iface, IBase single)
        {
            var val = GetSingle(inject.ValueType);
            if (val == null)
            {
                val = NewModel(inject.ValueType, inject.Args);
                if (val == null)
                {
                    if (_resolved)
                    {
                        Error($"Cannot resolve interface {inject.ValueType}");
                        return null;
                    }
                    var pi = new PendingInjection(model, inject, model.GetType(), iface, single);
                    Warn($"Adding {pi}");
                    _pendingInjections.Add(pi);
                }
            }
            inject.PropertyType.SetValue(model, val);
            return model;
        }

        private class PendingInjection
        {
            internal readonly IBase TargetModel;
            internal readonly Inject Injection;
            internal readonly IBase Single;
            internal readonly Type Interface;
            internal readonly Type ModelType;

            public PendingInjection(IBase targetModel, Inject inject, Type modelType, Type iface = null, IBase single = null)
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
        private readonly Dictionary<Guid, IBase> _models = new Dictionary<Guid, IBase>();
        private readonly Dictionary<Guid, Type> _idToType = new Dictionary<Guid, Type>();
        private readonly Dictionary<Type, Guid> _typeToGuid = new Dictionary<Type, Guid>();
        private readonly Dictionary<Type, Type> _bindings = new Dictionary<Type, Type>();
        private readonly Dictionary<Type, PrepareModel> _preparers = new Dictionary<Type, PrepareModel>();
        private readonly Dictionary<Type, IBase> _singles = new Dictionary<Type, IBase>();
        private IRegistry<IBase> _registry;

        #endregion
    }
}
