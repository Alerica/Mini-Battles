using UnityEngine;
using UnityEngine.Rendering;

public class PointToClick : MonoBehaviour
{
    [SerializeField]
    private float durationToDestroy = 1f;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private AnimationCurve animationCurve;
    private Vector3 initialScale;
    private float currentTime;
    private float scaleTime;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        initialScale = transform.localScale;
    }
    void Update()
    {
        currentTime += Time.deltaTime;
        scaleTime += Time.deltaTime;
        scaleTime %= 1f;

        float scaleMultiplier = animationCurve.Evaluate(currentTime);
        transform.localScale = initialScale * scaleMultiplier;

        if(currentTime >= durationToDestroy * 0.8)
        {
            float fadeOutProgress = (currentTime - durationToDestroy * 0.8f) / (durationToDestroy * 0.2f);
            spriteRenderer.color = new Color(1, 1, 1, 1 - fadeOutProgress);

        }
        if(currentTime > durationToDestroy)
        {
            Destroy(gameObject);
        }    
    }
}
