using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class FoundObject : MonoBehaviour
{
    [SerializeField]
    private bool hasLineOfSight = false;
    [SerializeField]
    private float distanceToInteract = 2f;
    private GameObject interactable;
    
    private HumanoidMovement humanoidMovement;
    private Animator animator;
    private Coroutine choppingCoroutine;

    void Awake()
    {
        interactable = GameObject.FindGameObjectWithTag(StringManager.tagInteractable);
        humanoidMovement = GetComponent<HumanoidMovement>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (humanoidMovement == null || humanoidMovement.IsMoving)  
        {
            StopChopping();
            return;
        }

        FindNearestInteractable();

        if (interactable == null) return;

        float distance = Vector2.Distance(transform.position, interactable.transform.position);
        int layerMask = ~LayerMask.GetMask("Humanoid");

        if (distance <= distanceToInteract) 
        {
            RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, interactable.transform.position - transform.position, Mathf.Infinity, layerMask);
            if (raycastHit2D.collider != null)
            {
                hasLineOfSight = raycastHit2D.collider.CompareTag("Interactable");
                if (hasLineOfSight)
                {
                    Debug.DrawRay(transform.position, interactable.transform.position - transform.position, Color.green);
                    AutoInteract();
                }
                else
                {
                    Debug.DrawRay(transform.position, interactable.transform.position - transform.position, Color.red);
                    StopChopping();
                }
            }
        }
        else
        {
            StopChopping();
        }
    }

     void FindNearestInteractable()
    {
        GameObject[] interactables = GameObject.FindGameObjectsWithTag("Interactable");
        float minDistance = Mathf.Infinity;
        GameObject nearestObject = null;

        foreach (GameObject obj in interactables)
        {
            float distance = Vector2.Distance(transform.position, obj.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestObject = obj;
            }
        }

        interactable = nearestObject; 
    }


    void AutoInteract()
    {
        if (interactable == null || interactable.GetComponent<Tree>() == null) return;

        animator.SetBool("isChopping", true);

        if (choppingCoroutine == null)
        {
            choppingCoroutine = StartCoroutine(ChopTree(interactable.GetComponent<Tree>()));
        }
    }

    IEnumerator ChopTree(Tree tree)
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            tree.SpawnWood();
        }
    }

    void StopChopping()
    {
        if (choppingCoroutine != null)
        {
            StopCoroutine(choppingCoroutine);
            choppingCoroutine = null;
        }
        animator.SetBool("isChopping", false);
    }
    

}
