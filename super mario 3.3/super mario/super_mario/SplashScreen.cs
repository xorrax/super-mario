﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace super_mario
{
    public class SplashScreen : GameScreen
    {
        SpriteFont font;
        List<Animation> animation;
        List<Texture2D> images;

        FileManager fileManager;
        int imageNumber;

        FadeAnimation fadeAnimation;

        public override void LoadContent(ContentManager Content, InputManager inputManager)
        {
            base.LoadContent(Content, inputManager);
            if (font == null)
                font = this.content.Load<SpriteFont>("Font1");

            imageNumber = 0;
            fileManager = new FileManager();
            animation = new List<Animation>();
            images = new List<Texture2D>();

            fileManager.LoadContent("Load/Splash.cme", attributes, contents);

            for (int i = 0; i < attributes.Count; i++)
            {
                for (int j = 0; j < attributes[i].Count; j++)
                {
                    switch (attributes[i][j])
                    {
                        case "Image":
                            images.Add(this.content.Load<Texture2D>(contents[i][j]));
                            animation.Add(new FadeAnimation());
                            break;
                    }
                }
            }

            for (int i = 0; i < attributes.Count; i++)
            {
                animation[i].LoadContent(content, images[i], "", new Vector2(ScreenManager.Instance.Dimensions.X / 2 - images[i].Width / 2, ScreenManager.Instance.Dimensions.Y / 2 - images[i].Height / 2));
                animation[i].Scale = 1.0f;
                animation[i].IsActive = true;
            }
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            fileManager = null;
        }

        public override void Update(GameTime gameTime)
        {
            inputManager.Update();

            Animation a = animation[imageNumber];
            fadeAnimation.Update(gameTime, ref a);

            if (animation[imageNumber].Alpha == 0.0f)
                imageNumber++;

            if (imageNumber >= animation.Count - 1 || inputManager.KeyPressed(Keys.Escape))
            {
                if (animation[imageNumber].Alpha != 1.0f)
                    ScreenManager.Instance.AddScreen(new TitleScreen(), inputManager, animation[imageNumber].Alpha);
                else
                    ScreenManager.Instance.AddScreen(new TitleScreen(), inputManager);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            animation[imageNumber].Draw(spriteBatch);
        }
    }
}
