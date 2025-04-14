using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private CanvasGroup _blackOutCg;
    [SerializeField] private float m_BlackOutTime = 1f;

    private bool m_IsLoading = false;

    public void PlayGame() 
    {
        LoadScene("EntryScene");
    }
   public void QuitGame()
   {
    Application.Quit();
   }
   public void BathroomStage()
   {
    LoadScene("Bathroom_set(interior)");
   }
   public void RestaurantStage()
   {
    LoadScene("RestrauntScene");
   }

    private void LoadScene(string sceneName)
    {
        if (m_IsLoading)
            return;

        m_IsLoading = true;
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        float t = 0f;
       _blackOutCg.blocksRaycasts = true;
        while (t < m_BlackOutTime)
        {
            yield return null;
            t += Time.deltaTime;
            _blackOutCg.alpha = Mathf.Lerp(0f, 1f, t / m_BlackOutTime);
        }
        SceneManager.LoadSceneAsync(sceneName);
    }
   // if aimed at, change appearance
    // if not, change back
   // if interacted with, activate
}
