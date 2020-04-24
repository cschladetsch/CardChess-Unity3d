using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cogobyte.ProceduralLibrary
{
    [CreateAssetMenu(fileName = "CircularPathArray", menuName = "Cogobyte/ProceduralLibrary/PathArrays/CircularPathArray", order = 2)]
    [System.Serializable]
    //Draws a Circle path
    public class CircularPathArray : PathArray
    {
        //Radius of the circle
        public float radius = 2f;
        //Number of circle segments
        [Range(3, 250)]
        public int levelOfDetail =12;
        //Y position of the circle
        float height = 0;

        public override void CalculatePath()
        {
            Quaternion rot = Quaternion.Euler(rotation);
            path = new List<Vector3>();
            float circleSection = (2 * Mathf.PI) / levelOfDetail;
            for (int i = 0; i <= levelOfDetail; i++)
            {
                path.Add(translation + rot * Vector3.Scale(scale, new Vector3(radius * Mathf.Cos(i * circleSection), height, radius * Mathf.Sin(i * circleSection))));
            }
            ObstacleCheck();
            base.CalculatePath();
        }
    }
}
