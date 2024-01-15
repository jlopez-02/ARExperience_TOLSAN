using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using DG.Tweening;
using Unity.VisualScripting;
using System;
using TMPro;
using System.Drawing;
using Unity.XR.CoreUtils;

public class ScanManager : MonoBehaviour
{
    [SerializeField] private GameObject startHorizontalScanButton;
    [SerializeField] private GameObject startVerticalScanButton;
    [SerializeField] private GameObject finishScanButton;
    [SerializeField] private GameObject restartScanButton;

    [SerializeField] private GameObject scanStatusMessage;
    [SerializeField] private GameObject showItemsBox;
    [SerializeField] private GameObject viewPlanesButton;
    [SerializeField] private GameObject hidePlanesButton;

    [SerializeField] private Material transparentMaterial;

    [SerializeField] private ARPlaneManager aRPlaneManager;

    private bool isPlaneDetectionActive;
    private bool isAllPlanesActive;

    public GameObject mensaje;

    private ARPlane floor;
    private ARPlane wall;

    private int contadorPlanos = 0;

    // Start is called before the first frame update
    void Start()
    {
        var arSession = FindObjectOfType<ARSession>();
        if (arSession != null)
        {
            Debug.Log("TAG: Posición: " + arSession.transform.position);
        }

        aRPlaneManager = GetComponent<ARPlaneManager>();

        StopPlaneDetection();
        isAllPlanesActive = true;
    }

    private void Update()
    {
        if (isAllPlanesActive)
        {
            DisableElement(viewPlanesButton);
            EnableElement(hidePlanesButton);
        }
        else
        {
            EnableElement(viewPlanesButton);
            DisableElement(hidePlanesButton);
        }
    }

    private void OnEnable()
    {
        aRPlaneManager.planesChanged += PlanesFound;
    }

    private void OnDisable()
    {
        aRPlaneManager.planesChanged -= PlanesFound;
    }

    private void PlanesFound(ARPlanesChangedEventArgs planeData)
    {
        
        //if (planeData.updated.Count > contadorPlanos || planeData.added.Count > 0)
        //{
            Debug.Log("TAG////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////");
            Debug.Log("TAG-----------------------------------------------------------------------------------------------------------------------------");
            Debug.Log("TAG: Planos añadidos: " + planeData.added.Count);
            Debug.Log("TAG: Planos actualizados: " + planeData.updated.Count);
            Debug.Log("TAG: Plano borrados: " + planeData.removed.Count);

            foreach (var plane in planeData.added)
            {
                AssignMaterial(plane);
                Debug.Log("TAG: Plano añadido: " + plane.transform.position + "-" + plane.normal);
            }

            foreach (var plane in planeData.updated)
            {
                Debug.Log("TAG: Plano actualizado: " + plane.transform.position + "-" + plane.normal);
            }

            foreach (var plane in planeData.removed)
            {
                Debug.Log("TAG: Plano eliminado: " + plane.transform.position + "-" + plane.normal);
            }

            Debug.Log("TAG////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////");

            if (isPlaneDetectionActive)
            {
                Debug.Log("TAG: Número de objetos en el trakable: " + aRPlaneManager.trackables.count);



                //if (planeData.updated.Count > 2)
                //{
                //    RestartARSession();
                //    StartPhase2PlaneDetection();
                //}

                Vector3 normalRotation_aux;
                if (planeData.added != null && planeData.added.Count > 0)
                {
                    foreach (var plane in planeData.updated)
                    {
                        if (IsVerticalPlane(plane))
                        {
                            normalRotation_aux = plane.normal;
                            if ((normalRotation_aux.x <= 0.1f && normalRotation_aux.x >= -0.1f && normalRotation_aux.y <= 0.1f && normalRotation_aux.y >= -0.1f && normalRotation_aux.z <= -0.9f && normalRotation_aux.z >= -1f))
                            {
                                wall = plane;
                                Debug.Log("TAG: Plano vertical IDEAL detectado");
                                Debug.Log("TAG: NORMAL PLANO VERTICAL: " + wall.normal);
                                Debug.Log("TAG: UP PLANO VERTICAL: " + wall.transform.up);
                                Debug.Log("TAG: FORWARD PLANO VERTICAL: " + wall.transform.forward);
                                FinishPlaneDetection();
                            }
                        }
                        else
                        {
                            floor = plane;
                            StartPhase2PlaneDetection();
                            Debug.Log("TAG: Plano horizontal IDEAL detectado");
                            
                            Debug.Log("TAG: NORMAL PLANO HORIZONTAL: " + floor.normal);
                            Debug.Log("TAG: UP DEL PLANO HORIZONTAL: " + floor.transform.up);
                            Debug.Log("TAG: FORWARD DEL PLANO HORIZONTAL: " + floor.transform.forward);
                        }
                    }

                }

                //Debug.Log("TAG: Detectando planos...");
            }



            Debug.Log("TAG----------------------------------------------------------------------------------------------------------------------------");
            Debug.Log("TAG////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////");

        //}

        contadorPlanos++;
    }

    //Step 1: Ground Planes detection
    public void StartPhase1PlaneDetection()
    { 
        StartHorizontalDetection();
        HideElement(startHorizontalScanButton);
        HideElement(finishScanButton);
        ShowElement(startVerticalScanButton);
        ShowElement(restartScanButton);
    }

    //Step 2: Vertical Planes detection
    public void StartPhase2PlaneDetection()
    {
        StartVerticalDetection();
        HideElement(startVerticalScanButton);
        ShowElement(finishScanButton);
    }

    //Step 3: Plane Detection complete

    public void FinishPlaneDetection()
    {
        int countV = 0;
        int countH = 0;

        

        if (aRPlaneManager.trackables.count > 0)
        {
            foreach (var plane in aRPlaneManager.trackables)
            {
                if (IsVerticalPlane(plane))
                {
                    countV++;
                }
                else
                {
                    
                    countH++;
                }
            }

            if (countH > 0 && countV > 0)
            {
                //EnableElement(showItemsBox);
                //ShowElement(showItemsBox);
            }
        }

        StopPlaneDetection();
        GameManager.instance.MainMenu();

        EnableElement(showItemsBox);
        ShowElement(showItemsBox);
    }

    public ARPlane getFloor()
    {
        if(floor != null)
        {
            return floor;
        }

        return floor = null;
        
    }

    public ARPlane getWall()
    {
        if(wall != null)
        {
            return wall;
        }

        return wall = null;
    }

    public void StopPlaneDetection()
    {
        isPlaneDetectionActive = false;

        aRPlaneManager.requestedDetectionMode = PlaneDetectionMode.None;

        if (isAllPlanesActive)
        {
            foreach (var plane in aRPlaneManager.trackables)
            {
                plane.gameObject.SetActive(true);
            }
        }
        else
        {
            foreach (var plane in aRPlaneManager.trackables)
            {
                plane.gameObject.SetActive(false);
            }
        }  
    }

    public void StartVerticalDetection()
    {
        ChangeToVerticalPlaneDetection();

        ChangeScanStatusMessage("Vertical");
    }

    public void StartHorizontalDetection()
    {
        ChangeToHorizontalPlaneDetection();

        ChangeScanStatusMessage("Horizontal");
    }

    private void ChangeToVerticalPlaneDetection()
    {
        isPlaneDetectionActive = true;

        aRPlaneManager.requestedDetectionMode = PlaneDetectionMode.Vertical;
        

        foreach (var plane in aRPlaneManager.trackables)
        {
            if (IsVerticalPlane(plane))
            {
                plane.gameObject.SetActive(true);
            }
            else
            {
                plane.gameObject.SetActive(false);
            }
        }
    }

    private void ChangeToHorizontalPlaneDetection()
    {
        isPlaneDetectionActive = true;

        aRPlaneManager.requestedDetectionMode = PlaneDetectionMode.Horizontal;

        foreach (var plane in aRPlaneManager.trackables)
        {
            if (!IsVerticalPlane(plane))
            {
                plane.gameObject.SetActive(true);
            }
            else
            {
                plane.gameObject.SetActive(false);
            }
        }
        
    }

    public void DeleteAll3DObjects()
    {
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("Item");

        foreach (GameObject obj in allObjects)
        {
            Destroy(obj);
        }
    }

    private bool IsVerticalPlane(ARPlane plane)
    {
        return Mathf.Abs(Vector3.Dot(plane.normal, Vector3.up)) < 0.1f;
    }

    public void ViewAllPlanes()
    {
        foreach (var plane in aRPlaneManager.trackables)
        {
            plane.gameObject.SetActive(true);
        }

        HideElement(viewPlanesButton);
        ShowElement(hidePlanesButton);

        isAllPlanesActive = true;
    }

    public void HideAllPlanes()
    {
        foreach (var plane in aRPlaneManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }

        ShowElement(viewPlanesButton);
        HideElement(hidePlanesButton);

        isAllPlanesActive = false;
    }

    public void RestartARSession()
    {
        HideElement(showItemsBox);
        DisableElement(showItemsBox);

        var arSession = FindObjectOfType<ARSession>();

        if (arSession != null)
        {
            isAllPlanesActive = false;

            arSession.Reset();
            Debug.Log("TAG: Posición: " + arSession.transform.position);
        }
    }
    private void ShowElement(GameObject element)
    {
        element.transform.DOScale(Vector3.one, 0.5f);
    }

    private void HideElement(GameObject element)
    {
        element.transform.DOScale(Vector3.zero, 0.3f);
    }

    private void EnableElement(GameObject element)
    {
        element.gameObject.SetActive(true);
    }

    private void DisableElement(GameObject element)
    {
        element.gameObject.SetActive(false);
    }

    private void ChangeScanStatusMessage(String mode)
    {
        ShowElement(scanStatusMessage);
        scanStatusMessage.transform.GetComponent<TMP_Text>().text = "Escaneo " + mode;
    }

    void AssignMaterial(ARPlane plane)
    {
        MeshRenderer meshRenderer = plane.GetComponent<MeshRenderer>();

        meshRenderer.material = transparentMaterial;
    }
}
