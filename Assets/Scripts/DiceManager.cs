using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public Transform activeDiceParent;
    public List<GameObject> activeDice;
    private EventManager eventManager;
    // Start is called before the first frame update
    void Start()
    {
        eventManager = GetComponent<EventManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
