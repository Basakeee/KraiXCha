using UnityEngine;
using UnityEngine.UI;

public class Buttonlistener : MonoBehaviour
{
    public Button Button;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Button.onClick.AddListener(AudioSetting.Instance.UpdateSlider);
    }

    // Update is called once per frame
    
}
