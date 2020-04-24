using Cogobyte.ProceduralLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cogobyte.ProceduralIndicators
{
    //Used to view arrow objects in play mode
    //Drag this script on empty object and add a path asset, and optional head and tail arrow tip assets.
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class ArrowObject : MonoBehaviour
    {
        public ArrowIndicator arrowIndicator;
        public ArrowPath arrowPath;
        public ArrowTip arrowTail;
        public ArrowTip arrowHead;
        //if true it will recalculate arrow every 1/update speed parameter
        public bool updatesByItself = true;
        [Range(0f, 10f)]
        public float updateSpeed = 0.01f;
        //If false it will create an instance of asset and change the instance instead of the original asset
        public bool permanentScriptableObjects = true;
        private float currentUpdateTime = 1.1f;
        //disables indicator rendering 
        public bool hideIndicator = false;
        public bool flatShading = false;

        void Awake()
        {
            arrowIndicator = ScriptableObject.CreateInstance<ArrowIndicator>();
            if (arrowPath == null)
            {
                arrowIndicator.arrowPath = ScriptableObject.CreateInstance<ArrowPath>();
                arrowPath = arrowIndicator.arrowPath;
                arrowIndicator.arrowPath.arrowPathMode = ArrowPath.ArrowPathMode.None;
            }
            arrowIndicator.arrowPath = arrowPath;
            if (!permanentScriptableObjects)
            {
                arrowIndicator.arrowPath = Instantiate(arrowIndicator.arrowPath);
                arrowPath = arrowIndicator.arrowPath;
            }
            if (arrowHead == null)
            {
                arrowHead = ScriptableObject.CreateInstance<ArrowTip>();
                arrowHead.arrowTipMode = ArrowTip.ArrowTipMode.None;
            }
            if (arrowTail == null)
            {
                arrowTail = ScriptableObject.CreateInstance<ArrowTip>();
                arrowTail.arrowTipMode = ArrowTip.ArrowTipMode.None;
            }
            arrowIndicator.arrowPath.arrowHead = arrowHead;
            arrowIndicator.arrowPath.arrowTail = arrowTail;
            if (permanentScriptableObjects)
            {
                if (arrowIndicator.arrowPath.arrowHead == arrowIndicator.arrowPath.arrowTail)
                {
                    arrowIndicator.arrowPath.arrowTail = Instantiate(arrowIndicator.arrowPath.arrowTail);
                    arrowTail = arrowIndicator.arrowPath.arrowTail;
                }
            }
            else
            {
                arrowIndicator.arrowPath.arrowHead = Instantiate(arrowIndicator.arrowPath.arrowHead);
                arrowHead = arrowIndicator.arrowPath.arrowHead;
                arrowIndicator.arrowPath.arrowTail = Instantiate(arrowIndicator.arrowPath.arrowTail);
                arrowTail = arrowIndicator.arrowPath.arrowTail;
            }
            if (flatShading)
            {
                arrowIndicator.extrudeObject = ScriptableObject.CreateInstance<FlatShadeExtrude>();
            }
            else
            {
                arrowIndicator.extrudeObject = ScriptableObject.CreateInstance<ComplexExtrude>();
            }
        }

        void LateUpdate()
        {
            if (updatesByItself)
            {
                if (currentUpdateTime > 1f)
                {
                    currentUpdateTime = 0;
                    if (flatShading)
                    {
                        arrowIndicator.extrudeObject = ScriptableObject.CreateInstance<FlatShadeExtrude>();
                    }
                    else
                    {
                        arrowIndicator.extrudeObject = ScriptableObject.CreateInstance<ComplexExtrude>();
                    }
                    updateArrowMesh();
                }
                else
                {
                    currentUpdateTime += updateSpeed * Time.deltaTime;
                }
            }
        }

        public void updateArrowMesh()
        {
            if (hideIndicator)
            {
                this.GetComponent<MeshRenderer>().enabled = false;
            }
            else
            {
                arrowIndicator.generate();
                DestroyImmediate(GetComponent<MeshFilter>().mesh, true);
                this.GetComponent<MeshFilter>().mesh = arrowIndicator.mesh;
                this.GetComponent<MeshRenderer>().enabled = true;

            }
        }
    }
}