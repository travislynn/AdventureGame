namespace AvatarAdventure.ConversationComponents
{
    public class SceneAction
    {
        public SceneAction()
        {

        }

        public SceneAction(ActionType action, string parameter)
        {
            Action = action;
            Parameter = parameter;
        }

        public ActionType Action;
        public string Parameter;
    }
}