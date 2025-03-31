using UnityEngine;
using System.Collections.Generic;

public class SingleStepHandler : StepResponse
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

    [SerializeField] private List<StepComponent> m_StepComponents;
    [SerializeField] private List<StepGameObject> m_StepGameObjects;

    protected override void Awake()
    {
        base.Awake();
        ToggleBeforeStep(false);
    }

    protected override void OnBeginStep()
    {
        Logger.Log(typeof(SingleStepHandler), this.gameObject, "Begin step " + Step, LogLevel.LOG);
        ToggleBeforeStep(true);
    }

    protected override void OnCompleteStep()
    {
        base.OnCompleteStep();
        Logger.Log(typeof(SingleStepHandler), this.gameObject, "Complete step " + Step, LogLevel.LOG);
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
            stepGameObject.GameObject.SetActive(!stepGameObject.DisabledAfterStep);
        }
    }

    public void ResetHandler()
    {
        ToggleBeforeStep(false);

        if (GlobalEvents.StepsEvents.OnBeginStep != null)
        {
            GlobalEvents.StepsEvents.OnBeginStep -= OnStepChange;
            GlobalEvents.StepsEvents.OnBeginStep += OnStepChange;
        }

        if (GlobalEvents.StepsEvents.OnCompleteStep != null)
        {
            GlobalEvents.StepsEvents.OnCompleteStep -= OnCompleteStep;
        }
    }
}
