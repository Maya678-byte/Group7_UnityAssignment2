using UnityEngine;
using UnityEngine.UI;

public class Slow : MonoBehaviour
{
    void Update()
    {
        GetComponent<Image>().enabled = ValueSingleton.Instance.isSlow;
        transform.GetChild(0).gameObject.SetActive(ValueSingleton.Instance.isSlow);
    }
}
