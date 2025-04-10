using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AcknowledgeStepButton : MonoBehaviour
{
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnAcknowledged);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }

    private void OnAcknowledged()
    {
        button.onClick.RemoveAllListeners();
        GlobalEvents.StepsEvents.OnCompleteStep?.Invoke();
    }
}