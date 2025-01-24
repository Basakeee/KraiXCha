using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Go2Main : MonoBehaviour
{
   
    public void GotoMain()
    {
        SceneManager.LoadScene(0);
    }
    public void GotoCredit()
    {
        SceneManager.LoadScene(1);
    }
}
