namespace App.View.Impl1
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using Dekuple.View.Impl;
    using Common;
    using CoLib;

    /// <summary>
    /// Overlay view of the board; highlighted squares
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
<<<<<<< HEAD
            var squares = (from Transform tr in transform select tr.GetComponent<BoardOverlaySquareView>()).Where(s => s != null).ToList();
            _Queue.Sequence(
                squares.ForEachParallel(sq => sq.Clear())
            );
            _Queue.Process();
=======
            var squares = (from Transform tr in transform select tr.GetComponent<BoardOverlaySquareView>())
                .Where(s => s != null).ToList();

            foreach (var sq in squares)
                sq.Clear();
            
            // fancier but slower
            // _Queue.Enqueue(
            //     squares.ForEachParallel(sq => sq.Clear())
            // );
            // _Queue.Process();
>>>>>>> 0d79684a249e5d19f2cd1de7351112f6c5354de9
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
                // TODO: object pool
                var sq = Instantiate(BoardOverlaySquareViewPrefab);
                squares.Add(sq);
                var tr = sq.transform;
                tr.SetParent(transform);
                tr.localPosition = new Vector3(c.x, c.y, Zoffset);
                tr.localScale = Vector3.one;
            }

<<<<<<< HEAD
            _Queue.Sequence(
                squares.ForEachParallel(sq => sq.SetColor(color))
            );
=======
            foreach (var sq in squares)
                sq.SetColor(color);
                
            // fancy but slow
            // _Queue.Enqueue(
            //     squares.ForEachParallel(sq => sq.SetColor(color))
            // );
>>>>>>> 0d79684a249e5d19f2cd1de7351112f6c5354de9
        }
    }
}
