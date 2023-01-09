using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public EventManager eventManager;
    public GameObject scorePanel;
    public GameObject throwPanel;
    public GameObject rethrowPanel;
    public GameObject setupPanel;
    public Button acceptSetupButton;
    public Button saveSetupButton;
    public GameObject scoreDetailsPanel;
    public GameObject creditsPanel;
    public GameObject worldChangePanel;
    public TMP_Text warningText;
    public GameObject setupAcceptButton;
    public GameObject adderContainer;
    public Toggle clearTableToggle;
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
        eventManager.EventAddNewDiceRollSetupOpened.AddListener(AddNewDiceRollSetupHandler);
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
            if(diceAdjust.dicePrefab.name == "D100")
            {
                totalDice += diceAdjust.GetAmount();
            }
        }

        if(totalDice <= 0)
        {
            acceptSetupButton.interactable = false;
            saveSetupButton.interactable = false;
            warningText.text = "Add at least one dice.";
            warningText.gameObject.SetActive(true);
        } else if(totalDice > GlobalSettings.Instance.maxDice)
        {
            acceptSetupButton.interactable = false;
            saveSetupButton.interactable = false;
            warningText.text = $"Maximum amount of dice is {GlobalSettings.Instance.maxDice}.\n D100 counts as two.";
            warningText.gameObject.SetActive(true);
        } else
        {
            warningText.gameObject.SetActive(false);
            acceptSetupButton.interactable = true;
            saveSetupButton.interactable = true;
        }
    }

    void AddNewDiceRollSetupHandler()
    {
        ShowAddDiceRollSetup();
    }

    public void HideUI()
    {
        throwPanel.SetActive(false);
        scorePanel.SetActive(false);
        setupPanel.SetActive(false);
        rethrowPanel.SetActive(false);
        scoreDetailsPanel.SetActive(false);
        creditsPanel.SetActive(false);
        worldChangePanel.SetActive(false);
    }

    public void OnThrowAgainPressed()
    {
        if (clearTableToggle.isOn)
        {
            eventManager.ThrowAgain();
        } else
        {
            eventManager.ThrowMore();
        }

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
        setupAcceptButton.SetActive(true);
        adderContainer.SetActive(false);
        setupPanel.SetActive(true);
    }

    void ShowAddDiceRollSetup()
    {
        HideUI();
        setupAcceptButton.SetActive(false);
        adderContainer.SetActive(true);
        setupPanel.SetActive(true);
    }

    public void ShowThrow()
    {
        HideUI();
        throwPanel.SetActive(true);
        worldChangePanel.SetActive(true);
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
