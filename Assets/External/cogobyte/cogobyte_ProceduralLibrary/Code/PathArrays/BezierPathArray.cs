using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cogobyte.ProceduralLibrary
{
    [CreateAssetMenu(fileName = "BezierPathArray", menuName = "Cogobyte/ProceduralLibrary/PathArrays/BezierPathArray", order = 1)]
    [System.Serializable]
    public class BezierPathArray : PathArray
    {
        public BezierSpline bezierSpline = new BezierSpline();
        public int levelOfDetail = 10;

        void OnValidate()
        {
            levelOfDetail = Mathf.Max(levelOfDetail, 2);
            if (bezierSpline.points.Count < 2) bezierSpline.Reset();
        }

        public override void CalculatePath()
        {
            path = new List<Vector3>();
            float pathLength = 0;
            Quaternion rot = Quaternion.Euler(rotation);
            float stepSize = 1f / levelOfDetail;
            for (int i = 0; i <= levelOfDetail; i++)
            {
                    path.Add(translation + rot * (bezierSpline.GetPoint(i * stepSize)));
            }
            for(int i = 1; i < path.Count; i++)
            {
                pathLength += (path[i] - path[i-1]).magnitude;
            }
            ObstacleCheck();
            base.CalculatePath();
        }
    }
}
