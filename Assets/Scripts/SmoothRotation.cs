using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothRotation : MonoBehaviour
{
    float maxSpeed = 0.3f;
    float maxAngle = 120f;
    float minSpeed = 0.05f;
    float minAngle = 15f;

    private float speed = 0f;
    private float RotAngleX = 0;
    private float RotAngleY = 0;
    private float RotAngleZ = 0;
    private DiceScore diceScore;

    private void Start()
    {
        diceScore = GetComponent<DiceScore>();
        speed = Random.Range(minSpeed, maxSpeed);
        RotAngleX = Random.Range(minAngle, maxAngle);
        RotAngleY = Random.Range(minAngle, maxAngle);
        RotAngleZ = Random.Range(minAngle, maxAngle);
    }

    void Update()
    {
        if(diceScore && diceScore.thrown)
        {
            Destroy(this);
        }

        float rX = Mathf.SmoothStep(0, RotAngleX, Mathf.PingPong(Time.time * speed, 1));
        float rY = Mathf.SmoothStep(0, RotAngleY, Mathf.PingPong(Time.time * speed, 1));
        float rZ = Mathf.SmoothStep(0, RotAngleZ, Mathf.PingPong(Time.time * speed, 1));
        transform.rotation = Quaternion.Euler(rX, rY, rZ);
    }
}
