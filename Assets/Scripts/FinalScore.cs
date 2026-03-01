using UnityEngine;
using UnityEngine.UI;

public class FinalScore : MonoBehaviour
{
    void Update()
    {
        GetComponent<Text>().text = "You Reached Wave " + ValueSingleton.Instance.wave.ToString() + " in " +
                                    ValueSingleton.Instance.timerText;
    }
}
