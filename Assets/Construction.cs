using UnityEngine;
public class Construction : MonoBehaviour
{
    [SerializeField] private int woodNeeded = 10;
    private int currentWood = 0;

    public void AddProgress()
    {
        currentWood++;
        Debug.Log($"Construction Progress: {currentWood}/{woodNeeded}");

        if (currentWood >= woodNeeded)
        {
            CompleteConstruction();
        }
    }

    public bool IsFull()
    {
        return currentWood >= woodNeeded; 
    }

    void CompleteConstruction()
    {
        Debug.Log("Construction Completed!");
    }
}
