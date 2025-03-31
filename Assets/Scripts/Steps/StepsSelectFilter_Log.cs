public class StepsSelectFilter_Log : StepsSelectFilter
{
    protected override void HandleCannotInteract()
    {
        Logger.Log(typeof(StepsSelectFilter_Log), gameObject, "Cannot select because of step", LogLevel.LOG);
    }
}