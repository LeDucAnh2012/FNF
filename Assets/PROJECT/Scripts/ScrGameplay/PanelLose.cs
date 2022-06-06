using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FridayNightFunkin.UI.GamePlayUI
{
    public class PanelLose : PanelBase
    {
        public override void Show()
        {
            base.Show();
        }
        public override void Hide()
        {
            base.Hide();
        }
        public void OnClickReplay()
        {
            SoundMusicManager.instance?.ClickButton();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        public void OnClickReturnListSong()
        {
            SoundMusicManager.instance?.ClickButtonPlayFree();
            VariableSystem.ShowListSong = true;
            SceneManager.LoadScene("SceneHome");
        }
    }
}