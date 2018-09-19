using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Sapper.Scripts.Controllers
{
    public class CellController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public int cellXPos;
        public int cellYPos;

        private bool _isOver = false;
        private bool _isFlagged = false;

        private void Update()
        {
            if (!_isOver) return;

            if (Input.GetMouseButtonDown(0))
            {
                if (_isFlagged) return;

                OpenCell();
            }

            else if (Input.GetMouseButtonDown(1))
            {
                FlagCell();
            }
        }

        public void OnOpenCell()
        {
            transform.GetChild(1).gameObject.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _isOver = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _isOver = false;
        }

        private void FlagCell()
        {
            GameObject childObject = transform.GetChild(2).gameObject;
            childObject.SetActive(!childObject.activeSelf);
            FindObjectOfType<GameManager>().FlagCell(cellXPos, cellYPos);

            _isFlagged = childObject.activeSelf;
        }

        private void OpenCell()
        {
            FindObjectOfType<GameManager>().OpenCell(cellXPos, cellYPos);
        }
    }
}
