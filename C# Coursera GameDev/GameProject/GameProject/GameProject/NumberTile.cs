using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameProject
{
    /// <remarks>
    /// A number tile
    /// </remarks>
    class NumberTile
    {
        #region Fields

        // original length of each side of the tile
        int originalSideLength;

        // original X and Y of tile (will be used to shrink into center instead of upper-left corner)
        int originalX;
        int originalY;

        // whether or not this tile is the correct number
        bool isCorrectNumber;
        bool numberGuessed;

        // drawing support
        Texture2D texture;
        Texture2D blinkTexture;
        Texture2D curTexture;
        Rectangle drawRectangle;
        Rectangle sourceRectangle;
        Boolean visible;
        Boolean blinking;
        Boolean shringing;
        Boolean clickStarted;
        Boolean buttonReleased;

        SoundBank soundBank;

        // blinking support
        const int TOTAL_BLINK_MILLISECONDS = 4000;
        int elapsedBlinkMilliseconds = 0;
        const int FRAME_BLINK_MILLISECONDS = 500;
        int elapsedFrameMilliseconds = 0;
        const int TOTAL_SHRINK_MILLISECONDS = 1000;
        int elapsedShrinkMilliseconds = 0;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="contentManager">the content manager</param>
        /// <param name="center">the center of the tile</param>
        /// <param name="sideLength">the side length for the tile</param>
        /// <param name="number">the number for the tile</param>
        /// <param name="correctNumber">the correct number</param>
        /// <param name="soundBank">the sound bank for playing cues</param>
        public NumberTile(ContentManager contentManager, Vector2 center, int sideLength,
            int number, int correctNumber, SoundBank _soundBank)
        {
            // set original side length field
            this.originalSideLength = sideLength;

            // set sound bank field
            soundBank = _soundBank;

            // load content for the tile and create draw rectangle
            LoadContent(contentManager, number);

            originalX = (int)center.X - sideLength / 2;
            originalY = (int)center.Y - sideLength / 2;
            drawRectangle = new Rectangle(originalX, originalY, sideLength, sideLength);

            // set isCorrectNumber flag
            isCorrectNumber = number == correctNumber;
            numberGuessed = false;
            visible = true;
            blinking = false;
            shringing = false;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Updates the tile based on game time and mouse state
        /// </summary>
        /// <param name="gameTime">the current GameTime</param>
        /// <param name="mouse">the current mouse state</param>
        /// <return>true if the correct number was guessed, false otherwise</return>
        public bool Update(GameTime gameTime, MouseState mouse)
        {
            float shrinkRatio;
            int newSideLength;

            if (blinking)
            {
                elapsedBlinkMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
                if (elapsedBlinkMilliseconds > TOTAL_BLINK_MILLISECONDS)
                    return true;

                elapsedFrameMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
                if (elapsedFrameMilliseconds > FRAME_BLINK_MILLISECONDS)
                {
                    elapsedFrameMilliseconds = 0;
                    if (sourceRectangle.X == 0)
                        sourceRectangle.X = curTexture.Width / 2;
                    else
                        sourceRectangle.X = 0;
                }
            }
            else if (shringing)
            {
                elapsedShrinkMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
                shrinkRatio = (float)(TOTAL_SHRINK_MILLISECONDS - elapsedShrinkMilliseconds) / TOTAL_SHRINK_MILLISECONDS;
                newSideLength = (int)(originalSideLength * shrinkRatio);
                drawRectangle.X = originalX + (originalSideLength - newSideLength) / 2;
                drawRectangle.Y = originalY + (originalSideLength - newSideLength) / 2;
                drawRectangle.Width = newSideLength;
                drawRectangle.Height = newSideLength;
                if (newSideLength <= 0)
                {
                    visible = false;
                }
            }
            else 
            {
                if (drawRectangle.Contains(mouse.X, mouse.Y)
                    && !numberGuessed)
                {
                    sourceRectangle.X = texture.Width / 2;
                    // check for click started on button
                    if (mouse.LeftButton == ButtonState.Pressed &&
                        buttonReleased)
                    {
                        clickStarted = true;
                        buttonReleased = false;
                    }
                    else if (mouse.LeftButton == ButtonState.Released)
                    {
                        buttonReleased = true;

                        // if click finished on button, change game state
                        if (clickStarted)
                        {
                            if (isCorrectNumber)
                            {
                                blinking = true;
                                curTexture = blinkTexture;
                                sourceRectangle.X = 0;
                                numberGuessed = true;
                                soundBank.PlayCue("correctGuess");
                            }
                            else
                            {
                                shringing = true;
                                soundBank.PlayCue("incorrectGuess");
                            }

                            clickStarted = false;
                        }
                    }
                }
                else
                    sourceRectangle.X = 0;
            }

            // if we get here, return false
            return false;
        }

        /// <summary>
        /// Draws the number tile
        /// </summary>
        /// <param name="spriteBatch">the SpriteBatch to use for the drawing</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // draw the tile
            if (visible)
                spriteBatch.Draw(curTexture, drawRectangle, sourceRectangle, Color.White);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Loads the content for the tile
        /// </summary>
        /// <param name="contentManager">the content manager</param>
        /// <param name="number">the tile number</param>
        private void LoadContent(ContentManager contentManager, int number)
        {
            // convert the number to a string
            string numberString = ConvertIntToString(number);

            // load content for the tile and set source rectangle
            texture = contentManager.Load<Texture2D>("graphics/" + this.ConvertIntToString(number));
            blinkTexture = contentManager.Load<Texture2D>("graphics/blinking" + this.ConvertIntToString(number));
            curTexture = texture;
            sourceRectangle = new Rectangle(0, 0, texture.Width / 2, texture.Height);
        }

        /// <summary>
        /// Converts an integer to a string for the corresponding number
        /// </summary>
        /// <param name="number">the integer to convert</param>
        /// <returns>the string for the corresponding number</returns>
        private String ConvertIntToString(int number)
        {
            switch (number)
            {
                case 1:
                    return "one";
                case 2:
                    return "two";
                case 3:
                    return "three";
                case 4:
                    return "four";
                case 5:
                    return "five";
                case 6:
                    return "six";
                case 7:
                    return "seven";
                case 8:
                    return "eight";
                case 9:
                    return "nine";
                default:
                    throw new Exception("Unsupported number for number tile");
            }

        }

        #endregion
    }
}
