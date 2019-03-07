using System;
using CoLib;
using NUnit.Framework;

namespace CoLib
{

[TestFixture]
[Category("Ease Functions")]
internal class TestEase
{
	[Test]	
	public static void TestEaseValid()
	{
		// Test easing identity, f(0) = 0, f(1) = 1.
		bool threwException = false;
		try {Ease.CeilStep(0); } catch (ArgumentOutOfRangeException) {threwException = true;}
		Assert.IsTrue(threwException, "CeilStep input 0 invalid.");
		Assert.AreEqual(Ease.CeilStep(5)(0.0f), 0.0f, 0.001f, "CeilStep");
		Assert.AreEqual(Ease.CeilStep(5)(1.0f), 1.0f, 0.001f, "CeilStep");
		Assert.AreEqual(Ease.CeilStep(1)(0.0f), 0.0f, 0.001f, "CeilStep");
		Assert.AreEqual(Ease.CeilStep(1)(1.0f), 1.0f, 0.001f, "CeilStep");
		
		threwException = false;
		try {Ease.FloorStep(0); } catch (ArgumentOutOfRangeException) {threwException = true;}
		Assert.IsTrue(threwException, "FloorStep input 0 invalid.");
		Assert.AreEqual(Ease.FloorStep(5)(0.0f), 0.0f, 0.001f, "FloorStep");
		Assert.AreEqual(Ease.FloorStep(5)(1.0f), 1.0f, 0.001f, "FloorStep");
		Assert.AreEqual(Ease.FloorStep(1)(0.0f), 0.0f, 0.001f, "FloorStep");
		Assert.AreEqual(Ease.FloorStep(1)(1.0f), 1.0f, 0.001f, "FloorStep");
		
		threwException = false;
		try {Ease.RoundStep(0); } catch (ArgumentOutOfRangeException) {threwException = true;}
		Assert.IsTrue(threwException, "RoundStep input 0 invalid.");
		Assert.AreEqual(Ease.RoundStep(5)(0.0f), 0.0f, 0.001f, "RoundStep");
		Assert.AreEqual(Ease.RoundStep(5)(1.0f), 1.0f, 0.001f, "RoundStep");
		Assert.AreEqual(Ease.RoundStep(1)(0.0f), 0.0f, 0.001f, "RoundStep");
		Assert.AreEqual(Ease.RoundStep(1)(1.0f), 1.0f, 0.001f, "RoundStep");
		
		Assert.AreEqual(Ease.Elastic(20.0f, 20.0f)(0.0f), 0.0f, 0.001f, "Elastic");
		Assert.AreEqual(Ease.Elastic(0.01f, 20.0f)(0.0f), 0.0f, 0.001f, "Elastic");
		Assert.AreEqual(Ease.Elastic(20.0f, 0.01f)(0.0f), 0.0f, 0.001f, "Elastic");
		Assert.AreEqual(Ease.Elastic(0.01f, 0.01f)(0.0f), 0.0f, 0.001f, "Elastic");
		Assert.AreEqual(Ease.Elastic(-1.0f, 1.0f)(0.0f), 0.0f, 0.001f, "Elastic");
		Assert.AreEqual(Ease.Elastic(1.0f, -1.0f)(0.0f), 0.0f, 0.001f, "Elastic");
		Assert.AreEqual(Ease.Elastic(-1.0f, -1.0f)(0.0f), 0.0f, 0.001f, "Elastic");
		
		Assert.AreEqual(Ease.Elastic(20.0f, 20.0f)(1.0f), 1.0f, 0.001f, "Elastic");
		Assert.AreEqual(Ease.Elastic(0.01f, 20.0f)(1.0f), 1.0f, 0.001f, "Elastic");
		Assert.AreEqual(Ease.Elastic(20.0f, 0.01f)(1.0f), 1.0f, 0.001f, "Elastic");
		Assert.AreEqual(Ease.Elastic(0.01f, 0.01f)(1.0f), 1.0f, 0.001f, "Elastic");
		Assert.AreEqual(Ease.Elastic(-1.0f, 1.0f)(1.0f), 1.0f, 0.001f, "Elastic");
		Assert.AreEqual(Ease.Elastic(1.0f, -1.0f)(1.0f), 1.0f, 0.001f, "Elastic");
		Assert.AreEqual(Ease.Elastic(-1.0f, -1.0f)(1.0f), 1.0f, 0.001f, "Elastic");
		
		Assert.AreEqual(Ease.InElastic()(0.0f), 0.0f, 0.001f, "InElastic");
		Assert.AreEqual(Ease.InElastic()(1.0f), 1.0f, 0.001f, "InElastic");
		Assert.AreEqual(Ease.OutElastic()(0.0f), 0.0f, 0.001f, "OutElastic");
		Assert.AreEqual(Ease.OutElastic()(1.0f), 1.0f, 0.001f, "OutElastic");		
		Assert.AreEqual(Ease.InOutElastic()(0.0f), 0.0f, 0.001f, "InOutElastic");
		Assert.AreEqual(Ease.InOutElastic()(1.0f), 1.0f, 0.001f, "InOutElastic");
		
		Assert.AreEqual(Ease.InBack(0.0f)(0.0f), 0.0f, 0.001f, "InBack");
		Assert.AreEqual(Ease.InBack(0.2f)(0.0f), 0.0f, 0.001f, "InBack");
		Assert.AreEqual(Ease.InBack(-0.2f)(0.0f), 0.0f, 0.001f, "InBack");
		Assert.AreEqual(Ease.InBack(0.0f)(1.0f), 1.0f, 0.001f, "InBack");
		Assert.AreEqual(Ease.InBack(0.2f)(1.0f), 1.0f, 0.001f, "InBack");
		Assert.AreEqual(Ease.InBack(-0.2f)(1.0f), 1.0f, 0.001f, "InBack");
		
		Assert.AreEqual(Ease.OutBack(0.0f)(0.0f), 0.0f, 0.001f, "OutBack");
		Assert.AreEqual(Ease.OutBack(0.2f)(0.0f), 0.0f, 0.001f, "OutBack");
		Assert.AreEqual(Ease.OutBack(-0.2f)(0.0f), 0.0f, 0.001f, "OutBack");
		Assert.AreEqual(Ease.OutBack(0.0f)(1.0f), 1.0f, 0.001f, "OutBack");
		Assert.AreEqual(Ease.OutBack(0.2f)(1.0f), 1.0f, 0.001f, "OutBack");
		Assert.AreEqual(Ease.OutBack(-0.2f)(1.0f), 1.0f, 0.001f, "OutBack");
		
		Assert.AreEqual(Ease.InOutBack(0.0f)(0.0f), 0.0f, 0.001f, "OutBack");
		Assert.AreEqual(Ease.InOutBack(0.2f)(0.0f), 0.0f, 0.001f, "OutBack");
		Assert.AreEqual(Ease.InOutBack(-0.2f)(0.0f), 0.0f, 0.001f, "OutBack");
		Assert.AreEqual(Ease.InOutBack(0.0f)(1.0f), 1.0f, 0.001f, "OutBack");
		Assert.AreEqual(Ease.InOutBack(0.2f)(1.0f), 1.0f, 0.001f, "OutBack");
		Assert.AreEqual(Ease.InOutBack(-0.2f)(1.0f), 1.0f, 0.001f, "OutBack");
		
		Assert.AreEqual(Ease.InBounce()(0.0f), 0.0f, 0.001f, "InBounce");
		Assert.AreEqual(Ease.InBounce()(1.0f), 1.0f, 0.001f, "InBounce");	
		Assert.AreEqual(Ease.OutBounce()(0.0f), 0.0f, 0.001f, "OutBounce");
		Assert.AreEqual(Ease.OutBounce()(1.0f), 1.0f, 0.001f, "OutBounce");
		Assert.AreEqual(Ease.InOutBounce()(0.0f), 0.0f, 0.001f, "InOutBounce");
		Assert.AreEqual(Ease.InOutBounce()(1.0f), 1.0f, 0.001f, "InOutBounce");
		
		Assert.AreEqual(Ease.InCirc()(0.0f), 0.0f, 0.001f, "InCirc");
		Assert.AreEqual(Ease.InCirc()(1.0f), 1.0f, 0.001f, "InCirc");	
		Assert.AreEqual(Ease.OutCirc()(0.0f), 0.0f, 0.001f, "OutCirc");
		Assert.AreEqual(Ease.OutCirc()(1.0f), 1.0f, 0.001f, "OutCirc");
		Assert.AreEqual(Ease.InOutCirc()(0.0f), 0.0f, 0.001f, "InOutCirc");
		Assert.AreEqual(Ease.InOutCirc()(1.0f), 1.0f, 0.001f, "InOutCirc");
		
		Assert.AreEqual(Ease.InCubic()(0.0f), 0.0f, 0.001f, "InCubic");
		Assert.AreEqual(Ease.InCubic()(1.0f), 1.0f, 0.001f, "InCubic");	
		Assert.AreEqual(Ease.OutCubic()(0.0f), 0.0f, 0.001f, "OutCubic");
		Assert.AreEqual(Ease.OutCubic()(1.0f), 1.0f, 0.001f, "OutCubic");
		Assert.AreEqual(Ease.InOutCubic()(0.0f), 0.0f, 0.001f, "InOutCubic");
		Assert.AreEqual(Ease.InOutCubic()(1.0f), 1.0f, 0.001f, "InOutCubic");
		
		Assert.AreEqual(Ease.InQuad()(0.0f), 0.0f, 0.001f, "InQuad");
		Assert.AreEqual(Ease.InQuad()(1.0f), 1.0f, 0.001f, "InQuad");	
		Assert.AreEqual(Ease.OutQuad()(0.0f), 0.0f, 0.001f, "OutQuad");
		Assert.AreEqual(Ease.OutQuad()(1.0f), 1.0f, 0.001f, "OutQuad");
		Assert.AreEqual(Ease.InOutQuad()(0.0f), 0.0f, 0.001f, "InOutQuad");
		Assert.AreEqual(Ease.InOutQuad()(1.0f), 1.0f, 0.001f, "InOutQuad");
		
		Assert.AreEqual(Ease.InQuart()(0.0f), 0.0f, 0.001f, "InQuart");
		Assert.AreEqual(Ease.InQuart()(1.0f), 1.0f, 0.001f, "InQuart");	
		Assert.AreEqual(Ease.OutQuart()(0.0f), 0.0f, 0.001f, "OutQuart");
		Assert.AreEqual(Ease.OutQuart()(1.0f), 1.0f, 0.001f, "OutQuart");
		Assert.AreEqual(Ease.InOutQuart()(0.0f), 0.0f, 0.001f, "InOutQuart");
		Assert.AreEqual(Ease.InOutQuart()(1.0f), 1.0f, 0.001f, "InOutQuart");
		
		Assert.AreEqual(Ease.InQuint()(0.0f), 0.0f, 0.001f, "InQuint");
		Assert.AreEqual(Ease.InQuint()(1.0f), 1.0f, 0.001f, "InQuint");	
		Assert.AreEqual(Ease.OutQuint()(0.0f), 0.0f, 0.001f, "OutQuint");
		Assert.AreEqual(Ease.OutQuint()(1.0f), 1.0f, 0.001f, "OutQuint");
		Assert.AreEqual(Ease.InOutQuint()(0.0f), 0.0f, 0.001f, "InOutQuint");
		Assert.AreEqual(Ease.InOutQuint()(1.0f), 1.0f, 0.001f, "InOutQuint");
		
		Assert.AreEqual(Ease.InExpo()(0.0f), 0.0f, 0.001f, "InExpo");
		Assert.AreEqual(Ease.InExpo()(1.0f), 1.0f, 0.001f, "InExpo");	
		Assert.AreEqual(Ease.OutExpo()(0.0f), 0.0f, 0.001f, "OutExpo");
		Assert.AreEqual(Ease.OutExpo()(1.0f), 1.0f, 0.001f, "OutExpo");
		Assert.AreEqual(Ease.InOutExpo()(0.0f), 0.0f, 0.001f, "InOutExpo");
		Assert.AreEqual(Ease.InOutExpo()(1.0f), 1.0f, 0.001f, "InOutExpo");
		
		Assert.AreEqual(Ease.InSin()(0.0f), 0.0f, 0.001f, "InSin");
		Assert.AreEqual(Ease.InSin()(1.0f), 1.0f, 0.001f, "InSin");	
		Assert.AreEqual(Ease.OutSin()(0.0f), 0.0f, 0.001f, "OutSin");
		Assert.AreEqual(Ease.OutSin()(1.0f), 1.0f, 0.001f, "OutSin");
		Assert.AreEqual(Ease.InOutSin()(0.0f), 0.0f, 0.001f, "InOutSin");
		Assert.AreEqual(Ease.InOutSin()(1.0f), 1.0f, 0.001f, "InOutSin");
	
	}
}

}
