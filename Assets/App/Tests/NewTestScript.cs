using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class NewTestScript : MonoBehaviour
{
    public Button Give;
    public Button Take;

    [Test]
    public void NewTestScriptSimplePasses() {
        // Use the Assert class to test conditions.
    }

    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
    public IEnumerator NewTestScriptWithEnumeratorPasses() {
        Debug.Log($"Test Runtime 1 give={Give} button");
        Debug.Log($"Test Runtime 1 take={Take} button");
        yield return null;
    }
}
