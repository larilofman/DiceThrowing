using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetArea : MonoBehaviour
{
    public Transform resetLocation;

    private void OnTriggerEnter(Collider other)
    {
        other.transform.position = resetLocation.position;
    }
}
