using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

public class UnitManager : MonoBehaviour
{
    private Vector3 startPosition;
    private List<Unit> selectedUnitList;
    private List<Tree> selectedTreeList;

    [SerializeField]
    private Transform selectionAreaTransform;


    private AudioSource audioSource;

    [SerializeField]
    private AudioClip selectSound;  
    [SerializeField]
    private AudioClip deselectSound;

    private void Awake()
    {
        selectionAreaTransform.gameObject.SetActive(false);
        selectedUnitList = new List<Unit>();
        selectedTreeList = new List<Tree>();
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            selectionAreaTransform.gameObject.SetActive(true);
            startPosition = GetWorldPosition();
        }

        if(Input.GetMouseButton(0)) 
        {
            Vector3 currentMountPosition = GetWorldPosition();
            Vector3 lowerLeft = new Vector3(
                Mathf.Min(startPosition.x, currentMountPosition.x),
                Mathf.Min(startPosition.y, currentMountPosition.y)
            );
            Vector3 upperRight = new Vector3(
                Mathf.Max(startPosition.x, currentMountPosition.x),
                Mathf.Max(startPosition.y, currentMountPosition.y)
            ); 
            selectionAreaTransform.position = lowerLeft;
            selectionAreaTransform.localScale = upperRight - lowerLeft;
        }


        if(Input.GetMouseButtonUp(0))
        {
            selectionAreaTransform.gameObject.SetActive(false);
            Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(startPosition, GetWorldPosition());

            bool hadSelection = selectedUnitList.Count > 0 || selectedTreeList.Count > 0; //

            foreach(Unit unit in selectedUnitList)
            {
                if(unit != null)
                {
                    unit.MoveTo(GetWorldPosition());
                }
                unit.SetSelectedVisible(false);
            }

            foreach(Tree tree in selectedTreeList)
            {
                tree.SetSelectedVisible(false);
            }

            selectedUnitList.Clear();
            selectedTreeList.Clear();

            bool hasNewSelection = false; 

             
            foreach(Collider2D collider2D in collider2DArray)
            {
                Unit unit = collider2D.GetComponent<Unit>();
                Tree tree = collider2D.GetComponent<Tree>();
                if(unit != null)
                {
                    unit.SetSelectedVisible(true);
                    selectedUnitList.Add(unit);
                    hasNewSelection = true;
                }

                if(tree != null)
                {
                    tree.SetSelectedVisible(true);
                    selectedTreeList.Add(tree);
                }
            }

            if (hasNewSelection && selectSound != null)
            {
                audioSource.PlayOneShot(selectSound);
            }
            else if (hadSelection && deselectSound != null)
            {
                audioSource.PlayOneShot(deselectSound);
            }

            Debug.Log("Tree: " + selectedTreeList.Count);
            Debug.Log("Unit: " + selectedUnitList.Count);
        }
    }

    private Vector2 GetWorldPosition()
    {
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return worldPoint;
    }
}
