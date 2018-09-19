using Sapper.Scripts.Models.Data;
using Sapper.Scripts.Models.Enums;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Sapper.Scripts.Controllers
{
    public class GameManager : MonoBehaviour
    {
        public InGameTimer inGameTimer;
        public WinManager winManager;

        public Text bombsCountText;

        public Transform gameFieldParent;
        public GameObject bombCellPrefab;
        public GameObject emptyCellPrefab;

        [SerializeField]
        private int _fieldWidth;
        [SerializeField]
        private int _fieldHeigth;
        [SerializeField]
        private int _bombInGame;

        private int _bombsCount;

        private bool isFirstClick = true;

        private GameField _gameField;

        void Start()
        {
            PrepareGame();
        }

        public void LoseGame()
        {
            inGameTimer.StopTimer();
            winManager.DisplayWinPanel(GameStates.Lose);

            OpenAllClosedCells();
        }

        public void WinGame()
        {
            inGameTimer.StopTimer();
            winManager.DisplayWinPanel(GameStates.Win);

            OpenAllClosedCells();
        }

        public void ReplayGame()
        {
            winManager.HideWinPanel();

            PrepareGame();
        }

        public void OpenCell(int xPos, int yPos)
        {
            if (isFirstClick)
            {
                isFirstClick = !isFirstClick;
                inGameTimer.StartTimer();
            }

            var cell = _gameField.gameField[xPos, yPos];

            cell.OpenCell();

            if (cell is BombCell)
            {
                LoseGame();

                return;
            }

            if ((cell as EmptyCell).bombsNearby == 0)
            {
                OpenNearbyCell(_gameField.gameField[xPos, yPos]);
            }

        }

        public void FlagCell(int xPos, int yPos)
        {
            var cell = _gameField.gameField[xPos, yPos];

            if (_bombsCount == 0 && !cell.isFlagged) return;

            if (!cell.isFlagged)
            {
                _bombsCount--;
            }
            else
            {
                _bombsCount++;
            }

            cell.FlagCell();

            DisplayBombsCount();

            if (_bombsCount == 0 && IsWin())
            {
                WinGame();
            }

        }

        private void OpenAllClosedCells()
        {
            var cells = _gameField.gameField;

            for (int i = 0; i < _fieldHeigth; i++)
            {
                for (int j = 0; j < _fieldWidth; j++)
                {
                    if(!cells[i,j].isOpen)
                    {
                        cells[i, j].OpenCell();
                    }
                }
            }
        }

        private void PrepareGame()
        {
            _bombsCount = _bombInGame;

            _gameField = new GameField(_fieldHeigth, _fieldWidth, _bombInGame);

            isFirstClick = true;
            
            ClearGameField();
            InstantiateCells();
            DisplayBombsCount();
            inGameTimer.RefreshTimer();
        }

        private void DisplayBombsCount()
        {
            bombsCountText.text = _bombsCount.ToString();
        }

        private void OpenNearbyCell(IBaseCell firstEmptyCell)
        {
            List<IBaseCell> cells = new List<IBaseCell>();

            AddNerbyCellsToList(cells, firstEmptyCell);


            while (true)
            {
                if (cells.Count == 0)
                {
                    break;
                }

                AddNerbyCellsToList(cells, cells[0]);

                cells[0].OpenCell();
                cells.Remove(cells[0]);

            }
        }

        private void AddNerbyCellsToList(List<IBaseCell> cells, IBaseCell checkingCell)
        {
            IBaseCell currentCell;

            if ((checkingCell as EmptyCell).bombsNearby != 0) return;

            if (checkingCell.PosX - 1 >= 0)
            {
                currentCell = _gameField.gameField[checkingCell.PosX - 1, checkingCell.PosY];

                if (IsCanAddToList(cells, currentCell))
                    cells.Add(currentCell);
            }

            if (checkingCell.PosX + 1 < _fieldHeigth)
            {
                currentCell = _gameField.gameField[checkingCell.PosX + 1, checkingCell.PosY];

                if (IsCanAddToList(cells, currentCell))
                    cells.Add(currentCell);
            }

            if (checkingCell.PosY - 1 >= 0)
            {
                currentCell = _gameField.gameField[checkingCell.PosX, checkingCell.PosY - 1];

                if (IsCanAddToList(cells, currentCell))
                    cells.Add(currentCell);
            }

            if (checkingCell.PosY + 1 < _fieldWidth)
            {
                currentCell = _gameField.gameField[checkingCell.PosX, checkingCell.PosY + 1];

                if (IsCanAddToList(cells, currentCell))
                    cells.Add(currentCell);
            }

            if (checkingCell.PosX - 1 >= 0 && checkingCell.PosY - 1 >= 0)
            {
                currentCell = _gameField.gameField[checkingCell.PosX - 1, checkingCell.PosY - 1];

                if (IsCanAddToList(cells, currentCell))
                    cells.Add(currentCell);
            }

            if (checkingCell.PosX - 1 >= 0 && checkingCell.PosY + 1 < _fieldWidth)
            {
                currentCell = _gameField.gameField[checkingCell.PosX - 1, checkingCell.PosY + 1];

                if (IsCanAddToList(cells, currentCell))
                    cells.Add(currentCell);
            }

            if (checkingCell.PosX + 1 < _fieldHeigth && checkingCell.PosY - 1 >= 0)
            {
                currentCell = _gameField.gameField[checkingCell.PosX + 1, checkingCell.PosY - 1];

                if (IsCanAddToList(cells, currentCell))
                    cells.Add(currentCell);
            }

            if (checkingCell.PosX + 1 < _fieldHeigth && checkingCell.PosY + 1 < _fieldWidth)
            {
                currentCell = _gameField.gameField[checkingCell.PosX + 1, checkingCell.PosY + 1];

                if (IsCanAddToList(cells, currentCell))
                    cells.Add(currentCell);
            }
        }

        private bool IsCanAddToList(List<IBaseCell> cells, IBaseCell targetCell)
        {
            if (targetCell.isOpen || targetCell.isFlagged || targetCell is BombCell || cells.Contains(targetCell))
            {
                return false;
            }

            return true;
        }

        private void InstantiateCells()
        {
            GameObject cellObject;
            CellController cellController;
            int bombsCount;

            var gameField = _gameField.gameField;

            for (int i = 0; i < _fieldWidth; i++)
            {
                for (int j = 0; j < _fieldHeigth; j++)
                {
                    if (gameField[i, j] is EmptyCell)
                    {
                        cellObject = Instantiate(emptyCellPrefab, gameFieldParent);
                        bombsCount = (gameField[i, j] as EmptyCell).bombsNearby;
                        cellObject.GetComponentInChildren<Text>().text = bombsCount > 0 ? bombsCount.ToString() : "";
                    }
                    else
                    {
                        cellObject = Instantiate(bombCellPrefab, gameFieldParent);
                    }

                    cellController = cellObject.GetComponent<CellController>();

                    gameField[i, j].OnOpenCell = cellController.OnOpenCell;

                    cellController.cellXPos = i;
                    cellController.cellYPos = j;
                }
            }
        }

        private void ClearGameField()
        {
            int childCount = gameFieldParent.childCount;

            for(int i = 0; i < childCount; i++)
            {
                Destroy(gameFieldParent.GetChild(i).gameObject);
            }

            gameFieldParent.DetachChildren();
        }

        private bool IsWin()
        {
            var bombCells = _gameField.bombCells;

            for (int i = 0; i < bombCells.Count; i++)
            {
                if (!bombCells[i].isFlagged)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
