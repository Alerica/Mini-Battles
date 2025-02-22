using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private PointToClick pointToClickPrefabs;

    private Vector2 InitialTouchPosition;

    void Update()
    {
        Vector2 inputPosition = Input.touchCount > 0 ? Input.GetTouch(0).position : Input.mousePosition; 

        if(Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)) 
        {
            InitialTouchPosition = inputPosition;
        } 

        if(Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            if(Vector2.Distance(InitialTouchPosition, inputPosition) < 10)
            {
                DetectClick(inputPosition);
            }
        }
    }

    void DetectClick(Vector2 inputPosition)
    {
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(inputPosition);
        HandleClickOnGround(worldPoint);
    }

    void HandleClickOnGround(Vector2 worldPoint)
    {
        Instantiate(pointToClickPrefabs, worldPoint, Quaternion.identity);
    }
}
