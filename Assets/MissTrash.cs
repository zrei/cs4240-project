using UnityEngine;

public class MissTrash : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Steps m_Step;
    [SerializeField] private Collider m_Collision;

    void Start()
    {
        
    }


    void reset()
    {
        RemovalPart removalPart = GetComponent<RemovalPart>();
        if (removalPart != null)
        {
            removalPart.AttachToConnector();
        }
        StepManager.Instance.GoToStep(m_Step);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider == m_Collision)
        {
            Debug.Log("Collision detected with: " + collision.gameObject.name);

            reset();
        }
    }




}
