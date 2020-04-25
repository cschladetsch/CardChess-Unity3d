using UnityEngine;
using System;
using System.Collections.Generic;

namespace Cogobyte.ProceduralLibrary
{
    [System.Serializable]
    public class BezierSpline
    {
        [SerializeField]
        public List<BezierSplinePoint> points = new List<BezierSplinePoint>();
        [SerializeField]
        public bool loop;

        public BezierSpline()
        {
            Reset();
        }

        public BezierControlPointMode GetControlPointMode(int index)
        {
            return points[index].controlPointMode;
        }

        //Sets control point mode and enforces it
        public void SetControlPointMode(int index, BezierControlPointMode mode)
        {
            points[index].controlPointMode = mode;
            points[index].MoveFrontControlPoint(points[index].frontControlPoint);
        }

        public int CurveCount
        {
            get
            {
                return points.Count - 1;
            }
        }

        //Gets point on curve for percentage t
        public Vector3 GetPoint(float t)
        {
            int i;
            if (t >= 1f)
            {
                t = 1f;
                if (loop) i = points.Count - 1;
                else i = points.Count - 2;
            }
            else
            {
                if (loop) t = Mathf.Clamp01(t) * (CurveCount+1);
                else t = Mathf.Clamp01(t) * CurveCount;
                i = (int)t;
                t -= i;
            }
            if(loop && i == points.Count - 1) return Bezier.GetPoint(points[i].position, points[i].frontControlPoint, points[0].backControlPoint, points[0].position, t);
            return Bezier.GetPoint(points[i].position, points[i].frontControlPoint, points[i + 1].backControlPoint, points[i + 1].position, t);
        }

        //Returns the vector of direction of curve at percentage t
        public Vector3 GetVelocity(float t)
        {
            int i;
            if (t >= 1f)
            {
                t = 1f;
                if (loop) i = points.Count - 1;
                else i = points.Count - 2;
            }
            else
            {
                if (loop) t = Mathf.Clamp01(t) * (CurveCount + 1);
                else t = Mathf.Clamp01(t) * CurveCount;
                i = (int)t;
                t -= i;
            }
            if (loop && i == points.Count - 1) return Bezier.GetFirstDerivative(points[i].position, points[i].frontControlPoint, points[0].backControlPoint, points[0].position, t);
            return Bezier.GetFirstDerivative(points[i].position, points[i].frontControlPoint, points[i + 1].backControlPoint, points[i + 1].position, t);
        }

        //Returns the vector of direction of curve at percentage t
        public Vector3 GetDirection(float t)
        {
            return GetVelocity(t).normalized;
        }

        //Removes a curve at index
        public void DeleteCurve(int index)
        {
            points.RemoveAt(index);
        }

        //Splits one curve into two in the middle
        public void SplitCurve(int index)
        {
            BezierSplinePoint p = new BezierSplinePoint();
            BezierSplinePoint secondPoint = (index != points.Count - 1) ? points[index + 1] : points[0];
            p.position = Bezier.GetPoint(points[index].position, points[index].frontControlPoint, secondPoint.backControlPoint, secondPoint.position, 0.5f);
            p.backControlPoint = p.position - Bezier.GetFirstDerivative(points[index].position, points[index].frontControlPoint, secondPoint.backControlPoint, secondPoint.position, 0.5f).normalized;
            p.frontControlPoint = p.position + Bezier.GetFirstDerivative(points[index].position, points[index].frontControlPoint, secondPoint.backControlPoint, secondPoint.position, 0.5f).normalized;
            p.controlPointMode = BezierControlPointMode.Free;
            points.Insert(index + 1, p);
        }

        //Adds a next curve
        public void AddCurve()
        {
            points.Add(new BezierSplinePoint());
            points[points.Count - 1].position = points[points.Count - 2].position + new Vector3(1, 0, 0);
            points[points.Count - 1].backControlPoint = points[points.Count - 1].position + new Vector3(-0.5f, 0, 0);
            points[points.Count - 1].frontControlPoint = points[points.Count - 1].position + new Vector3(0.5f, 0, 0);
            points[points.Count - 1].controlPointMode = BezierControlPointMode.Free;
        }

        //Sets the default curve
        public void Reset()
        {
            points = new List<BezierSplinePoint>();
            points.Add(new BezierSplinePoint());
            points[0].position = new Vector3(0, 0, 0);
            points[0].backControlPoint = new Vector3(-0.5f, 0, 0);
            points[0].frontControlPoint = new Vector3(0.5f, 0, 0);
            points[0].controlPointMode = BezierControlPointMode.Free;
            points.Add(new BezierSplinePoint());
            points[1].position = new Vector3(1, 0, 0);
            points[1].backControlPoint = new Vector3(0.5f, 0, 0);
            points[1].frontControlPoint = new Vector3(1.5f, 0, 0);
            points[1].controlPointMode = BezierControlPointMode.Free;
        }
    }

    //A bazier point containing two control points for previous and next curve
    [System.Serializable]
    public class BezierSplinePoint{
        public Vector3 position;
        public Vector3 backControlPoint;
        public Vector3 frontControlPoint;
        public BezierControlPointMode controlPointMode;

        public void MovePoint(Vector3 destination)
        {
            backControlPoint += destination - position;
            frontControlPoint += destination - position;
            position = destination;
        }

        //Enforces other control point if one is moved
        public void MoveFrontControlPoint(Vector3 destination)
        {
            frontControlPoint = destination;
            if (controlPointMode == BezierControlPointMode.Free) return;
            Vector3 enforcedTangent = position - frontControlPoint;
            if (controlPointMode == BezierControlPointMode.Aligned)
            {
                enforcedTangent = enforcedTangent.normalized * Vector3.Distance(position, backControlPoint);
            }
            backControlPoint = position + enforcedTangent;
        }

        //Enforces other control point if one is moved
        public void MoveBackControlPoint(Vector3 destination)
        {
            backControlPoint = destination;
            if (controlPointMode == BezierControlPointMode.Free) return;
            Vector3 enforcedTangent = position - backControlPoint;
            if (controlPointMode == BezierControlPointMode.Aligned)
            {
                enforcedTangent = enforcedTangent.normalized * Vector3.Distance(position, frontControlPoint);
            }
            frontControlPoint = position + enforcedTangent;
        }

    }

    public enum BezierControlPointMode
    {
        Free,
        Aligned,
        Mirrored
    }
}