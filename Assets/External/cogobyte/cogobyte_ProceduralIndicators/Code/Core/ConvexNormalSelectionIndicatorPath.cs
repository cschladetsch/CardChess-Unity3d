using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cogobyte.ProceduralIndicators
{
    [CreateAssetMenu(fileName = "ConvexNormalSelectionIndicatorPath", menuName = "Cogobyte/ProceduralIndicators/Selection/ConvexNormalSelectionIndicatorPath", order = 5)]
    public class ConvexNormalSelectionIndicatorPath : NormalSelectionIndicatorPath
    {
        public override void GenerateVertices()
        {
            List<Vector3> path = pathArray.path;
            float currentPathLength = 0;
            Quaternion rot = Quaternion.Euler(rotation);
            Vector3 centerPoint = pathArray.translation;
            Debug.DrawLine(centerPoint, centerPoint + Vector3.up * 6, Color.red, 20);
            for (int i = 0; i < path.Count; i++)
            {
                path[i] -= centerPoint;
            }
            vertices.Add(path[0].normalized * (path[0].magnitude * (1 + outerWidthFunction.Evaluate(0))) + heightFunction.Evaluate(0) * (rot * Vector3.up));
            upNormals.Add(rot * Vector3.up);
            sideNormals.Add(rot * Vector3.right);
            lowerVertices.Add(path[0].normalized * (path[0].magnitude * (1 + outerWidthFunction.Evaluate(0))) - lowerBoundFunction.Evaluate(0) * (rot * Vector3.up));
            vertColors.Add(outerColor.Evaluate(0));
            vertices.Add(path[0].normalized * (path[0].magnitude * (1 - innerWidthFunction.Evaluate(0))) + heightFunction.Evaluate(0) * (rot * Vector3.up));
            upNormals.Add(rot * Vector3.up);
            sideNormals.Add(rot * Vector3.left);
            lowerVertices.Add(path[0].normalized * (path[0].magnitude * (1 - innerWidthFunction.Evaluate(0))) - lowerBoundFunction.Evaluate(0) * (rot * Vector3.up));
            vertColors.Add(innerColor.Evaluate(0));
            for (int i = 1; i < path.Count - 1; i++)
            {
                currentPathLength += (path[i] - path[i - 1]).magnitude;
                rot = Quaternion.Lerp(Quaternion.LookRotation(path[i] - path[i - 1], Vector3.up), Quaternion.LookRotation(path[i + 1] - path[i], Vector3.up), 0.5f);
                vertices.Add(path[i].normalized * (path[i].magnitude * (1 + outerWidthFunction.Evaluate(currentPathLength / pathArray.maxPathLength * outerWidthFunctionLength))) + heightFunction.Evaluate(currentPathLength / pathArray.maxPathLength * heightFunctionLength) * (rot * Vector3.up));
                upNormals.Add(rot * Vector3.up);
                sideNormals.Add(rot * Vector3.right);
                lowerVertices.Add(path[i].normalized * (path[i].magnitude * (1 + outerWidthFunction.Evaluate(currentPathLength / pathArray.maxPathLength * outerWidthFunctionLength))) - lowerBoundFunction.Evaluate(currentPathLength / pathArray.maxPathLength * heightFunctionLength) * (rot * Vector3.up));
                vertColors.Add(outerColor.Evaluate(currentPathLength));
                vertices.Add(path[i].normalized * (path[i].magnitude * (1 - innerWidthFunction.Evaluate(currentPathLength / pathArray.maxPathLength * innerWidthFunctionLength))) + heightFunction.Evaluate(currentPathLength / pathArray.maxPathLength * heightFunctionLength) * (rot * Vector3.up));
                upNormals.Add(rot * Vector3.up);
                sideNormals.Add(rot * Vector3.left);
                lowerVertices.Add(path[i].normalized * (path[i].magnitude * (1 - innerWidthFunction.Evaluate(currentPathLength / pathArray.maxPathLength * innerWidthFunctionLength))) - lowerBoundFunction.Evaluate(currentPathLength / pathArray.maxPathLength * heightFunctionLength) * (rot * Vector3.up));
                vertColors.Add(innerColor.Evaluate(currentPathLength));
            }
            vertices.Add(vertices[0]);
            upNormals.Add(upNormals[0]);
            sideNormals.Add(sideNormals[0]);
            doubleVerts.Add(0);
            doubleVertsSecond.Add(vertices.Count - 1);
            lowerVertices.Add(lowerVertices[0]);
            vertColors.Add(outerColor.Evaluate(1));
            vertices.Add(vertices[1]);
            upNormals.Add(upNormals[1]);
            sideNormals.Add(sideNormals[1]);
            doubleVerts.Add(1);
            doubleVertsSecond.Add(vertices.Count - 1);
            lowerVertices.Add(lowerVertices[1]);
            vertColors.Add(innerColor.Evaluate(1));
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i] += centerPoint;
            }
            for (int i = 0; i < lowerVertices.Count; i++)
            {
                lowerVertices[i] += centerPoint;
            }
        }
    }
}
