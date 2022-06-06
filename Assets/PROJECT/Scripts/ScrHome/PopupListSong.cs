using FridayNightFunkin.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FridayNightFunkin.UI.HomeUI
{
    public class PopupListSong : PanelBase
    {
        [SerializeField] private List<ElementListSong> listElementSong;
        [SerializeField] private Transform parentList;
        [SerializeField] ElementListSong elementSpawn;
        private void Awake()
        {
            listElementSong.Clear();
            int countSong = DataSong.Count;
            for (int i = 0; i < countSong; i++)
            {
                ElementListSong _element = Instantiate(elementSpawn, parentList);
                _element.idSong = i;
                _element.InitSong();
                listElementSong.Add(_element);
            }
        }
        public override void Show()
        {
            base.Show();
        }
       
        public override void Hide()
        {
            base.Hide();
        }
        private void InitListSong()
        {
            for (int i = 0; i < DataSong.Count; i++)
                listElementSong[i].InitSong();
        }
        public void OnClickBackHome()
        {
            SoundMusicManager.instance?.ClickButton();
            UIHomeManager.instance.ShowPanelHomeUI();
            Hide();
        }
    }
}