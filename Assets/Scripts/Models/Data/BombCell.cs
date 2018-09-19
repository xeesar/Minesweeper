using System;

namespace Sapper.Scripts.Models.Data
{
    public class BombCell : IBaseCell
    {
        private bool _isBomb = false;

        public BombCell()
        {
            _isBomb = true;
        }

        public bool IsBomb { get { return _isBomb; } }

        public bool isFlagged { get; set; }
        public bool isOpen { get; set; }

        public int PosX { get; set; }
        public int PosY { get; set; }

        public Action OnOpenCell { get; set; }

        public void OpenCell()
        {
            isOpen = true;
            OnOpenCell();
        }

        public void FlagCell()
        {
            isFlagged = !isFlagged;
        }
    }
}
