using UnityEngine;

namespace Assets.Dekuple.View.Util
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
