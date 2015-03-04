using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace super_mario
{
    public class ObjectHandler
    {
        List<Mushroom> mushroomList = new List<Mushroom>();
        public static Texture2D mushroomImage;
        Vector2 mushroomPos;

        ContentManager content;

        bool spawnMushroom, spawnStar;

        public Vector2 MushroomPos
        {
            set { mushroomPos = value; }
        }

        public bool SpawnMushroom
        {
            set { spawnMushroom = value; }
        }

        public bool SpawnStar
        {
            set { spawnStar = value; }
        }

        public void Update(ref Player player)
        {
            if (spawnMushroom == true)
            {
                mushroomList.Add(new Mushroom(mushroomPos, mushroomImage));
                spawnMushroom = false;
            }

            if(spawnStar == true)
            {
                
            }

            foreach (Mushroom m in mushroomList)
                m.Update(ref player);
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < mushroomList.Count; i++)
            {
                spriteBatch.Draw(mushroomList[i].image, mushroomList[i].position, Color.White);
            }
        }
    }
}
