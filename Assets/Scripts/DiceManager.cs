using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DiceManager : MonoBehaviour
{
    public Transform activeDiceParent;
    public List<DiceAdjust> diceAdjusts;
    public List<BonusAdjust> bonusAdjusts;
    public List<GameObject> activeDice;
    private EventManager eventManager;
    private List<Vector3> positions = new List<Vector3>();
    private List<DiceScore> stoppedDices = new List<DiceScore>();
    // Start is called before the first frame update
    void Awake()
    {
        eventManager = GetComponent<EventManager>();
        eventManager.EventAdjustsSpawned.AddListener(AdjustsSpawnedEventHandler);
        eventManager.EventSetupAccepted.AddListener(SetupAcceptedEventHandler);
        eventManager.EventDiceStopped.AddListener(DiceStoppedEventHandler);
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
            eventManager.AllDiceStopped(stoppedDices);
            //CalculateScore();

            //Vector3 diceCenter = GetMeanVector();
            //camMover.ZoomToDice(diceCenter);
        }
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

    void ClearActiveDice()
    {
        foreach (GameObject dice in activeDice)
        {
            Destroy(dice);
        }
        activeDice.Clear();
    }

    public void SpawnDice(List<DiceAdjust> diceAdjusts)
    {
        int totalDice = 0;
        foreach (Adjust diceAdjust in diceAdjusts)
        {
            totalDice += diceAdjust.GetAmount();
        }

        positions = CreatePositions(totalDice);

        foreach (Adjust diceAdjust in diceAdjusts)
        {
            for (int i = 0; i < diceAdjust.GetAmount(); i++)
            {
                Vector3 spawnPos = transform.position + GetPosition();
                GameObject instantiatedDice = Instantiate(diceAdjust.dicePrefab, spawnPos, Quaternion.identity);
                activeDice.Add(instantiatedDice);
            }
        }

        //GameObject instantiatedBonus = Instantiate(bonusPrefab);
        //Bonus bonus = instantiatedBonus.GetComponent<Bonus>();
        //bonus.Init(bonusAdjust.GetAmount());
        //floatingBonus = instantiatedBonus;
        //scoreManager.SetBonus(bonus);
    }

    Vector3 GetPosition()
    {
        int index = Random.Range(0, positions.Count);
        Vector3 pos = positions[index];
        positions.RemoveAt(index);
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
