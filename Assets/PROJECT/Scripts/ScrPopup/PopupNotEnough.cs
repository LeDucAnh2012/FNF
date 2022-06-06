using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FridayNightFunkin.UI
{
    public class PopupNotEnough : PanelBase
    {
        [SerializeField] private Text txtNotEnough;
        public override void Show()
        {
            base.Show();
            Invoke(nameof(Hide), 2);
        }
        public override void Hide()
        {
            base.Hide();
        }
        public void ShowPopup(string _str)
        {
            txtNotEnough.text = _str;
            Show();
        }
        public void OnClickHide()
        {
            Hide();
        }
    }
}