using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiceRollSaverLoader : MonoBehaviour
{
    public TextAsset diceRollDefaults;
    EventManager eventManager;
    DiceRollSetups diceRollSetups;
    List<DiceAdjust> diceAdjusts;
    List<BonusAdjust> bonusAdjusts;
    // Start is called before the first frame update
    void Start()
    {
        eventManager = GetComponent<EventManager>();
        eventManager.EventAdjustsSpawned.AddListener(AdjustsSpawnedEventListener);
        LoadDiceRollSetups();
        //Debug.Log(JsonUtility.ToJson(diceRollSetups));
    }

    void AdjustsSpawnedEventListener(List<DiceAdjust> _diceAdjusts, List<BonusAdjust> _bonusAdjusts)
    {
        diceAdjusts = _diceAdjusts;
        bonusAdjusts = _bonusAdjusts;
    }

    void LoadDiceRollSetups()
    {
        diceRollSetups = JsonUtility.FromJson<DiceRollSetups>(diceRollDefaults.text);
        eventManager.UpdateDiceRollSetups(diceRollSetups);
    }

    public void AddNewDiceRollSetup()
    {
        Debug.Log(bonusAdjusts.Count);
        DiceRollSetup diceRollSetup = new DiceRollSetup();
        diceRollSetup.diceRollData = new List<DiceRollData>();
        foreach (DiceAdjust diceAdjust in diceAdjusts)
        {
            if (diceAdjust.GetAmount() > 0)
            {
                DiceRollData diceRollData = new DiceRollData();
                diceRollData.name = diceAdjust.dicePrefab.name;
                diceRollData.amount = diceAdjust.GetAmount();
                diceRollData.bonus = diceAdjust.BonusActive();
                diceRollData.penalty = diceAdjust.PenaltyActive();
                diceRollSetup.diceRollData.Add(diceRollData);
            }
        }

        diceRollSetup.bonus = bonusAdjusts[0].GetAmount();
        diceRollSetup.name = "testausta";
        diceRollSetups.savedRolls.Add(diceRollSetup);
        eventManager.UpdateDiceRollSetups(diceRollSetups);
    }
}
