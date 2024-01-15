using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ScriptPrueba : MonoBehaviour
{
    [SerializeField] private ARPlaneManager aRPlaneManager;
    [SerializeField] private GameObject model3DPrefab;

    private List<ARPlane> planeList = new List<ARPlane>();
    private GameObject model3DPlaced;

    void OnEnable()
    {
        aRPlaneManager.planesChanged += OnPlanesChanged;
    }

    void OnDisable()
    {
        aRPlaneManager.planesChanged -= OnPlanesChanged;
    }

    private void OnPlanesChanged(ARPlanesChangedEventArgs eventArgs)
    {
        foreach (var plane in eventArgs.added)
        {
            Vector3 planeNormal = plane.normal;

            // Instantiate the aligned object
            GameObject alignedObject = Instantiate(model3DPrefab, plane.center, Quaternion.identity);

            // Align the object with the plane's normal
            alignedObject.transform.rotation = Quaternion.LookRotation(planeNormal, Vector3.up);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
