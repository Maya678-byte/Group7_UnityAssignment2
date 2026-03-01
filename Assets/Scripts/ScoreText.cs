using UnityEngine;
using TMPro;
public class ScoreText : MonoBehaviour
{
    private void Update()
    {
        GetComponent<TextMeshPro>().text = ValueSingleton.Instance.ballsOut + " / " + ValueSingleton.Instance.wave;
    }
}
