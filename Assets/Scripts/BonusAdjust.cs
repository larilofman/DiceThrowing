using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
public class BonusAdjust : Adjust
{
    //void Awake()
    //{
    //    nameField = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    //    amountField = transform.GetChild(1).GetComponent<TMP_InputField>();
    //    nameField.text = "Bonus";
    //    SetAmount(0);

    //    minusButton = transform.GetChild(2).GetComponent<Button>();
    //    plusButton = transform.GetChild(3).GetComponent<Button>();
    //    minusButton.onClick.AddListener(Decrease);
    //    plusButton.onClick.AddListener(Increase);

    //    amountField.characterValidation = TMP_InputField.CharacterValidation.Integer;
    //    amountField.onValueChanged.AddListener(delegate (string value) { TrimValue(value); });
    //}

    public override void Init(string diceName, int amountDice, GameObject prefab = null)
    {
        base.Init(diceName, amountDice, prefab);
        dicePrefab = null;
        amountField.characterValidation = TMP_InputField.CharacterValidation.Integer;
        Debug.Log("???");
    }

    protected override void TrimValue(string value)
    {
        if (value == "")
        {
            amountField.text = "0";
        }

        if (value.Length > 1 && value.StartsWith("0"))
        {
            amountField.text = value.Substring(1);
        }

        if(value == "-0")
        {
            amountField.text = "0";
        }
    }

    public override void SetAmount(int value)
    {
        amountField.text = value.ToString();
    }
}
