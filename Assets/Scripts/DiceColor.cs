using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceColor : MonoBehaviour
{
    public List<Color> colors;
    private Renderer diceRenderer;
    // Start is called before the first frame update
    void Start()
    {
        diceRenderer = transform.GetChild(1).GetComponent<Renderer>();
        AddPrefabColor();
        SetRandomColor();
    }

    void AddPrefabColor()
    {
        colors.Add(diceRenderer.material.color);
    }

    void SetRandomColor()
    {
        int index = Random.Range(0, colors.Count);
        diceRenderer.material.SetColor("_Color", colors[index]);
    }
}
