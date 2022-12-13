using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public EventManager eventManager;
    public GameObject scorePanel;
    public GameObject throwPanel;
    public GameObject rethrowPanel;
    public GameObject setupPanel;
    public Button AcceptSetupButton;
    public GameObject scoreDetailsPanel;
    public GameObject creditsPanel;
    private List<DiceAdjust> diceAdjusts = new List<DiceAdjust>();

    void Awake()
    {
        eventManager.EventReadyToThrow.AddListener(ReadyToThrowEventHandler);
        eventManager.EventThrowPressed.AddListener(DiceThrownEventHandler);
        eventManager.EventSetupOpened.AddListener(SetupOpenedEventHandler);
        eventManager.EventSetupAccepted.AddListener(SetupAcceptedEventHandler);
        eventManager.EventAllDiceStopped.AddListener(AllDiceStoppedEventHandler);
        eventManager.EventThrowAgainPressed.AddListener(ThrowAgainPressedEventHandler);
        eventManager.EventThrowMorePressed.AddListener(ThrowMorePressedEventHandler);
        eventManager.EventAdjustsSpawned.AddListener(AdjustsSpawnedEventHandler);
        eventManager.EventAdjustsChanged.AddListener(AdjustsChangedEventHandler);
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

    void AllDiceStoppedEventHandler(List<DiceScore> dummy1, List<BonusAdjust> dummy2)
    {
        StartCoroutine(ThrowFinished());
    }

    void ThrowAgainPressedEventHandler()
    {
        StartCoroutine(ThrowAgain());
    }

    void ThrowMorePressedEventHandler()
    {
        StartCoroutine(ThrowMore());
    }

    void AdjustsSpawnedEventHandler(List<DiceAdjust> _diceAdjusts, List<BonusAdjust> _bonusAdjusts)
    {
        diceAdjusts = _diceAdjusts;
    }

    void AdjustsChangedEventHandler()
    {
        int totalDice = 0;
        foreach (DiceAdjust diceAdjust in diceAdjusts)
        {
            totalDice += diceAdjust.GetAmount();
        }

        if(totalDice > 0)
        {
            AcceptSetupButton.interactable = true;
        } else
        {
            AcceptSetupButton.interactable = false;
        }
    }

    public void HideUI()
    {
        throwPanel.SetActive(false);
        scorePanel.SetActive(false);
        setupPanel.SetActive(false);
        rethrowPanel.SetActive(false);
        scoreDetailsPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    public void ShowScore(bool hideOthers=false)
    {
        if (hideOthers)
        {
            HideUI();
        }
        scorePanel.SetActive(true);       
    }

    public void ShowRethrow()
    {
        rethrowPanel.SetActive(true);
    }

    public void ShowSetup()
    {
        HideUI();
        setupPanel.SetActive(true);
    }

    public void ShowThrow()
    {
        HideUI();
        throwPanel.SetActive(true);
    }

    public void ToggleScoreDetails()
    {
        scoreDetailsPanel.SetActive(!scoreDetailsPanel.activeSelf);
    }

    public void ShowCredits()
    {
        creditsPanel.SetActive(true);
    }

    public void HideCredits()
    {
        creditsPanel.SetActive(false);
    }

    private IEnumerator ThrowFinished()
    {
        float delay = GlobalSettings.Instance.zoomInTime;
        yield return new WaitForSeconds(delay);
        ShowScore();
        ShowRethrow();
    }

    private IEnumerator ThrowAgain()
    {
        ShowScore(true);
        float delay = GlobalSettings.Instance.zoomOutTime;
        yield return new WaitForSeconds(delay);
        ShowThrow();
    }

    private IEnumerator ThrowMore()
    {
        ShowScore(true);
        float delay = GlobalSettings.Instance.zoomOutTime;
        yield return new WaitForSeconds(delay);
        ShowThrow();
    }
}
