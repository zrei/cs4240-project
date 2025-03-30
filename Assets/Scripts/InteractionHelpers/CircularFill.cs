using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SegmentedCircularPlane))]
public class CircularFill : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float m_RequiredFillPercentage;
    [SerializeField] private bool m_AllowMultiInvoke = false;
    
    private SegmentedCircularPlane m_CircularPlane;
    
    private List<bool> m_FilledSections;
    public int NumFilledSegments => m_FilledSections.Where(x => x).Count();
    public float CurrPercentage => (float) NumFilledSegments / m_CircularPlane.NumberSegments;
    public FloatEvent OnPercentageChangeEvent;
    public VoidEvent OnFillPercentage;
    private bool m_HasBeenInvoked = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        m_CircularPlane = GetComponent<SegmentedCircularPlane>();
        m_FilledSections = new List<bool>();
        for (int i = 0; i < m_CircularPlane.NumberSegments; ++i)
        {
            m_FilledSections.Add(false);
        }
        Debug.Log($"NumfilledSegments {NumFilledSegments}");
        m_CircularPlane.SegmentTouchedEvent += OnSegmentTouch;
    }

    private void OnDestroy()
    {
        m_CircularPlane.SegmentTouchedEvent -= OnSegmentTouch;
    }

    private void OnSegmentTouch(int segmentIndex)
    {
        m_FilledSections[segmentIndex] = true;
        Debug.Log($"NumfilledSegments {NumFilledSegments}");
        OnPercentageChangeEvent?.Invoke(CurrPercentage);

        if ((m_AllowMultiInvoke || !m_HasBeenInvoked) && CurrPercentage >= m_RequiredFillPercentage)
        {
            OnFillPercentage?.Invoke();

            m_HasBeenInvoked = true;
        }
    }
}
