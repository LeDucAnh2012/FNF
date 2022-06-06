using FridayNightFunkin.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FridayNightFunkin.UI.HomeUI
{
    public class PanelHomeUI : PanelBase
    {
        [SerializeField] private StoreCoin storeCoin;
        public void ShowPanel()
        {
            storeCoin.InitCoin();
            Show();
        }
        public override void Show()
        {
            base.Show();
        }
        public override void Hide()
        {
            base.Hide();
        }

        //============
        public void OnClickPlayFree()
        {
            SoundMusicManager.instance?.ClickButtonPlayFree();
            UIHomeManager.instance.ShowListSong();
        }
        public void OnClickHackCoin()
        {
            VariableSystem.StoreHeart = VariableSystem.MaxHeart;
            VariableSystem.StoreCoin += 20000;
            storeCoin.InitCoin();
        }
        public void OnClickSetting()
        {
            SoundMusicManager.instance?.ClickButton();
            UIHomeManager.instance.ShowPanelSetting();
        }

    }
}
