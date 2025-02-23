using Unity.VisualScripting;
using UnityEngine;
public class Tree : MonoBehaviour
{
    [SerializeField]
    private GameObject woodPrefab;
    private GameObject selectedGameObject;
    private Animator animator;

    public bool IsBeingChopped { get; private set; } = false;

    public void StartChopping()
    {
        IsBeingChopped = true;
        if (animator != null)
        {
            animator.SetBool("isBeingChopped", true);
        }
    }

    public void StopChopping()
    {
        IsBeingChopped = false;
        if (animator != null)
        {
            animator.SetBool("isBeingChopped", false);
        }
    }


    void Awake()
    {
        animator = GetComponent<Animator>();
        selectedGameObject = transform.Find("Selected")?.gameObject;
        SetSelectedVisible(false);
    }

    public void SpawnWood()
    {
        Vector3 spawnPosition = transform.position + new Vector3(0, -1f, 0); 
        Instantiate(woodPrefab, spawnPosition, Quaternion.identity);
    }

    public void SetSelectedVisible(bool visible)
    {
        if (selectedGameObject != null)
        {
            selectedGameObject.SetActive(visible);
        }
    }

}
