using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    public TMP_Text highScoreUI;

    string newGameScene = "SampleScene";
    public AudioClip bg_music;
    public AudioSource main_channel;

    public SaveLoadManager saveLoadManager;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        main_channel.PlayOneShot(bg_music);
        int highScore = SaveLoadManager.Instance.LoadHighScore();
        highScoreUI.text = $"Top Wave Survived: {highScore}";

       
        if (EventSystem.current == null)
        {
            Debug.LogError("EventSystem not found in the scene.");
        }
    }

    public void StartNewGame()
    {
       
        SceneManager.LoadScene(newGameScene);
    }

    public void ExitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ResetScore()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        int highScore = SaveLoadManager.Instance.LoadHighScore();
        highScoreUI.text = $"Top Wave Survived: {highScore}";


    }
}
