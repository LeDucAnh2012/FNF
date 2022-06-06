using FridayNightFunkin.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using UnityEngine.UI;

namespace FridayNightFunkin.UI.GamePlayUI
{
    public class PanelWin : PanelBase
    {
        [TabGroup("1", "UI")] [SerializeField] private Text txtScore;
        [TabGroup("1", "UI")] [SerializeField] private Text txtCoin;

        [TabGroup("1", "TotalScore")] [SerializeField] private StoreCoin storeCoin;
        [TabGroup("1", "TotalScore")] [SerializeField] private AnimationCurve curve;
        [TabGroup("1", "TotalScore")] [SerializeField] private float timeTotal;

        private float timeCurrent;
        public override void Show()
        {
            base.Show();
            TotalScore();
            //TotalCoin();
        }
        public override void Hide()
        {
            base.Hide();
        }
        public void OnClickNextSong()
        {
            SoundMusicManager.instance?.ClickButton();
            VariableSystem.CurrentSong++;
            if (VariableSystem.CurrentSong > DataSong.Count - 1)
            {
                ShowListSong();
                return;
            }
            DataSong.UnlockSong(VariableSystem.CurrentSong);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        private void ShowListSong()
        {
            SoundMusicManager.instance?.ClickButtonPlayFree();
            VariableSystem.ShowListSong = true;
            SceneManager.LoadScene("SceneHome");
        }
        public void OnClickShowListSong()
        {
            SoundMusicManager.instance?.ClickButton();
            ShowListSong();
        }
        private void TotalScore()
        {
            StartCoroutine(IE_TotalScore(txtScore, VariableSystem.TotalScore, timeTotal));
        }
        private void TotalCoin()
        {
            StartCoroutine(IE_TotalScore(txtCoin, VariableSystem.TotalScore / 2, timeTotal));
        }
        private IEnumerator IE_TotalScore(Text txtChange, int valueWant, float timeInit)
        {
            timeCurrent = 0;
            int score = valueWant;
            while (timeCurrent < timeInit)
            {
                yield return new WaitForEndOfFrame();

                timeCurrent += Time.deltaTime;

                float value = score * curve.Evaluate(timeCurrent / timeInit);
                txtChange.text = ((int)value).ToString();
            }
            txtChange.text = score.ToString();
        }
    }
}