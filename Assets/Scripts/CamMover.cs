using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CamMover : MonoBehaviour
{
    public Vector3 offSet;
    public UIManager uiManager;
    public DiceSetup diceSetup;
    public DiceThrower diceThrower;
    public EventManager eventManager;
    private Vector3 originalPos;
    private Quaternion originalRot;
    private List<Transform> diceLocations = new List<Transform>();
    public int targetDiceIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;
        originalRot = transform.rotation;
        eventManager.EventAllDiceStopped.AddListener(AllDiceStoppedEventHandler);
        eventManager.EventThrowAgainPressed.AddListener(ThrowAgainPressedEventHandler);
        eventManager.EventThrowMorePressed.AddListener(ThrowMorePressedEventHandler);
    }

    void AllDiceStoppedEventHandler(List<DiceScore> dices, List<BonusAdjust> bonuses)
    {
        diceLocations.Clear();
        targetDiceIndex = 0;
        foreach (DiceScore dice in dices)
        {
            diceLocations.Add(dice.transform);
            if (dice.GetType() == typeof(D100Score))
            {
                D100Score d100Score = (D100Score)dice;
                diceLocations.Add(d100Score.childDiceScore.transform);
            }
        }
        ZoomToDice(GlobalSettings.Instance.zoomInTime);
    }

    void ThrowAgainPressedEventHandler()
    {
        float smoothTime = GlobalSettings.Instance.zoomOutTime;
        StopAllCoroutines();
        StartCoroutine(ResetCamera(smoothTime));
    }

    void ThrowMorePressedEventHandler()
    {
        float smoothTime = GlobalSettings.Instance.zoomOutTime;
        StopAllCoroutines();
        StartCoroutine(ResetCamera(smoothTime));
    }

    public void ZoomToDice(float smoothTime)
    {
        Vector3 targetPos = diceLocations[targetDiceIndex].position;
        StartCoroutine(Zoom(targetPos, smoothTime));
    }

    private void NextDicePos()
    {
        if(targetDiceIndex >= diceLocations.Count - 1)
        {
             targetDiceIndex = 0;
        }
        else
        {
            targetDiceIndex++;
        }
    }

    private IEnumerator Zoom(Vector3 targetPos, float smoothTime)
    {
        Vector3 startPosition = transform.position;
        Vector3 finalPosition = targetPos + offSet;
        Quaternion startRotation = transform.rotation;

        Vector3 relativePos = targetPos - finalPosition;
        Quaternion finalRotation = Quaternion.LookRotation(relativePos);

        float elapsedTime = 0f;

        while (elapsedTime < smoothTime)
        {
            float lerpFactor = Mathf.SmoothStep(0f, 1f, elapsedTime / smoothTime);
            elapsedTime += Time.deltaTime;

            transform.position = Vector3.Lerp(startPosition, finalPosition, lerpFactor);
            transform.rotation = Quaternion.Slerp(startRotation, finalRotation, lerpFactor);

            yield return null;
        }
        yield return new WaitForSeconds(GlobalSettings.Instance.cameraStayOnDiceTime);
        NextDicePos();
        ZoomToDice(GlobalSettings.Instance.cameraMoveToNextDiceTime);
    }

    private IEnumerator ResetCamera(float smoothTime)
    {
        Vector3 startPosition = transform.position;
        Vector3 finalPosition = originalPos;
        Quaternion startRotation = transform.rotation;
        Quaternion finalRotation = originalRot;

        float elapsedTime = 0f;

        while (elapsedTime < smoothTime)
        {
            float lerpFactor = Mathf.SmoothStep(0f, 1f, elapsedTime / smoothTime);
            elapsedTime += Time.deltaTime;

            transform.position = Vector3.Lerp(startPosition, finalPosition, lerpFactor);
            transform.rotation = Quaternion.Slerp(startRotation, finalRotation, lerpFactor);

            yield return null;
        }
    }

    private IEnumerator ResetThrow(float delay, bool resetScore)
    {
        uiManager.ShowScore(true);
        yield return new WaitForSeconds(delay);
        uiManager.ShowThrow();
    }

    private Vector3 GetMeanVector(List<DiceScore> diceScores)
    {
        List<Vector3> positions = new List<Vector3>();
        foreach (DiceScore diceScore in diceScores)
        {
            positions.Add(diceScore.transform.position);
        }
        if (positions.Count == 0)
        {
            return Vector3.zero;
        }

        Vector3 meanVector = Vector3.zero;

        foreach (Vector3 pos in positions)
        {
            meanVector += pos;
        }

        return (meanVector / positions.Count);
    }
}
