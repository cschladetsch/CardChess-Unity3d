using System.Collections.Generic;
using System.Linq;
using CoLib;
using Dekuple.View.Impl;
using UnityEngine;

namespace App.View.Impl1
{
    using Common;

    /// <summary>
    /// Overlay view of the board; highligted squares
    /// </summary>
    public class BoardOverlayView
        : ViewBase
    {
        /// <summary>
        /// What to use to represent an overlaid square
        /// </summary>
        public BoardOverlaySquareView BoardOverlaySquareViewPrefab;

        /// <summary>
        /// The game is rendered using orthogonal projection; this is the offset of the overlay
        /// relative to the board itself.
        /// </summary>
        public float Zoffset = 1;

        protected override bool Create()
        {
            if (!base.Create())
                return false;
            
            Clear();

            return true;
        }

        [ContextMenu("BoardOverlay-Clear")]
        public void Clear()
        {
            var squares = (from Transform tr in transform select tr.GetComponent<BoardOverlaySquareView>()).Where(s => s != null).ToList();
            _Queue.Sequence(
                squares.ForEachParallel(sq => sq.Clear())
            );
            _Queue.Process();
        }

        /// <summary>
        /// Add a set of coordinates with the given color to the overlay
        /// </summary>
        /// <param name="coords">The coords to render</param>
        /// <param name="color">The color to use when rendering the overlay for these coords</param>
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

            _Queue.Sequence(
                squares.ForEachParallel(sq => sq.SetColor(color))
            );
        }
    }
}
