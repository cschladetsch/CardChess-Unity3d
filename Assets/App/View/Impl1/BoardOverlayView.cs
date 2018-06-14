using System.Collections.Generic;
using UnityEngine;

namespace App.View.Impl1
{
    using Common;

    public class BoardOverlayView
        : ViewBase
    {
        public BoardOverlaySquareView BoardOverlaySquareViewPrefab;
        public float Zoffset = 1;

        protected override void Begin()
        {
            base.Begin();
            Clear();
        }

        [ContextMenu("BoardOverlay-Clear")]
        public void Clear()
        {
            foreach (Transform tr in transform)
                Destroy(tr.gameObject);
        }

        [ContextMenu("BoardOverlay-Mock")]
        public void AddMock()
        {
            var coords = new Coord[]
            {
                new Coord(0, 0),
                new Coord(1, 1),
                new Coord(2, 2),
                new Coord(3, 3),
                new Coord(4, 4),
            };
            Add(coords, Color.red);

            var coords2 = new Coord[]
            {
                new Coord(4, 0),
                new Coord(4, 1),
                new Coord(4, 2),
                new Coord(4, 3),
                new Coord(4, 4),
            };
            Add(coords2, Color.green);
        }

        public void Add(IEnumerable<Coord> coords, Color color)
        {
            if (coords == null)
                return;

            color.a = 0.5f;
            foreach (var c in coords)
            {
                var sq = Instantiate(BoardOverlaySquareViewPrefab);
                sq.Color = color;
                sq.transform.SetParent(transform);
                sq.transform.localPosition = new Vector3(c.x, c.y, Zoffset);
                sq.transform.localScale = Vector3.one;
            }
        }
    }
}
