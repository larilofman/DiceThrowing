using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CamMover : MonoBehaviour
{
    public Vector3 offSet;
    public float moveDuration;
    public UIManager uiManager;
    public DiceSetup diceSetup;
    public ScoreManager scoreManager;
    public DiceThrower diceThrower;
    private Vector3 originalPos;
    private Quaternion originalRot;

    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;
        originalRot = transform.rotation;
    }

    public void ZoomToDice(Vector3 targetPos)
    {
        float smoothTime = moveDuration;
        StartCoroutine(Zoom(targetPos, smoothTime));
        StartCoroutine(ShowScore(smoothTime));
    }

    public void Reset(bool resetScore)
    {
        float smoothTime = moveDuration * 0.50f;
        StartCoroutine(ResetCamera(smoothTime));
        StartCoroutine(ResetThrow(smoothTime * 1.25f, resetScore));
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

        diceThrower.StoreDice();
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

    private IEnumerator ShowScore(float delay)
    {
        yield return new WaitForSeconds(delay);
        uiManager.ShowScore();
        uiManager.ShowRethrow();
    }

    private IEnumerator ResetThrow(float delay, bool resetScore)
    {
        uiManager.ShowScore(true);
        yield return new WaitForSeconds(delay);
        uiManager.ShowThrow();
        if (resetScore)
        {
            scoreManager.ResetScore();
            diceThrower.ClearTable();
        }
        diceSetup.PrepaceDice();
    }
}
