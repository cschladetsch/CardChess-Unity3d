using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using UniRx;

namespace App
{
    public static class Util
    {
        public static void Bind(this Button button, Action action)
        {
            Assert.IsNotNull(button);
            Assert.IsNotNull(action);

            button.OnClickAsObservable()
                .Subscribe(_ => action())
                .AddTo(button);
        }
    }
}
