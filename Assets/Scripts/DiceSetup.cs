using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

[Serializable]
public struct DicePrefabPair
{
    public string name;
    public GameObject prefab;
}
public class DiceSetup : MonoBehaviour
{
    public RectTransform adjustParent;
    public List<GameObject> dicePrefabs = new List<GameObject>();
    public DicePrefabPair[] dicePrefabPairs;
    public GameObject diceAdjustPrefab;
    public GameObject bonusAdjustPrefab;
    private EventManager eventManager;
    private List<DiceAdjust> diceAdjusts = new List<DiceAdjust>();
    private List<BonusAdjust> bonusAdjusts = new List<BonusAdjust>();
    private Dictionary<string, DiceAdjust> diceAdjustDict = new Dictionary<string, DiceAdjust>();

    // Start is called before the first frame update
    void Start()
    {
        eventManager = GetComponent<EventManager>();
        eventManager.EventDiceRollSetupLoaded.AddListener(LoadDiceRollSetup);
        InitDiceAdjusts();
    }

    void InitDiceAdjusts()
    {
        for (int i = 0; i < dicePrefabs.Count; i++)
        {
            GameObject diceAdjustObj = Instantiate(diceAdjustPrefab, adjustParent.transform);
            DiceAdjust diceAdjust = diceAdjustObj.GetComponent<DiceAdjust>();

            // Add one dice for the first type on the dice prefabs list, others empty
            int initialAmount = 0;
            if (i == 0)
            {
                initialAmount = 1;
            }
            diceAdjust.Init(dicePrefabs[i], initialAmount, eventManager);

            diceAdjusts.Add(diceAdjust);
        }
        GameObject bonusAdjustObj = Instantiate(bonusAdjustPrefab, adjustParent.transform);
        BonusAdjust bonusAdjust = bonusAdjustObj.GetComponent<BonusAdjust>();
        bonusAdjust.Init(null, 0, eventManager);
        bonusAdjusts.Add(bonusAdjust);

        CreateDictionary();

        eventManager.AdjustsSpawned(diceAdjusts, bonusAdjusts);
    }

    void CreateDictionary()
    {
        foreach (DiceAdjust diceAdjust in diceAdjusts)
        {
            diceAdjustDict.Add(diceAdjust.dicePrefab.name, diceAdjust);
        }
    }

    void LoadDiceRollSetup(DiceRollSetup diceRollSetup)
    {
        foreach (DiceAdjust diceAdjust in diceAdjusts)
        {
            diceAdjust.SetAmount(0);
        }

        foreach (BonusAdjust bonusAdjust in bonusAdjusts)
        {
            bonusAdjust.SetAmount(0);
        }

        for (int i = 0; i < diceRollSetup.diceNames.Count; i++)
        {
            string name = diceRollSetup.diceNames[i];
            int amount = diceRollSetup.diceAmounts[i];
            int bonusPenalty = diceRollSetup.diceBonusPenalties[i];
            DiceAdjust diceAdjust = diceAdjustDict[name];
            bool bonus = false;
            bool penalty = false;
            if (bonusPenalty > 0)
            {
                bonus = true;
            }
            else if (bonusPenalty < 0)
            {
                penalty = true;
            }

            diceAdjust.SetAmount(amount, true);

            if (bonus)
            {
                diceAdjust.EnableBonus();
            }
            else if (penalty)
            {
                diceAdjust.EnablePenalty();
            }
        }

        bonusAdjusts[0].SetAmount(diceRollSetup.bonus);

        eventManager.AcceptSetup();
    }
}
