namespace Agents.Actions.LiveableActions
{
    public enum LiveableActionType
    {
        SearchForFood,
        Eat,
        Exploration,
        Breed,
        Socialization,
        Rest,
        Evasion,
        SearchForPreys
    }
    public abstract class LiveableAction
    {
        public LiveableActionType ActionType;
        public abstract bool CheckConditions();
        public abstract void Invoke();
    }
}