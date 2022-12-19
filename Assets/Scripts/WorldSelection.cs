using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorldSelection : MonoBehaviour
{
    public Transform worldParent;
    public TMP_Dropdown dropdown;
    private List<GameObject> worldList = new List<GameObject>();
    private List<string> worldNames = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform world in worldParent)
        {
            worldList.Add(world.gameObject);
            worldNames.Add(world.name);
        }

        dropdown.AddOptions(worldNames);
        dropdown.onValueChanged.AddListener(ChangeWorld);
    }

    void ChangeWorld(int worldID)
    {
        foreach (GameObject world in worldList)
        {
            if (world.transform.GetSiblingIndex() == worldID)
            {
                world.SetActive(true);
            }
            else
            {
                world.SetActive(false);
            }
        }
    }
}
