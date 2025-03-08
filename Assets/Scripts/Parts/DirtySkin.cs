using UnityEngine;

public class DirtySkin : MonoBehaviour
{
    [SerializeField] private CircularFill m_CircularFill;
    [SerializeField] private GameObject m_DirtyObj;

    private void Start()
    {
        m_CircularFill.OnFillPercentage += OnComplete;
        m_CircularFill.OnPercentageChangeEvent += OnPercentageUpdate;
    }

    private void OnDestroy()
    {
        m_CircularFill.OnFillPercentage -= OnComplete;
        m_CircularFill.OnPercentageChangeEvent -= OnPercentageUpdate;
    }

    private void OnComplete()
    {
        // visual update
    }

    private void OnPercentageUpdate(float currPercentage)
    {
        Debug.Log(currPercentage);
    }
}
