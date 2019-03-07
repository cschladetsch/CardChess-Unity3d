using System;
using System.Collections.Generic;
using UnityEngine.UI;

using UniRx;

namespace Dekuple
{
    public static class Binding
    {
        public static void BindToInteractable(this Selectable selectable, UniRx.IObservable<bool> observable,
            ICollection<IDisposable> disposables = null)
        {
            if (disposables != null)
            {
                observable
                    .Subscribe(enabled => { selectable.interactable = enabled; })
                    .AddTo(disposables);
            }
            else
            {
                observable
                    .Subscribe(enabled => { selectable.interactable = enabled; })
                    .AddTo(selectable);
            }
        }
    }
}
