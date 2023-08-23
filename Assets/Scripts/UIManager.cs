using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameInput;
    [SerializeField] TextMeshProUGUI welcomeText;
    [SerializeField] TextMeshProUGUI highScoreText;

    private void Awake()
    {
        UpdateName();
        UILoadScore();
    }
    public void SaveNameInput()
    {
        PlayerDataManager.Instance.playerName = nameInput.text;
        Debug.Log("Your name is " + nameInput.text);

    }

    public void LoadName()
    {
        if (PlayerDataManager.Instance.playerName != null)
        {
            
            nameInput.text = PlayerDataManager.Instance.playerName;
        }
    }

    public void UILoadScore()
    {
        highScoreText.text = "Your Best Score is: " + PlayerDataManager.Instance.bestScore;
    }
    public void UpdateName()
    {
        if (PlayerDataManager.Instance.playerName != null)
        {
            welcomeText.text = "Welcome Pilot " + PlayerDataManager.Instance.playerName;
        }
        else
        {
            welcomeText.text = "";
        }
    }

    public void ResetScore()
    {
        PlayerDataManager.Instance.bestScore = 0;
        PlayerDataManager.Instance.SaveHighScore();
        UILoadScore();
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void BacktoMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
#if  UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
