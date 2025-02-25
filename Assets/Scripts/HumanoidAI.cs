using UnityEngine;

public class HumanoidAI : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 3f;
    private Vector3? humanoidDestination;
    private Vector3? Destination => humanoidDestination;
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if(Destination.HasValue)
        {
            var direction = humanoidDestination.Value - transform.position;
            transform.position += direction.normalized * Time.deltaTime * movementSpeed;

            var distanceToDestination = Vector3.Distance(transform.position, humanoidDestination.Value);
            if(distanceToDestination < 0.3f)
            {
                humanoidDestination = null;
                animator.SetBool(StringManager.isMoving,false);
            }
        }
    }

    public void SetDestination(Vector3 destination)
    {
        humanoidDestination = destination;
        animator.SetBool(StringManager.isMoving, true);
    }
}
