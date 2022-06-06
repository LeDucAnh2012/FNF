using FridayNightFunkin.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;
using FridayNightFunkin.GamePlay;

namespace FridayNightFunkin.UI.GamePlayUI
{
    public class UIGameplayManager : MonoBehaviour
    {
        public static UIGameplayManager instance;
        [TabGroup("1", "UI")] [SerializeField] private Text txtNameSong;
        [TabGroup("1", "UI")] public Button btnPause;
        [TabGroup("1", "PANEL")] [SerializeField] private GameObject objectFinish;
        [TabGroup("1", "PANEL")] [SerializeField] private GameObject objectLose;
        [TabGroup("1", "PANEL")] [SerializeField] private PanelWin panelWin;
        [TabGroup("1", "PANEL")] [SerializeField] private PanelLose panelLose;
        [TabGroup("1", "PANEL")] public PanelPause panelPause;

        [SerializeField] private Camera mainCam;
        private void Awake()
        {
            instance = this;
        }
        private void Start()
        {
            LoadText();

            panelLose.gameObject.SetActive(false);
            objectLose.gameObject.SetActive(false);

            panelWin.gameObject.SetActive(false);
            objectFinish.gameObject.SetActive(false);

            VariableSystem.StoreHeart--;
        }
        private void Update()
        {
            CountTimeGetHeart();
        }
        private float _timer = 0.0f;
        private void CountTimeGetHeart()
        {
            if (VariableSystem.StoreHeart >= VariableSystem.MaxHeart)
                return;
            _timer += Time.deltaTime;
            if (_timer >= 1)
            {
                _timer = 0.0f;
                VariableSystem.CountTimeGetHeart--;
                if (VariableSystem.CountTimeGetHeart <= 0)
                {
                    VariableSystem.CountTimeGetHeart = (int)VariableSystem.TimeGetHeart;
                    VariableSystem.StoreHeart++;
                }
            }
        }
        private void LoadText()
        {
            txtNameSong.text = "SONG: " + DataSong.GetNameSongByID(VariableSystem.CurrentSong);
        }
        public void OnClickBackHome()
        {
            SoundMusicManager.instance?.ClickButtonExit();
            SceneManager.LoadScene(0);
        }
        public void ShowWin()
        {
            panelLose.gameObject.SetActive(false);
            objectLose.gameObject.SetActive(false);
            objectFinish.gameObject.SetActive(false);

            panelWin.Show();
        }
        public void ShowLose()
        {
            panelWin.gameObject.SetActive(false);
            objectFinish.gameObject.SetActive(false);
            objectLose.gameObject.SetActive(false);

            panelLose.Show();
        }
        public void ShowObjectLose()
        {
            objectLose.SetActive(true);
            Invoke(nameof(ShowLose), 1);
        }
        public void ShowObjectWin()
        {
            objectFinish.SetActive(true);
            Invoke(nameof(ShowWin), 1);
        }

        public float GetAspectCamera()
        {
            return mainCam.aspect;
        }
        public void OnClickShowPause()
        {
            SoundMusicManager.instance.ClickButton();
            Song.instance.PauseSong();
            panelPause.Show();
        }
    }
}