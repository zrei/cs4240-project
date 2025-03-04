using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandGrab : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference m_GrabInput;

    [Header("Grab parameters")]
    [SerializeField] private float m_GrabRadius = 5f;
    [SerializeField] private LayerMask m_GrabbableLayer;
    [SerializeField] private Transform m_GrabPositionRef;
    [SerializeField] private bool m_AllowMultiGrab = false;

    #region Grab Status
    private bool m_CurrentlyGrabbing = false;
    private List<IGrabbable> m_CurrGrabbedList = new();
    #endregion

    private bool m_IsSubscribedToInput = false;

    private void OnEnable()
    {
        ToggleControls(true);
        
    }

    private void OnDisable()
    {
        ToggleControls(false);
        
    }


    #region Input
    private void ToggleControls(bool subscribe)
    {
        if (subscribe && !m_IsSubscribedToInput)
        {
            m_GrabInput.action.performed += OnGrabInputPerformed;
            m_GrabInput.action.canceled += OnGrabInputCanceled;
            m_GrabInput.action.Enable();
        }
        else if (!subscribe && m_IsSubscribedToInput)
        {
            m_GrabInput.action.performed -= OnGrabInputPerformed;
            m_GrabInput.action.canceled -= OnGrabInputCanceled;
            m_GrabInput.action.Disable();
        }
        m_IsSubscribedToInput = subscribe;
    }

    private void OnGrabInputPerformed(InputAction.CallbackContext context)
    {
        
        if (!m_CurrentlyGrabbing)
        {
            TryGrab();
        }
    }

    private void OnGrabInputCanceled(InputAction.CallbackContext context)
    {
        
        if (m_CurrentlyGrabbing)
        {
            Release();
        }
    }
    #endregion

    #region Grab Handle
    private void TryGrab()
    {
        Vector3 direction = m_GrabPositionRef.forward;
        float maxDistance = m_GrabRadius * 2;

        RaycastHit[] hits = Physics.SphereCastAll(m_GrabPositionRef.position, m_GrabRadius, direction, maxDistance, m_GrabbableLayer);

        Debug.Log($"Hits found: {hits.Length}");

        IGrabbable closest = null;
        float minDistance = 0f;

        foreach (RaycastHit hit in hits)
        {
            Debug.Log( "hit some object");
            // any additional checks that need to be done here, e.g. tag checks
            IGrabbable grabbable = hit.transform.GetComponentInParent<IGrabbable>();

            if (grabbable != null)
            {
                if (!m_AllowMultiGrab)
                {
                    if (closest == null)
                    {
                        closest = grabbable;
                        minDistance = hit.distance;
                    }
                    else if (hit.distance < minDistance)
                    {
                        minDistance = hit.distance;
                        closest = grabbable;
                    }
                }
                else
                {
                    if (grabbable.OnGrab(m_GrabPositionRef) == GrabState.GRABBED)
                    {
                        m_CurrentlyGrabbing = true;
                        m_CurrGrabbedList.Add(grabbable);
                    }
                }
            }
        }

        if (!m_AllowMultiGrab && closest != null)
        {
            if (closest.OnGrab(m_GrabPositionRef) == GrabState.GRABBED)
            {
                m_CurrentlyGrabbing = true;
                m_CurrGrabbedList.Add(closest);
            }
        }
    }

    private void Release()
    {
        foreach (IGrabbable grabbable in m_CurrGrabbedList)
            grabbable.OnRelease();
        m_CurrGrabbedList.Clear();
        m_CurrentlyGrabbing = false;
    }
    #endregion
}