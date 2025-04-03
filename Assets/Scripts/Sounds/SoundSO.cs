using UnityEngine;

[CreateAssetMenu(fileName = "SoundSO", menuName = "ScriptableObject/SoundSO")]
public class SoundSO : ScriptableObject
{
    public AudioClip m_AudioClip;
    public float m_BaseVolumeMultiplier = 1.0f;
    public float m_BaseSpeedMultiplier = 1.0f;
}