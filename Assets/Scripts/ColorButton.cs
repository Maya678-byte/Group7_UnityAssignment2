using UnityEngine;

public class ColorButton : MonoBehaviour
{
    public Color c;
    public void SetColor()
    {
        ValueSingleton.Instance.color = c;
    }
}
