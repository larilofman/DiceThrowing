using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public EventManager eventManager;
    public GameObject scorePanel;
    public GameObject throwPanel;
    public GameObject rethrowPanel;
    public GameObject setupPanel;
    public TextMeshProUGUI scoreTitle;
    int totalScore;
    // Start is called before the first frame update
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

    //public void ClearScore()
    //{
    //    totalScore = 0;

    //    foreach (Transform child in scoreTextParent)
    //    {
    //        Destroy(child.gameObject);
    //    }
    //}

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

    void UpdateTitle(Dictionary<string, int> diceAmounts, int bonusAmount)
    {
        List<string> dices = new List<string>();
        foreach (KeyValuePair<string, int> dice in diceAmounts)
        {
            dices.Add($"{dice.Value}{dice.Key}");
        }

        string titleString = "";
        for (int i = 0; i < dices.Count; i++)
        {
            titleString += dices[i];
            if (dices.Count > 1 && i != dices.Count - 1) 
            {
                titleString += "+";
            }
        }
        if(bonusAmount > 0)
        {
            titleString += $"+{bonusAmount}";
        } else if (bonusAmount < 0)
        {
            titleString += $"{bonusAmount}";
        }
        
        scoreTitle.text = titleString;
    }
}
