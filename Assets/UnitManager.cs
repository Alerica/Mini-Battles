using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

public class UnitManager : MonoBehaviour
{
    private Vector3 startPosition;
    private List<Unit> selectedUnitList;

    private void Awake()
    {
        selectedUnitList = new List<Unit>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            startPosition = GetWorldPosition();
        }
        if(Input.GetMouseButtonUp(0))
        {
            Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(startPosition, GetWorldPosition());

            foreach(Unit unit in selectedUnitList)
            {
                if(unit != null)
                {
                    unit.MoveTo(GetWorldPosition());
                }
                unit.SetSelectedVisible(false);
            }

            selectedUnitList.Clear();

            foreach(Collider2D collider2D in collider2DArray)
            {
                Unit unit = collider2D.GetComponent<Unit>();
                if(unit != null)
                {
                    unit.SetSelectedVisible(true);
                    selectedUnitList.Add(unit);
                }
            }

            Debug.Log("" + selectedUnitList.Count);
        }
    }

    private Vector2 GetWorldPosition()
    {
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return worldPoint;
    }
}
