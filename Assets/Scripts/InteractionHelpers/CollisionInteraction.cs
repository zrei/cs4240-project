using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Simple collision interaction - triggering the collider will instantly call step completion
/// Best to pair this with a step handler to disable interactions if not at the correct step
/// </summary>
[RequireComponent(typeof(Collider))]
public class CollisionInteraction : MonoBehaviour
{
    [SerializeField] private LayerMask m_AllowedLayers;
    [SerializeField] private bool m_DestroyCollidedObject = false;

    /// <summary>
    /// For additional hook-ons
    /// </summary>
    public VoidEvent OnCollisionInteraction;

    private void OnTriggerEnter(Collider other)
    {
        //Logger.Log(typeof(CollisionInteraction), this.gameObject, "Trigger enter", LogLevel.LOG);
        if (((1 << other.gameObject.layer) & m_AllowedLayers) != 0)
        {
            if (m_DestroyCollidedObject)
            {
                Destroy(other.gameObject);
            }
            //Logger.Log(typeof(CollisionInteraction), this.gameObject, other.gameObject.name + ", position: " + other.gameObject.transform.position, LogLevel.LOG);
            OnCollisionInteraction?.Invoke();
            GlobalEvents.StepsEvents.OnCompleteStep?.Invoke();
        }
    }
}