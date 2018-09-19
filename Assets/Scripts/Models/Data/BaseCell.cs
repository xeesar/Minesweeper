using System;

namespace Sapper.Scripts.Models.Data
{
    public interface IBaseCell
    {
        Action OnOpenCell { get; set; }

        void OpenCell();
        void FlagCell();

        bool isFlagged { get; set; }
        bool isOpen { get; set; }

        int PosX { get; set; }
        int PosY { get; set; }
    }
}
