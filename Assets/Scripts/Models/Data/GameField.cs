using UnityEngine;
using System.Collections.Generic;

namespace Sapper.Scripts.Models.Data
{
    public class GameField
    {
        public IBaseCell[,] gameField;
        public List<IBaseCell> bombCells = new List<IBaseCell>();

        private int _bombsInField;
        private int _fieldWidth;
        private int _fieldHeigth;

        public GameField(int width, int heigth, int bombsCount)
        {
            gameField = new IBaseCell[width, heigth];

            _bombsInField = bombsCount;
            _fieldWidth = width;
            _fieldHeigth = heigth;

            GameFieldPreparation(width, heigth);
        }

        private void GameFieldPreparation(int fieldHeigth, int fieldWidth)
        {

            for (int i = 0; i < fieldWidth; i++)
            {
                for (int j = 0; j < fieldHeigth; j++)
                {
                    gameField[i, j] = new EmptyCell();

                    gameField[i, j].PosX = i;
                    gameField[i, j].PosY = j;
                }
            }

            GenerateBombs();
        }

        private void GenerateBombs()
        {
            Vector2 bombPos;
            for (int i = 0; i < _bombsInField; i++)
            {
                bombPos = GetRandomBombPos();

                bombCells.Add(gameField[(int)bombPos.x, (int)bombPos.y] = new BombCell());

                gameField[(int)bombPos.x, (int)bombPos.y].PosX = (int)bombPos.x;
                gameField[(int)bombPos.x, (int)bombPos.y].PosY = (int)bombPos.y;

                AddBombToNearbyCells((int)bombPos.x, (int)bombPos.y);
            }
        }

        private Vector2 GetRandomBombPos()
        {
            Vector2 randomBombPos = new Vector2();
            System.Random rnd = new System.Random();

            while (true)
            {
                randomBombPos.x = rnd.Next(0, _fieldHeigth);
                randomBombPos.y = rnd.Next(0, _fieldWidth);

                if (gameField[(int)randomBombPos.x, (int)randomBombPos.y].GetType() != typeof(BombCell))
                {
                    break;
                }
            }

            return randomBombPos;
        }

        private void AddBombToNearbyCells(int bombXPos, int bombYPos)
        {
            if (bombXPos - 1 >= 0)
            {
                if (gameField[bombXPos - 1, bombYPos] is EmptyCell)
                {
                    (gameField[bombXPos - 1, bombYPos] as EmptyCell).bombsNearby++;
                }
            }

            if (bombXPos + 1 < _fieldHeigth)
            {
                if (gameField[bombXPos + 1, bombYPos] is EmptyCell)
                {
                    (gameField[bombXPos + 1, bombYPos] as EmptyCell).bombsNearby++;
                }
            }

            if (bombYPos - 1 >= 0)
            {
                if (gameField[bombXPos, bombYPos - 1] is EmptyCell)
                {
                    (gameField[bombXPos, bombYPos - 1] as EmptyCell).bombsNearby++;
                }
            }

            if (bombYPos + 1 < _fieldWidth)
            {
                if (gameField[bombXPos, bombYPos + 1] is EmptyCell)
                {
                    (gameField[bombXPos, bombYPos + 1] as EmptyCell).bombsNearby++;
                }
            }

            if (bombXPos - 1 >= 0 && bombYPos - 1 >= 0)
            {
                if (gameField[bombXPos - 1, bombYPos - 1] is EmptyCell)
                {
                    (gameField[bombXPos - 1, bombYPos - 1] as EmptyCell).bombsNearby++;
                }
            }

            if (bombXPos - 1 >= 0 && bombYPos + 1 < _fieldWidth)
            {
                if (gameField[bombXPos - 1, bombYPos + 1] is EmptyCell)
                {
                    (gameField[bombXPos - 1, bombYPos + 1] as EmptyCell).bombsNearby++;
                }
            }

            if (bombXPos + 1 < _fieldHeigth && bombYPos + 1 < _fieldWidth)
            {
                if (gameField[bombXPos + 1, bombYPos + 1] is EmptyCell)
                {
                    (gameField[bombXPos + 1, bombYPos + 1] as EmptyCell).bombsNearby++;
                }
            }

            if (bombXPos + 1 < _fieldHeigth && bombYPos - 1 >= 0)
            {
                if (gameField[bombXPos + 1, bombYPos - 1] is EmptyCell)
                {
                    (gameField[bombXPos + 1, bombYPos - 1] as EmptyCell).bombsNearby++;
                }
            }
        }
    }
}