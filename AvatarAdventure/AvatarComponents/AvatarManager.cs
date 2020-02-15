using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Content;

namespace AvatarAdventure.AvatarComponents
{
    public static class AvatarManager
    {
        public static Dictionary<string, Avatar> AvatarList { get; } = new Dictionary<string, Avatar>();

        public static void AddAvatar(string name, Avatar avatar)
        {
            if (!AvatarList.ContainsKey(name))
                AvatarList.Add(name, avatar);
        }
        public static Avatar GetAvatar(string name)
        {
            if (AvatarList.ContainsKey(name))
                return (Avatar)AvatarList[name].Clone();

            return null;
        }

        // todo: use json instead of csv
        public static void FromFile(string fileName, ContentManager content)
        {
            using (Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    using (TextReader reader = new StreamReader(stream))
                    {
                        try
                        {
                            string lineIn = "";
                            do
                            {
                                lineIn = reader.ReadLine();
                                if (lineIn != null)
                                {
                                    Avatar avatar = Avatar.FromString(lineIn, content);
                                    if (!AvatarList.ContainsKey(avatar.Name.ToLowerInvariant()))
                                        AvatarList.Add(avatar.Name.ToLowerInvariant(), avatar);
                                }
                            } while (lineIn != null);
                        }
                        catch
                        {
                        }
                        finally
                        {
                            if (reader != null)
                                reader.Close();
                        }
                    }
                }
                catch
                {
                }
                finally
                {
                    if (stream != null)
                        stream.Close();
                }
            }
        }
    }
}
