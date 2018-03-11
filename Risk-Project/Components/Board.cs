﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Risk_Project.Components
{
    class Board : SimpleDrawableGameComponent
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
        public Action CurrentAction;
        public Phase CurrentPhase;
        #endregion

        #region Constructor
        public Board()
        {
            
        }
        #endregion

        #region Methods
        public override void Draw(GameTime gameTime)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public void Init()
        {

        }

        private void DistributePlayers()
        {

        }

        private void DistributePlayerTerritories()
        {

        }

        private void DistributeTerritoryUnits()
        {

        }

        public void SetPlayerOrder()
        {

        }

        public void SetMoveOrder()
        {

        }

        public void Commit()
        {

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