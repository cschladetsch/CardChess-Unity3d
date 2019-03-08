using System.Collections.Generic;
using System.Linq;
using CoLib;
using Dekuple.View.Impl;
using UnityEngine;

namespace App.View.Impl1
{
    using Common;

    public class BoardOverlayView
        : ViewBase
    {
        public BoardOverlaySquareView BoardOverlaySquareViewPrefab;
        public float Zoffset = 1;

        public override void Create()
        {
            base.Create();
            Clear();
        }

        [ContextMenu("BoardOverlay-Clear")]
        public void Clear()
        {
            var squares = (from Transform tr in transform select tr.GetComponent<BoardOverlaySquareView>()).Where(s => s != null).ToList();
            ////var anims = squares.Select(s => s.Clear()).ToList();
            //var destroy = squares.Select(s => Commands.Do(() => Destroy(s)));
            _Queue.Enqueue(
                squares.ForEachParallel(sq => sq.Clear())//,
                // TODO Why are there multiple Destroy's on same object?
                //squares.ForEachParallel(sq => Commands.Do(() => Destroy(sq.gameObject)))
            );
            _Queue.Process();
        }

        public void Add(IEnumerable<Coord> coords, Color color)
        {
            if (coords == null)
                return;

            color.a = 0.5f;
            var squares = new List<BoardOverlaySquareView>();
            foreach (var c in coords)
            {
                var sq = Instantiate(BoardOverlaySquareViewPrefab);
                squares.Add(sq);
                sq.transform.SetParent(transform);
                sq.transform.localPosition = new Vector3(c.x, c.y, Zoffset);
                sq.transform.localScale = Vector3.one;
            }
            _Queue.Enqueue(
                squares.ForEachParallel(sq => sq.SetColor(color))
            );
        }
    }
}
