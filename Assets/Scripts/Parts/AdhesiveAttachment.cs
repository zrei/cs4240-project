using UnityEngine;

public class AdhesiveAttachment : MonoBehaviour
{
    [SerializeField] private CircularFill m_CircularFill;
    [SerializeField] private Transform m_ColliderParent;

    private void Start()
    {
        m_ColliderParent.gameObject.SetActive(false);
        m_CircularFill.OnPercentageChangeEvent += OnPercentageFill;
        m_CircularFill.OnFillPercentage += OnFinishSticking;
    }

    private void OnDestroy()
    {
        m_CircularFill.OnPercentageChangeEvent -= OnPercentageFill;
        m_CircularFill.OnFillPercentage -= OnFinishSticking;
    }

    // might be OnColliderEnter()
    // should only be active once it has been "picked up"
    private void OnTriggerEnter()
    {
        // attach to another point
        // transform.parent = attachParent;
        m_ColliderParent.gameObject.SetActive(true);
    }

    private void OnPercentageFill(float currPercentage)
    {

    }

    private void OnFinishSticking()
    {
        
    }
}
