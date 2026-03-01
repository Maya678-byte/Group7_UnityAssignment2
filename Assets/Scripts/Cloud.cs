using UnityEngine;

public class Cloud : MonoBehaviour
{
    private float s;
    public int speed;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        s = Random.Range(0.5f, 1.25f);
        transform.localScale = new Vector3(s, s, 2);
        Invoke("DestroyAfterTime",100f);
    }
    
    void Update()
    {
        transform.Translate(Vector3.up * -speed * Time.deltaTime);
    }

    void DestroyAfterTime()
    {
        Destroy(gameObject);
    }
}
