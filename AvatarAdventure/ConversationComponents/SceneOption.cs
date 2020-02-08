using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvatarAdventure.ConversationComponents
{
    public class SceneOption
    {
        private string optionText;
        private string optionScene;
        private SceneAction optionAction;
        private SceneOption()
        {
        }
        public string OptionText
        {
            get { return optionText; }
            set { optionText = value; }
        }
        public string OptionScene
        {
            get { return optionScene; }
            set { optionScene = value; }
        }
        public SceneAction OptionAction
        {
            get { return optionAction; }
            set { optionAction = value; }
        }
        public SceneOption(string text, string scene, SceneAction action)
        {
            optionText = text;
            optionScene = scene;
            optionAction = action;
        }
    }
}
