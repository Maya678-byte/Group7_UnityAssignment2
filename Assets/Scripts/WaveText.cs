using System;
using TMPro;
using UnityEngine;

public class WaveText : MonoBehaviour
{
    private void Update()
    {
        GetComponent<TextMeshPro>().text = "Wave " + ValueSingleton.Instance.wave;
    }
}
