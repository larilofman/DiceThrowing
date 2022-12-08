using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public UnityEvent EventReadyToThrow;
    public UnityEvent EventThrowPressed;
    public UnityEvent EventSetupOpened;
    public UnityEvent EventSetupAccepted;
    public UnityEvent<DiceScore> EventDiceStopped;
    public UnityEvent<List<DiceScore>, List<BonusAdjust>> EventAllDiceStopped;
    public UnityEvent<List<DiceAdjust>, List<BonusAdjust>> EventAdjustsSpawned;
    public UnityEvent EventAdjustsChanged;

    public void Ready()
    {
        EventReadyToThrow.Invoke();
    }
    public void Throw()
    {
        EventThrowPressed.Invoke();
    }

    public void OpenSetup()
    {
        EventSetupOpened.Invoke();
    }

    public void AcceptSetup()
    {
        EventSetupAccepted.Invoke();
    }

    public void DiceStopped(DiceScore diceScore)
    {
        EventDiceStopped.Invoke(diceScore);
    }

    public void AllDiceStopped(List<DiceScore> dices, List<BonusAdjust> bonuses)
    {
        EventAllDiceStopped.Invoke(dices, bonuses);
    }

    public void AdjustsSpawned(List<DiceAdjust> diceAdjusts, List<BonusAdjust> bonusAdjusts)
    {
        EventAdjustsSpawned.Invoke(diceAdjusts, bonusAdjusts);
    }

    public void AdjustsChanged()
    {
        EventAdjustsChanged.Invoke();
    }
}
