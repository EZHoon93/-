

namespace BehaviorDesigner.Runtime.Tasks.My
{
    [System.Serializable]
    public class SharedInputControllerObject : SharedVariable<InputControllerObject>
    {
        public static implicit operator SharedInputControllerObject(InputControllerObject value) { return new SharedInputControllerObject { Value = value }; }
    }
}