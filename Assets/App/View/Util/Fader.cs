using CameraFading;

namespace App
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// What this class does and how it works with other classes.
    /// </summary>
    public class Fader
        : MonoBehaviour
    {
        private void Awake()
        {
            CameraFading.CameraFade.In(5);
        }

        private void Start()
        {
        }

        private void Update()
        {
        }
    }
}

