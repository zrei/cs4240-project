using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DebugInputs : MonoBehaviour
{
    [SerializeField] private bool _enable = false;
    [SerializeField] private InputActionReference _quitAction;
    [SerializeField] private InputActionReference _resetAction;

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
    }

    private void OnDestroy()
    {
        _quitAction.action.performed -= OnQuit;
        _resetAction.action.performed -= OnReset;
    }

    private void OnQuit(InputAction.CallbackContext callbackContext)
    {
        Application.Quit();
    }

    private void OnReset(InputAction.CallbackContext callbackContext)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
