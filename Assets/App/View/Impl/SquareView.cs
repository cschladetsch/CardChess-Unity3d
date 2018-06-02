using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Common;

#pragma warning disable 649

namespace App.View.Impl
{
    class SquareView
        : ViewBase
    {
        public EColor Color { get; }
        public float Length;
    }
}
