using System.Collections;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject itemPrefab;



    void Start()
    {
        StartCoroutine(SpawnItem());
    }


    IEnumerator SpawnItem()
    {
        yield return new WaitForSeconds(6f);
        Vector3 randomPos = new Vector3(Random.Range(80f, 53f), 4.8f, Random.Range(43f, 15f));
        GameObject obj = Instantiate(itemPrefab,randomPos,Quaternion.identity);
        yield return new WaitForSeconds(6f);
        StartCoroutine(SpawnItem());
    }
    
    
}
