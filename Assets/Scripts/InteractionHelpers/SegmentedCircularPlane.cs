using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Use as a component to hook up with interactions that require touching a circle
/// </summary>
public class SegmentedCircularPlane : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private int m_NumberSegments;
    [SerializeField] private float m_Radius;
    [SerializeField] private string m_InteractionLayerName;
    [SerializeField] private Transform m_MeshTransform;
    [SerializeField] private Transform m_ColliderParent;
    [SerializeField] private float m_Height = 0.01f;

    [Header("Interaction")]
    [SerializeField] private LayerMask m_InteractionMask;

    public int NumberSegments => m_NumberSegments;
#if UNITY_EDITOR
    public float Radius => m_Radius;
    public string InteractionLayerName => m_InteractionLayerName;
    public Transform MeshTransform => m_MeshTransform;
    public float Height => m_Height;
    public LayerMask InteractionMask => m_InteractionMask;
#endif    
    public IntEvent SegmentTouchedEvent;
    public Transform ColliderParent => m_ColliderParent;

    private List<bool> m_InContactSections;

    public VoidEvent OnStartContactEvent;
    public VoidEvent OnEndContactEvent;

    private void Awake()
    {
        m_InContactSections = new();
        for (int i = 0; i < ColliderParent.childCount; ++i)
        {
            int index = i;
            SegmentedCircularPlaneCollider collider = ColliderParent.GetChild(i).GetComponent<SegmentedCircularPlaneCollider>();
            collider.Init(m_InteractionMask);
            collider.OnTriggerEnterEvent += () => OnSegmentTriggerEnter(index);
            collider.OnTriggerExitEvent += () => OnSegmentTriggerExit(index);
            m_InContactSections.Add(false);
        }
    }

    private void OnSegmentTriggerEnter(int index)
    {
        m_InContactSections[index] = true;
        SegmentTouchedEvent?.Invoke(index);

        for (int i = 0; i < m_InContactSections.Count; ++i)
        {
            if (i == index)
                continue;
            if (m_InContactSections[i])
                return;
        }
        OnStartContactEvent?.Invoke();
    }

    private void OnSegmentTriggerExit(int index)
    {
        m_InContactSections[index] = false;

        if (m_InContactSections.All(x => !x))
            OnEndContactEvent?.Invoke();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SegmentedCircularPlane))]
public class CircularFillAreaEditor : Editor
{
    private SegmentedCircularPlane m_Target;

    private void OnEnable()
    {
        m_Target = (SegmentedCircularPlane)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(10f);
        
        GUILayout.Label("Do not put anything other than the colliders under the collider parent!");
        
        if (GUILayout.Button("Setup circle"))
        {
            int childCount = m_Target.ColliderParent.childCount;
            for (int i = 0; i < childCount; ++i)
            {
                DestroyImmediate(m_Target.ColliderParent.GetChild(0).gameObject);
            }

            float circumference = 2 * m_Target.Radius * Mathf.PI;
            float width = circumference / m_Target.NumberSegments;
            float angleInterval = 360 / m_Target.NumberSegments;

            m_Target.MeshTransform.localScale = new Vector3(m_Target.Radius, m_Target.Height, m_Target.Radius);
            m_Target.ColliderParent.localScale = Vector3.one;

            for (int i = 0; i <  m_Target.NumberSegments; ++i)
            {
                SegmentedCircularPlaneCollider colliderObj = new GameObject("Collider " + i, typeof(BoxCollider), typeof(SegmentedCircularPlaneCollider)).GetComponent<SegmentedCircularPlaneCollider>();
                colliderObj.Setup(m_Target.ColliderParent, m_Target.Radius, width, m_Target.Height, angleInterval * i, m_Target.InteractionLayerName);
            }

            PrefabUtility.RecordPrefabInstancePropertyModifications(m_Target.gameObject);
            
        }
    }
}
#endif