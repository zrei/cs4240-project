using UnityEngine;

public delegate void IntEvent(int _);
public delegate void VoidEvent();
public delegate void FloatEvent(float _);
public delegate void Vector3Event(Vector3 _);

public static class GlobalEvents {
  
    public static class StepsEvents
    {
        public delegate void StepSOEvent(StepSO _);
        
        public static VoidEvent OnCompleteStep;
        public static StepSOEvent OnBeginStep;
    }
}