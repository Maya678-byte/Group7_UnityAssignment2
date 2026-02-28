using UnityEngine;

public class Goal : MonoBehaviour
{
    public static Goal Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        

        Instance = this;
    }
}
