using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ItemButtonManager : MonoBehaviour
{
    private string itemName;
    private Sprite itemImage;
    private string itemDescription;
    private GameObject item3DModel;
    private string urlBundleModel;
    private RawImage imageBundle;

    private ARInteractionManager interactionManager;

    private LoadingScreen loadingScreen;

    public string ItemName
    {
        set
        {
            itemName = value;
        }
    }

    public Sprite ItemImage
    {
        set => itemImage = value; //Es lo mismo que {}
    }

    public string ItemDescription
    {
        set => itemDescription = value;
    }

    public GameObject Item3DModel
    {
        set => item3DModel = value;
    }
    public string URLBundleModel
    {
        set => urlBundleModel = value;
    }

    public RawImage ImageBundle
    {
        get => imageBundle;
        set => imageBundle = value;
    }

    public Decimal normalVector;

    void Start()
    {
        loadingScreen = FindObjectOfType<LoadingScreen>();

        transform.GetChild(0).GetComponent<TMP_Text>().text = itemName;
        transform.GetChild(1).GetComponent<RawImage>().texture = itemImage.texture;
        //imageBundle = transform.GetChild(1).GetComponent<RawImage>();
        transform.GetChild(2).GetComponent<TMP_Text>().text = itemDescription;

        var button = GetComponent<Button>();
        button.onClick.AddListener(GameManager.instance.ARPosition);
        button.onClick.AddListener(Create3DModel);

        interactionManager = FindObjectOfType<ARInteractionManager>();
    }

    private void Create3DModel()
    {
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("Item");

        foreach (GameObject obj in allObjects)
        {
            Destroy(obj);
        }

        interactionManager.Item3DModel = Instantiate(item3DModel);
        interactionManager.Position3DModel();
        //StartCoroutine(DownloadAssetBundle(urlBundleModel));
    }

    IEnumerator DownloadAssetBundle(string urlAssetBundle)
    {
        Debug.Log("Activa");
        loadingScreen.ActivateLoadingScreen();
        Debug.Log("Sale");

        UnityWebRequest serverRequest = UnityWebRequestAssetBundle.GetAssetBundle(urlAssetBundle);
        yield return serverRequest.SendWebRequest();
        if (serverRequest.result == UnityWebRequest.Result.Success)
        {
            AssetBundle model3D = DownloadHandlerAssetBundle.GetContent(serverRequest);
            if (model3D != null)
            {
                interactionManager.Item3DModel = Instantiate(model3D.LoadAsset(model3D.GetAllAssetNames()[0]) as GameObject);
                model3D.Unload(false);
            }
            else
            {
                Debug.Log("Not a valid Asset Bundle");
            }
        }
        else
        {
            Debug.Log("Error l91");
        }
        Debug.Log("Desactiva");
        loadingScreen.DeactivateLoadingScreen();
        
    }

}
