using UnityEngine;

public class DirtySkin : MonoBehaviour
{
    [SerializeField] private CircularFill m_CircularFill;
    [SerializeField] private GameObject m_DirtyObj;

    [SerializeField] private SoundCue m_CleanSkinSound;

    private bool m_IsCompleted = false;

    private void Start()
    {
        m_CircularFill.OnFillPercentage += OnComplete;
        m_CircularFill.OnContactStartEvent += OnContactStart;
        m_CircularFill.OnContactEndEvent += OnContactEnd;
    }

    private void OnDestroy()
    {
        m_CircularFill.OnFillPercentage -= OnComplete;
        m_CircularFill.OnContactStartEvent -= OnContactStart;
        m_CircularFill.OnContactEndEvent -= OnContactEnd;
    }

    private void OnComplete()
    {
        // visual update
        GlobalEvents.StepsEvents.OnCompleteStep?.Invoke();
        Debug.Log("Dirty SKin Event should be called");
        m_IsCompleted = true;
        m_CleanSkinSound.StopSound();
    }

    private void OnContactStart()
    {
        if (!m_IsCompleted)
        {
            m_CleanSkinSound.ToggleSoundPlaying(true);
        }
    }

    private void OnContactEnd()
    {
        m_CleanSkinSound.StopSound();
    }
}
