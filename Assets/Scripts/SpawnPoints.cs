using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    [SerializeField] float xOffset, zOffset;
    [SerializeField] GameObject gridPoint;
    // Start is called before the first frame update
    void Start()
    {
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
