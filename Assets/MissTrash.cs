using UnityEngine;

public class MissTrash : ResetPart
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Steps m_Step;
    [SerializeField] private Collider m_Collision;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider == m_Collision)
        {
            Debug.Log("Collision detected with: " + collision.gameObject.name);

            Reset();
        }
    }

    protected override void OnReset()
    {
        RemovalPart removalPart = GetComponent<RemovalPart>();
        if (removalPart != null)
        {
            removalPart.AttachToConnector();
        }
        StepManager.Instance.GoToStep(m_Step);
    }
}
