using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using UniRx;
using UnityEngine;

namespace Assets.Plugins.UniRx.Test.Editor
{
	[TestFixture]
	public class TestCollections
	{
		[Test]
		public void TestList()
		{
			IReactiveCollection<int> list = new ReactiveCollection<int>();
			list.ObserveAdd().Subscribe(print);
			list.Add(1);
			list.Add(2);
		}

		static void print(CollectionAddEvent<int> e)
		{
			Debug.Log(e);
		}
	}
}
