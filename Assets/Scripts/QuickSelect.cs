using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuickSelect : MonoBehaviour
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

    private void SelectSetup(int index)
    {
        dropdown.SetValueWithoutNotify(-1);
        DiceRollSetup selectedSetup = diceRollSetups.savedRolls[index];
        eventManager.LoadDiceScoreSetup(selectedSetup);
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

        dropdown.AddOptions(options);
        dropdown.SetValueWithoutNotify(-1);
    }
}
