using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ReturnToMenuButton : MonoBehaviour
{
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
        SceneManager.LoadScene(0);
    }
}
