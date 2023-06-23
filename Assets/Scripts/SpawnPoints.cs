using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    public static SpawnPoints Instance { get; private set; }
    [SerializeField] float xOffset, zOffset;
    [SerializeField] GameObject gridPoint;
    private float increment, prevIncrement;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        for (int i = 0; i < 30; i++)
        {
            GameObject row = new GameObject("row");
            row.transform.parent = transform;
            for (int j = 0; j < 60; j++)
            {
                GameObject g = Instantiate(gridPoint);
                g.transform.position = new Vector3(xOffset + j, 0, zOffset + i);
                g.transform.parent = row.transform;
            }
        }
        increment = 1000;
        prevIncrement = 1000;
    }

    public void ReSpawnPoint(string s)
    {
        int f;
        if (int.TryParse(s, out f))
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                for (int j = 0; j < transform.GetChild(i).childCount; j++)
                {
                    GameObject g = transform.GetChild(i).GetChild(j).gameObject;
                    g.transform.position = new Vector3((xOffset + j) * f / 1000, 0, (zOffset + i) * f / 1000);
                }
            }
            prevIncrement = increment;
            increment = f;
            MouseSelector.Instance.RedrawLines();
        }
    }

    public float GetIncrement()
    {
        return increment;
    }
    public float GetPrevIncrement()
    {
        return prevIncrement;
    }
}
