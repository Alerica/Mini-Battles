using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField]
    protected HumanoidAI aI;
    private SpriteRenderer spriteRenderer;
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
