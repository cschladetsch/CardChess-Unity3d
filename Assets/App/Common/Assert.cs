using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Common
{
    public class AssertionException : Exception
    {
        public AssertionException(string text) : base(text)
        {
            Assert.Log.Error(text);
        }
    }

    /// <summary>
    /// I just wanted one simple set of runtime assertions I can use on any platform, with or
    /// without Unity or Nuint or any other dependancies.
    /// </summary>
    public static class Assert
    {
        public static Flow.Impl.Logger Log = new Flow.Impl.Logger("Assert", true, true);
        public static bool AssertionsThrow = true;

        public static void Throw(string text = "")
        {
            if (AssertionsThrow)
                throw new Exception(text);
            Log.Error(text);
        }

        public static void Error(string text)
        {
            if (AssertionsThrow)
                throw new AssertionException(text);
            Log.Error(text);
        }

        public static void IsNull(object q)
        {
            if (q == null)
                return;
            Error("Null expected");
        }

        public static void IsNotNull(object q)
        {
            if (q != null)
                return;
            Error("Non-Null expected");
        }

        public static void IsNotEmpty<T>(IEnumerable<T> e)
        {
            if (e != null && e.Any())
                return;
            Error($"Expected not empty");
        }

        public static void IsEmpty<T>(IEnumerable<T> e)
        {
            if (e == null || !e.Any())
                return;
            Error($"Expected empty");
        }

        public static void IsTrue(bool val)
        {
            if (val)
                return;
            Error("Exptected true");
        }

        public static void IsFalse(bool val)
        {
            if (!val)
                return;
            Error($"Expected false");
        }

        public static void AreEqual<T>(T a, T b)
        {
            if (a.Equals(b))
                return;
            Error($"{a} != {b}");
        }

        public static void AreNotEqual<T>(T a, T b)
        {
            if (!a.Equals(b))
                return;
            Error($"{a} == {b}");
        }

        public static void AreSame(object a, object b)
        {
            if (ReferenceEquals(a, b))
                return;
            Error("$Expect same objects");
        }

        public static void AreNotSame(object a, object b)
        {
            if (!ReferenceEquals(a, b))
                return;
            Error($"Expect not same objects");
        }

        public static void IsLess(int a, int b)
        {
            if (a < b)
                return;
            Error("{a} >= b");
        }

        public static void IsGreater(int a, int b)
        {
            if (a > b)
                return;
            Error("{a} <= b");
        }
    }
}
