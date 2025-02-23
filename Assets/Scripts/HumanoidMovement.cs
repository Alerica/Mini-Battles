using UnityEngine;

public class HumanoidMovement : Unit
{
    protected Vector2 velocity;
    protected Vector3 lastPosition;
    private float stuckCheckTimer = 0f;
    private float stuckThreshold = 0.5f; 
    private Vector3 lastCheckedPosition;

    public float CurrentSpeed => velocity.magnitude;

    void Start()
    {
        lastCheckedPosition = transform.position; 
    }

    void Update()
    {
        velocity = new Vector2(
            (transform.position.x - lastPosition.x),
            (transform.position.y - lastPosition.y)
        ) / Time.deltaTime;

        IsMoving = velocity.magnitude > 0.01f; 

        stuckCheckTimer += Time.deltaTime;
        if (stuckCheckTimer >= stuckThreshold)
        {
            CheckIfStuck();
            stuckCheckTimer = 0f; 
        }

        lastPosition = transform.position;
    }

    private void CheckIfStuck()
    {
        if (Vector3.Distance(transform.position, lastCheckedPosition) < 0.1f) 
        {
            Debug.Log("Stopping movement.");
            StopMoving();
        }

        lastCheckedPosition = transform.position; 
    }

    private void StopMoving()
    {
        velocity = Vector2.zero;
        IsMoving = false;

        if (aI != null)
        {
            aI.SetDestination(transform.position); 
        }
    }
}
