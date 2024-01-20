using Agents.LiveableAgents;
using CoinPackage.Debugging;

namespace Agents.Actions.LiveableActions
{
    public enum LiveableActionType
    {
        NotChosen,
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
        public LiveableActionType ActionType { get; } = LiveableActionType.NotChosen;
        public abstract bool CheckConditions(Liveable agent);
        public abstract void Invoke(Liveable agent);

        public override string ToString() {
            return $"{ActionType}" % Colorize.Magenta;
        }
    }
    
}