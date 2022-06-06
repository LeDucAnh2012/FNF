using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace FridayNightFunkin.UI
{
    public class PopupLoading : PanelBase
    {
        [SerializeField] private Image imgFilled;
        public override void Show()
        {
            base.Show();
        }
        public override void Hide()
        {
            base.Hide();
        }
        public void ShowPanel(float valueEnd, float timeDuration)
        {
            FilledImage(valueEnd, timeDuration);
            Show();
        }
        private void FilledImage(float valueEnd, float timeDuration)
        {
            imgFilled.DOFillAmount(valueEnd, timeDuration);
        }
    }
}
