using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMainMenu : MonoBehaviour
{
    private bool m_Levelloaded;


    public void Start()
    {
        DontDestroyOnLoad(this);
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    public void OnDestroy()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        m_Levelloaded = true;
    }

    private void Update()
    {
        if (m_Levelloaded)
        {
            Canvas component = gameObject.GetComponent<Canvas>();
            component.enabled = false;
            component.enabled = true;
            m_Levelloaded = false;
        }
    }


    public void GoBackToMainMenu()
    {
        Debug.Log("going back to main menu");
        SceneManager.LoadScene("MainMenu");
    }
}
