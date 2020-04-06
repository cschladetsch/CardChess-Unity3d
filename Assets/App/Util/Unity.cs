namespace App
{
    using UnityEngine;

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
        
        public static void SetAlpha(this UnityEngine.UI.Image image, float a)
        {
            var c = image.color;
            c.a = a;
            image.color = c;
        }
    }
}
