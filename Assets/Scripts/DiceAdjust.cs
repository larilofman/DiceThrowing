using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class DiceAdjust : MonoBehaviour
{
    public TextMeshProUGUI nameField;
    public TMP_InputField amountField;
    public GameObject dicePrefab;
    public Button minusButton;
    public Button plusButton;
    public DiceSetup diceSetup;

    public void Init(GameObject _dicePrefab, int amountDice)
    {
        nameField = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        amountField = transform.GetChild(1).GetComponent<TMP_InputField>();
        nameField.text = _dicePrefab.name;
        dicePrefab = _dicePrefab;
        SetAmount(amountDice);

        minusButton = transform.GetChild(2).GetComponent<Button>();
        plusButton = transform.GetChild(3).GetComponent<Button>();
        minusButton.onClick.AddListener(Decrease);
        plusButton.onClick.AddListener(Increase);

        amountField.characterValidation = TMP_InputField.CharacterValidation.Digit;
        amountField.onValueChanged.AddListener(delegate (string value){ TrimValue(value); });
    }

    void TrimValue(string value)
    {
        if(value == "")
        {
            amountField.text = "0";
        }

        if(value.Length > 1 && value.StartsWith("0"))
        {
            amountField.text = value.Substring(1);
        }

        if(diceSetup.TotalDiceSetup() < 1)
        {
            amountField.text = "1";
        }
    }

    public void SetAmount(int value)
    {
        if (value < 0)
        {
            value = 0;
        }

        amountField.text = value.ToString();
    }

    public int GetAmount()
    {
        return int.Parse(amountField.text);
    }

    private void Increase()
    {
        int newAmount = GetAmount() + 1;
        SetAmount(newAmount);
    }

    private void Decrease()
    {
        int newAmount = GetAmount() - 1;
        SetAmount(newAmount);
    }
}
