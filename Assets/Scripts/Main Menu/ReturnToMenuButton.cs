using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ReturnToMenuButton : MonoBehaviour
{
    [SerializeField] private CanvasGroup m_Cg;
    [SerializeField] private float m_BlackOutTime = 1f;

    private Button m_Button;

    private void Start()
    {
        m_Button = GetComponent<Button>();
        m_Button.onClick.AddListener(ReturnToMainMenu);
    }

    private void OnDestroy()
    {
        if (m_Button)
            m_Button.onClick.RemoveAllListeners();
    }

    private void ReturnToMainMenu()
    {
        m_Button.onClick.RemoveAllListeners();
        StartCoroutine(BlackOutCoroutine());
    }

    private IEnumerator BlackOutCoroutine()
    {
        float t = 0f;
        m_Cg.blocksRaycasts = true;
        while (t < m_BlackOutTime)
        {
            yield return null;
            t += Time.deltaTime;
            m_Cg.alpha = Mathf.Lerp(0f, 1f, t / m_BlackOutTime);
        }
        SceneManager.LoadScene(0);
    }
}
