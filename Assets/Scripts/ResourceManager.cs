using UnityEngine;
using TMPro;

public class ResourceManager : MonoBehaviour
{

    public static ResourceManager Instance { get; private set; }

    [SerializeField] 
    private TextMeshProUGUI woodCountText;
    [SerializeField]
    private int woodResource = 20;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        UpdateWoodUI();
    }

    public void AddWood(int amount)
    {
        woodResource += amount;
        UpdateWoodUI();
    }

    public void RemoveWood(int amount)
    {
        woodResource = Mathf.Max(0, woodResource - amount); 
        UpdateWoodUI();
    }

    private void UpdateWoodUI()
    {
        if (woodCountText != null)
        {
            woodCountText.text = $"{woodResource}";
        }
        else
        {
            Debug.LogError("Wood count UI is not assigned");
        }
    }

    public int GetWoodCount()
    {
        return woodResource;
    }
}
