using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DiceThrower : MonoBehaviour
{
    public Camera cam;
    public float throwingForce;
    public List<GameObject> floatingDice = new List<GameObject>();
    public List<GameObject> storedDice = new List<GameObject>();
    public GameObject floatingBonus;
    public GameObject bonusPrefab;
    private ScoreManager scoreManager;
    private List<Vector3> positions = new List<Vector3>();
    float randomness = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        scoreManager = GetComponent<ScoreManager>();
        //SpawnDice(); 
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void ClearDice()
    {
        foreach (GameObject oldDice in floatingDice)
        {
            Destroy(oldDice);
        }
        floatingDice.Clear();

        Destroy(floatingBonus);
        floatingBonus = null;
    }

    public void ClearTable()
    {
        foreach (GameObject oldDice in storedDice)
        {
            Destroy(oldDice);
        }
        storedDice.Clear();
    }

    public void StoreDice()
    {
        foreach (GameObject oldDice in floatingDice)
        {
            storedDice.Add(oldDice);
        }
        floatingDice.Clear();
    }

    public void SpawnDice(List<DiceAdjust> diceAdjusts, BonusAdjust bonusAdjust)
    {
        int totalDice = 0;
        foreach (DiceAdjust diceAdjust in diceAdjusts)
        {
            totalDice += diceAdjust.GetAmount();
        }
        positions = CreatePositions(totalDice);

        foreach (DiceAdjust diceAdjust in diceAdjusts)
        {
            for (int i = 0; i < diceAdjust.GetAmount(); i++)
            {
                Vector3 spawnPos = transform.position + GetPosition();
                GameObject instantiatedDice = Instantiate(diceAdjust.dicePrefab, spawnPos, Quaternion.identity);
                floatingDice.Add(instantiatedDice);
            }
        }

        GameObject instantiatedBonus = Instantiate(bonusPrefab);
        Bonus bonus = instantiatedBonus.GetComponent<Bonus>();
        bonus.Init(bonusAdjust.GetAmount());
        floatingBonus = instantiatedBonus;
        scoreManager.SetBonus(bonus);
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

        positions = new List<Vector3>();

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
            if(placedOnRow == maxWidth)
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
        return Random.Range(-randomness, randomness);
    }

    public void StartThrow()
    {
        foreach (GameObject dice in floatingDice)
        {
            Rigidbody diceRb = dice.GetComponent<Rigidbody>();
            diceRb.constraints = RigidbodyConstraints.None;
            DiceScore diceScore = dice.GetComponent<DiceScore>();
            diceScore.thrown = true;
            diceScore.scoreManager = scoreManager;
            ThrowDice(diceRb);
        }
    }

    void ThrowDice(Rigidbody diceRb)
    {
        diceRb.useGravity = true;
        Vector3 throwDirection = cam.transform.forward;
        diceRb.AddForce(throwDirection * throwingForce);
        Vector3 throwTorque = new(
            Random.Range(-30, 30),
            Random.Range(-30, 30),
            Random.Range(-30, 30));
        diceRb.AddTorque(throwTorque, ForceMode.Impulse);
    }
}
