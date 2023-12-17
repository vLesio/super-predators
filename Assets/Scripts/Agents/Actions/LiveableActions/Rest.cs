using Agents.LiveableAgents;

namespace Agents.Actions.LiveableActions
{
    public class Rest : LiveableAction
    {
        public readonly LiveableActionType Type = LiveableActionType.Rest;
        public override bool CheckConditions(Liveable agent)
        {
            return true;
        }

        public override void Invoke(Liveable agent)
        {
            return; // Nothing is happening purposefully
        }
    }
}