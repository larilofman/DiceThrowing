using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public struct DiceResult
{
    public int result;
    public string type;
    public int typeN;

    public DiceResult(int result, string type)
    {
        this.result = result;
        this.type = type;
        this.typeN = int.Parse(Regex.Replace(type, "[^0-9]", ""));
    }
}
