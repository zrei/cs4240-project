using System.Linq;
using UnityEngine;

/// <summary>
/// Simple collision interaction - triggering the collider will instantly call step completion
/// Best to pair this with a step handler to disable interactions if not at the correct step
/// </summary>
[RequireComponent(typeof(Collider))]
public class CollisionInteraction : MonoBehaviour
{
    [SerializeField] private LayerMask m_AllowedLayers;
    [SerializeField] private bool m_DestroyCollidedObject = false;
    [SerializeField] private bool m_LimitByTag = false;
    [Tooltip("Ignored if LimitByTag is not true")]
    [SerializeField] private string[] m_AllowedTags;

    /// <summary>
    /// For additional hook-ons
    /// </summary>
    public VoidEvent OnCollisionInteraction;

    private void OnTriggerEnter(Collider other)
    {
        if ((((1 << other.gameObject.layer) & m_AllowedLayers) != 0) && (!m_LimitByTag || m_AllowedTags.Contains(other.gameObject.tag)))
        {
            if (m_DestroyCollidedObject)
            {
                Destroy(other.gameObject);
            }
            OnCollisionInteraction?.Invoke();
            GlobalEvents.StepsEvents.OnCompleteStep?.Invoke();
        }
    }

#if UNITY_EDITOR
    public void OnValidate()
    {
        if (GetComponentInChildren<Collider>() == null)
            Logger.Log(typeof(CollisionInteraction), this.gameObject, "Cannot find a collider!", LogLevel.ERROR);
    }
#endif
}