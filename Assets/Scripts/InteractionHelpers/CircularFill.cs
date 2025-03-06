using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SegmentedCircularPlane))]
public abstract class CircularFill : MonoBehaviour
{
    [SerializeField, Range(0, 1)] private float m_RequiredFillPercentage;

    private List<bool> m_FilledSections;
    public int NumFilledSegments => m_FilledSections.Where(x => x).Count();
    public float CurrPercentage => NumFilledSegments / m_CircularPlane.NumberSegments;

    private SegmentedCircularPlane m_CircularPlane;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        m_FilledSections = new List<bool>();
        for (int i = 0; i < m_CircularPlane.NumberSegments; ++i)
        {
            m_FilledSections.Add(false);
        }
        m_CircularPlane.SegmentTouchedEvent += OnSegmentTouch;
    }

    private void OnDestroy()
    {
        m_CircularPlane.SegmentTouchedEvent -= OnSegmentTouch;
    }

    private void OnSegmentTouch(int segmentIndex)
    {
        m_FilledSections[segmentIndex] = true;

        if (CurrPercentage >= m_RequiredFillPercentage)
        {
            OnFillPercentage();
        }
    }

    protected abstract void OnFillPercentage();
}
