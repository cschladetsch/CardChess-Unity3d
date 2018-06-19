using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class TransformExt
{
    //public static T FindComponentInChildren<T>(this Transform tr) where T : class
    //{
    //    foreach ()
    //}

    public static T FindChild<T>(this Transform transform) where T : class
    {
        return transform.Cast<Transform>()
            .SelectMany(ch => ch.GetComponents<Component>())
            .OfType<T>()
            .FirstOrDefault();
    }

    public static IEnumerable<T> Get<T>(this Transform tr) where T : class
    {
        return tr.GetComponents<Component>().OfType<T>();
    }

    public static void ForEach<T>(this Transform tr, Action<T> act) where T : class
    {
        foreach (var ch in tr.Get<Component>().OfType<T>())
            act(ch);
    }
}
