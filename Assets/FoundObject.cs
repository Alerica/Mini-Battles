using UnityEngine;
using System.Collections;

public class FoundObject : MonoBehaviour
{
    [SerializeField] private float distanceToInteract = 0.5f;
    [SerializeField] protected HumanoidAI aI;
    private HumanoidMovement humanoidMovement;
    private Animator animator;
    private bool isChopping = false;
    private Vector3 lastTreePosition;
    private bool isReturningToTownHall = false; 

    void Awake()
    {
        humanoidMovement = GetComponent<HumanoidMovement>();
        animator = GetComponent<Animator>();

        if (aI == null)
        {
            Debug.LogError("HumanoidAI reference missing!");
        }
    }

    void FixedUpdate()
    {
        if (!humanoidMovement.IsMoving && !isChopping && !isReturningToTownHall)
        {
            CheckForTree();
        }
    }

    private void CheckForTree()
    {
        int treeLayer = LayerMask.NameToLayer(StringManager.layerTree);
        Collider2D treeCollider = Physics2D.OverlapCircle(transform.position, distanceToInteract, 1 << treeLayer);

        if (treeCollider != null)
        {
            lastTreePosition = treeCollider.transform.position - new Vector3(0.5f, 0 ,0); // To offset 
            Debug.Log("Tree found! Starting chopping.");
            StartCoroutine(StartChopping());
        }
        else
        {
            Debug.Log("No tree found nearby.");
        }
    }

    private IEnumerator StartChopping()
    {
        isChopping = true;
        animator.SetBool("isChopping", true);
        Debug.Log("Chopping tree...");

        yield return new WaitForSeconds(5f);

        StopChopping();
    }

    private void StopChopping()
    {
        animator.SetBool("isChopping", false);

        GameObject townHall = GameObject.FindWithTag(StringManager.tagTownHall);
        if (townHall != null)
        {
            Debug.Log("Returning to Town Hall.");
            isReturningToTownHall = true; 
            humanoidMovement.MoveTo(townHall.transform.position); 

            StartCoroutine(ReturnToTree());
        }
        else
        {
            Debug.LogError("No Town Hall found! Check the tag.");
            isChopping = false; 
        }
    }

    private IEnumerator ReturnToTree()
    {
        yield return new WaitUntil(() => !humanoidMovement.IsMoving); 

        Debug.Log("Reached Town Hall, now returning to the tree.");
        isReturningToTownHall = false; 

        yield return new WaitForSeconds(2f); 
        humanoidMovement.MoveTo(lastTreePosition);

        yield return new WaitUntil(() => !humanoidMovement.IsMoving); 

        yield return new WaitForSeconds(1f); 

        Debug.Log("Back at the tree, restarting chopping.");
        CheckForTree(); 
    }
}
