using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cogobyte.ProceduralLibrary
{
    //Path specified by a list of points
    [CreateAssetMenu(fileName = "CustomPathArray", menuName = "Cogobyte/ProceduralLibrary/PathArrays/CustomPathArray", order = 1)]
    [System.Serializable]
    public class CustomPathArray : PathArray
    {
        //List of points to use
        public List<Vector3> customPath = new List<Vector3>() {new Vector3(0,0,0),new Vector3(1,0,0)};
        //place additional points between specified points at even intervals
        [Range(1, 100)]
        public int levelOfDetailAlongPath = 1;
        private static float errorRate = 0.0001f;

        void OnValidate()
        {
            if (customPath.Count < 2) {
                customPath = new List<Vector3>() { new Vector3(0, 0, 0), new Vector3(1, 0, 0) };
            }
        }

        public override void CalculatePath()
        {
            Quaternion rot = Quaternion.Euler(rotation);
            path = new List<Vector3>();
            if (customPath.Count < 2) return;
            float pathLength = 0;
            float currentPercentage = 0;
            Vector3 firstPathPoint = translation + rot * Vector3.Scale(scale, customPath[0]);
            Vector3 secondPathPoint;
            path.Add(firstPathPoint);
            currentPercentage += 1.0f / levelOfDetailAlongPath;
            for (int i = 1; i < customPath.Count; i++)
            {
                if ((customPath[i] - customPath[i - 1]).magnitude < errorRate) continue;
                secondPathPoint = translation + rot * Vector3.Scale(scale, customPath[i]);
                while (true)
                {
                    if (currentPercentage + 1.0f / levelOfDetailAlongPath >= 1.0f)
                    {
                        if ((secondPathPoint - firstPathPoint).magnitude > errorRate)
                        {
                            path.Add(secondPathPoint);
                        }
                        currentPercentage = 1.0f / levelOfDetailAlongPath;
                        break;
                    }
                    if ((secondPathPoint - firstPathPoint).magnitude > errorRate)
                    {
                        path.Add(firstPathPoint + currentPercentage * (secondPathPoint - firstPathPoint));
                    }
                    currentPercentage += 1.0f / levelOfDetailAlongPath;
                }
                pathLength += (secondPathPoint - firstPathPoint).magnitude;
                firstPathPoint = secondPathPoint;
            }
            if (closed)
            {
                if ((customPath[0] - customPath[customPath.Count - 1]).magnitude > errorRate)
                {
                    secondPathPoint = translation + rot * Vector3.Scale(scale, customPath[0]);
                    while (true)
                    {
                        if (currentPercentage + 1.0f / levelOfDetailAlongPath >= 1.0f)
                        {
                            if ((secondPathPoint - firstPathPoint).magnitude > errorRate)
                            {
                                path.Add(secondPathPoint);
                            }
                            currentPercentage = 1.0f / levelOfDetailAlongPath;
                            break;
                        }
                        if ((secondPathPoint - firstPathPoint).magnitude > errorRate)
                        {
                            path.Add(firstPathPoint + currentPercentage * (secondPathPoint - firstPathPoint));
                        }
                        currentPercentage += 1.0f / levelOfDetailAlongPath;
                    }
                    pathLength += (secondPathPoint - firstPathPoint).magnitude;
                }
            }
            ObstacleCheck();
            base.CalculatePath();
        }
    }
}
