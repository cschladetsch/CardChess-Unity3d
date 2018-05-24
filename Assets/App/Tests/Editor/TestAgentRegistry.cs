using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

using App;
using App.Agent;
using App.Model;

[TestFixture]
public class TestAgentRegistry {

    [Test]
    public void TestAgent()
	{
		AgentRegistry reg = new AgentRegistry();

        reg.Bind<IArbiterAgent, ArbiterAgent>(new ArbiterAgent());
		reg.Bind<ICardAgent, CardAgent>();

		ICardAgent cardAgent = reg.New<ICardAgent>();
    }

    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
    public IEnumerator NewTestScriptWithEnumeratorPasses() {
        // Use the Assert class to test conditions.
        // yield to skip a frame
        yield return null;
    }
}
