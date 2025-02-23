using Unity.VisualScripting;
using UnityEngine;

public class Tree : MonoBehaviour
{
    [SerializeField]
    private GameObject woodPrefabs;
    private GameObject selectedGameObject;
    

    public void SpawnWood()
    {
        Instantiate(woodPrefabs, transform.position, Quaternion.identity);
        
    }

    void Awake()
    {
        selectedGameObject = transform.Find("Selected").gameObject;
        SetSelectedVisible(false);
    }

    public void SetSelectedVisible(bool visible)
    {
        selectedGameObject.SetActive(visible);
    }
}
