using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class DiceScore : Score
{
    [System.NonSerialized] // Added by DiceThrower upon instantiating dice
    public ScoreManager scoreManager;
    public bool thrown = false;
    private GameObject sides;
    private Rigidbody rb;
    private bool stopped = false;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sides = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        if (thrown && !stopped && rb.IsSleeping())
        {
            Stop();
        }
    }

    void Stop()
    {
        stopped = true;
        scoreManager.DiceStopped(this);
    }

    public override int GetResult()
    {
        GameObject highestSide = sides.transform.GetChild(0).gameObject;
        foreach (Transform child in sides.transform)
        {
            if(child.position.y > highestSide.transform.position.y)
            {
                highestSide = child.gameObject;
            }
        }
        int score = int.Parse(highestSide.name);
        return score;
    }
}
