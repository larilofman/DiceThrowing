using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class DiceScore : Score
{
    [System.NonSerialized] // Added by DiceThrower upon instantiating dice
    public bool thrown = false;
    private GameObject sides;
    private Rigidbody rb;
    private bool stopped = false;
    private EventManager eventManager;
    
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

    public void Init(EventManager _eventManager)
    {
        thrown = true;
        eventManager = _eventManager;
    }

    void Stop()
    {
        stopped = true;
        eventManager.DiceStopped(this);
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
