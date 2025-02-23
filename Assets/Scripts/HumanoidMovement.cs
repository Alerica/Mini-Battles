using UnityEngine;

public class HumanoidMovement : Unit
{
     protected Vector2 velocity;
     protected Vector3 lastPosition;
     public float CurrentSpeed => velocity.magnitude;

    void Update()
    {
        velocity = new Vector2(
            (transform.position.x - lastPosition.x),
            (transform.position.y - lastPosition.y)
        ) / Time.deltaTime;

        lastPosition = transform.position;
        IsMoving = velocity.magnitude > 0;
        
    }
}
