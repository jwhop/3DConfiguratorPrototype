using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

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

    private void OnEnable()
    {
        MouseSelector.onReset += _OnResetPoint;
    }
    private void OnDisable()
    {
        MouseSelector.onReset -= _OnResetPoint;
    }

    void OnMouseOver()
    {
        if (!selected && !EventSystem.current.IsPointerOverGameObject())
        {
            if(MouseSelector.Instance.GetStartedSelection())
            {

                if (((MouseSelector.Instance.currentLastPos.position.x == transform.position.x) ||
                    ( MouseSelector.Instance.currentLastPos.position.z == transform.position.z)))
                {
                    float inc = SpawnPoints.Instance.GetIncrement();
                    inc /= 1000f;

                    float prevInc = SpawnPoints.Instance.GetPrevIncrement();
                    prevInc /= 1000f;
                    prevInc = 1f;


                    bool valid = true;
                    if (MouseSelector.Instance.currentLastPos.position.x == transform.position.x)
                    {
                        
                        int direction = MouseSelector.Instance.currentLastPos.position.z > transform.position.z ? 1 : -1;
                        int distance = Mathf.Abs(transform.parent.GetSiblingIndex() - MouseSelector.Instance.currentLastPos.parent.GetSiblingIndex());

                        for (int i = distance - 1; i > 0; i--)
                        {
                            GameObject prevPoint = transform.parent.parent.GetChild(transform.parent.GetSiblingIndex() + i * direction).GetChild(transform.GetSiblingIndex()).gameObject;
                            if (prevPoint.GetComponent<GridPoint>().selected)
                            {
                                valid = false;
                                break;
                            }
                        }
                        
                    }
                    else
                    {
                        if (MouseSelector.Instance.currentLastPos.position.z == transform.position.z)
                        {

                            int direction = MouseSelector.Instance.currentLastPos.position.x > transform.position.x ? 1 : -1;
                            int distance = Mathf.Abs(transform.GetSiblingIndex() - MouseSelector.Instance.currentLastPos.GetSiblingIndex());

                            for (int i = distance - 1; i > 0; i--)
                            {
                                GameObject prevPoint = transform.parent.GetChild(transform.GetSiblingIndex() + i * direction).gameObject;
                                if (prevPoint.GetComponent<GridPoint>().selected)
                                {
                                    valid = false;
                                    break;
                                }
                            }

                        }
                    }

                    if (valid)
                    {
                        MouseSelector.Instance.canHoverSelection = true;
                        gameObject.layer = 7;
                        MouseSelector.Instance.SetHoverDot(gameObject);
                    }
                    else
                    {
                        MouseSelector.Instance.canHoverSelection = false;
                        MouseSelector.Instance.SetHoverDot(null);
                    }

                }
                else
                {
                    MouseSelector.Instance.canHoverSelection = false;
                    MouseSelector.Instance.SetHoverDot(null);
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
        if (!selected && !EventSystem.current.IsPointerOverGameObject() && 
            (MouseSelector.Instance.currentLastPos == null || 
            (MouseSelector.Instance.currentLastPos.position.x == transform.position.x || 
            MouseSelector.Instance.currentLastPos.position.z == transform.position.z
            )))
        {
            SelectPoint();
            
            if (MouseSelector.Instance.GetStartedSelection())
            {
                MouseSelector.Instance.ContinueSelection(transform);
            }
            else
            {
                MouseSelector.Instance.StartSelection(transform);
            }
        }
    }
    public void SelectPoint()
    {
        selected = true;
        gameObject.layer = 7;
        mr.material = isSelected;
    }
    void _OnResetPoint()
    {
        selected = false;
        mr.material = nonSelected;
        gameObject.layer = 0;
    }
}
