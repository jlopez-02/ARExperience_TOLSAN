using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARInteractionManager : MonoBehaviour
{
    [SerializeField] private GameObject mensaje;

    public ARPlaneManager aRPlaneManager;
    public Canvas canvas;
    public Camera arCamera;
    public TMP_InputField rotationInputField;
    public TMP_InputField movementInputField;

    private GameObject aRPointer;
    private GameObject item3DModel;
    private GameObject originPointer;

    private float degrees = 5f;
    private float movementUnits = 5f; //millimeters

    public ScanManager scanManager;

    public Toggle checkbox;

    private ARAnchorManager anchorManager;

    private bool itemPositioning = false;

    public GameObject Item3DModel
    {
        set
        {
            item3DModel = value;
            item3DModel.transform.position = aRPointer.transform.position;
            item3DModel.transform.parent = aRPointer.transform;
        }
    }

    void Start()
    {
        aRPointer = transform.GetChild(0).gameObject;
        originPointer = transform.GetChild(1).gameObject;
        aRPointer.transform.parent = originPointer.transform;
        anchorManager = FindObjectOfType<ARAnchorManager>();

        GameManager.instance.OnMainMenu += SetItemPosition;
    }

    void Update()
    {
        if (itemPositioning)
        {
            Position3DModel();
        }
    }

    public void Position3DModel()
    {
        float z_position = 0f;
        
        if (scanManager.getFloor() != null && scanManager.getWall() != null)
        {
            Debug.Log("TAG: Vector UP antes:" + transform.up);
            transform.up = scanManager.getFloor().normal;
            Debug.Log("TAG: Vector UP despues:" + transform.up);

            Debug.Log("TAG: Vector Forward antes:" + transform.forward);

            //if (checkbox.isOn)
            //{ // DETECCION DE PLANOS VERTICALES
            //    transform.forward = scanManager.getWall().normal;
            //    Debug.Log("TAG: VECTOR FORWARD CAMBIADO (DETECCIÓN DE PLANOS): " + transform.forward);
            //    z_position = scanManager.getWall().transform.position.z;
            //    Debug.Log("TAG: Z CAMBIADA CON LA Z DEL PLANO VERTICAL (DETECCIÓN DE PLANOS): " + transform.forward);
            //}
            //else
            //{ //DETECCIÓN DE BOLA
            //    z_position = transform.position.z;
            //    Debug.Log("TAG: Z CAMBIADA CON LA Z DE LA BOLA (DETECCIÓN DE BOLA): " + transform.forward);
            //}
            transform.forward = scanManager.getWall().normal;
            z_position = scanManager.getWall().transform.position.z;
            Debug.Log("TAG: Z CAMBIADA CON LA Z DE LA BOLA (DETECCIÓN DE BOLA): " + transform.forward);

            Debug.Log("TAG: Vector Forward después:" + transform.forward);

            Debug.Log("TAG: Posición objeto antes:" + transform.position);
            transform.position = new Vector3(transform.position.x, scanManager.getFloor().transform.position.y, z_position);
            Debug.Log("TAG: Posición objeto después:" + transform.position);

            if (item3DModel != null)
            {
                aRPointer.SetActive(true);
                originPointer.SetActive(true);
            }
        }

        itemPositioning = true;
    }

    private void SetItemPosition()
    {
        if (item3DModel != null)
        {
            GameObject anchorGameObject = new GameObject("ModelAnchor");
            anchorGameObject.transform.position = item3DModel.transform.position;
            anchorGameObject.transform.rotation = item3DModel.transform.rotation;

            ARAnchor anchor = anchorGameObject.AddComponent<ARAnchor>();

            if (anchorGameObject == null)
            {
                Debug.Log("Error al crear el anclaje");
                return;
            }

            item3DModel.transform.parent = anchorGameObject.transform;
            aRPointer.SetActive(false);
            originPointer.SetActive(false);

            item3DModel = null;
            itemPositioning = false;
        }
    }
    public void DeleteItem()
    {
        if (item3DModel != null)
        {
            aRPointer.SetActive(false);
            originPointer.SetActive(false);
            Destroy(item3DModel);
        }
        GameManager.instance.MainMenu();
    }
    public void RotateModelRight()
    {
        degrees = GetRotationDegreesFromInput();
        item3DModel.transform.Rotate(0f, degrees, 0f);
    }
    public void RotateModelLeft()
    {
        degrees = GetRotationDegreesFromInput();
        item3DModel.transform.Rotate(0f, -degrees, 0f);
        
    }
    public void MoveModelRight()
    {
        originPointer.transform.Translate(Vector3.right * GetMovementUnits() * 0.001f);
    }
    public void MoveModelLeft()
    {
        originPointer.transform.Translate(Vector3.left * GetMovementUnits() * 0.001f);
    }

    public void MoveModelUp()
    {
        originPointer.transform.Translate(Vector3.up * GetMovementUnits() * 0.001f);
    }
    public void MoveModelDown()
    {
        originPointer.transform.Translate(Vector3.down * GetMovementUnits() * 0.001f);
    }

    public void MoveModelForward()
    {
        originPointer.transform.Translate(Vector3.forward * GetMovementUnits() * 0.001f);
    }
    public void MoveModelBackward()
    {
        originPointer.transform.Translate(Vector3.back * GetMovementUnits() * 0.001f);
    }

    private float GetRotationDegreesFromInput()
    {
        if (float.TryParse(rotationInputField.text, out float degrees))
        {
            return degrees;
        }
        else
        {
            return 5f;
        }
    }
    private float GetMovementUnits()
    {
        if (float.TryParse(movementInputField.text, out float movementUnits))
        {
            return movementUnits;
        }
        else
        {
            return 5f;
        }
    }
    private void Buscar(string patronRegex)
    {
        // Encontrar todos los GameObjects con el nombre que coincida con el patrón regex
        GameObject[] gameObjectsEncontrados = GameObject.FindObjectsOfType<GameObject>();

        // Crear una expresión regular a partir del patrón
        Regex regex = new Regex(patronRegex);

        // Filtrar los GameObjects encontrados por el nombre que coincida con el patrón regex
        foreach (GameObject go in gameObjectsEncontrados)
        {
            if (regex.IsMatch(go.name))
            {
                // Hacer algo con el GameObject encontrado
                Debug.Log("Se encontró un GameObject con nombre que coincide con el patrón: " + go.name);
            }
        }
    }

}
