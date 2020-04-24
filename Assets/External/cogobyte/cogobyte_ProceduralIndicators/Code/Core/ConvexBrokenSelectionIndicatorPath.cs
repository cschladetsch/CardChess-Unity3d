using Cogobyte.ProceduralLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cogobyte.ProceduralIndicators
{
    [CreateAssetMenu(fileName = "ConvexBrokenSelectionIndicatorPath", menuName = "Cogobyte/ProceduralIndicators/Selection/ConvexBrokenSelectionIndicatorPath", order = 6)]
    public class ConvexBrokenSelectionIndicatorPath : BrokenSelectionIndicatorPath
    {
        public override void generate()
        {
            base.generate();
        }

        public override void GenerateVertices()
        {
            List<Vector3> path = pathArray.path;
            float brokenLineLength = pathArray.maxPathLength / numberOfBrokenLines;
            float brakeDistance = percentageOfEmptySpace * brokenLineLength;
            brokenLineLength -= brakeDistance;
            //iterator for path
            int currentPathIndex = 1;
            //distance traveled ?since last brokenPath point
            float currentDistance = 0;
            //is it currently in brokenLine or brake
            bool drawStatus = false;
            //it will always start with line
            currentDistance = brakeDistance;
            float magnitudeOfPathPart = (path[1] - path[0]).magnitude;
            float currentPathLength = 0;

            brokenOpenClosePath = new List<int>();
            Vector3 centerPoint = new Vector3(0, 0, 0);
            for (int i = 0; i < path.Count; i++)
            {
                centerPoint += path[i];
            }
            centerPoint /= path.Count;
            for (int i = 0; i < path.Count; i++)
            {
                path[i] -= centerPoint;
            }
            Vector3 currentShapeLocation = path[0];
            Quaternion rot = Quaternion.Euler(rotation);
            while (currentPathIndex < path.Count)
            {
                //while we have points but still no ending for brokenline or brake
                while (currentDistance >= magnitudeOfPathPart)
                {
                    //add extrude point only if we have brokenline
                    if (drawStatus)
                    {
                        vertices.Add(path[currentPathIndex].normalized * (path[currentPathIndex].magnitude * (1 + outerWidthFunction.Evaluate(currentPathLength / pathArray.maxPathLength * outerWidthFunctionLength))) + rot * Vector3.up * heightFunction.Evaluate(currentPathLength / pathArray.maxPathLength * heightFunctionLength));
                        upNormals.Add(rot * Vector3.up);
                        sideNormals.Add(path[currentPathIndex].normalized);
                        lowerVertices.Add(path[currentPathIndex].normalized * (path[currentPathIndex].magnitude * (1 + outerWidthFunction.Evaluate(currentPathLength / pathArray.maxPathLength * outerWidthFunctionLength))) - rot * Vector3.up * lowerBoundFunction.Evaluate(currentPathLength / pathArray.maxPathLength * lowerBoundFunctionLength));
                        vertColors.Add(outerColor.Evaluate(currentPathLength));
                        vertices.Add(path[currentPathIndex].normalized * (path[currentPathIndex].magnitude * (1 - innerWidthFunction.Evaluate(currentPathLength / pathArray.maxPathLength * innerWidthFunctionLength))) + rot * Vector3.up * heightFunction.Evaluate(currentPathLength / pathArray.maxPathLength * heightFunctionLength));
                        upNormals.Add(rot * Vector3.up);
                        sideNormals.Add(-path[currentPathIndex].normalized);
                        lowerVertices.Add(path[currentPathIndex].normalized * (path[currentPathIndex].magnitude * (1 - innerWidthFunction.Evaluate(currentPathLength / pathArray.maxPathLength * innerWidthFunctionLength))) - rot * Vector3.up * lowerBoundFunction.Evaluate(currentPathLength / pathArray.maxPathLength * lowerBoundFunctionLength));
                        vertColors.Add(innerColor.Evaluate(currentPathLength));
                        if (brokenOpenClosePath.Count == 0 || brokenOpenClosePath[brokenOpenClosePath.Count - 1] == 2)
                        {
                            brokenOpenClosePath.Add(0);
                        }
                        else
                        {
                            brokenOpenClosePath.Add(1);
                        }
                    }
                    currentPathLength += magnitudeOfPathPart;
                    currentDistance -= magnitudeOfPathPart;
                    currentShapeLocation = path[currentPathIndex];
                    currentPathIndex++;
                    if (currentPathIndex >= path.Count) break;
                    magnitudeOfPathPart = (path[currentPathIndex] - path[currentPathIndex - 1]).magnitude;
                }
                if (currentPathIndex >= path.Count) break;
                //switch line and brake in distance between two path points
                while (currentDistance < magnitudeOfPathPart)
                {
                    currentPathLength += currentDistance;
                    Vector3 bPathItem = currentShapeLocation + (path[currentPathIndex] - path[currentPathIndex - 1]).normalized * currentDistance;
                    vertices.Add(bPathItem.normalized * (bPathItem.magnitude * (1 + outerWidthFunction.Evaluate(currentPathLength / pathArray.maxPathLength * outerWidthFunctionLength))) + rot * Vector3.up * heightFunction.Evaluate(currentPathLength / pathArray.maxPathLength * heightFunctionLength));
                    upNormals.Add(rot * Vector3.up);
                    sideNormals.Add(bPathItem.normalized);
                    lowerVertices.Add(bPathItem.normalized * (bPathItem.magnitude * (1 + outerWidthFunction.Evaluate(currentPathLength / pathArray.maxPathLength * outerWidthFunctionLength))) - rot * Vector3.up * lowerBoundFunction.Evaluate(currentPathLength / pathArray.maxPathLength * lowerBoundFunctionLength));
                    vertColors.Add(outerColor.Evaluate(currentPathLength));
                    vertices.Add(bPathItem.normalized * (bPathItem.magnitude * (1 - innerWidthFunction.Evaluate(currentPathLength / pathArray.maxPathLength * innerWidthFunctionLength))) + rot * Vector3.up * heightFunction.Evaluate(currentPathLength / pathArray.maxPathLength * heightFunctionLength));
                    upNormals.Add(rot * Vector3.up);
                    sideNormals.Add(-bPathItem.normalized);
                    lowerVertices.Add(bPathItem.normalized * (bPathItem.magnitude * (1 - innerWidthFunction.Evaluate(currentPathLength / pathArray.maxPathLength * innerWidthFunctionLength))) - rot * Vector3.up * lowerBoundFunction.Evaluate(currentPathLength / pathArray.maxPathLength * lowerBoundFunctionLength));
                    vertColors.Add(innerColor.Evaluate(currentPathLength));

                    if (drawStatus)
                    {
                        brokenOpenClosePath.Add(2);
                        drawStatus = !drawStatus;
                        currentDistance += brakeDistance;
                    }
                    else
                    {
                        brokenOpenClosePath.Add(0);
                        drawStatus = !drawStatus;
                        currentDistance += brokenLineLength;
                    }
                }
            }
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