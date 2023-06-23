using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class MeshGeneration : MonoBehaviour
{
    public static MeshGeneration Instance { get; private set; }
    [SerializeField] private Material doorMaterial;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateDoor(List<Vector2> pts) 
    {
        for (int i = 0; i < pts.Count; i++)
        {
            Vector3 size = new Vector3(1, 1, 0.1f);

            //if end, make it half the length
            if (i == 0 || i == pts.Count - 1)
            {
                size.x = 0.5f;
            }
            else
            {
                //if corner, make two segments
                if ((pts[i].x == pts[i + 1].x && pts[i].y == pts[i-1].y) ||
                    (pts[i].y == pts[i + 1].y && pts[i].x == pts[i - 1].x))
                {
                    size.x = 0.5f;
                }
            }
            
            ProBuilderMesh door = ShapeGenerator.GenerateCube(PivotLocation.Center, size);
            door.gameObject.transform.position = new Vector3(pts[i].x, 1f, pts[i].y);
            
            //if vertical piece, rotate 90 degrees
            if(i != pts.Count -1 && pts[i].x == pts[i + 1].x)
            {
                door.gameObject.transform.Rotate(Vector3.up, 90f);
            }
            else if (i == pts.Count - 1 && pts[i].x == pts[i - 1].x)
            {
                door.gameObject.transform.Rotate(Vector3.up, 90f);
            }

            if (size.x == 0.5)
            {
                Vector2 neighbor = i == pts.Count - 1 ? pts[i - 1] : pts[i + 1];
                int direction;
                if(pts[i].x == neighbor.x)
                {
                    direction = pts[i].y < neighbor.y ? -1 : 1;
                }
                else
                {
                    direction = pts[i].x < neighbor.x ? 1 : -1;
                }
                door.gameObject.transform.Translate(new Vector3(0.25f * direction, 0, 0), Space.Self);
                //if not first or last, it's a corner
                if(i != 0 && i != pts.Count - 1)
                {
                    ProBuilderMesh otherDoor = ShapeGenerator.GenerateCube(PivotLocation.Center, size);
                    otherDoor.gameObject.transform.position = new Vector3(pts[i].x, 1f, pts[i].y);

                    otherDoor.gameObject.transform.rotation = door.gameObject.transform.rotation;
                    otherDoor.gameObject.transform.Rotate(Vector3.up, -90f);
                    int secondaryDirection;
                    Vector2 otherNeighbor = pts[i - 1];
                    if (pts[i].x == otherNeighbor.x)
                    {
                        secondaryDirection = pts[i].y < otherNeighbor.y ? 1 : -1;
                    }
                    else
                    {
                        secondaryDirection = pts[i].x < otherNeighbor.x ? 1 : -1;
                    }
                    otherDoor.gameObject.transform.Translate(new Vector3(0.25f * secondaryDirection, 0, 0), Space.Self);
                    otherDoor.SetMaterial(otherDoor.faces, doorMaterial);

                }
            }
            
            door.SetMaterial(door.faces, doorMaterial);
        }
        
    }
}
