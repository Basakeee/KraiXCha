using UnityEngine;

public class Test : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
            AudioSetting.Instance.PlaySFX("Walk");
        if (Input.GetKeyDown(KeyCode.F))
            AudioSetting.Instance.PlaySFX("hit");
    }
}
