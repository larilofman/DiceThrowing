using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class DiceAdjust : Adjust
{
    public Toggle bonusToggle;
    public override void Init(GameObject prefab, int amount, EventManager _eventManager)
    {
        bonusToggle = transform.GetChild(4).GetComponent<Toggle>();
        bonusToggle.onValueChanged.AddListener(value =>  OnBonusToggled(value));
        base.Init(prefab, amount, _eventManager);
        dicePrefab = prefab;
        nameField.text = prefab.name;
        amountField.characterValidation = TMP_InputField.CharacterValidation.Digit;       
    }

    protected override void TrimValue(string value)
    {
        if(value == "")
        {
            amountField.text = "0";
        }

        if(value.Length > 1 && value.StartsWith("0"))
        {
            amountField.text = value.Substring(1);
        }

        //if(diceSetup.TotalDiceSetup() < 1)
        //{
        //    amountField.text = "1";
        //}
    }

    public override void SetAmount(int value, bool invokeChange=true)
    {
        if (value < 0)
        {
            value = 0;
        }

        if (value > 1)
        {
            bonusToggle.interactable = true;
        }
        else
        {
            bonusToggle.interactable = false;
            bonusToggle.isOn = false;
        }

        amountField.text = value.ToString();
        if (invokeChange)
        {
            OnAmountChanged();
        }
    }

    public bool BonusActive()
    {
        if (!bonusToggle.interactable)
        {
            return false;
        }
        return bonusToggle.isOn;
    }

    private void OnBonusToggled(bool selected)
    {
        OnAmountChanged();
    }
}
