using Sapper.Scripts.Models.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Sapper.Scripts.Controllers
{
    public class WinManager : MonoBehaviour
    {

        public GameObject winPanel;

        public Image endGameImage;

        public Sprite winGameSprite;
        public Sprite loseGameSprite;

        public void DisplayWinPanel(GameStates state)
        {
            switch (state)
            {
                case GameStates.Win:
                    endGameImage.sprite = winGameSprite;
                    break;
                case GameStates.Lose:
                    endGameImage.sprite = loseGameSprite;
                    break;
            }

            winPanel.SetActive(true);
        }

        public void HideWinPanel()
        {
            winPanel.SetActive(false);
        }

    }
}
