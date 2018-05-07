using System;
using CoLib;
using NUnit.Framework;

namespace CoLib
{

[TestFixture]
[Category("CommandScheduler")]
internal class TestCommandScheduler
{	
	[Test]
	public static void TestOrdering()
	{
		CommandScheduler scheduler = new CommandScheduler();
		
		int a = 0;
		scheduler.Add(
			Commands.Sequence(
				Commands.WaitForSeconds(1.0f),
				Commands.Do(() => ++a)
			)
		);
		
		int b = 0;
		scheduler.Add(
			Commands.Sequence(
				Commands.WaitForSeconds(1.0f),
				Commands.Do(() => ++b)
			)
		);
		
		scheduler.Add(
			Commands.Sequence(
				Commands.WaitForSeconds(1.5f),
				Commands.Do(() => ++b)
			)
		);
		Assert.AreEqual(a, 0);
		Assert.AreEqual(b, 0);
		scheduler.Update(1.0f);
		Assert.AreEqual(a, 1);
		Assert.AreEqual(b, 1);
		scheduler.Update(0.5f);
		Assert.AreEqual(a, 1);
		Assert.AreEqual(b, 2);
	}
}

}

