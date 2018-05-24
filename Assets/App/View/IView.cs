using System;
using UnityEngine;

namespace App.View
{
	using Common;
	using Registry;

	public interface IView : IKnown, IHasDestroyHandler<IView>, IHasRegistry<IView>
    {
		GameObject GameObject { get; }
    }
}
