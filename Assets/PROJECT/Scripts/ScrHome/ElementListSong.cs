using FridayNightFunkin.Data;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace FridayNightFunkin.UI.HomeUI
{
    public class ElementListSong : MonoBehaviour
    {
        [TabGroup("1", "Data")] public int idSong;
        [TabGroup("1", "UI")] [SerializeField] Text txtNameSong;
        [TabGroup("1", "UI")] [SerializeField] Text txtScore;
        [TabGroup("1", "UI")] [SerializeField] Button btnBuy;
        [TabGroup("1", "UI")] [SerializeField] Text txtCoin;
        [TabGroup("1", "UI")] [SerializeField] Button btnPlay;
        [TabGroup("1", "UI")] [SerializeField] Button btnWatchAds;
        [TabGroup("1", "UI")] [SerializeField] Text txtDifficul;
        public void InitSong()
        {
            txtCoin.text = DataSong.GetCostSong(idSong).ToString();
            txtNameSong.text = DataSong.GetNameSongByID(idSong);
            //if (DataSong.CheckSongUnlock(idSong))
            //    SongUnlock();
            //else
            //{
            //    switch (DataSong.GetTypeUnLock(idSong))
            //    {
            //        case SongConfig.TypeUnlockSong.Coin:
            //            SongUnLockByCoin();
            //            break;
            //        case SongConfig.TypeUnlockSong.ADS:
            //            SongUnlockByADS();
            //            break;
            //        case SongConfig.TypeUnlockSong.LuckyWheel:
            //            break;
            //    }
            //}
            ActiveButton(DataSong.CheckSongUnlock(idSong));
        }
        private void ActiveButton(bool isState)
        {
            btnWatchAds.gameObject.SetActive(!isState);
            btnBuy.gameObject.SetActive(!isState);
            btnBuy.GetComponent<Image>().color = isState ? Color.white : Color.blue;

            btnPlay.gameObject.SetActive(isState);
            btnPlay.GetComponent<Image>().color = !isState ? Color.white : new Color(0, 255, 255, 255);

            txtScore.gameObject.SetActive(isState);
            txtScore.text = DataSong.GetScoreSong(idSong).ToString();
        }
        public void SongUnlock()
        {
            ActiveButton(true);
        }
        public void SongUnLockByCoin()
        {
            ActiveButton(false);
        }
        public void SongUnlockByADS()
        {
            ActiveButton(false);
        }
        public void OnClickWatchADS()
        {
            CallBackWatchADS();
        }
        private void CallBackWatchADS()
        {
            VariableSystem.CurrentSong = idSong;
            SoundMusicManager.instance?.ClickButton();
            SceneManager.LoadScene(1);
        }
        public void OnClickPlay()
        {
            if (!DataSong.CheckSongUnlock(idSong))
                return;
            VariableSystem.CurrentSong = idSong;
            SoundMusicManager.instance?.ClickButton();
            if (VariableSystem.StoreHeart <= 0)
            {
                UIHomeManager.instance.ShowPopupNotEnough("Not Enough Heart");
                //show popup not enough heart
                return;
            }
            SceneManager.LoadScene(1);
        }
        public void OnClickBuy()
        {
            int costSkin = DataSong.GetCostSong(idSong);
            if (costSkin == 0)
                return;
            if (VariableSystem.StoreCoin < costSkin)
            {
                UIHomeManager.instance.ShowPopupNotEnough();
                return;
            }
            SoundMusicManager.instance?.ClickButton();
            VariableSystem.StoreCoin -= costSkin;
            DataSong.UnlockSong(idSong);
            InitSong();
        }
        public void OnClickChangeDifficul(int id)
        {
            SoundMusicManager.instance?.ClickButton();
            switch (id)
            {
                case 0:
                    switch (DataSong.GetTypePlay(this.idSong))
                    {
                        case SongConfig.TypePlay.Eazy:
                            DataSong.SetTypePlay(idSong, SongConfig.TypePlay.Normal);
                            break;
                        case SongConfig.TypePlay.Normal:
                            DataSong.SetTypePlay(idSong, SongConfig.TypePlay.Hard);
                            break;
                        case SongConfig.TypePlay.Hard:
                            DataSong.SetTypePlay(idSong, SongConfig.TypePlay.Eazy);
                            break;
                    }
                    break;
                case 1:
                    switch (DataSong.GetTypePlay(this.idSong))
                    {
                        case SongConfig.TypePlay.Eazy:
                            DataSong.SetTypePlay(idSong, SongConfig.TypePlay.Hard);
                            break;
                        case SongConfig.TypePlay.Hard:
                            DataSong.SetTypePlay(idSong, SongConfig.TypePlay.Normal);
                            break;
                        case SongConfig.TypePlay.Normal:
                            DataSong.SetTypePlay(idSong, SongConfig.TypePlay.Eazy);
                            break;
                    }
                    break;
            }
            txtDifficul.text = DataSong.GetTypePlay(this.idSong).ToString();
        }
    }
}