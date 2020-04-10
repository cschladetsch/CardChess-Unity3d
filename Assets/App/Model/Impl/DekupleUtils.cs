// TODO: move to Dekuple package
namespace App.Model
{
    using System;
    using Dekuple;

    public static class DekupleUtils
    {
        public static T AddTo<T>(this T disposable, IHasSubscriptions container)
            where T : IDisposable
        {
            if (disposable == null) throw new ArgumentNullException("disposable");
            if (container == null) throw new ArgumentNullException("container");

            container.Add(disposable);

            return disposable;
        }
    }
}