using System.Collections;
using System.Collections.Generic;
using App.Common;
using UnityEngine;
using UnityEngine.UI;

using CoLib;
using Flow;

namespace App.View.Impl1
{
    public class BoardOverlayView
        : ViewBase
    {
        public BoardOverlaySquareView BoardOverlaySquareViewPrefab;
        public float Zoffset = 1;

        [ContextMenu("BoardOverlay-Clear")]
        public void Clear()
        {
            foreach (Transform tr in transform)
#if UNITY_EDITOR
                DestroyImmediate(tr.gameObject);
#else
                Destroy(tr.gameObject);
#endif
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
