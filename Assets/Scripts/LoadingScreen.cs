using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LoadingScreen : MonoBehaviour
{
    public float rotationSpeed = 200f;

    private GameObject loadingScreen;
    
    private GameObject panel;
    private GameObject loadingIcon;
    
    void Start()
    {
        loadingScreen = transform.GetChild(0).gameObject;
        loadingIcon = loadingScreen.transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (loadingScreen.gameObject.activeSelf)
        {
            loadingIcon.transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
        
    }

    public void ActivateLoadingScreen()
    {
        loadingScreen.gameObject.SetActive(true);
    }

    public void DeactivateLoadingScreen()
    {
        loadingScreen.gameObject.SetActive(false);
    }
}
