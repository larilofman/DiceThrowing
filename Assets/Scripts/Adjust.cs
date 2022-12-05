using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

abstract public class Adjust : MonoBehaviour
{
    public TextMeshProUGUI nameField;
    public TMP_InputField amountField;
    public GameObject dicePrefab;
    public Button minusButton;
    public Button plusButton;
    public virtual void Init(string diceName, int amountDice, GameObject prefab = null)
    {
        nameField = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        amountField = transform.GetChild(1).GetComponent<TMP_InputField>();
        nameField.text = diceName;

        SetAmount(amountDice);

        minusButton = transform.GetChild(2).GetComponent<Button>();
        plusButton = transform.GetChild(3).GetComponent<Button>();
        minusButton.onClick.AddListener(Decrease);
        plusButton.onClick.AddListener(Increase);

        amountField.onValueChanged.AddListener(delegate (string value) { TrimValue(value); });
    }

    public int GetAmount()
    {
        return int.Parse(amountField.text);
    }
    protected void Increase()
    {
        int newAmount = GetAmount() + 1;
        SetAmount(newAmount);
    }

    protected void Decrease()
    {
        int newAmount = GetAmount() - 1;
        SetAmount(newAmount);
        Debug.Log(amountField.characterValidation);
    }

    public abstract void SetAmount(int amount);

    protected abstract void TrimValue(string value);
}
