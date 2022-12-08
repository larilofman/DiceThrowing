using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public EventManager eventManager;
    public GameObject scorePanel;
    public GameObject throwPanel;
    public GameObject rethrowPanel;
    public GameObject setupPanel;

    void Awake()
    {
        eventManager.EventReadyToThrow.AddListener(ReadyToThrowEventHandler);
        eventManager.EventThrowPressed.AddListener(DiceThrownEventHandler);
        eventManager.EventSetupOpened.AddListener(SetupOpenedEventHandler);
        eventManager.EventSetupAccepted.AddListener(SetupAcceptedEventHandler);
    }

    void DiceThrownEventHandler()
    {
        HideUI();
    }

    void ReadyToThrowEventHandler()
    {
        ShowThrow();
    }

    void SetupOpenedEventHandler()
    {
        ShowSetup();
    }

    void SetupAcceptedEventHandler()
    {
        ShowThrow();
    }

    public void HideUI()
    {
        throwPanel.gameObject.SetActive(false);
        scorePanel.gameObject.SetActive(false);
        setupPanel.gameObject.SetActive(false);
        rethrowPanel.gameObject.SetActive(false);
    }

    public void ShowScore(bool hideOthers=false)
    {
        if (hideOthers)
        {
            HideUI();
        }
        scorePanel.gameObject.SetActive(true);       
    }

    public void ShowRethrow()
    {
        rethrowPanel.gameObject.SetActive(true);
    }

    public void ShowSetup()
    {
        HideUI();
        setupPanel.gameObject.SetActive(true);
    }

    public void ShowThrow()
    {
        HideUI();
        throwPanel.gameObject.SetActive(true);
    }
}
