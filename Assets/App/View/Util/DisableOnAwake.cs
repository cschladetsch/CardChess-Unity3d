using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.App.View.Util
{
    class DisableOnAwake : MonoBehaviour
    {
        void Awake()
        {
            gameObject.SetActive(false);
        }
    }
}

public static class TransformExt
{
    public static T FindComponentInChildren<T>(this Transform tr) where T : class
    {
        return null;
    }
}
