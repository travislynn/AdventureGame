using System;
using System.IO;
using AvatarAdventure.AvatarComponents;
using AvatarAdventure.CharacterComponents;
using AvatarAdventure.Components;
using AvatarAdventure.ConversationComponents;
using AvatarAdventure.PlayerComponents;
using AvatarAdventure.TileEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AvatarAdventure.GameStates
{
    public class GamePlayState : BaseGameState, IGamePlayState
    {
        Engine engine = new Engine(Game1.ScreenRectangle, 64, 64);
        World world;
        Camera camera;
        Player player;

        public GamePlayState(Game game) : base(game)
        {
            game.Services.AddService(typeof(IGamePlayState), this);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 motion = Vector2.Zero;
            int cp = 8;
            if (Xin.KeyboardState.IsKeyDown(Keys.W) && Xin.KeyboardState.IsKeyDown(Keys.A))
            {
                motion.X = -1;
                motion.Y = -1;
                player.Sprite.CurrentAnimation = AnimationKey.WalkLeft;
            }
            else if (Xin.KeyboardState.IsKeyDown(Keys.W) && Xin.KeyboardState.IsKeyDown(Keys.D))
            {
                motion.X = 1;
                motion.Y = -1;
                player.Sprite.CurrentAnimation = AnimationKey.WalkRight;
            }
            else if (Xin.KeyboardState.IsKeyDown(Keys.S) && Xin.KeyboardState.IsKeyDown(Keys.A))
            {
                motion.X = -1;
                motion.Y = 1;
                player.Sprite.CurrentAnimation = AnimationKey.WalkLeft;
            }
            else if (Xin.KeyboardState.IsKeyDown(Keys.S) && Xin.KeyboardState.IsKeyDown(Keys.D))
            {
                motion.X = 1;
                motion.Y = 1;
                player.Sprite.CurrentAnimation = AnimationKey.WalkRight;
            }
            else if (Xin.KeyboardState.IsKeyDown(Keys.W))
            {
                motion.Y = -1;
                player.Sprite.CurrentAnimation = AnimationKey.WalkUp;
            }
            else if (Xin.KeyboardState.IsKeyDown(Keys.S))
            {
                motion.Y = 1;
                player.Sprite.CurrentAnimation = AnimationKey.WalkDown;
            }
            else if (Xin.KeyboardState.IsKeyDown(Keys.A))
            {
                motion.X = -1;
                player.Sprite.CurrentAnimation = AnimationKey.WalkLeft;
            }
            else if (Xin.KeyboardState.IsKeyDown(Keys.D))
            {
                motion.X = 1;
                player.Sprite.CurrentAnimation = AnimationKey.WalkRight;
            }
            if (motion != Vector2.Zero)
            {
                motion.Normalize();
                motion *= (player.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds);

                Rectangle playerRect = new Rectangle(
                    (int)player.Sprite.Position.X + (int)motion.X + cp,
                    (int)player.Sprite.Position.Y + (int)motion.Y + cp,
                    Engine.TileWidth - cp,
                    Engine.TileHeight - cp);

                foreach (string characterName in world.CurrentMap.Characters.Keys)
                {
                    ICharacter character = GameRef.CharacterManager.GetCharacter(characterName);

                    Rectangle characterRect = new Rectangle(
                        (int)world.CurrentMap.Characters[characterName].X * Engine.TileWidth + cp,
                        (int)world.CurrentMap.Characters[characterName].Y * Engine.TileHeight + cp,
                        Engine.TileWidth - cp,
                        Engine.TileHeight - cp);

                    // if player collides with character, stop their movement
                    if (playerRect.Intersects(characterRect))
                    {
                        motion = Vector2.Zero;
                        break;
                    }
                }

                Vector2 newPosition = player.Sprite.Position + motion;
                player.Sprite.Position = newPosition;
                player.Sprite.IsAnimating = true;
                player.Sprite.LockToMap(new Point(world.CurrentMap.WidthInPixels, world.CurrentMap.HeightInPixels));
            }
            else
            {
                player.Sprite.IsAnimating = false;
            }

            camera.LockToSprite(world.CurrentMap, player.Sprite, Game1.ScreenRectangle);
            player.Sprite.Update(gameTime);

            if (Xin.CheckKeyReleased(Keys.Space) || Xin.CheckKeyReleased(Keys.Enter))
            {
                foreach (string characterName in world.CurrentMap.Characters.Keys)
                {
                    ICharacter character = CharacterManager.Instance.GetCharacter(characterName);

                    float distance = Vector2.Distance(player.Sprite.Center, character.Sprite.Center);

                    // if player is close to character
                    if (Math.Abs(distance) < 72f)
                    {

                        // get the main game conversation state controller.   one per game
                        IConversationState conversationState = (IConversationState)GameRef.Services.GetService(typeof(IConversationState));

                        manager.PushState(
                            (ConversationState)conversationState,
                            PlayerIndexInControl);

                        conversationState.SetConversation(player, character);
                        conversationState.StartConversation();
                    }
                }
                foreach (Rectangle r in world.CurrentMap.PortalLayer.Portals.Keys)
                {
                    Portal p = world.CurrentMap.PortalLayer.Portals[r];
                    float distance = Vector2.Distance(
                    player.Sprite.Center,
                    new Vector2(
                    r.X * Engine.TileWidth + Engine.TileWidth / 2,
                    r.Y * Engine.TileHeight + Engine.TileHeight / 2));
                    if (Math.Abs(distance) < 64f)
                    {
                        world.ChangeMap(p.DestinationLevel, new Rectangle(p.DestinationTile.X, p.DestinationTile.Y, 32, 32));
                        player.Position = new Vector2(
                            p.DestinationTile.X * Engine.TileWidth,
                            p.DestinationTile.Y * Engine.TileHeight);
                        camera.LockToSprite(world.CurrentMap, player.Sprite, Game1.ScreenRectangle);
                        return;
                    }
                }
            }
            if (Xin.CheckKeyReleased(Keys.B))
            {
                foreach (string s in world.CurrentMap.Characters.Keys)
                {
                    ICharacter c = CharacterManager.Instance.GetCharacter(s);
                    float distance = Vector2.Distance(player.Sprite.Center, c.Sprite.Center);
                    if (Math.Abs(distance) < 72f && !c.Battled)
                    {
                        GameRef.BattleState.SetAvatars(player.CurrentAvatar, c.BattleAvatar);
                        manager.PushState(
                        (BattleState)GameRef.BattleState,
                        PlayerIndexInControl);
                        c.Battled = true;
                    }
                }
            }

            // TODO:  Save game data as json.  Create GameSave class that has all necessary objects and serialize it.
            // F1 = Save Game
            // Produces:
            // avatar.sav
            // level1Lance,teacherone,WalkDown.Water,Water,0,100,1,9,12,10,50,-3,Tackle,Block*Marissa,teachertwo,WalkDown,MarissaHello,0-Wind,Wind,0,100,1,10,10,12,50,50,Tackle,Block.Earth,Earth,0,100,1,10,10,9,60,60,Tackle,Block
            if (Xin.CheckKeyReleased(Keys.F1))
            {
                FileStream stream = new FileStream("avatars.sav", FileMode.Create, FileAccess.Write);
                BinaryWriter writer = new BinaryWriter(stream);
                world.Save(writer);
                player.Save(writer);
                writer.Close();
                stream.Close();
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            if (world.CurrentMap != null && camera != null)
                world.CurrentMap.Draw(gameTime, GameRef.SpriteBatch, camera);
            GameRef.SpriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null,
                null,
                null,
                camera.Transformation);
            player.Sprite.Draw(gameTime, GameRef.SpriteBatch);
            GameRef.SpriteBatch.End();
        }

        

        protected override void LoadContent()
        {
        }

        public void SetUpNewGame()
        {
            Texture2D spriteSheet = content.Load<Texture2D>(@"PlayerSprites\maleplayer");
            TileMap map = null;
            world = new World();

            // player initialize
            // TODO:  Let player enter their own name
            player = new Player(GameRef, "Wesley", false, spriteSheet);
            player.AddAvatar("fire", AvatarManager.GetAvatar("fire"));
            player.SetAvatar("fire");

            Texture2D tiles = GameRef.Content.Load<Texture2D>(@"Tiles\tileset1");
            TileSet set = new TileSet(8, 8, 32, 32);
            set.Texture = tiles;
            TileLayer background = new TileLayer(200, 200);
            TileLayer edge = new TileLayer(200, 200);
            TileLayer building = new TileLayer(200, 200);
            TileLayer decor = new TileLayer(200, 200);

            // configure map 1
            map = new TileMap(set, background, edge, building, decor, "test-map");
            map.FillEdges();
            map.FillBuilding();
            map.FillDecoration();
            building.SetTile(4, 4, 18);

            ConversationManager.CreateConversations(GameRef);

            // TODO:  Build characters from data file
            ICharacter teacherOne = Character.FromString(GameRef,
                "Lance,teacherone,WalkDown,teacherone,water");
            ICharacter teacherTwo = PCharacter.FromString(GameRef,
                "Marissa,teachertwo,WalkDown,tearchertwo,0,wind,earth");

            teacherOne.SetConversation("LanceHello");
            teacherTwo.SetConversation("MarissaHello");

            GameRef.CharacterManager.AddCharacter("teacherone", teacherOne);
            GameRef.CharacterManager.AddCharacter("teachertwo", teacherTwo);

            map.Characters.Add("teacherone", new Point(0, 4));
            map.Characters.Add("teachertwo", new Point(4, 0));

            map.PortalLayer.Portals.Add(Rectangle.Empty, new Portal(Point.Zero, Point.Zero, "level1"));
            map.PortalLayer.Portals.Add(new Rectangle(4, 4, 32, 32), new Portal(new Point(4, 4), new
                Point(10, 10), "inside"));

            world.AddMap("level1", map);
            world.ChangeMap("level1", Rectangle.Empty);

            // configure map 2 
            background = new TileLayer(20, 20, 23);
            edge = new TileLayer(20, 20);
            building = new TileLayer(20, 20);
            decor = new TileLayer(20, 20);
            map = new TileMap(set, background, edge, building, decor, "inside");
            map.FillEdges();
            map.FillBuilding();
            map.FillDecoration();
            map.BuildingLayer.SetTile(9, 19, 18);
            map.PortalLayer.Portals.Add(new Rectangle(9, 19, 32, 32), new Portal(new Point(9, 19), new
                Point(4, 4), "level1"));
            world.AddMap("inside", map);

            camera = new Camera();
        }

        public void LoadExistingGame()
        {
        }

        public void StartGame()
        {
        }
    }
}

