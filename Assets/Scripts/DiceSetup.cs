using System;
using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
        eventManager = GetComponent<EventManager>();
        InitDiceAdjusts();
    }

    void InitDiceAdjusts()
    {
        List<DiceAdjust> diceAdjusts = new List<DiceAdjust>();
        List<BonusAdjust> bonusAdjusts = new List<BonusAdjust>();

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
        eventManager.AdjustsSpawned(diceAdjusts, bonusAdjusts);
    }
}
