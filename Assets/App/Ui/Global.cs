using UIWidgets;
using UnityEditor.IMGUI.Controls;
using UnityEngine.Audio;

namespace App
{
    using UnityEngine;

    /// <summary>
    /// What this class does and how it works with other classes.
    /// </summary>
    public class Global
        : MonoBehaviour
    {
        public GameObject Options;
        public CenteredSlider Music;
        public CenteredSlider Sfx;

        private void Awake()
        {
            Options.SetActive(false);
        }
        
        public void Pressed()
        {
            Options.SetActive(!Options.activeSelf);
        }

        public void SetMusicVolume(float f)
        {
            Debug.Log($"Music {Music.Value}");
        }
        
        public void SetSfxVolume(float f)
        {
            Debug.Log($"Sfx {Sfx.Value}");
        }
    }
}

