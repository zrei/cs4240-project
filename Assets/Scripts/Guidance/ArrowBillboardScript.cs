using UnityEngine;

/// <summary>
/// Script to help rotate the arrow - the arrow head will continue to point towards the target, but it will also rotate towards the camera
/// </summary>
public class ArrowBillboardScript : MonoBehaviour
{
    [SerializeField] private Transform m_TargetA;
    [SerializeField] private Transform m_TargetB;
    [SerializeField] private float m_ArrowAnimateSpeed = 5f;
    [SerializeField] private float m_ArrowAnimateAmplitude = 10f;

    private Camera m_MainCamera;
    private Transform m_ChildSpriteRenderer;
    private float m_ArrowAnimateDirection = 1f;

    private void Start()
    {
        m_MainCamera = Camera.main;
        m_ChildSpriteRenderer = transform.GetChild(0);
        SetTarget(m_TargetA);
    }

    private void OnEnable()
    {
        if (m_ChildSpriteRenderer != null)
            m_ChildSpriteRenderer.localPosition = Vector3.zero;
        m_ArrowAnimateDirection = 1f;
    }

    private void Update()
    {
        transform.rotation = Quaternion.FromToRotation(-Vector3.up, m_TargetB.position - transform.position);
        transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, 0, transform.localEulerAngles.z);

        float height = m_ArrowAnimateSpeed * Time.deltaTime * m_ArrowAnimateDirection + m_ChildSpriteRenderer.localPosition.y;
        if (Mathf.Abs(height) > m_ArrowAnimateAmplitude)
        {
            height = m_ArrowAnimateDirection * m_ArrowAnimateAmplitude;
            m_ArrowAnimateDirection *= -1;
        }
        m_ChildSpriteRenderer.localPosition = new Vector3(0f, height, 0f);
    }

    private void SetTarget(bool targetA)
    {
        Transform targetTransform = targetA ? m_TargetA : m_TargetB;
        Transform newParent = targetTransform.GetComponentInChildren<ArrowAttachPoint>(true).transform;
        transform.parent = newParent;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void OnHoverEnter()
    {
        SetTarget(m_TargetB);
    }

    public void OnHoverExit()
    {
        SetTarget(m_TargetA);
    }
}
