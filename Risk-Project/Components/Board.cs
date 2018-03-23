﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Gui;
using Risk_Project.World_Objects;

namespace Risk_Project.Components
{
    public class Board : DrawableGameComponent
    {
        #region Properties
        public enum Action
        {
            Inspect,
            Deploy,
            Transfer,
            Wait
        }

        public enum Phase
        {
            Add,
            Move,
            Battle,
            Wait
        }

        public int Turns;
        public int CurrentTurn;
        public List<Continent> Continents;
        private List<Continent> DefaultContinents;
        public Action CurrentAction;
        public Phase CurrentPhase;
        #endregion

        #region Constructor
        public Board(Game game) : base(game)
        {
            //GuiSkin currentSkin = new GuiSkin();
            //GuiScreen currentScreen = new GuiScreen(currentSkin);

            Init();
        }
        #endregion

        #region Methods
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = Game.Services.GetService<SpriteBatch>();

            foreach (var Continent in Continents)
            {
                foreach (var Territory in Continent.Territories)
                {
                    Territory.Draw(spriteBatch);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var Continent in Continents)
            {
                foreach (var Territory in Continent.Territories)
                {
                    Territory.Update(gameTime);
                }
            }
        }

        public void Init()
        {
            Continents = new List<Continent>();

            CreateContinents();
            DistributePlayers();
        }

        private void CreateContinents()
        {
            // Create Continents
            Continent Europe = new Continent("Europe", Color.CornflowerBlue);
            Continent Asia = new Continent("Asia", Color.Orange);

            #region Create Territories
            List<Territory> EuropeTerritories = new List<Territory>()
            {
                new Territory("Ireland", CreateArmies(GameRoot.DEFAULT_ARMIES), 
                GameRoot.TextureResource["t001"], 
                GameRoot.TextureResource["t001o"], 
                Europe.OutlineColor)
            };

            List<Territory> AsiaTerritories = new List<Territory>()
            {
                new Territory("England", CreateArmies(GameRoot.DEFAULT_ARMIES), 
                GameRoot.TextureResource["t002"],
                GameRoot.TextureResource["t002o"],
                Asia.OutlineColor)
            };
            #endregion

            #region Set Territory Positions
            EuropeTerritories[0].Texture.Position = new Vector2(220, 200);
            AsiaTerritories[0].Texture.Position = new Vector2(460, 200);
            #endregion

            #region Add Territories to Continents
            Europe.AddTerritories(EuropeTerritories);
            Asia.AddTerritories(AsiaTerritories);
            #endregion

            // Add to boards list of Continents
            Continents.Add(Europe);
            Continents.Add(Asia);

            // Store default Continents for Game Restart
            // This helps us revert back to the initial continent state
            // i.e. when the continents were first created
            DefaultContinents = Continents;
        }

        private List<Unit> CreateArmies(int amount)
        {
            List<Unit> tempUnits = new List<Unit>();

            for (int i = 0; i < amount; i++)
            {
                tempUnits.Add(new Unit());
            }

            return tempUnits;
        }

        private void DistributePlayers()
        {
            foreach (Territory territory in Continents[0].Territories)
            {
                territory.Parent = GameRoot.Players[0];
                territory.UpdateColor();
            }

            foreach (Territory territory in Continents[1].Territories)
            {
                territory.Parent = GameRoot.Players[1];
                territory.UpdateColor();
            }
        }

        private void DistributeTerritoryUnits()
        {
            // Add units to players armies.
        }

        public void SetMoveOrder()
        {

        }

        public void Commit()
        {
            // When commit button is clicked, start movements, transfers and battles.
        }

        private void Battle()
        {

        }

        public void ResetBoard()
        {

        }
        #endregion
    }
}
