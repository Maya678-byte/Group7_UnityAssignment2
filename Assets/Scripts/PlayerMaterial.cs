using UnityEngine;

public class PlayerMaterial : MonoBehaviour
{
    public Material material;
    Color color;
    void Update()
    {
        color = ValueSingleton.Instance.color;
        if (material.color != color)
        {
            material.color = color;
        }
    }
}
