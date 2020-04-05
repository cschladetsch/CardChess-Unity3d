namespace App
{
    using UnityEngine;

    /// <summary>
    /// Adds entries to the Arbiter Response User Interface list.
    ///
    /// Use F1 key to toggle.
    /// </summary>
    public class ArbiterResponseList
        : MonoBehaviour
    {
        public GameObject Visual;
        public GameObject HeaderPrefab;
        public GameObject EntryPrefab;
        public GameObject Container;

        private void Awake()
        {
            Visual.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
                Visual.SetActive(!Visual.activeSelf);
        }

        public void AddHeader(string text)
            => Add(HeaderPrefab, text);

        public void AddEntry(string text)
            => Add(EntryPrefab, text);

        private void Add(GameObject prefab, string text)
            => Instantiate(prefab, Container.transform).GetComponent<ArbiterTextEntry>().Text.text = text;
    }
}

