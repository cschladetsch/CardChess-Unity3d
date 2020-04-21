namespace App
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// What this class does and how it works with other classes.
    /// </summary>
    public class RandomRotation
        : MonoBehaviour
    {
        private Quaternion _toRotation;
        private float _lastChange;

        private void Awake()
        {
            _toRotation = Quaternion.Euler(Random.onUnitSphere);
        }
        
        private void Update()
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, _toRotation, Mathf.Clamp01(Time.deltaTime * 10));
            if (Random.Range(0, 1) < 0.3f)
                _toRotation = Quaternion.Euler(Random.onUnitSphere);
        }
    }
}

