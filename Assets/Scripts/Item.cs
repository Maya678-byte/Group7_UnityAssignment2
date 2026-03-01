using UnityEngine;

public class Item : MonoBehaviour
{
    public LayerMask playerLayer;
    
    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 1f, playerLayer);

        foreach (Collider hit in hits)
        {
            int i = Random.Range(0, 2);

            if (i == 0)
            {
                ValueSingleton.Instance.SlowTime();
            }
            else
            {
                ValueSingleton.Instance.BoostTime();
            }
            
            Destroy(gameObject);
        }
    }
}
