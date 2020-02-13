using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AvatarAdventure.ConversationComponents
{
    public class ConversationManager
    {
        public static Dictionary<string, Conversation> ConversationList { get; private set; } = new Dictionary<string, Conversation>();

        public ConversationManager()
        {
        }
        public static void AddConversation(string name, Conversation conversation)
        {
            if (!ConversationList.ContainsKey(name))
                ConversationList.Add(name, conversation);
        }
        public static Conversation GetConversation(string name)
        {
            if (ConversationList.ContainsKey(name))
                return ConversationList[name];
            return null;
        }
        public static bool ContainsConversation(string name)
        {
            return ConversationList.ContainsKey(name);
        }

        // 
        public static void ToFile(string fileName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement root = xmlDoc.CreateElement("Conversations");
            xmlDoc.AppendChild(root);
            foreach (string s in ConversationManager.ConversationList.Keys)
            {
                Conversation c = ConversationManager.GetConversation(s);
                XmlElement conversation = xmlDoc.CreateElement("Conversation");
                XmlAttribute name = xmlDoc.CreateAttribute("Name");
                name.Value = s;
                conversation.Attributes.Append(name);
                XmlAttribute firstScene = xmlDoc.CreateAttribute("FirstScene");
                firstScene.Value = c.FirstScene;
                conversation.Attributes.Append(firstScene);
                XmlAttribute backgroundName = xmlDoc.CreateAttribute("BackgroundName");
                backgroundName.Value = c.BackgroundName;
                conversation.Attributes.Append(backgroundName);
                XmlAttribute fontName = xmlDoc.CreateAttribute("FontName");
                fontName.Value = c.FontName;
                conversation.Attributes.Append(fontName);
                foreach (string sc in c.Scenes.Keys)
                {
                    GameScene g = c.Scenes[sc];
                    XmlElement scene = xmlDoc.CreateElement("GameScene");
                    XmlAttribute sceneName = xmlDoc.CreateAttribute("Name");
                    sceneName.Value = sc;
                    scene.Attributes.Append(sceneName);
                    XmlElement text = xmlDoc.CreateElement("Text");
                    text.InnerText = c.Scenes[sc].Text;
                    foreach (SceneOption option in g.Options)
                    {
                        XmlElement sceneOption = xmlDoc.CreateElement("GameSceneOption");
                        XmlAttribute oText = xmlDoc.CreateAttribute("Text");
                        oText.Value = option.OptionText;
                        sceneOption.Attributes.Append(oText);
                        XmlAttribute oOption = xmlDoc.CreateAttribute("Option");
                        oOption.Value = option.OptionScene;
                        sceneOption.Attributes.Append(oOption);
                        XmlAttribute oAction = xmlDoc.CreateAttribute("Action");
                        oAction.Value = option.OptionAction.ToString();
                        sceneOption.Attributes.Append(oAction);
                        XmlAttribute oParam = xmlDoc.CreateAttribute("Parameter");
                        oParam.Value = option.OptionAction.Parameter;
                        scene.AppendChild(sceneOption);
                    }
                    conversation.AppendChild(scene);
                }
                root.AppendChild(conversation);
            }
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.Encoding = Encoding.UTF8;
            FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            XmlWriter writer = XmlWriter.Create(stream, settings);
            xmlDoc.Save(writer);
        }

        public static void FromFile(string fileName, Game gameRef, bool editor = false)
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(fileName);
                XmlNode root = xmlDoc.FirstChild;
                if (root.Name == "xml")
                    root = root.NextSibling;
                if (root.Name != "Conversations")
                    throw new Exception("Invalid conversation file!");
                foreach (XmlNode node in root.ChildNodes)
                {
                    if (node.Name == "#comment")
                        continue;
                    if (node.Name != "Conversation")
                        throw new Exception("Invalid conversation file!");
                    string conversationName = node.Attributes["Name"].Value;
                    string firstScene = node.Attributes["FirstScene"].Value;
                    string backgroundName = node.Attributes["BackgroundName"].Value;
                    string fontName = node.Attributes["FontName"].Value;
                    Texture2D background = gameRef.Content.Load<Texture2D>(@"Backgrounds\" +
                   backgroundName);
                    SpriteFont font = gameRef.Content.Load<SpriteFont>(@"Fonts\" +
                   fontName);
                    Conversation conversation = new Conversation(conversationName,
                   firstScene, background, font);
                    conversation.BackgroundName = backgroundName;
                    conversation.FontName = fontName;
                    foreach (XmlNode sceneNode in node.ChildNodes)
                    {
                        string text = "";
                        string optionText = "";
                        string optionScene = "";
                        string optionAction = "";
                        string optionParam = "";
                        string sceneName = "";
                        if (sceneNode.Name != "GameScene")
                            throw new Exception("Invalid conversation file!");
                        sceneName = sceneNode.Attributes["Name"].Value;
                        List<SceneOption> sceneOptions = new List<SceneOption>();
                        foreach (XmlNode innerNode in sceneNode.ChildNodes)
                        {
                            if (innerNode.Name == "Text")
                                text = innerNode.InnerText;
                            if (innerNode.Name == "GameSceneOption")
                            {
                                optionText = innerNode.Attributes["Text"].Value;
                                optionScene = innerNode.Attributes["Option"].Value;
                                optionAction = innerNode.Attributes["Action"].Value;
                                optionParam = innerNode.Attributes["Parameter"].Value;
                                SceneAction action = new SceneAction();
                                action.Parameter = optionParam;
                                action.Action = (ActionType)Enum.Parse(typeof(ActionType),
                               optionAction);
                                SceneOption option = new SceneOption(optionText,
                               optionScene, action);
                                sceneOptions.Add(option);
                            }
                        }
                        GameScene scene = null;
                        if (editor)
                            scene = new GameScene(text, sceneOptions);
                        else
                            scene = new GameScene(gameRef, text, sceneOptions);
                        conversation.AddScene(sceneName, scene);
                    }
                    ConversationList.Add(conversationName, conversation);
                }
            }
            catch
            {
            }
            finally
            {
                xmlDoc = null;
            }
        }

        public static void ClearConversations()
        {
            ConversationList = new Dictionary<string, Conversation>();
        }

        public static void CreateConversations(Game gameRef)
        {
            Texture2D sceneTexture = gameRef.Content.Load<Texture2D>(@"Scenes\scenebackground");
            SpriteFont sceneFont = gameRef.Content.Load<SpriteFont>(@"Fonts\scenefont");

            // TODO:  Build conversations from data file
            // TODO:  Further refactors to simplify
           

            // 'MarissaHello' conversation
            Conversation marissaConversation = new Conversation("MarissaHello", "Hello", sceneTexture, sceneFont);
            marissaConversation.BackgroundName = "scenebackground";
            marissaConversation.FontName = "scenefont";

            marissaConversation.AddScene("Hello", new GameScene(
                gameRef,
                "Hello, my name is Marissa. I'm still learning about summoning avatars.",
                new List<SceneOption>() {
                    new SceneOption("Good bye.", "",
                        new SceneAction(ActionType.End, "none"))
                }));

            ConversationList.Add("MarissaHello", marissaConversation);

            // 'LanceHello' Conversation --------------------------------
            Conversation lanceConversation = new Conversation("LanceHello", "Hello", sceneTexture, sceneFont);
            lanceConversation.BackgroundName = "scenebackground";
            lanceConversation.FontName = "scenefont";

            // scene 1 'hello' for lance
            lanceConversation.AddScene("Hello", new GameScene(
                gameRef,
                "Fire avatars are my favorites. Do you like fire type avatars too?",
                new List<SceneOption> {
                    new SceneOption("No", "IDislikeFire",
                        new SceneAction(ActionType.Talk, "none")),
                    new SceneOption("Yes", "ILikeFire",
                        new SceneAction(ActionType.Talk, "none"))
                }));

            // 'ILikeFire' scene - ends
            lanceConversation.AddScene("ILikeFire", new GameScene(
                gameRef,
                "That's cool. I have one if you want to see!",
                new List<SceneOption>() {
                    new SceneOption("Good bye.", "",
                        new SceneAction(ActionType.End, "none"))
                }));

            // 'IDislikeFire' scene - ends
            lanceConversation.AddScene("IDislikeFire", new GameScene(
                gameRef,
                "Each to their own I guess.  I don't like water at all.  Do you?",
                new List<SceneOption>() {
                    new SceneOption("Yeah man water is cool!", "CoolWater",
                        new SceneAction(ActionType.Talk, "none")),
                    new SceneOption("No water for this hombre", "NoWater",
                        new SceneAction(ActionType.Talk, "none"))
                }));

            lanceConversation.AddScene("CoolWater", new GameScene(
                gameRef,
                "That's so cool that you like water.  I guess you better go play the game now.",
                new List<SceneOption>() {
                    new SceneOption("Ok", "",
                        new SceneAction(ActionType.End, "none")),
                    new SceneOption("Umm, ok", "",
                        new SceneAction(ActionType.End, "none")),
                    new SceneOption("Screw you Lance!", "",
                        new SceneAction(ActionType.End, "none"))
                }));

            lanceConversation.AddScene("NoWater", new GameScene(
                gameRef,
                "Hmm, it seems you don't like many avatars.  You DO understand what this game is, right?",
                new List<SceneOption>() {
                    new SceneOption("Avatars!", "",
                        new SceneAction(ActionType.End, "none")),
                    new SceneOption("Characters!", "",
                        new SceneAction(ActionType.End, "none")),
                    new SceneOption("Sweet Maps!", "",
                        new SceneAction(ActionType.End, "none"))
                }));

            ConversationList.Add("LanceHello", lanceConversation);
        }
    }
}