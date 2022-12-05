using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class DiceAdjust : Adjust
{
    public override void Init(string diceName, int amountDice, GameObject prefab = null)
    {
        base.Init(diceName,amountDice, prefab);
        dicePrefab = prefab;
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

    public override void SetAmount(int value)
    {
        if (value < 0)
        {
            value = 0;
        }

        amountField.text = value.ToString();
    }
}
