using System.Linq;
using UnityEngine;

public static class TransformExt
{
    public static T FindComponentInChildren<T>(this Transform tr) where T : class
    {
        return null;
    }
    public static T FindChild<T>(this Transform transform) where T : class
    {
        return transform.Cast<Transform>()
            .SelectMany(ch => ch.GetComponents<Component>())
            .OfType<T>()
            .FirstOrDefault();
    }
}
