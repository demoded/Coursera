using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using XnaCards;

namespace ProgrammingAssignment6
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        const int WIDTH  = 800;
        const int HEIGHT = 600;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Deck deck;
        
        // keep track of game state and current winner
        static GameState gameState = GameState.Play;

        // hands and battle piles for the players
        WarHand player1, player2;
        WarBattlePile battlePile1, battlePile2;
        Card playCard1, playCard2;

        // winner messages for players
        WinnerMessage playerOneWin, playerTwoWin;

        // menu buttons
        MenuButton flipButton, quitButton, collectButton;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = WIDTH;
            graphics.PreferredBackBufferHeight = HEIGHT;

            // make mouse visible and set resolution
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // create the deck object and shuffle
            deck = new Deck(Content, 100, 100);
            deck.Shuffle();
            for (int i = 1; i < 45; i++) deck.TakeTopCard();

            // create the player hands and fully deal the deck into the hands
            player1 = new WarHand((int)(graphics.PreferredBackBufferWidth / 2), (int)(graphics.PreferredBackBufferHeight * 0.2));
            player2 = new WarHand((int)(graphics.PreferredBackBufferWidth / 2), (int)(graphics.PreferredBackBufferHeight * 0.8));

            while ( !deck.Empty )
            {
                player1.AddCard(deck.TakeTopCard());
                player2.AddCard(deck.TakeTopCard());
            }

            // create the player battle piles
            battlePile1 = new WarBattlePile((int)(graphics.PreferredBackBufferWidth / 2), (int)(graphics.PreferredBackBufferHeight * 0.4));
            battlePile2 = new WarBattlePile((int)(graphics.PreferredBackBufferWidth / 2), (int)(graphics.PreferredBackBufferHeight * 0.6));
            
            // create the player winner messages
            playerOneWin = new WinnerMessage(Content, (int)(graphics.PreferredBackBufferWidth * 0.7), (int)(graphics.PreferredBackBufferHeight * 0.2));
            playerTwoWin = new WinnerMessage(Content, (int)(graphics.PreferredBackBufferWidth * 0.7), (int)(graphics.PreferredBackBufferHeight * 0.8));

            // create the menu buttons
            flipButton = new MenuButton(Content, "flipbutton", (int)(graphics.PreferredBackBufferWidth * 0.2), (int)(graphics.PreferredBackBufferHeight * 0.3), GameState.Flip);
            collectButton = new MenuButton(Content, "collectwinningsbutton", (int)(graphics.PreferredBackBufferWidth * 0.2), (int)(graphics.PreferredBackBufferHeight * 0.5), GameState.CollectWinnings);
            quitButton = new MenuButton(Content, "quitbutton", (int)(graphics.PreferredBackBufferWidth * 0.2), (int)(graphics.PreferredBackBufferHeight * 0.7), GameState.Quit);
            flipButton.Visible = false;
            collectButton.Visible = false;
            quitButton.Visible = false;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            MouseState mouse = Mouse.GetState();

            // update the menu buttons
            flipButton.Update(mouse);
            collectButton.Update(mouse);
            quitButton.Update(mouse);
            
            switch (gameState)
            { 
                case(GameState.Play):
                    flipButton.Visible = true;
                    collectButton.Visible = false;
                    playerOneWin.Visible = false;
                    playerTwoWin.Visible = false;
                    quitButton.Visible = true;
                    break;

                case(GameState.Flip):
                    flipButton.Visible = false;
                    collectButton.Visible = true;
                    break;

                case(GameState.Quit):
                    Exit();
                    break;
            }
 
            // update based on game state
            switch (gameState)
            { 
                case(GameState.Flip):
                    if (battlePile1.Empty && battlePile2.Empty)
                    {
                        takeCardsForBattle();
                        while (playCard1.WarValue == playCard2.WarValue)
                        {
                            takeCardsForBattle();
                        }
                    }
                    if (playCard1.WarValue > playCard2.WarValue)
                    {
                        //player1 win
                        playerOneWin.Visible = true;
                    }
                    else
                    {
                        //player2 win
                        playerTwoWin.Visible = true;
                    }

                    break;

                case(GameState.CollectWinnings):
                    if (playCard1.WarValue > playCard2.WarValue)
                    {
                        player1.AddCards(battlePile1);
                        player1.AddCards(battlePile2);
                    }
                    else
                    {
                        player2.AddCards(battlePile1);
                        player2.AddCards(battlePile2);
                    }

                    if (player1.Empty || player2.Empty)
                    {
                        flipButton.Visible = false;
                        collectButton.Visible = false;
                    }
                    else
                    {
                        ChangeState(GameState.Play);
                    }

                    break;
            }
 
            base.Update(gameTime);
        }

        /// <summary>
        ///     take one card from player1 & player2, make cards visible
        ///     and put into the battle piles
        /// </summary>
        private void takeCardsForBattle()
        {
            //take one card from player1 to his battle pile
            playCard1 = player1.TakeTopCard();
            playCard1.FlipOver();

            //take one card from player2 to his battle pile
            playCard2 = player2.TakeTopCard();
            playCard2.FlipOver();

            battlePile2.AddCard(playCard2);
            battlePile1.AddCard(playCard1);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Goldenrod);
            spriteBatch.Begin();
            // draw the game objects
            player1.Draw(spriteBatch);
            player2.Draw(spriteBatch);
            battlePile1.Draw(spriteBatch);
            battlePile2.Draw(spriteBatch);

            // draw the winner messages
            playerOneWin.Draw(spriteBatch);
            playerTwoWin.Draw(spriteBatch);
 
            // draw the menu buttons
            flipButton.Draw(spriteBatch);
            collectButton.Draw(spriteBatch);
            quitButton.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Changes the state of the game
        /// </summary>
        /// <param name="newState">the new game state</param>
        public static void ChangeState(GameState newState)
        {
            gameState = newState;
        }
    }
}
