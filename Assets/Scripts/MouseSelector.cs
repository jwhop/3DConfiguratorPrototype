using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSelector : MonoBehaviour
{
    public static MouseSelector Instance { get; private set; }
    private bool startedSelection;
    public bool canHoverSelection;
    private LineRenderer lr;
    private int positionsCount;
    public Vector3 currentLastPos;
    private List<Vector2> PointPositions;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        startedSelection = false;
        lr = GetComponent<LineRenderer>();
        positionsCount = 0;
        lr.positionCount = 0;
        currentLastPos = Vector3.zero;
        canHoverSelection = false;
        PointPositions = new List<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        if (startedSelection)
        {
            Vector3 pt = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 adjustedPt = new Vector3(pt.x, 0, pt.z);
            float angle = Mathf.Atan2(pt.z - currentLastPos.z, pt.x - currentLastPos.x) * 180 / Mathf.PI;
            angle += 180f;
            if((angle > 0 && angle < 45) || (angle > 135 && angle < 225) || (angle > 315 && angle < 361))
            {
                lr.SetPosition(positionsCount - 1, new Vector3(currentLastPos.x + (adjustedPt.x - currentLastPos.x), 0, currentLastPos.z));
            }
            else
            {
                lr.SetPosition(positionsCount - 1, new Vector3(currentLastPos.x, 0, currentLastPos.z + (adjustedPt.z - currentLastPos.z)));
            }

        }
    }

    public void StartSelection(Vector3 pos)
    {
        startedSelection = true;
        lr.positionCount = 2;
        lr.SetPosition(0, pos);
        positionsCount = 2;
        currentLastPos = pos;
        PointPositions.Add(new Vector2(pos.x, pos.z));
    }

    public void ContinueSelection(Vector3 pos)
    {
        positionsCount++;
        lr.positionCount = positionsCount;
        lr.SetPosition(positionsCount - 2, pos);
        currentLastPos = pos;
        PointPositions.Add(new Vector2(pos.x, pos.z));
        print(PointPositions);
    }

    public bool GetStartedSelection()
    {
        return startedSelection;
    }

    public void GenerateMeshFromPath()
    {
        List<Vector3> vertices = new List<Vector3>();
        for (int i = 1; i < PointPositions.Count; i++)
        {
            float offsetX = PointPositions[i - 1].x == PointPositions[i].x ? -0.25f : 0f;
            float offsetZ = PointPositions[i - 1].y == PointPositions[i].y ? -0.25f : 0f;
            vertices.Add(new Vector3(PointPositions[i-1].x + offsetX, 0, PointPositions[i-1].y + offsetZ));
            vertices.Add(new Vector3(PointPositions[i].x + offsetX, 0, PointPositions[i].y + offsetZ));
            vertices.Add(new Vector3(PointPositions[i - 1].x + offsetX, 2, PointPositions[i - 1].y + offsetZ));
        }
    }


}
