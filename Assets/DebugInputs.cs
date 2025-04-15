using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DebugInputs : MonoBehaviour
{
    [SerializeField] private bool _enable = false;
    [SerializeField] private InputActionReference _quitAction;
    [SerializeField] private InputActionReference _resetAction;
    [SerializeField] private InputActionReference _backToMenuAction;

     // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if (!_enable)
        {
            this.enabled = false;
            return;
        }
        _quitAction.action.performed += OnQuit;
        _resetAction.action.performed += OnReset;

        if (_backToMenuAction)
            _backToMenuAction.action.performed += BackToMenu;
    }

    private void OnDestroy()
    {
        _quitAction.action.performed -= OnQuit;
        _resetAction.action.performed -= OnReset;

        if (_backToMenuAction)
            _backToMenuAction.action.performed -= BackToMenu;
    }

    private void OnQuit(InputAction.CallbackContext callbackContext)
    {
        Application.Quit();
    }

    private void OnReset(InputAction.CallbackContext callbackContext)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void BackToMenu(InputAction.CallbackContext callbackContext)
    {
        SceneManager.LoadScene(0);
    }
}
