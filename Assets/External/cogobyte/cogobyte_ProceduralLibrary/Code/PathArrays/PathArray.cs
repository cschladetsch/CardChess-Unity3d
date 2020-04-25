using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cogobyte.ProceduralLibrary
{
    public abstract class PathArray : ScriptableObject
    {
        //Calculated Path Points
        [System.NonSerialized]
        public List<Vector3> path;
        //change the transform of the indicator
        public Vector3 translation;
        public Vector3 rotation;
        public Vector3 scale = new Vector3(1,1,1);
        //Type of path projection
        public enum ObstacleCheckMode { None, Parellel, Projection }
        public ObstacleCheckMode obstacleCheck = ObstacleCheckMode.None;
        //This vector has origin in position of the center of the path outline
        public Vector3 obstacleCheckDirection = new Vector3(0, -10, 0);
        //Layers that will be projected to
        public LayerMask obstacleLayer;
        //How far from ground will the path be projected
        public float distanceFromGround = 1;
        //Calculated length of the path for percentage calculations
        [System.NonSerialized]
        public float maxPathLength;
        //additional endpoint at start point
        public bool closed = true;

        public bool counterClockwise = false;

        public virtual void CalculatePath()
        {
            maxPathLength = 0;
            for (int i = 1; i < path.Count; i++)
            {
                maxPathLength += (path[i] - path[i - 1]).magnitude;
            }
            if (counterClockwise)
            {
                path.Reverse();
            }
        }

        //Changes the path points by projecting them on projectable objects (or if no objects are hit by ray to the length of the check direction)
        public void ObstacleCheck()
        {
            if (obstacleCheck == ObstacleCheckMode.Parellel)
            {
                RaycastHit hit;
                Vector3 obstacleCheckPosition = (translation - obstacleCheckDirection);
                for (int i = 0; i < path.Count; i++)
                {
                    obstacleCheckPosition = path[i] - obstacleCheckDirection;
                    if (Physics.Raycast(obstacleCheckPosition, -Vector3.up, out hit, obstacleCheckDirection.magnitude, obstacleLayer, QueryTriggerInteraction.Ignore))
                    {
                        path[i] = hit.point + (obstacleCheckPosition - hit.point).normalized * distanceFromGround;
                    }

                }
            }
            else if (obstacleCheck == ObstacleCheckMode.Projection)
            {
                for (int i = 0; i < path.Count; i++)
                {
                    RaycastHit hit;
                    Vector3 obstacleCheckPosition = (translation - obstacleCheckDirection);
                    if (Physics.Raycast(obstacleCheckPosition, path[i] - obstacleCheckPosition, out hit, (path[i] - obstacleCheckPosition).magnitude, obstacleLayer, QueryTriggerInteraction.Ignore))
                    {
                        path[i] = hit.point + (obstacleCheckPosition - hit.point).normalized * distanceFromGround;
                    }
                }
            }
        }
        //Checks projection from translation as origin
        public void ParralelObstacleCheck(List<Vector3> checkedPath)
        {
            RaycastHit hit;
            Vector3 obstacleCheckPosition = (translation - obstacleCheckDirection);
            for (int i = 0; i < checkedPath.Count; i++)
            {
                obstacleCheckPosition = checkedPath[i] - obstacleCheckDirection;
                if (Physics.Raycast(obstacleCheckPosition, -Vector3.up, out hit, obstacleCheckDirection.magnitude, obstacleLayer, QueryTriggerInteraction.Ignore))
                {
                    checkedPath[i] = hit.point + (obstacleCheckPosition - hit.point).normalized * distanceFromGround;
                }

            }
        }

        public void ProjectionObstacleCheck(List<Vector3> checkedPath)
        {
            for (int i = 0; i < checkedPath.Count; i++)
            {
                RaycastHit hit;
                Vector3 obstacleCheckPosition = (translation - obstacleCheckDirection);
                if (Physics.Raycast(obstacleCheckPosition, checkedPath[i] - obstacleCheckPosition, out hit, (checkedPath[i] - obstacleCheckPosition).magnitude, obstacleLayer, QueryTriggerInteraction.Ignore))
                {
                    checkedPath[i] = hit.point + (obstacleCheckPosition - hit.point).normalized * distanceFromGround;
                }
            }
        }

        //Checks projection from a point as origin
        public void ParralelObstacleCheckPoint(ref Vector3 point)
        {
            RaycastHit hit;
            if (Physics.Raycast(point - obstacleCheckDirection, -Vector3.up, out hit, obstacleCheckDirection.magnitude, obstacleLayer, QueryTriggerInteraction.Ignore))
            {
                point = hit.point + (point - obstacleCheckDirection - hit.point).normalized * distanceFromGround;
            }
        }

        public void ProjectionObstacleCheckPoint(ref Vector3 point)
        {
            RaycastHit hit;
            Vector3 obstacleCheckPosition = (translation - obstacleCheckDirection);
            if (Physics.Raycast(obstacleCheckPosition, point - obstacleCheckPosition, out hit, (point - obstacleCheckPosition).magnitude, obstacleLayer, QueryTriggerInteraction.Ignore))
            {
                point = hit.point + (obstacleCheckPosition - hit.point).normalized * distanceFromGround;
            }
        }
    }
}
