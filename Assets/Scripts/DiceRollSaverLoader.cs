using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRollSaverLoader : MonoBehaviour
{
    public TextAsset diceRollDefaults;
    EventManager eventManager;
    DiceRollSetups diceRollSetups;
    // Start is called before the first frame update
    void Start()
    {
        eventManager = GetComponent<EventManager>();
        InitDiceRollSetups();
        //Debug.Log(JsonUtility.ToJson(diceRollSetups));
    }

    void InitDiceRollSetups()
    {
        diceRollSetups = JsonUtility.FromJson<DiceRollSetups>(diceRollDefaults.text);
        eventManager.UpdateDiceRollSetups(diceRollSetups);
    }

    public void TestLoad()
    {
        eventManager.LoadDiceScoreSetup(diceRollSetups.savedRolls[1]);
    }
}
