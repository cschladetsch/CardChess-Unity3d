using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cogobyte.ProceduralLibrary;

namespace Cogobyte.ProceduralIndicators
{
    //Used to view arrow objects in play mode
    //Drag this script on empty object and add a SelectionIndicatorPath asset and PathArray asset.
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class SelectionIndicator : MonoBehaviour
    {
        public PathArray pathArray;
        public SelectionIndicatorPath selectionIndicatorPath;
        //if true it will recalculate indicator every 1/update speed parameter
        public bool updatesByItself = true;
        [Range(0f, 100f)]
        public float updateSpeed = 0.01f;
        //If false it will create an instance of asset and change the instance instead of the original asset
        public bool permanentScriptableObjects = true;
        private float currentUpdateTime = 1.1f;
        //disables indicator rendering 
        public bool hideIndicator = false;

        void Awake()
        {
            if (!permanentScriptableObjects) {
                pathArray = Instantiate(pathArray);
                selectionIndicatorPath = Instantiate(selectionIndicatorPath);
            }
        }

        void LateUpdate()
        {
            if (updatesByItself)
            {
                if (currentUpdateTime > 1f)
                {
                    currentUpdateTime = 0;
                    updateIndicatorMesh();
                }
                else
                {
                    currentUpdateTime += updateSpeed * Time.deltaTime;
                }
            }
        }

        public void updateIndicatorMesh()
        {
            if (hideIndicator || pathArray == null)
            {
                this.GetComponent<MeshRenderer>().enabled = false;
            }
            else
            {
                this.GetComponent<MeshRenderer>().enabled = true;
                pathArray.closed = true;
                pathArray.CalculatePath();
                selectionIndicatorPath.pathArray = pathArray;
                selectionIndicatorPath.generate();
                DestroyImmediate(GetComponent<MeshFilter>().mesh, true);
                this.GetComponent<MeshFilter>().mesh = selectionIndicatorPath.mesh;
            }
        }
    }
}