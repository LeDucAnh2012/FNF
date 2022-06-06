using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;
namespace FridayNightFunkin.UI.HomeUI
{
    public class PanelSetting : PanelBase
    {
        [TabGroup("1", "Object")] [SerializeField] private GameObject obj;
        [TabGroup("1", "Object")] [SerializeField] private GameObject objMusic;
        [TabGroup("1", "Object")] [SerializeField] private GameObject objSound;
        [TabGroup("1", "Object")] [SerializeField] private GameObject objVibration;

        [TabGroup("1", "UI")] [SerializeField] private Text txtMusic;
        [TabGroup("1", "UI")] [SerializeField] private Text txtSound;
        [TabGroup("1", "UI")] [SerializeField] private Text txtVibration;
        [TabGroup("1", "UI")] [SerializeField] private Color colorTextON;
        [TabGroup("1", "UI")] [SerializeField] private Color colorTextOFF;
        public override void Show()
        {
            base.Show();



            Init();

        }
        public override void Hide()
        {
            base.Hide();
        }
        public void OnClickBackHome()
        {
            SoundMusicManager.instance?.ClickButtonExit();
            Hide();
        }
        bool isChange = true;
        public void ClickChangeSound()
        {
            if (!isChange) return;
            SoundMusicManager.instance?.ClickButton();
            isChange = false;
            SoundMusicManager.instance?.SetSound(!SoundMusicManager.instance.GetSound());
            Init();
        }
        public void ClickChangeMusic()
        {
            if (!isChange) return;
            SoundMusicManager.instance?.ClickButton();
            isChange = false;
            SoundMusicManager.instance?.SetMusic(!SoundMusicManager.instance.GetMusic());
            Init();
        }
        public void ClickChangeVibration()
        {
            if (!isChange) return;
            SoundMusicManager.instance?.ClickButton();
            isChange = false;
            VariableSystem.Vibration = !VariableSystem.Vibration;
            Init();
        }
        public void Init()
        {
            Vector3 _vt;
            bool isState;
            float valueEnd;

            _vt = objSound.transform.localPosition;
            isState = SoundMusicManager.instance.GetSound();
            valueEnd = isState ? Mathf.Abs(_vt.x) : (Mathf.Abs(_vt.x) * -1);

            objSound.transform.DOLocalMoveX(valueEnd, 0.15f).OnComplete(() =>
            {
                txtSound.text = isState ? "ON" : "OFF";
                txtSound.DOColor(isState ? colorTextON : colorTextOFF, 0.15f);
                isChange = true;
            });

            Vector3 _vt_1;
            bool isState_1;
            float valueEnd_1;
            _vt_1 = objMusic.transform.localPosition;
            isState_1 = SoundMusicManager.instance.GetMusic();
            valueEnd_1 = isState_1 ? Mathf.Abs(_vt_1.x) : (Mathf.Abs(_vt_1.x) * -1);

            objMusic.transform.DOLocalMoveX(valueEnd_1, 0.15f).OnComplete(() =>
            {
                txtMusic.text = isState_1 ? "ON" : "OFF";
                txtMusic.DOColor(isState_1 ? colorTextON : colorTextOFF, 0.15f);
                isChange = true;
            });

            Vector3 _vt_2;
            bool isState_2;
            float valueEnd_2;
            _vt_2 = objVibration.transform.localPosition;
            isState_2 = VariableSystem.Vibration;
            valueEnd_2 = isState_2 ? Mathf.Abs(_vt_2.x) : (Mathf.Abs(_vt_2.x) * -1);

            objVibration.transform.DOLocalMoveX(valueEnd_2, 0.15f).OnComplete(() =>
            {
                txtVibration.text = isState_2 ? "ON" : "OFF";
                txtVibration.DOColor(isState_2 ? colorTextON : colorTextOFF, 0.15f);
                isChange = true;
            });
        }
    }
}