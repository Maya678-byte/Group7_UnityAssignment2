using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemies;
    
    public static EnemySpawner Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject); 
            return;
        }
        
        Instance = this;
    }


    public void OnSpawn()
    {
        for (int i = 0; i < ValueSingleton.Instance.wave; i++)
        {
            int k = Random.Range(0, enemies.Length);
            Vector3 randomPos = new Vector3(Random.Range(60f, 75f), 6f, Random.Range(11f, 20f));
            GameObject obj = Instantiate(enemies[k],randomPos,Quaternion.identity);
        }
    }
}
