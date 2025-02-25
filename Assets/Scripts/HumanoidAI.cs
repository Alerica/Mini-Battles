using UnityEngine;

public class HumanoidAI : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 3f;
    private Vector3? humanoidDestination;
    private Vector3? Destination => humanoidDestination;
    private Animator animator;
    private GameObject woodGameObject;
    private GameObject woodGameObject1;
    private GameObject woodGameObject2;

    void Awake()
    {
        animator = GetComponent<Animator>();
        woodGameObject = transform.Find("Wood_0").gameObject;
        woodGameObject1 = transform.Find("Wood_1").gameObject;
        woodGameObject2 = transform.Find("Wood_2").gameObject;
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

    public void SetWoodVisible(bool visible)
    {
        woodGameObject.SetActive(visible);
        woodGameObject1.SetActive(visible);
        woodGameObject2.SetActive(visible);
    }
}
