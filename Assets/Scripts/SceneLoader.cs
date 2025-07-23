using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void ContinueScene(string sceneName)
    {
        StartCoroutine(ContinueSceneCoroutine(sceneName));
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private IEnumerator ContinueSceneCoroutine(string sceneName)
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(sceneName);

        if (GameSettings.Instance)
            GameSettings.Instance.SetContinueBool(false);
    }
    public void ExitApp()
    {
        Application.Quit();
    }
}


