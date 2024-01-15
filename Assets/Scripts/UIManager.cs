using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject MainMenuCanvas;
    [SerializeField] private GameObject ItemsMenuCanvas;
    [SerializeField] private GameObject ARPositionCanvas;
    [SerializeField] private GameObject ButtonMenuCanvas;
    [SerializeField] private GameObject ScanScreenCanvas;

    [SerializeField] private GameObject showUIButton;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.OnMainMenu += ActivateMainMenu;
        GameManager.instance.OnItemsMenu += ActivateItemsMenu;
        GameManager.instance.OnARPosition += ActivateARPosition;
        GameManager.instance.OnButtonMenu += ActivateButtonMenu;
        GameManager.instance.OnScanScreen += ActivateScanScreen;
    }

    private void ActivateMainMenu()
    {
        ShowMainMenu();

        HideItemsMenu();

        HideARPosition();

        HideButtonMenu();

        HideScanScreen();

    }

    private void ActivateItemsMenu()
    {
        HideMainMenu(1);

        ShowItemsMenu();
    }

    private void ActivateARPosition()
    {
        HideMainMenu(1);

        HideItemsMenu();

        ShowARPosition();
    }

    private void ActivateButtonMenu()
    {
        HideMainMenu(0);

        ShowButtonMenu();
    }


    private void ActivateScanScreen()
    {
        HideMainMenu(1);

        HideButtonMenu();

        ShowScanScreen();
    }

    //                               //
    // DOTWEEN TRANSFORMATIONS BELOW //
    //                               //
    private void ShowMainMenu()
    {
        for(int i= 0; i < MainMenuCanvas.transform.childCount - 2; i++)
        {
            Transform child = MainMenuCanvas.transform.GetChild(i);
            child.DOScale(Vector3.one, 0.3f);
        }
        showUIButton.transform.DOScale(Vector3.zero, 0.5f);
        //MoveComponentY(MainMenuCanvas, 3, 0.91f);
    }

    private void HideMainMenu(int HideOption)
    {
        for (int i = 0; i < MainMenuCanvas.transform.childCount - 2; i++)
        {
            Transform child = MainMenuCanvas.transform.GetChild(i);
            child.DOScale(Vector3.zero, 0.3f);
        }

        //if (HideOption == 0)
        //{
        //    //MainMenuCanvas.transform.GetChild(3).transform.DOMoveY(1530, 0.3f);
        //    MoveComponentY(MainMenuCanvas, 3, 0.75f);
        //}
        //else
        //{
        //    MainMenuCanvas.transform.GetChild(3).transform.DOScale(Vector3.zero, 0.3f);
        //}
    }

    private void ShowItemsMenu()
    {
        ItemsMenuCanvas.transform.GetChild(0).transform.DOScale(Vector3.one, 0.5f);
        ItemsMenuCanvas.transform.GetChild(1).transform.DOScale(Vector3.one, 0.3f);
        ItemsMenuCanvas.transform.GetChild(1).transform.DOMoveY(300, 0.3f);
    }

    private void HideItemsMenu()
    {
        ItemsMenuCanvas.transform.GetChild(0).transform.DOScale(Vector3.zero, 0.5f);
        ItemsMenuCanvas.transform.GetChild(1).transform.DOScale(Vector3.zero, 0.3f);
        ItemsMenuCanvas.transform.GetChild(1).transform.DOMoveY(180, 0.3f);
    }

    private void ShowARPosition()
    {
        foreach (Transform child in ARPositionCanvas.transform)
        {
            child.DOScale(Vector3.one, 0.5f);
        }
    }

    private void HideARPosition()
    {
        foreach (Transform child in ARPositionCanvas.transform)
        {
            child.DOScale(Vector3.zero, 0.5f);
        }
    }

    private void ShowButtonMenu()
    {
        ButtonMenuCanvas.transform.GetChild(0).transform.DOScale(Vector3.one, 0.3f);
        ButtonMenuCanvas.transform.GetChild(1).transform.DOScale(Vector3.one, 0.5f);
        ButtonMenuCanvas.transform.GetChild(2).transform.DOScale(Vector3.one, 0.5f);

        if (ButtonMenuCanvas.transform.GetChild(3).gameObject.activeSelf)
        {
            ButtonMenuCanvas.transform.GetChild(3).transform.DOScale(Vector3.one, 0.5f);
        }
        else
        {
            ButtonMenuCanvas.transform.GetChild(4).transform.DOScale(Vector3.one, 0.5f);
        }

        MoveComponentY(ButtonMenuCanvas, 0, 0.91f);
        MoveComponentY(ButtonMenuCanvas, 1, 0.91f);
        MoveComponentY(ButtonMenuCanvas, 2, 0.91f);
        MoveComponentY(ButtonMenuCanvas, 3, 0.91f);
        MoveComponentY(ButtonMenuCanvas, 4, 0.91f);

    }

    private void HideButtonMenu()
    {
        ButtonMenuCanvas.transform.GetChild(0).transform.DOScale(Vector3.zero, 0.5f);
        ButtonMenuCanvas.transform.GetChild(1).transform.DOScale(Vector3.zero, 0.3f);
        ButtonMenuCanvas.transform.GetChild(2).transform.DOScale(Vector3.zero, 0.3f);
        ButtonMenuCanvas.transform.GetChild(3).transform.DOScale(Vector3.zero, 0.3f);
        ButtonMenuCanvas.transform.GetChild(4).transform.DOScale(Vector3.zero, 0.3f);

        MoveComponentY(ButtonMenuCanvas, 0, 1.04f);
        MoveComponentY(ButtonMenuCanvas, 1, 1.04f);
        MoveComponentY(ButtonMenuCanvas, 2, 1.04f);
        MoveComponentY(ButtonMenuCanvas, 3, 1.04f);
        MoveComponentY(ButtonMenuCanvas, 4, 1.04f);
    }

    private void ShowScanScreen()
    {
        ScanScreenCanvas.transform.GetChild(0).transform.DOScale(Vector3.one, 0.5f);
        ScanScreenCanvas.transform.GetChild(1).transform.DOScale(Vector3.one, 0.5f);
    }

    private void HideScanScreen()
    {
        foreach (Transform child in ScanScreenCanvas.transform)
        {
            child.DOScale(Vector3.zero, 0.5f);
        }
    }

    private void MoveComponentY(GameObject menu, int child,float porcentage)
    {
        RectTransform canvasRectTransform = menu.GetComponent<RectTransform>();
        float canvasHeight = canvasRectTransform.sizeDelta.y;
        float targetY = canvasHeight * porcentage;

        menu.transform.GetChild(child).transform.DOMoveY(targetY, 0.3f);
    }
    public void HideUI()
    {
        HideMainMenu(1);
        showUIButton.transform.DOScale(Vector3.one, 0.5f);
    }
    public void ShowUI()
    {
        ShowMainMenu();
        showUIButton.transform.DOScale(Vector3.zero, 0.5f);
    }
}
