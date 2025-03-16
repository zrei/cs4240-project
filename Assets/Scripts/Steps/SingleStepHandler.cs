using UnityEngine;
using System.Collections.Generic;

public class SingleStepHandler : MonoBehaviour
{
    [System.Serializable]
    private struct StepComponent
    {
        public MonoBehaviour Component;
        public bool DisabledBeforeStep;
        public bool DisabledAfterStep;
    }

    [System.Serializable]
    private struct StepGameObject
    {
        public GameObject GameObject;
        public bool DisabledBeforeStep;
        public bool DisabledAfterStep;
    }

    [SerializeField] private Steps m_Step;
    [SerializeField] private List<StepComponent> m_StepComponents;
    [SerializeField] private List<StepGameObject> m_StepGameObjects;

    public Steps Step => m_Step;
    private bool m_IsActive = false;

    private void Awake()
    {
        GlobalEvents.StepsEvents.OnBeginStep += OnBeginStep;
        ToggleBeforeStep(false);
    }

    private void OnDestroy()
    {
        GlobalEvents.StepsEvents.OnBeginStep -= OnBeginStep;
        GlobalEvents.StepsEvents.OnCompleteStep -= OnCompleteStep;
    }

    private void OnBeginStep(StepSO stepSO)
    {
        m_IsActive = stepSO.m_Step == m_Step;
        
        if (m_IsActive)
        {
            GlobalEvents.StepsEvents.OnBeginStep -= OnBeginStep;
            Logger.Log(typeof(SingleStepHandler), this.gameObject, "Begin step " + m_Step, LogLevel.LOG);
            ToggleBeforeStep(true);
            GlobalEvents.StepsEvents.OnCompleteStep += OnCompleteStep;
        }
    }

    private void OnCompleteStep()
    {
        GlobalEvents.StepsEvents.OnCompleteStep -= OnCompleteStep;
        Logger.Log(typeof(SingleStepHandler), this.gameObject, "Complete step " + m_Step, LogLevel.LOG);
        m_IsActive = false;
        ToggleAfterStep();
    }

    private void ToggleBeforeStep(bool enable)
    {
        ToggleComponentsBeforeStep(enable);
        ToggleGameObjectsBeforeStep(enable);
    }

    private void ToggleAfterStep()
    {
        ToggleComponentsAfterStep();
        ToggleGameObjectsAfterStep();
    }

    private void ToggleComponentsBeforeStep(bool enable)
    {
        foreach (StepComponent stepComponent in m_StepComponents)
        {
            stepComponent.Component.enabled = enable || !stepComponent.DisabledBeforeStep;
        }
    }

    private void ToggleComponentsAfterStep()
    {
        foreach (StepComponent stepComponent in m_StepComponents)
        {
            stepComponent.Component.enabled = !stepComponent.DisabledAfterStep;
        }
    }

    private void ToggleGameObjectsBeforeStep(bool enable)
    {
        foreach (StepGameObject stepGameObject in m_StepGameObjects)
        {
            stepGameObject.GameObject.SetActive(enable || !stepGameObject.DisabledBeforeStep);
        }
    }

    private void ToggleGameObjectsAfterStep()
    {
        foreach (StepGameObject stepGameObject in m_StepGameObjects)
        {
            Debug.Log(!stepGameObject.DisabledAfterStep);
            stepGameObject.GameObject.SetActive(!stepGameObject.DisabledAfterStep);
        }
    }
}
