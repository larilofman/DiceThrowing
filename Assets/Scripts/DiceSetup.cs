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
    private List<Adjust> diceAdjusts = new List<Adjust>();
    private Dictionary<string, GameObject> dicePrefabDictionary = new Dictionary<string, GameObject>();
    private DiceThrower diceThrower;
    private EventManager eventManager;
    private BonusAdjust bonusAdjust;

    private void Awake()
    {
        foreach (DicePrefabPair pair in dicePrefabPairs)
        {
            dicePrefabDictionary.Add(pair.name, pair.prefab);
        }

        //foreach (KeyValuePair<string, GameObject> pair in dicePrefabDictionary)
        //{
        //    Debug.Log(pair.Key);
        //    Debug.Log(pair.Value);
        //}
    }
    // Start is called before the first frame update
    void Start()
    {
        diceThrower = GetComponent<DiceThrower>();
        eventManager = GetComponent<EventManager>();
        eventManager.EventReadyToThrow.AddListener(ReadyToThrowEventHandler);
        eventManager.EventSetupAccepted.AddListener(SetupAcceptedEventHandler);
        InitDiceAdjusts();
    }

    void ReadyToThrowEventHandler()
    {
        PrepareDice();
    }

    void SetupAcceptedEventHandler()
    {
        PrepareDice();
    }
    void InitDiceAdjusts()
    {
        foreach (KeyValuePair<string, GameObject> pair in dicePrefabDictionary)
        {
            GameObject adjustPrefab = diceAdjustPrefab;
            if(pair.Key == "Bonus")
            {
                adjustPrefab = bonusAdjustPrefab;
            }

            GameObject diceAdjustObj = Instantiate(adjustPrefab, adjustParent.transform);
            //diceAdjustObj.GetComponent<RectTransform>().anchoredPosition = spawnPos;
            Adjust adjust = diceAdjustObj.GetComponent<Adjust>();
            //diceAdjust.diceSetup = this;

            // Add one dice for the first type on the dice prefabs list, others empty
            int initialAmount = 0;
            if (pair.Key == "D3")
            {
                initialAmount = 1;
            }
            adjust.Init(pair.Key, initialAmount, pair.Value);

            diceAdjusts.Add(adjust);
        }


        //Vector2 firstPos = new(0, 0);
        //for (int i = 0; i < dicePrefabs.Count; i++)
        //{
        //    Vector2 spawnPos = firstPos;
        //    GameObject diceAdjustObj = Instantiate(diceAdjustPrefab, adjustParent.transform);
        //    diceAdjustObj.GetComponent<RectTransform>().anchoredPosition = spawnPos;
        //    DiceAdjust diceAdjust = diceAdjustObj.GetComponent<DiceAdjust>();
        //    diceAdjust.diceSetup = this;

        //    // Add one dice for the first type on the dice prefabs list, others empty
        //    int initialAmount = 0;
        //    if(i == 0)
        //    {
        //        initialAmount = 1;
        //    }
        //    diceAdjust.Init(dicePrefabs[i], initialAmount);

        //    diceAdjusts.Add(diceAdjust);
        //}
        //GameObject bonusAdjustObj = Instantiate(bonusAdjustPrefab, adjustParent.transform);
        //bonusAdjust = bonusAdjustObj.GetComponent<BonusAdjust>();
    }

    public void PrepareDice()
    {
        diceThrower.SpawnDice(diceAdjusts);
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
