using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Inherited by DiceScore and Bonus
abstract public class Score : MonoBehaviour
{
    abstract public int GetResult();
}
