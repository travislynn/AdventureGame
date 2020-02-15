namespace AvatarAdventure.ConversationComponents
{
    public class SceneOption
    {
        public string OptionText { get; set; }

        public string OptionScene { get; set; }

        public SceneAction OptionAction { get; set; }

        public SceneOption(string text, string scene, SceneAction action)
        {
            OptionText = text;
            OptionScene = scene;
            OptionAction = action;
        }
    }
}
