using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class DiceManager : MonoBehaviour
{
    public Transform activeDiceParent;
    public List<DiceAdjust> diceAdjusts;
    public List<BonusAdjust> bonusAdjusts;
    public List<GameObject> activeDice;
    public List<GameObject> storedDice;
    public List<DiceScore> stoppedDices = new List<DiceScore>();
    private EventManager eventManager;
    private List<Vector3> allPositions = new List<Vector3>();
    private List<Vector3> availablePositions = new List<Vector3>();
    private int spawnSpots = 300;
    private List<List<DiceScore>> bonusDiceGroups = new List<List<DiceScore>>();
    public List<DiceScore> diceToFade = new List<DiceScore>();
    // Start is called before the first frame update
    void Awake()
    {
        allPositions = CreatePositions(spawnSpots);
        eventManager = GetComponent<EventManager>();
        eventManager.EventAdjustsSpawned.AddListener(AdjustsSpawnedEventHandler);
        eventManager.EventSetupAccepted.AddListener(SetupAcceptedEventHandler);
        eventManager.EventDiceStopped.AddListener(DiceStoppedEventHandler);
        eventManager.EventThrowAgainPressed.AddListener(ThrowAgainPressedEventHandler);
        eventManager.EventThrowMorePressed.AddListener(ThrowMorePressedEventHandler);
    }

    void Start()
    {
        eventManager.Ready();
    }

    public void DiceStoppedEventHandler(DiceScore diceScore)
    {
        stoppedDices.Add(diceScore);

        if (stoppedDices.Count >= activeDice.Count)
        {
            if (bonusDiceGroups.Count > 0)
            {
                FindLowests();
                StartCoroutine(RemoveLowest());
            }

            eventManager.AllDiceStopped(stoppedDices, bonusAdjusts);
        }
    }

    private void FindLowests()
    {
        Debug.Log("bonusdicegroups: " + bonusDiceGroups.Count);
        foreach (List<DiceScore> group in bonusDiceGroups)
        {
            DiceScore lowestScore = null;
            foreach (DiceScore dice in group)
            {
                if (!lowestScore)
                {
                    lowestScore = dice;
                }
                else if (dice.GetResult() < lowestScore.GetResult())
                {
                    lowestScore = dice;
                }
            }
            diceToFade.Add(lowestScore);
        }   
    }

    IEnumerator RemoveLowest()
    {
        //foreach (List<DiceScore> group in bonusDiceGroups)
        //{
        //    DiceScore lowestScore = null;
        //    foreach (DiceScore dice in group)
        //    {
        //        if (!lowestScore)
        //        {
        //            lowestScore = dice;
        //        } else if(dice.GetResult() < lowestScore.GetResult()) {
        //            lowestScore = dice;
        //        }
        //    }
        //    Debug.Log(lowestScore.GetResult());
        //    stoppedDices.Remove(lowestScore);
        //    activeDice.Remove(lowestScore.gameObject);
        //    lowestScore.AddComponent<DiceFader>();
        //}

        foreach (DiceScore dice in diceToFade)
        {
            if(dice != null)
            {
                stoppedDices.Remove(dice);
                activeDice.Remove(dice.gameObject);
                dice.AddComponent<DiceFader>();
            }
        }

        yield return GlobalSettings.Instance.bonusDiceVanishTime;
    }

    public void DEBUG_AllDiceStopped()
    {
        eventManager.AllDiceStopped(stoppedDices, bonusAdjusts);
    }

    void AdjustsSpawnedEventHandler(List<DiceAdjust> _diceAdjusts, List<BonusAdjust> _bonusAdjusts)
    {
        diceAdjusts = _diceAdjusts;
        bonusAdjusts = _bonusAdjusts;
        SpawnDice(diceAdjusts);
    }

    void SetupAcceptedEventHandler()
    {
        ClearActiveDice();
        SpawnDice(diceAdjusts);
    }

    void ThrowAgainPressedEventHandler()
    {
        StartCoroutine(SpawnNewDice());
    }

    void ThrowMorePressedEventHandler()
    {
        StartCoroutine(SpawnMoreDice());
    }

    void ClearActiveDice()
    {
        foreach (GameObject dice in activeDice)
        {
            Destroy(dice);
        }
        activeDice.Clear();
        stoppedDices.Clear();
    }

    void ClearStoredDice()
    {
        foreach (GameObject dice in storedDice)
        {
            Destroy(dice);
        }
        storedDice.Clear();
    }

    void ClearFades()
    {
        diceToFade.Clear();
        bonusDiceGroups.Clear();
    }

    void StoreDice()
    {
        foreach (GameObject dice in activeDice)
        {
            storedDice.Add(dice);
        }
        activeDice.Clear();
        stoppedDices.Clear();
    }

    private IEnumerator SpawnNewDice()
    {
        float delay = GlobalSettings.Instance.zoomOutTime;
        yield return new WaitForSeconds(delay);
        ClearActiveDice();
        ClearStoredDice();
        ClearFades();
        SpawnDice(diceAdjusts);
    }
    private IEnumerator SpawnMoreDice()
    {
        float delay = GlobalSettings.Instance.zoomOutTime;
        yield return new WaitForSeconds(delay);
        StoreDice();
        ClearFades();
        SpawnDice(diceAdjusts);
    }

    public void SpawnDice(List<DiceAdjust> diceAdjusts)
    {
        int totalDice = 0;
        foreach (Adjust diceAdjust in diceAdjusts)
        {
            totalDice += diceAdjust.GetAmount();

            // D100 (and possibly other non-normal dice that use more than one visible dice)
            if (diceAdjust.dicePrefab.name.Length > 3)
            {
                totalDice += diceAdjust.GetAmount();
            }
        }

        availablePositions = allPositions.GetRange(0, totalDice);

        foreach (Adjust diceAdjust in diceAdjusts)
        {
            List<DiceScore> diceGroup = new List<DiceScore>();
            bool bonusActive = false;
            if(diceAdjust.GetType() == typeof(DiceAdjust))
            {
                DiceAdjust diceAdj = (DiceAdjust)diceAdjust;
                bonusActive = diceAdj.BonusActive();
                if (bonusActive)
                {
                    bonusDiceGroups.Add(diceGroup);
                }      
            }

            for (int i = 0; i < diceAdjust.GetAmount(); i++)
            {
                Vector3 spawnPos = transform.position + GetPosition();
                GameObject instantiatedDice = Instantiate(diceAdjust.dicePrefab, spawnPos, Quaternion.identity, activeDiceParent);
                activeDice.Add(instantiatedDice);

                DiceScore diceScore = instantiatedDice.GetComponent<DiceScore>();
                diceScore.Init(eventManager);

                if (bonusActive)
                {
                    diceGroup.Add(diceScore);
                }

                // D100 (and possibly other non-normal dice that use more than one visible dice)
                if (diceAdjust.dicePrefab.name.Length > 3 && diceAdjust.dicePrefab.name == "D100")
                {
                    D100Score d100Score = instantiatedDice.GetComponent<D100Score>();
                    GameObject childDicePrefab = d100Score.childDicePrefab;

                    spawnPos = transform.position + GetPosition();
                    GameObject instantiatedChildDice = Instantiate(childDicePrefab, spawnPos, Quaternion.identity, activeDiceParent);
                    d100Score.InitChild(instantiatedChildDice);
                }
            }
        }
    }

    Vector3 GetPosition()
    {
        int index = Random.Range(0, availablePositions.Count);
        Vector3 pos = availablePositions[index];
        availablePositions.RemoveAt(index);
        return pos;
    }

    private List<Vector3> CreatePositions(int amount)
    {
        int maxWidth = 7;
        int maxHeight = 2;
        float gap = 1.5f;

        List<Vector3> positions = new List<Vector3>();

        float x = 0;
        float y = 0;
        float z = 0;
        int placedOnRow = 0;
        int rowsPlaced = 0;
        int columnsPlaced = 0;

        for (int i = 0; i < amount; i++)
        {
            // place every other dice on right and every other on left
            if (i % 2 == 0)
            {
                x = (-placedOnRow - 1) / 2 * gap;
            }
            else
            {
                x = (placedOnRow + 1) / 2 * gap;
            }
            y = rowsPlaced * gap;
            z = columnsPlaced * gap;
            placedOnRow++;

            // next row
            if (placedOnRow == maxWidth)
            {
                placedOnRow = 0;
                rowsPlaced++;

                // next z-depth-thingy
                if (rowsPlaced == maxHeight)
                {
                    rowsPlaced = 0;
                    columnsPlaced++;
                }
            }
            Vector3 pos = new Vector3(x + GetRandomness(), y + GetRandomness(), z + GetRandomness());
            positions.Add(pos);
        }

        return positions;
    }

    float GetRandomness()
    {
        float randomness = 0.1f;
        return Random.Range(-randomness, randomness);
    }

}
