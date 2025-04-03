using UnityEngine;

public enum SoundChannel
{
    UI,
    SFX
}

[RequireComponent(typeof(AudioSource))]
public class SoundCue : MonoBehaviour
{
    [SerializeField] private SoundChannel m_Channel = SoundChannel.SFX;
    [SerializeField] private SoundSO m_SoundSO = null;
    [SerializeField] private float m_AdditionalAudioVolumeMultiplier = 1.0f;
    [SerializeField] private bool m_LoopOncePlaying = false;

    private AudioSource m_Source;

    private void Start()
    {
        m_Source = GetComponent<AudioSource>();
        m_Source.volume = m_AdditionalAudioVolumeMultiplier * (m_SoundSO != null ? m_SoundSO.m_BaseVolumeMultiplier : 1.0f);
        m_Source.playOnAwake = false;
        m_Source.loop = m_LoopOncePlaying;
        
        if (m_SoundSO)
        {
            m_Source.clip = m_SoundSO.m_AudioClip;
        }
        // global events for audio channel pausing if required
    }

    public void ToggleSoundPlaying(bool play)
    {
        if (play)
           m_Source?.Play();
        else
            m_Source?.Pause();
    }

    public void StopSound()
    {
        m_Source?.Stop();
    }
}