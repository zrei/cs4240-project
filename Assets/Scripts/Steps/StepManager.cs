using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Text;
using UnityEngine.SceneManagement;

public class StepManager : Singleton<StepManager>
{
    [Tooltip("Steps in the order they should be performed")]
    [SerializeField] private List<StepSO> m_Steps;

    private int m_CurrStepIndex;
    public StepSO CurrStepSO => m_Steps[m_CurrStepIndex];
    public int CurrStepNumber => m_CurrStepIndex + 1;
    public bool HasCompletedAllSteps => m_CurrStepIndex >= m_Steps.Count;

#if UNITY_EDITOR
    public List<StepSO> Steps => m_Steps;
#endif

    protected override void HandleAwake()
    {
        base.HandleAwake();

        m_CurrStepIndex = 0;
        OnGoToStep();

        GlobalEvents.StepsEvents.OnCompleteStep += OnCompleteStep;
    }

    protected override void HandleDestroy()
    {
        base.HandleDestroy();

        GlobalEvents.StepsEvents.OnCompleteStep -= OnCompleteStep;
    }

    public void GoToStep(Steps targetStep)
    {
        StepSO targetStepSO = m_Steps.Find(step => step.m_Step == targetStep);

        if (targetStepSO == null)
        {
            Debug.Log("Not a valid step");
            return;
        }

        int targetIndex = m_Steps.IndexOf(targetStepSO);

        var handlers = FindObjectsOfType<SingleStepHandler>();
        foreach (var handler in handlers)
        {
            handler.ResetHandler();
        }

        m_CurrStepIndex = targetIndex;

        OnGoToStep();
    }



    #region Step Handling
    private void OnCompleteStep()
    {
        Logger.Log(typeof(StepManager), "Complete step: " + CurrStepSO.m_Step, LogLevel.LOG);
        ++m_CurrStepIndex;

        OnGoToStep();
    }

    private void OnGoToStep()
    {
        if (!HasCompletedAllSteps)
        {
            Logger.Log(typeof(StepManager), "Go to step: " + CurrStepSO.m_Step, LogLevel.LOG);
            GlobalEvents.StepsEvents.OnBeginStep?.Invoke(CurrStepSO);
        }
        else
            GlobalEvents.StepsEvents.OnCompleteAllSteps?.Invoke();
    }
    #endregion
}

#if UNITY_EDITOR
[CustomEditor(typeof(StepManager))]
public class StepManagerEditor : Editor
{
    private StepManager m_Target;

    private void OnEnable()
    {
        m_Target = (StepManager)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Debug step order"))
        {
            DebugStepOrder();
        }

        if (GUILayout.Button("Find single step handlers"))
        {
            FindSingleStepHandlers();
        }
    }

    private void DebugStepOrder()
    {
        StringBuilder stringBuilder = new StringBuilder("\n");

        int index = 1;
        foreach (StepSO stepSO in m_Target.Steps)
        {
            stringBuilder.Append("Step " + index + ": " + stepSO.m_Step + "\n");
            ++index;
        }

        Logger.LogEditor(typeof(StepManager), stringBuilder.ToString(), LogLevel.LOG);
    }

    private void FindSingleStepHandlers()
    {
        StringBuilder stringBuilder = new("\n");

        Dictionary<Steps, List<SingleStepHandler>> singleStepHandlers = new();

        GameObject[] rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();

        foreach (GameObject rootGameObject in rootGameObjects)
        {
            TraverseObjects(rootGameObject, singleStepHandlers);
        }

        int index = 1;
        
        foreach (StepSO stepSO in m_Target.Steps)
        {
            if (!singleStepHandlers.ContainsKey(stepSO.m_Step))
            {
                stringBuilder.Append("No single step handler found for step " + index + ": " + stepSO.m_Step + "\n");
            }
            else
            {
                foreach (SingleStepHandler singleStepHandler in singleStepHandlers[stepSO.m_Step])
                {
                    stringBuilder.Append("Step handler for step " + index + ": " + stepSO.m_Step + " found on " + singleStepHandler.gameObject.name + "\n");
                }
            }

            stringBuilder.Append("\n");
            singleStepHandlers.Remove(stepSO.m_Step);
            ++index;
        }

        foreach (KeyValuePair<Steps, List<SingleStepHandler>> pair in singleStepHandlers)
        {
            foreach (SingleStepHandler stepHandler in pair.Value)
            {
                stringBuilder.Append("Step handler for step " + pair.Key + " (not found in " + typeof(StepManager).Name + "'s steps) found on " + stepHandler.gameObject.name + "\n");
            }
        }

        Logger.LogEditor(typeof(StepManager), stringBuilder.ToString(), LogLevel.LOG);
    }

    private void TraverseObjects(GameObject gameObject, Dictionary<Steps, List<SingleStepHandler>> singleStepHandlers)
    {
        if (gameObject == null)
            return;

        SingleStepHandler[] singleStepHandlersOnObj = gameObject.GetComponents<SingleStepHandler>();
        foreach (SingleStepHandler singleStepHandler in singleStepHandlersOnObj)
        {
            if (!singleStepHandlers.ContainsKey(singleStepHandler.Step))
            {
                singleStepHandlers.Add(singleStepHandler.Step, new());
            }

            singleStepHandlers[singleStepHandler.Step].Add(singleStepHandler);
        }

        foreach (Transform child in gameObject.transform)
        {
            TraverseObjects(child.gameObject, singleStepHandlers);
        }
    }
}
#endif