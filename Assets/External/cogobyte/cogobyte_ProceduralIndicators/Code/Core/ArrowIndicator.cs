using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cogobyte.ProceduralLibrary;

namespace Cogobyte.ProceduralIndicators
{
    //Arrow Indicator Asset - Holds the arrow path and tips and combinesthem into final mesh
    [System.Serializable]
    public class ArrowIndicator : SolidMesh
    {
        //Path to generate, that path contains the head and tail tips
        [SerializeField]
        public ArrowPath arrowPath;
        //Extruded path + tips together into one extrude object
        public ComplexExtrude extrudeObject;
        int extrudePathIndex;
        [NonSerialized]
        public bool saveToSolid = false;

        //Fills the extrude object with tail part extrudes
        public void generateArrowTail()
        {
            ArrowTip arrowTail = arrowPath.arrowTail;
            if (arrowTail.arrowTipMode == ArrowTip.ArrowTipMode.Extrude)
            {
                //Tail has to be reversed for the complete extrude
                for (int i = arrowTail.extrudePoints.Length - 1; i >= 0; i--)
                {
                    extrudeObject.extrudePath[extrudePathIndex] = arrowTail.extrudePoints[i];
                    extrudeObject.polygonsToExtrude[extrudePathIndex] = arrowTail.primitives[i];
                    extrudeObject.openCloseExtrude[extrudePathIndex] = arrowTail.openClosed[i];
                    //Start extrude and end extrude are also reveresed when putting into complete extrude
                    if (extrudeObject.openCloseExtrude[extrudePathIndex] == 0)
                    {
                        extrudeObject.openCloseExtrude[extrudePathIndex] = 2;
                    }
                    else
                    {
                        if (extrudeObject.openCloseExtrude[extrudePathIndex] == 2) extrudeObject.openCloseExtrude[extrudePathIndex] = 0;
                    }
                    extrudePathIndex++;
                }
            }
        }

        //Fills the extrude object with path part extrudes
        public void generateArrowPath()
        {
            if (arrowPath.arrowPathMode != ArrowPath.ArrowPathMode.None && arrowPath.arrowPathMode != ArrowPath.ArrowPathMode.MadeOfShapes)
            {
                for (int i = 0; i < arrowPath.extrudePoints.Length; i++)
                {
                    extrudeObject.extrudePath[extrudePathIndex] = arrowPath.extrudePoints[i];
                    extrudeObject.polygonsToExtrude[extrudePathIndex] = arrowPath.primitives[i];
                    extrudeObject.openCloseExtrude[extrudePathIndex] = arrowPath.openClosed[i];
                    extrudePathIndex++;
                }
            }
        }

        //Fills the extrude object with head part extrudes
        public void generateArrowHead()
        {
            ArrowTip arrowHead = arrowPath.arrowHead;
            if (arrowHead.arrowTipMode == ArrowTip.ArrowTipMode.Extrude)
            {
                for (int i = 0; i < arrowHead.extrudePoints.Length; i++)
                {
                    extrudeObject.extrudePath[extrudePathIndex] = arrowHead.extrudePoints[i];
                    extrudeObject.polygonsToExtrude[extrudePathIndex] = arrowHead.primitives[i];
                    extrudeObject.openCloseExtrude[extrudePathIndex] = arrowHead.openClosed[i];
                    extrudePathIndex++;
                }
            }
        }

        public override void generate()
        {
            //Create a reference to path for future use
            arrowPath.arrowHead.arrowPath = arrowPath;
            arrowPath.arrowTail.arrowPath = arrowPath;
            //Default directions
            arrowPath.arrowHead.direction = 1;
            arrowPath.arrowTail.direction = -1;
            //Initialize the final mesh
            DestroyImmediate(mesh,true);
            mesh = new Mesh();
            //Final mesh vertices and triangles 
            List<Vector3> vertices = new List<Vector3>();
            List<Vector3> normalsList = new List<Vector3>();
            List<int> triangles = new List<int>();
            //Caluclate the main path trajectory 
            arrowPath.CalculatePath();
            //Caluclate the parts for tips from main path
            arrowPath.calculateArrowHeadStartPoints();
            //Calculate the main extrude part
            int extrudeLength = 0;
            int numverts = 0;
            //Generate the path mesh part
            if (arrowPath.arrowPathMode == ArrowPath.ArrowPathMode.Extrude || arrowPath.arrowPathMode == ArrowPath.ArrowPathMode.BrokenExtrude)
            {
                //If templates are not set, set them to default
                if (arrowPath.templatePrimitives == null)
                {
                    arrowPath.templatePrimitives = ScriptableObject.CreateInstance<PrimitivesList>();
                }
                if (arrowPath.templatePrimitives.primitivesList == null || arrowPath.templatePrimitives.primitivesList.Count == 0)
                {
                    arrowPath.templatePrimitives.primitivesList = new List<Primitive>();
                    arrowPath.templatePrimitives.primitivesList.Add(ScriptableObject.CreateInstance<PlanePrimitive>());
                }
                //Generate the template mesh that is needed for extrude calculations
                for (int i = 0; i < arrowPath.templatePrimitives.primitivesList.Count; i++)
                {
                    arrowPath.templatePrimitives.primitivesList[i].generate();
                }
                //Generete the path extrude part
                arrowPath.generate();
                if (!arrowPath.invalidPath)
                {
                    extrudeLength += arrowPath.extrudePoints.Length;
                }
            }
            else
            {
                //Calcualate vertices and triangles for made of shapes
                //that will be added to main mesh
                if (arrowPath.arrowPathMode == ArrowPath.ArrowPathMode.MadeOfShapes)
                {
                    arrowPath.GenerateMadeOfShapes(vertices, triangles,normalsList);
                    numverts = vertices.Count;
                }
            }
            //Calucalate the tail part trajectory
            arrowPath.arrowTail.calculatePath();
            if (arrowPath.arrowTail.arrowTipMode == ArrowTip.ArrowTipMode.Extrude)
            {
                //Generate and generate default template primitive shapes if not present
                if (arrowPath.arrowTail.templatePrimitives == null)
                {
                    arrowPath.arrowTail.templatePrimitives = ScriptableObject.CreateInstance<PrimitivesList>();
                }
                if (arrowPath.arrowTail.templatePrimitives.primitivesList == null || arrowPath.arrowTail.templatePrimitives.primitivesList.Count == 0)
                {
                    arrowPath.arrowTail.templatePrimitives.primitivesList = new List<Primitive>();
                    arrowPath.arrowTail.templatePrimitives.primitivesList.Add(ScriptableObject.CreateInstance<PlanePrimitive>());
                }
                for (int i = 0; i < arrowPath.arrowTail.templatePrimitives.primitivesList.Count; i++)
                {
                    arrowPath.arrowTail.templatePrimitives.primitivesList[i].generate();
                }
                //Caluclate tail extrude part
                arrowPath.arrowTail.generate();
                if (!arrowPath.arrowTail.invalidTip)
                    extrudeLength += arrowPath.arrowTail.extrudePoints.Length;
            }
            else
            {
                if (arrowPath.arrowTail.arrowTipMode == ArrowTip.ArrowTipMode.Mesh)
                {
                    //calculate tail mesh vertices that will be added to main mesh
                    arrowPath.arrowTail.generateMesh(vertices, triangles, normalsList);
                }
            }
            //Calucalate the head part trajectory
            arrowPath.arrowHead.calculatePath();
            if (arrowPath.arrowHead.arrowTipMode == ArrowTip.ArrowTipMode.Extrude)
            {
                //Generate and generate default template primitive shapes if not present
                if (arrowPath.arrowHead.templatePrimitives == null)
                {
                    arrowPath.arrowHead.templatePrimitives = ScriptableObject.CreateInstance<PrimitivesList>();
                }
                if (arrowPath.arrowHead.templatePrimitives.primitivesList == null || arrowPath.arrowHead.templatePrimitives.primitivesList.Count == 0)
                {
                    arrowPath.arrowHead.templatePrimitives.primitivesList = new List<Primitive>();
                    arrowPath.arrowHead.templatePrimitives.primitivesList.Add(ScriptableObject.CreateInstance<PlanePrimitive>());
                }
                for (int i = 0; i < arrowPath.arrowHead.templatePrimitives.primitivesList.Count; i++)
                {
                    arrowPath.arrowHead.templatePrimitives.primitivesList[i].generate();
                }
                //Caluclate head extrude part
                arrowPath.arrowHead.generate();
                if (!arrowPath.arrowHead.invalidTip)
                    extrudeLength += arrowPath.arrowHead.extrudePoints.Length;
            }
            else
            {
                if (arrowPath.arrowHead.arrowTipMode == ArrowTip.ArrowTipMode.Mesh)
                {
                    //calculate head mesh vertices that will be added to main mesh
                    arrowPath.arrowHead.generateMesh(vertices, triangles, normalsList);
                }
            }


            if (extrudeLength != 0)
            {
                //Generate final extrude vertices if path,tail or head are extruded (at least one of them)
                //Set default values
                extrudeObject.extrudePath = new Matrix4x4[extrudeLength];
                extrudeObject.openCloseExtrude = new int[extrudeLength];
                extrudeObject.polygonsToExtrude = new Primitive[extrudeLength];
                extrudePathIndex = 0;
                extrudeObject.colorFunctions = arrowPath.colorFunctions;
                extrudeObject.useShapeColors = arrowPath.useShapeColors;
                //Add previusly generated extrudes to main extrude
                if (!arrowPath.arrowTail.invalidTip)
                {
                    generateArrowTail();
                }
                if (!arrowPath.invalidPath)
                {
                    generateArrowPath();
                }
                if (!arrowPath.arrowHead.invalidTip)
                {
                    generateArrowHead();
                }
                //Generate final extrude
                extrudeObject.generate();
                //Add calculated vertices and triangles to main mesh verts and tris
                int startVertIndex = vertices.Count;
                Vector3[] tempVerts = extrudeObject.verts;
                Vector3[] tempNormals = extrudeObject.normals;
                for (int i = 0; i < tempVerts.Length; i++)
                {
                    vertices.Add(tempVerts[i]);
                    normalsList.Add(tempNormals[i]);
                }
                int[] tempTris = extrudeObject.tris;
                for (int i = 0; i < tempTris.Length; i++)
                {
                    triangles.Add(startVertIndex + tempTris[i]);
                }
            }
            //Uv calcualtion for main mesh
            uvs = new Vector2[vertices.Count];
            Color32[] meshColors = new Color32[vertices.Count];
            int uvIndex = 0;
            if (arrowPath.arrowPathMode == ArrowPath.ArrowPathMode.MadeOfShapes)
            {
                //Place uv in upper left corner for meshes used in main path
                Vector2[][] tempShapeUvs = new Vector2[arrowPath.customShapes.meshesList.Count][];
                int shapeIndex = 0;
                int shapeVertIndex = 0;
                for (int i = 0; i < numverts; i++)
                {
                    if (tempShapeUvs[shapeIndex] == null) tempShapeUvs[shapeIndex] = arrowPath.customShapes.meshesList[shapeIndex].mesh.uv;
                    uvs[uvIndex] = new Vector2(tempShapeUvs[shapeIndex][shapeVertIndex].x * 0.5f, 0.5f + tempShapeUvs[shapeIndex][shapeVertIndex].y * 0.5f);
                    meshColors[uvIndex] = arrowPath.madeOfShapesColors[i];
                    uvIndex++;
                    shapeVertIndex++;
                    if (shapeVertIndex == tempShapeUvs[shapeIndex].Length)
                    {
                        shapeVertIndex = 0;
                        shapeIndex++;
                        if (shapeIndex == arrowPath.customShapes.meshesList.Count)
                        {
                            shapeIndex = 0;
                        }
                    }
                }
            }
            Vector2[] tempUvs;
            Color32[] tempColors;
            if (arrowPath.arrowTail.arrowTipMode == ArrowTip.ArrowTipMode.Mesh)
            {
                //Place uv in lower right corner for mesh used as tail
                tempUvs = arrowPath.arrowTail.mesh.uvs;
                tempColors = arrowPath.arrowTail.mesh.colors;
                for (int i = 0; i < tempUvs.Length; i++)
                {
                    uvs[uvIndex] = new Vector2(0.5f + tempUvs[i].x * 0.5f, tempUvs[i].y * 0.5f);
                    meshColors[uvIndex] = tempColors[i];
                    uvIndex++;
                }
            }
            if (arrowPath.arrowHead.arrowTipMode == ArrowTip.ArrowTipMode.Mesh)
            {
                //Place uv in upper right corner for mesh used as tail
                tempUvs = arrowPath.arrowHead.mesh.uvs;
                tempColors = arrowPath.arrowHead.mesh.colors;
                for (int i = 0; i < tempUvs.Length; i++)
                {
                    uvs[uvIndex] = new Vector2(0.5f + tempUvs[i].x * 0.5f, 0.5f + tempUvs[i].y * 0.5f);
                    meshColors[uvIndex] = tempColors[i];
                    uvIndex++;
                }
            }
            if (extrudeLength != 0)
            {
                //Place uv in lower left corner for extruded parts
                tempUvs = extrudeObject.uvs;
                tempColors = extrudeObject.colors;
                for (int i = 0; i < tempUvs.Length; i++)
                {
                    uvs[uvIndex] = new Vector2(tempUvs[i].x * 0.5f, tempUvs[i].y * 0.5f);
                    meshColors[uvIndex] = tempColors[i];
                    uvIndex++;
                }
            }
            if (saveToSolid)
            {
                verts = vertices.ToArray();
                tris = triangles.ToArray();
                colors = meshColors;
                normals = normalsList.ToArray();
            }
            //Set main mesh
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
            mesh.uv = uvs;
            mesh.colors32 = meshColors;
            normals = normalsList.ToArray();
            mesh.SetNormals(normalsList);
        }
    }
}