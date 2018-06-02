using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Common;
using UniRx;
using UnityEngine;

#pragma warning disable 649

namespace App.View.Impl
{
    class SquareView
        : ViewBase
    {
        public EColor Color;
        public float Length;
        public Coord Coord;

        public IReadOnlyReactiveProperty<bool> MouseOver => _mouseOver;

        protected override void Begin()
        {
            _mouseOver = new BoolReactiveProperty(false);
            base.Begin();
            //_layerBitMask = 1 << gameObject.layer;//LayerMask.NameToLayer("BoardSquare");
        }

        protected override void Step()
        {
            base.Step();
        }

        public void EnterMouseOver()
        {
            Info("Enter {this}");
            _mouseOver.Value = true;
        }

        public override int GetHashCode()
        {
            return Coord.GetHashCode();
        }

        public override bool Equals(object other)
        {
            var sq = other as SquareView;
            if (sq == null)
                return false;
            return Coord == sq.Coord;
        }

        public void LeaveMouseOver()
        {
            Info("Leave {this}");
            _mouseOver.Value = false;
        }

        public override string ToString()
        {
            return $"{Color}-square at {Coord}";// (WP={transform.position})";
        }

        private int _layerBitMask;
        private BoolReactiveProperty _mouseOver;
    }
}
