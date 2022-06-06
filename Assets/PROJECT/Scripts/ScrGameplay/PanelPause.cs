using FridayNightFunkin.GamePlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FridayNightFunkin.UI.GamePlayUI
{
    public class PanelPause : PanelBase
    {
        public override void Show()
        {
            base.Show();
        }
        public override void Hide()
        {
            base.Hide();
        }
        public void OnClickContinue()
        {
            SoundMusicManager.instance?.ClickButtonExit();

            Song.instance.stopwatch.Start();
            Song.instance.beatStopwatch.Start();

            foreach (AudioSource source in Song.instance.musicSources)
            {
                source.UnPause();
            }

            Song.instance.vocalSource.UnPause();

            Hide();
        }
        public void OnClickBackHome()
        {
            SoundMusicManager.instance?.ClickButton();
            UIGameplayManager.instance.OnClickBackHome();
        }

        public void OnClickRestartSong()
        {
            SoundMusicManager.instance?.ClickButton();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void OnClickQuitSong()
        {
            SoundMusicManager.instance?.ClickButtonPlayFree();
            //VariableSystem.ShowListSong = true;
            SceneManager.LoadScene("SceneHome");
        }
    }
}
