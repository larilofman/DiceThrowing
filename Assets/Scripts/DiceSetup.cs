using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class DiceSetup : MonoBehaviour
{
    public DiceThrower diceThrower;
    public RectTransform adjustParent;
    public List<GameObject> dicePrefabs = new List<GameObject>();
    public GameObject diceAdjustPrefab;
    public GameObject bonusAdjustPrefab;
    private List<DiceAdjust> diceAdjusts = new List<DiceAdjust>();
    private BonusAdjust bonusAdjust;
    // Start is called before the first frame update
    void Start()
    {
        diceThrower = GetComponent<DiceThrower>();
        InitDice();
        PrepaceDice();
    }
    void InitDice()
    {
        Vector2 firstPos = new(0, 0);
        for (int i = 0; i < dicePrefabs.Count; i++)
        {
            Vector2 spawnPos = firstPos;
            GameObject diceAdjustObj = Instantiate(diceAdjustPrefab, adjustParent.transform);
            diceAdjustObj.GetComponent<RectTransform>().anchoredPosition = spawnPos;
            DiceAdjust diceAdjust = diceAdjustObj.GetComponent<DiceAdjust>();
            diceAdjust.diceSetup = this;

            // Add one dice for the first type on the dice prefabs list, others empty
            int initialAmount = 0;
            if(i == 0)
            {
                initialAmount = 1;
            }
            diceAdjust.Init(dicePrefabs[i], initialAmount);

            diceAdjusts.Add(diceAdjust);
        }
        GameObject bonusAdjustObj = Instantiate(bonusAdjustPrefab, adjustParent.transform);
        bonusAdjust = bonusAdjustObj.GetComponent<BonusAdjust>();
    }

    public void PrepaceDice()
    {
        diceThrower.SpawnDice(diceAdjusts, bonusAdjust);
    }

    public int TotalDiceSetup()
    {
        int totalDice = 0;
        foreach (DiceAdjust diceAdjust in diceAdjusts)
        {
            totalDice += diceAdjust.GetAmount();
        }
        return totalDice;
    }
}
