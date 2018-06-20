using UnityEngine;

namespace App
{
    public static class Unity
    {
        public static void Destroy(Transform tr)
        {
            if (tr != null)
                Destroy(tr.gameObject);
        }

        public static void Destroy(GameObject go)
        {
            if (Application.isEditor)
                Object.DestroyImmediate(go);
            else
                Object.Destroy(go);
        }
    }
}
