using UnityEngine;

/// <summary>
/// Script to help rotate the arrow - the arrow head will continue to point towards the target, but it will also rotate towards the camera
/// </summary>
public class ArrowBillboardScript : MonoBehaviour
{
    [SerializeField] private Transform m_Target;
    [SerializeField] private SpriteRenderer m_ArrowSpriteRenderer;

    private Camera m_MainCamera;

    
    private void Start()
    {
        m_MainCamera = Camera.main;
    }

    private void Update()
    {
        transform.rotation = Quaternion.FromToRotation(-Vector3.up, m_Target.position - transform.position);
        transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, 0, transform.localEulerAngles.z);
    }
}
