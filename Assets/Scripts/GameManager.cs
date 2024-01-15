using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Android;

public class GameManager : MonoBehaviour
{

    public event Action OnMainMenu;
    public event Action OnItemsMenu;
    public event Action OnARPosition;
    public event Action OnButtonMenu;
    public event Action OnScanScreen;

    private bool isMainMenuActive;

    public static GameManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        MainMenu();
    }
    public void MainMenu()
    {
        OnMainMenu?.Invoke();

        isMainMenuActive = true;
    }

    public void ItemsMenu()
    {
        OnItemsMenu?.Invoke();
    }

    public void ARPosition()
    {
        OnARPosition?.Invoke();
    }

    public void ButtonMenu()
    {
        if (isMainMenuActive)
        {
            OnButtonMenu?.Invoke();
            isMainMenuActive = false;
        }
        else
        {
            OnMainMenu?.Invoke();
            isMainMenuActive = true;
        }
    }

    public void ScanScreen()
    {
        OnScanScreen?.Invoke();
    }

    public void CloseAPP()
    { 
        Application.Quit();
    }
}
