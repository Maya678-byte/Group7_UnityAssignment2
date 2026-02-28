using System;
using UnityEngine;
using UnityEngine.UI;


public class Health : MonoBehaviour
{
    public int i;
    private Image img;
    private Color c;

    void Start()
    {
        img = gameObject.GetComponent<Image>();

        c = img.color;
    }

    private void Update()
    {
            if (ValueSingleton.Instance.health >= i)
            {
                c.a = 1f;
                img.color = c;
            }
            else
            {
                c.a = 0.5f;
                img.color = c;
            }
    }
}
