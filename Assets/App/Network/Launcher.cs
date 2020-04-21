namespace App.Network
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// What this class does and how it works with other classes.
    /// </summary>
    public class Launcher
        : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
        }

        private void Update()
        {
        }
    }
}

