using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private Button continueButton;
    private void Start()
    {
        if (GameSettings.Instance && continueButton && GameSettings.Instance.HasSaveData())
        {
            continueButton.interactable = true;

            GameSettings.Instance.LoadGridFile();
        }
    }
    private IEnumerator LoadSceneInternal(string sceneName, bool isContinuing)
    {
        yield return new WaitForSeconds(0.1f);

        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("Scene name is null or empty.");
            yield break;
        }

        if (GameSettings.Instance)
            GameSettings.Instance.SetContinueBool(isContinuing);

        SceneManager.LoadScene(sceneName);

        yield return new WaitForSeconds(0.1f);

        if (isContinuing && ScoreManager.Instance)
            ScoreManager.Instance.LoadScoreData();
        else if (!isContinuing && ScoreManager.Instance)
        {
            ScoreManager.Instance.ResetScore();
            ScoreManager.Instance.ResetCombo();
        }
    }

    public void ContinueScene(string sceneName)
    {
        StartCoroutine(LoadSceneInternal(sceneName, true));
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneInternal(sceneName, false));
    }


    public void ExitApp()
    {
        Application.Quit();
    }

    public void SetGridSize(int index)
    {
        if (GameSettings.Instance)
        {
            GameSettings.Instance.SetGridSizeByIndex(index);
        }

    }
}


