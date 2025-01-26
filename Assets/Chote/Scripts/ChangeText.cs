using UnityEngine;
using TMPro;
using System.Linq;

public class ChangeText : MonoBehaviour
{
    public TMP_Text  text;
    public PlayerMovement playerMovement;

    // Update is called once per frame
    void Update()
    {
        text.text = $"{VideoController.instance.isEnd.ToString()} \n" +
            $"{Input.GetAxisRaw("Horizontal")}";
        Debug.Log(VideoController.instance.isEnd);
    }
}
