using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPoint : MonoBehaviour
{
    [SerializeField] Material isSelected, nonSelected;
    private MeshRenderer mr;
    private bool selected;
    // Start is called before the first frame update
    void Start()
    {
        mr = GetComponent<MeshRenderer>();
        selected = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnMouseOver()
    {
        if (!selected)
        {
            if(MouseSelector.Instance.GetStartedSelection())
            {
                if ((MouseSelector.Instance.currentLastPos.x == transform.position.x || MouseSelector.Instance.currentLastPos.z == transform.position.z))
                {
                    MouseSelector.Instance.canHoverSelection = true;
                    gameObject.layer = 7;
                }
                else
                {
                    MouseSelector.Instance.canHoverSelection = false;
                }
            }
            mr.material = isSelected;
        }
    }

    void OnMouseExit()
    {
        if (!selected)
        {
            mr.material = nonSelected;
            MouseSelector.Instance.canHoverSelection = false;
            gameObject.layer = 0;

        }
    }

    void OnMouseDown()
    {
        if (!selected && (MouseSelector.Instance.currentLastPos == Vector3.zero || (MouseSelector.Instance.currentLastPos.x == transform.position.x || MouseSelector.Instance.currentLastPos.z == transform.position.z)))
        {
            selected = true;
            gameObject.layer = 7;
            mr.material = isSelected;
            if (MouseSelector.Instance.GetStartedSelection())
            {
                MouseSelector.Instance.ContinueSelection(transform.position);
            }
            else
            {
                MouseSelector.Instance.StartSelection(transform.position);
            }
        }
    }
    
}
