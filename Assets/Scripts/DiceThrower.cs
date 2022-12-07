using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DiceThrower : MonoBehaviour
{
    public Camera cam;
    public float throwingForce;
    private EventManager eventManager;
    private DiceManager diceManager;

    void Awake()
    {
        eventManager = GetComponent<EventManager>();
        diceManager = GetComponent<DiceManager>();
        eventManager.EventThrowPressed.AddListener(DiceThrownEventHandler);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void DiceThrownEventHandler()
    {
        StartThrow();
    }

    void StartThrow()
    {
        foreach (GameObject dice in diceManager.activeDice)
        {
            Rigidbody diceRb = dice.GetComponent<Rigidbody>();
            diceRb.constraints = RigidbodyConstraints.None;
            DiceScore diceScore = dice.GetComponent<DiceScore>();
            diceScore.Init(eventManager);
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
