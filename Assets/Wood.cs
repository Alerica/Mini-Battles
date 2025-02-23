using UnityEngine;

public class Wood : MonoBehaviour
{
    public bool IsReserved { get; private set; } = false;

    public void Reserve()
    {
        IsReserved = true;
    }

    
}

