using UnityEngine;
using UnityEngine.UI;

public class Boost : MonoBehaviour
{
    void Update()
    {
        GetComponent<Image>().enabled = ValueSingleton.Instance.isBoost;
        transform.GetChild(0).gameObject.SetActive(ValueSingleton.Instance.isBoost);

    }
}
