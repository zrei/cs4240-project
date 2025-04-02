using UnityEngine;
using UnityEngine.EventSystems;

public class onHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject newPrefab; // Prefab to switch to on hover
    private GameObject originalPrefabInstance; // Reference to the original prefab instance
    private GameObject currentInstance; // Tracks the currently active instance

    void Start()
    {
        // Store the original instance (the GameObject this script is attached to)
        originalPrefabInstance = gameObject;
        currentInstance = originalPrefabInstance;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (newPrefab != null && currentInstance == originalPrefabInstance)
        {
            // Switch to the new prefab
            Vector3 position = currentInstance.transform.position;
            Quaternion rotation = currentInstance.transform.rotation;
            Transform parent = currentInstance.transform.parent;

            // Destroy the current instance and replace it
            Destroy(currentInstance);
            currentInstance = Instantiate(newPrefab, position, rotation, parent);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentInstance != null && currentInstance != originalPrefabInstance)
        {
            // Switch back to the original prefab
            Vector3 position = currentInstance.transform.position;
            Quaternion rotation = currentInstance.transform.rotation;
            Transform parent = currentInstance.transform.parent;

            // Destroy the current instance and replace it
            Destroy(currentInstance);
            currentInstance = Instantiate(originalPrefabInstance, position, rotation, parent);
        }
    }
}
