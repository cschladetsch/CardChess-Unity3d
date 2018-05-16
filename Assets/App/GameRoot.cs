using UnityEngine;

// field not assigned - because it is assigned in Unity3d editor
#pragma warning disable CS0649
namespace App
{
    /// <inheritdoc />
    /// <summary>
    /// The intended root of all non-canvas objects in the scene.
    /// </summary>
    class GameRoot : MonoBehaviour
    {
        /// <summary>
        /// What makes the decisions.
        /// </summary>
        public ArbiterGameObject Arbiter;

        private void Awake()
        {
        }

        private void Start()
        {
        }
    }
}
