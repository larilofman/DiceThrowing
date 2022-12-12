using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DepthOfFieldController : MonoBehaviour
{
    public VolumeProfile volumeProfile;
    public DepthOfField depthOfField;
    public DiceManager diceManager;
    float smoothSpeed = 5f;

    float distance;
    // Start is called before the first frame update
    void Start()
    {
        if (!volumeProfile) throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));
        if (!volumeProfile.TryGet(out depthOfField)) throw new System.NullReferenceException(nameof(depthOfField));
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 diceMean = GetDiceMean();
        distance = Vector3.Distance(transform.position, diceMean);
        float targetStart = distance + 10f;
        float targetEnd = distance + 30f;

        float currStart = Mathf.Lerp(depthOfField.gaussianStart.value, targetStart, smoothSpeed);
        depthOfField.gaussianStart.Override(currStart);

        float currEnd = Mathf.Lerp(depthOfField.gaussianEnd.value, targetEnd, smoothSpeed);
        depthOfField.gaussianEnd.Override(currEnd);
    }

    private Vector3 GetDiceMean()
    {
        List<Vector3> positions = new List<Vector3>();
        foreach (GameObject dice in diceManager.activeDice)
        {
            positions.Add(dice.transform.position);
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
