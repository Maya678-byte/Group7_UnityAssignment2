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
        if (ValueSingleton.Instance.wave % 3 == 0)
        {
            for (int i = 0; i < ValueSingleton.Instance.wave - 1; i++)
            {
                int k = Random.Range(0, enemies.Length - 1);
                Vector3 randomPos = new Vector3(Random.Range(60f, 75f), 6f, Random.Range(11f, 20f));
                GameObject obj = Instantiate(enemies[k],randomPos,Quaternion.identity);
                obj.GetComponent<Enemy>().isPoint = true;
            }
            
            Vector3 r = new Vector3(Random.Range(60f, 75f), 6f, Random.Range(11f, 20f));
            GameObject o = Instantiate(enemies[4],r,Quaternion.identity);
            o.GetComponent<Enemy>().isPoint = true;
        }
        else
        {
            for (int i = 0; i < ValueSingleton.Instance.wave; i++)
            {
                int k = Random.Range(0, enemies.Length - 1);
                Vector3 randomPos = new Vector3(Random.Range(60f, 75f), 6f, Random.Range(11f, 20f));
                GameObject obj = Instantiate(enemies[k],randomPos,Quaternion.identity);
                obj.GetComponent<Enemy>().isPoint = true;
            }
        }
        
    }
}
