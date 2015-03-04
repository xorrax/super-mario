using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace super_mario
{
    public class Mushroom
    {
        public Vector2 position;
        public Texture2D image;
        ContentManager content;

        public Rectangle GetRectangle()
        {
            return new Rectangle((int)position.X, (int)position.Y, image.Width, image.Height);
        }

        public Mushroom(Vector2 pos, Texture2D image)
        {
            this.position = pos;
            this.image = image;
        }

        public void LoadContent(ContentManager content)
        {
            this.content = new ContentManager(content.ServiceProvider, "content");
            position = Vector2.Zero;
            image = this.content.Load<Texture2D>("Objects/mushroom");
        }

        public void Update(ref Player player)
        {
            if (player.Position.X > this.position.X)
                position.X += 1;
            else
                position.X -= 1;
        }

        //public void Draw(SpriteBatch spriteBatch)
        //{
        //    spriteBatch.Draw(image, position, Color.White);
        //}
    }
}
