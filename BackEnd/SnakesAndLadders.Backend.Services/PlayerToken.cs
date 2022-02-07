﻿using SnakesAndLadders.BackEnd.Contracts.Constants;
using SnakesAndLadders.BackEnd.Contracts.Enums;
using SnakesAndLadders.BackEnd.Contracts.Models;
using SnakesAndLadders.BackEnd.Contracts.Services;
using System;

namespace SnakesAndLadders.BackEnd.Services
{
    public class PlayerToken : IPlayerToken
    {
        #region Internal vars
        private readonly IBoard _board;
        #endregion

        #region Properties
        public int Position { get; private set; }
        #endregion

        #region Events
        public event Action OnPlayerWins;
        #endregion

        #region Constructors
        public PlayerToken(IBoard board)
        {
            this._board = board;
            this.Reset();
        }
        #endregion

        #region Methods & Functions
        public void Move(int tiles)
        {
            if (tiles < 1)
                throw PlayerToken.FormatNegativeTilesToMoveException(argumentName: nameof(tiles), argumentValue: tiles);

            this.Position += tiles;

            this.NotifyWinEventIfPlayerIsOnFinishTile();
        }

        public void Reset() => this.Position = BoardConstants.START_TILE_NUMBER;

        private bool IsCurrentPositionSnakeOrLadderTile(out int targetTileNumber)
        {
            BoardTile tile = this._board.GetTileByNumberPosition(this.Position);
            targetTileNumber = 0;

            if (tile.type != BoardTileTypes.Normal)
                targetTileNumber = tile.jumpToTileNumber;

            return targetTileNumber > 0;
        }

        private void NotifyWinEventIfPlayerIsOnFinishTile()
        {
            if (this.Position == BoardConstants.FINISH_TILE_NUMBER)
                this.OnPlayerWins?.Invoke();
        }

        private static ArgumentException FormatNegativeTilesToMoveException(string argumentName, int argumentValue)
        {
            return new($"{nameof(IPlayerToken)}::{nameof(IPlayerToken.Move)}: The \"{argumentName}\" argument value must be a value over 0 ({argumentName}: {argumentValue}).");
        }
        #endregion
    }
}
