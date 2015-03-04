﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace super_mario
{
    public class Entity
    {
        protected int health;
        protected Animation moveAnimation;
        protected SpriteSheetAnimation spriteSheetAnimation;
        protected float moveSpeed, gravity;

        protected ContentManager content;
        protected FileManager fileManager;

        protected Texture2D image;
        protected Vector2 position, velocity, prevPos;

        protected List<List<string>> attributes, contents;

        public bool activateGravity, syncTilePosition;


        public Vector2 PrevPosition
        {
            get { return prevPos; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public bool ActivateGravity
        {
            set { activateGravity = value; }
        }

        public bool SyncTilePosition
        {
            get { return syncTilePosition; }
            set { syncTilePosition = value; }
        }

        public Animation Animation
        {
            get { return moveAnimation; }
        }
        public virtual void LoadContent(ContentManager content, InputManager input)
        {
            this.content = new ContentManager(content.ServiceProvider, "Content");
            attributes = new List<List<string>>();
            contents = new List<List<string>>();
        }

        public virtual void UnloadContent()
        {
            content.Unload();
        }

        public virtual void Update(GameTime gameTime, InputManager input, Collision col, Layer layer)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
