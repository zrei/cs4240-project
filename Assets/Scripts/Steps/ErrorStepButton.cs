using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Button))]
public class ErrorStepButton : MonoBehaviour
{
    [SerializeField] private SoundCue m_SoundCue;

    private Button _button;

    private void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnPlayErrorSound);
    }

    private void OnDestroy()
    {
        if (_button)
            _button.onClick.RemoveAllListeners();
    }

    private void OnPlayErrorSound()
    {
        m_SoundCue.ToggleSoundPlaying(true);
    }
}