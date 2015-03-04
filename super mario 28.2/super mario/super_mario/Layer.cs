using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace super_mario
{
    public class Layer
    {
        public List<List<Tile>> tiles;
        List<List<string>> attributes, contents;
        List<string> motion, solid, special;
        FileManager fileManager;
        ContentManager content;
        Texture2D tileSheet;
        string[] getMotion;
        string TileMoveSpeed = "";
        Layer layer;

        static public Vector2 TileDimensions
        {
            get { return new Vector2(16, 16); }
        }

        public void LoadContent(Map map, string layerID)
        {
            tiles = new List<List<Tile>>();
            attributes = new List<List<string>>();
            contents = new List<List<string>>();
            motion = new List<string>();
            solid = new List<string>();
            special = new List<string>();
            fileManager = new FileManager();
            layer = this;
            content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");

            fileManager.LoadContent("Load/Maps/" + map.ID + ".cme", attributes, contents, layerID);

            int indexY = 0;

            for (int i = 0; i < attributes.Count; i++)
            {
                for (int j = 0; j < attributes[i].Count; j++)
                {
                    switch (attributes[i][j])
                    {
                        case "TileSet":
                            tileSheet = content.Load<Texture2D>("TileSets/" + contents[i][j]);
                            break;
                        case "Solid":
                            solid.Add(contents[i][j]);
                            break;
                        case "Motion":
                            motion.Add(contents[i][j]);
                            break;
                        case"Special":
                            special.Add(contents[i][j]);
                            break;
                        case"MoveSpeed":
                            TileMoveSpeed = contents[i][j];
                            break;
                        case "StartLayer":
                            List<Tile> tempTiles = new List<Tile>();
                            Tile.Motion tempMotion = Tile.Motion.Static;
                            Tile.State tempState;
                            Tile.Specials tempSpecials = Tile.Specials.Normal;

                            for (int k = 0; k < contents[i].Count; k++)
                            {
                                string[] split = contents[i][k].Split(',');
                                tempTiles.Add(new Tile());

                                if (solid.Contains(contents[i][k]))
                                    tempState = Tile.State.Solid;
                                else
                                    tempState = Tile.State.Passive;

                                foreach (string m in motion)
                                {
                                    getMotion = m.Split(':');
                                    if (getMotion[0] == contents[i][k])
                                    {
                                        tempMotion = (Tile.Motion)Enum.Parse(typeof(Tile.Motion), getMotion[1]);
                                        break;
                                    }
                                }

                                foreach (string s in special)
                                {
                                    getMotion = s.Split(':');
                                    if (getMotion[0] == contents[i][k])
                                    {
                                        tempSpecials = (Tile.Specials)Enum.Parse(typeof(Tile.Specials), getMotion[1]);
                                        break;
                                    }
                                }
                                float tempSpeed;
                                tempSpeed = float.Parse(TileMoveSpeed);

                                tempTiles[k].SetTile(tempState, tempMotion, tempSpecials, new Vector2(k * 16, indexY * 16), tileSheet,
                                    new Rectangle(int.Parse(split[0]) * 16, int.Parse(split[1]) * 16, 16, 16), tempSpeed);
                            }

                            tiles.Add(tempTiles);
                            indexY++;
                            break;
                    }
                }
            }
        }

        public void Update(GameTime gameTime, ref Player player)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                for (int j = 0; j < tiles[i].Count; j++)
                {
                    tiles[i][j].Update(gameTime, ref player, ref layer);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                for (int j = 0; j < tiles[i].Count; j++)
                    tiles[i][j].Draw(spriteBatch);
            }
        }
    }
}
