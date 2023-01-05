using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DiceRollSaverLoader : MonoBehaviour
{
    public TextAsset diceRollDefaults;
    public TMP_InputField newSetupNameField;
    EventManager eventManager;
    DiceRollSetups diceRollSetups;
    List<DiceAdjust> diceAdjusts;
    List<BonusAdjust> bonusAdjusts;
    private string cachedTitle = "";
    DiceRollSetup cachedSetup = null;
    // Start is called before the first frame update
    void Start()
    {
        eventManager = GetComponent<EventManager>();
        eventManager.EventAdjustsSpawned.AddListener(AdjustsSpawnedEventListener);
        eventManager.EventTitleChanged.AddListener(UpdateSetupName);
        eventManager.EventAddNewDiceRollSetupOpened.AddListener(CacheDiceRollSetup);
        eventManager.EventDiceRollSetupDeleted.AddListener(DeleteDiceRollSetup);
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
        DiceRollSetup diceRollSetup = CreateDiceRollSetup();

        diceRollSetup.name = newSetupNameField.text;
        diceRollSetups.savedRolls.Add(diceRollSetup);

        eventManager.UpdateDiceRollSetups(diceRollSetups);
    }

    public void DeleteDiceRollSetup(int index)
    {
        Debug.Log("deleting: " + index);
        diceRollSetups.savedRolls.RemoveAt(index);
        eventManager.UpdateDiceRollSetups(diceRollSetups);
    }

    DiceRollSetup CreateDiceRollSetup()
    {
        DiceRollSetup diceRollSetup = new DiceRollSetup();
        diceRollSetup.diceRollData = new List<DiceRollData>();

        foreach (DiceAdjust diceAdjust in diceAdjusts)
        {

            DiceRollData diceRollData = new DiceRollData();
            diceRollData.name = diceAdjust.dicePrefab.name;
            diceRollData.amount = diceAdjust.GetAmount();
            diceRollData.bonus = diceAdjust.BonusActive();
            diceRollData.penalty = diceAdjust.PenaltyActive();
            diceRollSetup.diceRollData.Add(diceRollData);

        }

        diceRollSetup.bonus = bonusAdjusts[0].GetAmount();

        return diceRollSetup;
    }

    void UpdateSetupName(string name)
    {
        newSetupNameField.text = name;
    }

    void CacheDiceRollSetup()
    {
        cachedTitle = newSetupNameField.text;
        cachedSetup = CreateDiceRollSetup();
    }

    public void RecoverCachedDiceRollSetup()
    {
        eventManager.ChangeTitle(cachedTitle);
        eventManager.LoadDiceScoreSetup(cachedSetup);
    }
}
