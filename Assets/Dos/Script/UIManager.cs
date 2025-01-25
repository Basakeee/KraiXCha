using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public PlayerMovement player;
    public GameObject gameOverPanel;
    public TMP_Text Score, HighScore, GOscore;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.curHP <= 0)
        {
            gameOverPanel.SetActive(true);
            HighScore.text = $"Highscore : {player.maxDepth}";
        }
        Score.text = $"{player.curDepth}";
        GOscore.text = Score.text ;
    }
    
    public void Restart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
