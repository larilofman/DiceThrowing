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

        diceRollSetups = JsonUtility.FromJson<DiceRollSetups>(diceRollDefaults.text);
    }

    public void TestLoad()
    {
        //string name = "D6";
        //int amount = 3;
        //int bonusPenalty = 0;


        //string name2 = "D10";
        //int amount2 = 2;
        //int bonusPenalty2 = 0;

        //int bonus = 1;

        //List<string> diceNames = new List<string>();
        //List<int> diceAmounts = new List<int>();
        //List<int> diceBonusPenalties = new List<int>();

        //diceNames.Add(name);
        //diceAmounts.Add(amount);
        //diceBonusPenalties.Add(bonusPenalty);
        //diceNames.Add(name2);
        //diceAmounts.Add(amount2);
        //diceBonusPenalties.Add(bonusPenalty2);


        //DiceRollSetup setup = new DiceRollSetup(diceNames, diceAmounts, diceBonusPenalties, bonus);
        //string json = JsonUtility.ToJson(setup);
        //Debug.Log(json);
        eventManager.LoadDiceScoreSetup(diceRollSetups.savedRolls[0]);
    }
}
