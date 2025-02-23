using UnityEngine;
using System.Collections;

public class FoundObject : MonoBehaviour
{
    [SerializeField] private bool hasLineOfSight = false;
    [SerializeField] private float distanceToInteract = 3f;

    private GameObject interactable;
    private GameObject targetWood;
    private GameObject targetConstruction;
    private GameObject carriedWood; 
    private bool carryingWood = false;

    private HumanoidMovement humanoidMovement;
    private Animator animator;
    private Coroutine choppingCoroutine;
    private Tree currentTree;

    void Awake()
    {
        humanoidMovement = GetComponent<HumanoidMovement>();
        animator = GetComponent<Animator>();
        StartCoroutine(AutoCollectWood());
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
        int layerMask = ~LayerMask.GetMask(StringManager.layerHumanoid);

        if (distance <= distanceToInteract) 
        {
            RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, interactable.transform.position - transform.position, Mathf.Infinity, layerMask);
            if (raycastHit2D.collider != null)
            {
                hasLineOfSight = raycastHit2D.collider.CompareTag(StringManager.tagInteractable);
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
        GameObject[] woodObjects = GameObject.FindGameObjectsWithTag("Wood");

        float minDistance = Mathf.Infinity;
        GameObject nearestObject = null;

        foreach (GameObject wood in woodObjects)
        {
            Wood woodComponent = wood.GetComponent<Wood>();
            if (woodComponent != null && !woodComponent.IsReserved)
            {
                float distance = Vector2.Distance(transform.position, wood.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestObject = wood;
                }
            }
        }

        if (nearestObject == null)
        {
            foreach (GameObject obj in interactables)
            {
                float distance = Vector2.Distance(transform.position, obj.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestObject = obj;
                }
            }
        }

        interactable = nearestObject; 
    }
    void AutoInteract()
    {
        Tree tree = interactable.GetComponent<Tree>();
        if (tree != null && !tree.IsBeingChopped) 
        {
            tree.StartChopping();
            animator.SetBool(StringManager.isChopping, true);

            if (choppingCoroutine == null)
            {
                choppingCoroutine = StartCoroutine(ChopTree(tree));
                currentTree = tree;
            }
        }
    }

    IEnumerator ChopTree(Tree tree)
{
    while (true)
    {
        yield return new WaitForSeconds(10f); 
        tree.SpawnWood(); 
        FindNearestWood();
        
        if (targetWood != null)
        {
            humanoidMovement.MoveTo(targetWood.transform.position);
            yield break; 
        }
    }
}


    void StopChopping()
    {
        if (choppingCoroutine != null)
        {
            StopCoroutine(choppingCoroutine);
            choppingCoroutine = null;
        }

        animator.SetBool(StringManager.isChopping, false);

        if (currentTree != null)
        {
            currentTree.StopChopping();
            currentTree = null;
        }
    }
    IEnumerator AutoCollectWood()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);

            if (!carryingWood)
            {
                FindNearestWood();
                if (targetWood != null)
                {
                    humanoidMovement.MoveTo(targetWood.transform.position);
                }
            }
        }
    }

    void FindNearestWood()
    {
        GameObject[] woods = GameObject.FindGameObjectsWithTag("Wood");
        float minDistance = Mathf.Infinity;
        GameObject nearest = null;

        foreach (GameObject wood in woods)
        {
            Wood woodComponent = wood.GetComponent<Wood>();
            if (woodComponent != null && !woodComponent.IsReserved)
            {
                float distance = Vector2.Distance(transform.position, wood.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearest = wood;
                }
            }
        }

        if (nearest != null)
        {
            nearest.GetComponent<Wood>().Reserve(); 
            targetWood = nearest;
        }
    }


    void Update()
    {
        if (carryingWood && targetConstruction != null && !humanoidMovement.IsMoving)
        {
            DeliverWood();
        }

        if (!carryingWood && targetWood != null && !humanoidMovement.IsMoving)
        {
            PickUpWood();
        }

        if (carriedWood != null)
        {
            Vector3 offset = new Vector3(0.5f, 0.5f, 0); 
            carriedWood.transform.position = transform.position + offset;
        }
    }

    void PickUpWood()
    {
        if (targetWood == null) return;

        float distance = Vector2.Distance(transform.position, targetWood.transform.position);
        if (distance <= distanceToInteract)
        {
            carriedWood = Instantiate(targetWood, transform.position, Quaternion.identity);
            carriedWood.transform.SetParent(transform);
            carriedWood.GetComponent<Collider2D>().enabled = false; 
            
            Destroy(targetWood); 
            carryingWood = true;

            FindNearestConstruction(); 
            if (targetConstruction != null)
            {
                Debug.Log("Moving to construction site: " + targetConstruction.name);
                humanoidMovement.MoveTo(targetConstruction.transform.position); 
            }
            else
            {
                Debug.LogWarning("No construction site found!");
            }
        }
    }



    void FindNearestConstruction()
    {
        GameObject[] constructions = GameObject.FindGameObjectsWithTag("Construction");
        float minDistance = Mathf.Infinity;
        GameObject nearest = null;

        foreach (GameObject construction in constructions)
        {
            Construction constructionComponent = construction.GetComponent<Construction>();
            
            if (constructionComponent != null && !constructionComponent.IsFull())
            {
                float distance = Vector2.Distance(transform.position, construction.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearest = construction;
                }
            }
        }

        targetConstruction = nearest;

        if (targetConstruction != null)
        {
            humanoidMovement.MoveTo(targetConstruction.transform.position);
        }
    }


    void DeliverWood()
    {
        if (targetConstruction == null) return;

        float distance = Vector2.Distance(transform.position, targetConstruction.transform.position);
        if (distance <= distanceToInteract)
        {
            targetConstruction.GetComponent<Construction>().AddProgress();

            if (carriedWood != null)
            {
                Destroy(carriedWood);
                carriedWood = null;
            }

            carryingWood = false;

            if (currentTree != null)
            {
                humanoidMovement.MoveTo(currentTree.transform.position);
                StartCoroutine(ChopTree(currentTree));
            }
        }
    }
}
