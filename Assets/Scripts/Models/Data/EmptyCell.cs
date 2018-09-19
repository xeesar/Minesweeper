using System;

namespace Sapper.Scripts.Models.Data
{
    public class EmptyCell : IBaseCell
    {
        public int bombsNearby = 0;

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
