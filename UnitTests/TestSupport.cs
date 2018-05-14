using System;
using System.Collections;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using App.Main;

using Flow;

namespace App
{
    [TestClass]
    public class TestSupport
    {
        [TestMethod]
        public void TestBasicPrint()
        {
            var k = Flow.Create.Kernel();
            var f = k.Factory;

            var t0 = f.Transient("Trans0");
            var t1 = f.Transient("Trans1");
            var t2 = f.Transient("Trans2");
            var r0 = f.Trigger("Trigger0", t2, t1);
            var b0 = f.Barrier("Barrier0", t0, r0);
            var f0 = f.NamedFuture<int>("Future<int>");
            var g0 = f.Group("Group0", f0, b0);

            k.Root.Add(g0);

            // allow all objects to be placed
            for (int n = 0; n < 10; ++n)
                k.Step();

            var s0 = Flow.PrettyPrinter.ToString(g0);
            Console.WriteLine(s0);
        }

        private static void Step(Flow.IGenerator gen, int steps)
        {
            for (var n = 0; n < steps; ++n)
                gen.Step();
        }

        [TestMethod]
        public void TestFlowSequence()
        {
            var k = Flow.Create.Kernel();
            var f = k.Factory;
            var r = k.Root;
            r.Add(f.Sequence(
                f.Coroutine(StartGame).SetName("StartGame"),
                f.Coroutine(PlayerTurn, EColor.White),
                f.Coroutine(EndGame))
            );

            k.Step();
            Trace.WriteLine(r);
            k.Step();
            Trace.WriteLine(r);
            k.Step();
            Trace.WriteLine(r);
            k.Step();
            Trace.WriteLine(r);
            k.Step();
            Trace.WriteLine(r);
        }

        IEnumerator PlayerTurn(IGenerator self, App.EColor color)
        {
            Trace.WriteLine($"PlayerTurn: {color}");
            yield return null;
            Trace.WriteLine($"PlayerTurn: Again {color}");
            yield return null;
            Trace.WriteLine($"PlayerTurn: Again Again {color}");
        }

        IEnumerator EndGame(IGenerator self)
        {
            Trace.WriteLine("EndGame");
            yield break;
        }

        IEnumerator StartGame(IGenerator self)
        {
            Trace.WriteLine("StartGame");
            yield break;
        }
    }
}
