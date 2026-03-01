using UnityEngine;
using System.Collections;
public class CloudSpawner : MonoBehaviour
{
    public GameObject cloud;

    public Transform start;
    public Transform end;
    void Start()
    {
        StartCoroutine(spawn());

    }
    

    IEnumerator spawn()
    {
        yield return new WaitForSecondsRealtime(8);
        GameObject obj = Instantiate(cloud,new Vector3(Random.Range(end.position.x,start.position.x),Random.Range(29.1f,33.1f),start.position.z), Quaternion.Euler(-90,0,0));
        StartCoroutine(spawn());
    }
}
