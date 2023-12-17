using Agents.LiveableAgents;

namespace Agents.Actions.LiveableActions
{
    public class Exploration : LiveableAction
    {
        public readonly LiveableActionType Type = LiveableActionType.Exploration;
        public override bool CheckConditions(Liveable agent)
        {
            return true;
        }

        public override void Invoke(Liveable agent)
        {
            // TODO: Choose random direction and move by speed
            // TODO: Divide curiosity in cognition map by 1.5
        }
    }
}