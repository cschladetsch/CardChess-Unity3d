//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using App.Common;
//using App.Registry;
//using NUnit.Framework;
//using UniRx;
//using Assert = NUnit.Framework.Assert;

//namespace Assets.App.Test.Editor
//{
//    interface IBase
//        : IHasId
//        , IHasRegistry<IBase>
//        , IHasDestroyHandler<IBase>
//    {
//    }

//    class Base : IBase
//    {
//        public Guid Id { get; set; }
//        public IRegistry<IBase> Registry { get; set; }
//        public IReadOnlyReactiveProperty<bool> Destroyed => _destroyed;
//        public event Action<IBase> OnDestroyed;

//        public void Destroy()
//        {
//            Trace.WriteLine($"Destroy {Id} {GetType()}");
//            OnDestroyed?.Invoke(this);
//        }

//        private readonly BoolReactiveProperty _destroyed = new BoolReactiveProperty();
//    }

//    class Handler : Base
//    {
//        public void FooDead(IBase foo)
//        {
//            Trace.WriteLine($"{foo} died");
//        }
//    }

//    class Foo : Base
//    {
//        private readonly int _num = 123;
//        public int Num = 42;

//        public override bool Equals(object obj)
//        {
//            var other = obj as Foo;
//            if (other == null)
//                return false;
//            return other._num == _num && other.Num == Num;
//        }

//        public override int GetHashCode()
//        {
//            return _num;
//        }

//        public override string ToString()
//        {
//            return $"_num={_num}, Num={Num}";
//        }
//    }

//    // never going to work because we can't persist event delegates without our own reactive library
//    //[TestFixture]
//    class TestRegistry
//    {
//        //[Test]
//        public void TestInts()
//        {
//            var reg = new Registry<IBase>();
//            reg.Bind<Foo, Foo>();
//            reg.Bind<Handler, Handler>();

//            var foo = reg.New<Foo>();
//            var handler = reg.New<Handler>();

//            foo.OnDestroyed += f => handler.FooDead(f);

//            var text = reg.Save();
//            Trace.WriteLine(text);

//            var reg2 = new Registry<IBase>();
//            reg2.Load(text);

//            var foo2 = reg2.Get(foo.Id);

//            Assert.AreEqual(foo, foo2);
//            foo.Destroy();
//        }
//    }
//}
