using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Go2Main : MonoBehaviour
{
   public void Exit()
    {
        Application.Quit();
        Debug.Log("Exit");
    }    
    public void GotoMain()
    {
        SceneManager.LoadScene(0);
    }
    public void GotoCredit()
    {
        SceneManager.LoadScene(1);
    }
    public void GotoGame()
    {
        SceneManager.LoadScene(2);
    }
}
