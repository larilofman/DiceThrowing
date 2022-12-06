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
    private EventManager eventManager;
    private DiceManager diceManager;

    void Awake()
    {
        scoreManager = GetComponent<ScoreManager>();
        eventManager = GetComponent<EventManager>();
        diceManager = GetComponent<DiceManager>();
        eventManager.EventThrowPressed.AddListener(DiceThrownEventHandler);
        eventManager.EventSetupOpened.AddListener(SetupOpenedEventHandler);
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

    void SetupOpenedEventHandler()
    {
        ClearFloatingDice();
    }

    public void ClearFloatingDice()
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
