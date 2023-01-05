using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuickSelect : MonoBehaviour, IPointerClickHandler
{
    public string DefaultText;  
    public EventManager eventManager;
    private TMP_Dropdown dropdown;
    private DiceRollSetups diceRollSetups;

    private void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        eventManager.EventDiceRollSetupsChanged.AddListener(BuildDropdown);
        dropdown.onValueChanged.AddListener(SelectSetup);
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            DisableDeleteButtonFromAddNew();
        }
    }

    private void DisableDeleteButtonFromAddNew()
    {
        Transform dropdownList = transform.GetChild(4);
        Transform content = dropdownList.GetChild(0).GetChild(0);
        Transform lastChild = content.GetChild(content.childCount - 1);
        Button lastDeleteButton = lastChild.GetComponentInChildren<Button>();
        lastDeleteButton.gameObject.SetActive(false);
    }

    private void SelectSetup(int index)
    {
        dropdown.SetValueWithoutNotify(-1);
        if (index == diceRollSetups.savedRolls.Count)
        {
            eventManager.OpenAddNewDiceRollSetup();
        } else
        {         
            DiceRollSetup selectedSetup = diceRollSetups.savedRolls[index];
            eventManager.LoadDiceScoreSetup(selectedSetup);
        }

    }

    private void BuildDropdown(DiceRollSetups _diceRollSetups)
    {
        diceRollSetups = _diceRollSetups;
        dropdown.ClearOptions();

        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

        foreach (DiceRollSetup diceRollSetup in diceRollSetups.savedRolls)
        {
            TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData(diceRollSetup.name);
            options.Add(optionData);
        }

        TMP_Dropdown.OptionData addNewOption = new TMP_Dropdown.OptionData("        Add new");
        options.Add(addNewOption);

        dropdown.AddOptions(options);
        dropdown.SetValueWithoutNotify(-1);
    }
}
