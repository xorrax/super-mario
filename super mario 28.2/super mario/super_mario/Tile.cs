using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace super_mario
{
    public class Tile
    {
        public enum State { Solid, Passive };
        public enum Motion { Static, Horizontal, Vertical };
        public enum Specials { Destroyable, PowerUp, Normal};

        State state;
        Motion motion;
        Specials specials;
        Vector2 position, prevPosition, velocity;
        Texture2D tileImage;
        public FloatRect rect = new FloatRect(0, 0, 0, 0);
        FloatRect prevTile = new FloatRect(0, 0, 0, 0);

        List<Mushroom> mushroomList;

        float range, moveSpeed;
        int counter;
        bool increase, onTile;

        Animation animation;

        public Rectangle GetRectangle()
        {
            return new Rectangle((int)position.X, (int)position.Y, (int)tileImage.Width, (int)tileImage.Height);
        }

        private Texture2D CropImage(Texture2D tileSheet, Rectangle tileArea)
        {
            Texture2D croppedImage = new Texture2D(tileSheet.GraphicsDevice, tileArea.Width, tileArea.Height);

            Color[] tileSheetData = new Color[tileSheet.Width * tileSheet.Height];
            Color[] croppedImageData = new Color[croppedImage.Width * croppedImage.Height];

            tileSheet.GetData<Color>(tileSheetData);

            int index = 0;
            for (int y = tileArea.Y; y < tileArea.Y + tileArea.Height; y++)
            {
                for (int x = tileArea.X; x < tileArea.X + tileArea.Width; x++)
                {
                    croppedImageData[index] = tileSheetData[y * tileSheet.Width + x];
                    index++;
                }
            }

            croppedImage.SetData<Color>(croppedImageData);

            return croppedImage;
        }

        public void SetTile(State state, Motion motion, Specials specials, Vector2 position, Texture2D tileSheet, Rectangle tileArea, float MoveSpeed)
        {
            this.state = state;
            this.motion = motion;
            this.specials = specials;
            this.position = position;
            increase = true;

            tileImage = CropImage(tileSheet, tileArea);
            range = 80;
            counter = 0;
            moveSpeed = MoveSpeed;
            animation = new Animation();
            animation.LoadContent(ScreenManager.Instance.Content, tileImage, "", position);
            onTile = false;
            velocity = Vector2.Zero;
        }

        public void Update(GameTime gameTime, ref Player player, ref Layer layer)
        {
            counter++;
            prevPosition = position;

            if (counter >= range)
            {
                counter = 0;
                increase = !increase;
            }

            if (motion == Motion.Horizontal)
            {
                if (increase)
                    velocity.X = moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                else
                    velocity.X = -moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (motion == Motion.Vertical)
            {
                if (increase)
                    velocity.Y = moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                else
                    velocity.Y = -moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            position += velocity;
            animation.Position = position;
            rect = new FloatRect(position.X, position.Y, Layer.TileDimensions.X, Layer.TileDimensions.Y);
            FloatRect prevPlayer = new FloatRect(player.PrevPosition.X, player.PrevPosition.Y, player.Animation.FrameWidth, player.Animation.FrameHeight);
            prevTile = new FloatRect(prevPosition.X, prevPosition.Y, Layer.TileDimensions.X, Layer.TileDimensions.Y);

            if (player.tileCol == true)
            {
                player.tileTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (player.tileTimer >= 6f)
                {
                    player.tileCol = false;
                    player.tileTimer = 0f;
                }
            }

            if (player.tileCol == false)
            {
                for (int i = 0; i < layer.tiles.Count; i++)
                {
                    for (int j = 0; j < layer.tiles[i].Count; j++)
                    {
                        if (layer.tiles[i][j].specials == Specials.Destroyable)
                        {
                            if (player.GetRectangle().Intersects(layer.tiles[i][j].GetRectangle()) && player.PrevPosition.Y > layer.tiles[i][j].position.Y)
                            {
                                layer.tiles[i].RemoveAt(j);
                                player.tileCol = true;
                                player.Velocity = new Vector2(player.Velocity.X, 0);
                            }
                        }

                        if (layer.tiles[i][j].specials == Specials.PowerUp)
                        {
                            if (player.GetRectangle().Intersects(layer.tiles[i][j].GetRectangle()) && player.PrevPosition.Y > layer.tiles[i][j].position.Y)
                            {
                                player.tileCol = true;
                                player.Velocity = new Vector2(player.Velocity.X, 0);
                            }
                        }
                    }
                }
            }
            
            


            if (onTile)
            {
                if (!player.SyncTilePosition)
                {
                    player.Position += velocity;
                    player.SyncTilePosition = true;
                }

                if(player.Rect.Right < rect.Left || player.Rect.Left > rect.Right || player.Rect.Bottom != rect.Top)
                {
                    onTile = false;
                    player.ActivateGravity = true;
                }
            }

            if(player.Rect.Intersects(rect) && state == State.Solid)
            {
                if(player.Rect.Bottom >= rect.Top && prevPlayer.Bottom <= prevTile.Top)
                {
                    player.Position = new Vector2(player.Position.X, position.Y - player.Animation.FrameHeight);
                    player.ActivateGravity = false;
                    onTile = true;
                }
                else if(player.Rect.Top <= rect.Bottom && prevPlayer.Top >= prevTile.Bottom)
                {
                    player.Position = new Vector2(player.Position.X, position.Y + Layer.TileDimensions.Y);
                    player.Velocity = new Vector2(player.Velocity.X, 0);
                    player.ActivateGravity = true;
                }
                else
                {
                    player.Position -= player.Velocity;
                }
            }

            player.Animation.Position = player.Position;
        }

        
        public void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);
        }
    }
}
