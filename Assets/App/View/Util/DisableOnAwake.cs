using UnityEngine;

namespace Assets.App.View.Util
{
    class DisableOnAwake
        : MonoBehaviour
    {
        void Awake()
        {
            gameObject.SetActive(false);
        }
    }
}
