using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField]
    public HumanoidAI aI;
    private SpriteRenderer spriteRenderer;
    
    public bool IsMoving;

    protected Animator animator;
    private GameObject selectedGameObject;

    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        selectedGameObject = transform.Find("Selected").gameObject;
        SetSelectedVisible(false);
    }
    public void MoveTo(Vector3 destination)
    {
        var direction = (destination - transform.position).normalized;
        spriteRenderer.flipX = direction.x < 0;
        aI.SetDestination(destination);
    }

    public void SetSelectedVisible(bool visible)
    {
        selectedGameObject.SetActive(visible);
    }
    
}
