using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MouseSelector : MonoBehaviour
{
    public static MouseSelector Instance { get; private set; }
    private bool startedSelection, doorGenerated;
    public bool canHoverSelection;
    private LineRenderer lr;
    private int positionsCount;
    public Transform currentLastPos;
    private List<Vector2> PointPositions;
    [SerializeField] GameObject textCanvas;
    [SerializeField] TextMeshProUGUI text;
    public delegate void OnReset();
    public delegate void OnDrawMesh();
    public static OnReset onReset;
    public static OnDrawMesh onDrawMesh;
    private GameObject hoverDot;
    [SerializeField] Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        startedSelection = false;
        doorGenerated = false;
        lr = GetComponent<LineRenderer>();
        positionsCount = 0;
        lr.positionCount = 0;
        currentLastPos = null;
        canHoverSelection = false;
        PointPositions = new List<Vector2>();
        hoverDot = null;
    }

    // Update is called once per frame
    void Update()
    {
        float camSize = Camera.main.orthographicSize;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            startedSelection = false;
            lr.positionCount = 0;
            textCanvas.GetComponent<RectTransform>().position = new Vector3(100 + 2.5f, 0.25f, 100 + 1f);
            currentLastPos = null;
            PointPositions.Clear();
            onReset?.Invoke();
        }
        if (startedSelection && !doorGenerated)
        {
            /*
            Vector3 pt = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            textCanvas.GetComponent<RectTransform>().position = new Vector3(pt.x + 2.5f, 0.25f, pt.z + 1f);
            Vector3 adjustedPt = new Vector3(pt.x, 0, pt.z);
            float angle = Mathf.Atan2(pt.z - currentLastPos.z, pt.x - currentLastPos.x) * 180 / Mathf.PI;
            angle += 180f;
            if((angle > 0 && angle < 45) || (angle > 135 && angle < 225) || (angle > 315 && angle < 361))
            {
                lr.SetPosition(positionsCount - 1, new Vector3(currentLastPos.x + (adjustedPt.x - currentLastPos.x), 0, currentLastPos.z));
                text.text = (1000 * Vector2.Distance(new Vector2(currentLastPos.x, currentLastPos.z), new Vector2(currentLastPos.x + (adjustedPt.x - currentLastPos.x), currentLastPos.z))).ToString().Split('.')[0];
            }
            else
            {
                lr.SetPosition(positionsCount - 1, new Vector3(currentLastPos.x, 0, currentLastPos.z + (adjustedPt.z - currentLastPos.z)));
                text.text = (1000 * Vector2.Distance(new Vector2(currentLastPos.x, currentLastPos.z), new Vector2(currentLastPos.x, currentLastPos.z + (adjustedPt.z - currentLastPos.z)))).ToString().Split('.')[0];
            }
            */
            if(hoverDot != null)
            {
                lr.SetPosition(positionsCount - 1, hoverDot.transform.position);
                textCanvas.GetComponent<RectTransform>().position = new Vector3(hoverDot.transform.position.x + 1f, 0.25f, hoverDot.transform.position.z + 1f);
                text.text = (1000 * Vector2.Distance(new Vector2(currentLastPos.position.x, currentLastPos.position.z), new Vector2(hoverDot.transform.position.x, hoverDot.transform.position.z))).ToString().Split('.')[0];

            }
            else
            {
                Debug.Log("no hover dot", gameObject);
                lr.SetPosition(positionsCount - 1, currentLastPos.position);
                textCanvas.GetComponent<RectTransform>().position = new Vector3(100 + 2.5f, 0.25f, 100 + 1f);
            }


        }
        Camera.main.orthographicSize = Mathf.Clamp(camSize + -Input.mouseScrollDelta.y, 2.5f, 15f);
    }

    public void StartSelection(Transform pos)
    {
        startedSelection = true;
        lr.positionCount = 2;
        lr.SetPosition(0, pos.position);
        positionsCount = 2;
        currentLastPos = pos;
        PointPositions.Add(new Vector2(pos.position.x, pos.position.z));
    }

    public void ContinueSelection(Transform tr)
    {
        float inc = SpawnPoints.Instance.GetIncrement();
        inc /= 1000;

        float prevInc = SpawnPoints.Instance.GetPrevIncrement();
        prevInc  = 1f;


        if (tr.position.x == currentLastPos.position.x) // change in z
        {
            int direction = currentLastPos.position.z > tr.position.z ? 1 : -1;
            int distance = Mathf.Abs(tr.parent.GetSiblingIndex() - currentLastPos.parent.GetSiblingIndex());
            for (int i = distance - 1; i > 0; i--)
            {
                positionsCount++;
                lr.positionCount = positionsCount;
                Transform intermediatePt = tr.parent.parent.GetChild(tr.parent.GetSiblingIndex() + i * direction).GetChild(tr.GetSiblingIndex());
                lr.SetPosition(positionsCount - 2, intermediatePt.position);
                PointPositions.Add(new Vector2(tr.position.x, intermediatePt.position.z));
                intermediatePt.gameObject.GetComponent<GridPoint>().SelectPoint();
            }
        }
        else // change in x
        {
            int direction = currentLastPos.position.x > tr.position.x ? 1 : -1;
            int distance = Mathf.Abs(tr.GetSiblingIndex() - currentLastPos.GetSiblingIndex());

            for (int i = distance - 1; i > 0; i--)
            {
                positionsCount++;
                lr.positionCount = positionsCount;
                Transform intermediatePt = tr.parent.GetChild(tr.GetSiblingIndex() + i * direction);
                lr.SetPosition(positionsCount - 2, intermediatePt.position);
                PointPositions.Add(new Vector2(intermediatePt.position.x, tr.position.z));
                intermediatePt.gameObject.GetComponent<GridPoint>().SelectPoint();
            }
        }
        positionsCount++;
        lr.positionCount = positionsCount;
        lr.SetPosition(positionsCount - 2, tr.position);
        currentLastPos = tr;
        PointPositions.Add(new Vector2(tr.position.x, tr.position.z));
    }

    public void SetHoverDot(GameObject g)
    {
        hoverDot = g;
    }
    public bool GetStartedSelection()
    {
        return startedSelection;
    }

    public bool GetDoorGenerated()
    {
        return doorGenerated;
    }

    public void RedrawLines()
    {
        float inc = SpawnPoints.Instance.GetIncrement();
        inc /= 1000f;
        
        float prevInc = SpawnPoints.Instance.GetPrevIncrement();
        prevInc /= 1000f;
        
        for (int i = 0; i < lr.positionCount; i++)
        {
            Vector3 OldPosition = lr.GetPosition(i);
            lr.SetPosition(i, (inc/prevInc) * OldPosition);
        }
        for (int i = 0; i < PointPositions.Count; i++)
        {
            PointPositions[i] *= (inc / prevInc);
        }
        //currentLastPos.position *= (inc/prevInc);
    }

    public void GenerateDoor()
    {
        if(PointPositions.Count > 1)
        {
            doorGenerated = true;
            lr.positionCount = 0;
            currentLastPos = null;
            onDrawMesh?.Invoke();
            startedSelection = false;
            MeshGeneration.Instance.GenerateDoor(PointPositions);
            slider.value = 1;
        }
        
    }

}
