using UnityEngine;

public class Test : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            AudioSetting.Instance.PlaySFX("Walk");
        if (Input.GetKeyDown(KeyCode.F))
            AudioSetting.Instance.PlaySFX("hit");
    }
}
