using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverSetActive : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("On Hover - Activate")]
    public GameObject[] objectsToActivate; // List of GameObjects to activate on hover

    [Header("On Hover - Deactivate")]
    public GameObject[] objectsToDeactivate; // List of GameObjects to deactivate on hover

    [Header("On Exit - Activate")]
    public GameObject[] objectsToActivateOnExit; // List of GameObjects to activate on exit

    [Header("On Exit - Deactivate")]
    public GameObject[] objectsToDeactivateOnExit; // List of GameObjects to deactivate on exit

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Activate specified objects
        foreach (GameObject obj in objectsToActivate)
        {
            if (obj != null) obj.SetActive(true);
        }

        // Deactivate specified objects
        foreach (GameObject obj in objectsToDeactivate)
        {
            if (obj != null) obj.SetActive(false);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Activate specified objects on exit
        foreach (GameObject obj in objectsToActivateOnExit)
        {
            if (obj != null) obj.SetActive(true);
        }

        // Deactivate specified objects on exit
        foreach (GameObject obj in objectsToDeactivateOnExit)
        {
            if (obj != null) obj.SetActive(false);
        }
    }
}
