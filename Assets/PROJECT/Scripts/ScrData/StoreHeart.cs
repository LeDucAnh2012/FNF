using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace FridayNightFunkin.Data
{
    public class StoreHeart : MonoBehaviour
    {
        [SerializeField] private Text txtHeart;
        [SerializeField] private Text txtCountTime;

        [SerializeField] private AnimationCurve curve;
        [SerializeField] private float timeInit;

        private float timeCurrent;
        private void Start()
        {
            InitHeart();

            float timePlus = 0;
            if (VariableSystem.TimeEndGame != "")
            {
                DateTime dayNow = DateTime.Now;

                DateTime dayEndGame;
                if (DateTime.TryParse(VariableSystem.TimeEndGame, out dayEndGame))
                {
                    Debug.Log("day1 " + dayEndGame);
                }
                TimeSpan timeSpan = dayNow.Subtract(dayEndGame);

                int totalSecondEndGame = (int)timeSpan.TotalSeconds;
                Debug.Log("totalSecondEndGame " + totalSecondEndGame);
                if (totalSecondEndGame / VariableSystem.TimeGetHeart >= VariableSystem.MaxHeart - VariableSystem.StoreHeart)
                {
                    VariableSystem.StoreHeart = VariableSystem.MaxHeart;
                    VariableSystem.TimeEndGame = "";
                    return;
                }
                else
                {
                    VariableSystem.StoreHeart += (int)(totalSecondEndGame / VariableSystem.TimeGetHeart);
                    timePlus = totalSecondEndGame;
                    VariableSystem.TimeEndGame = "";
                }
            }
            if (VariableSystem.StoreHeart < VariableSystem.MaxHeart && VariableSystem.CountTimeGetHeart == 0)
            {
                VariableSystem.CountTimeGetHeart = (int)(VariableSystem.TimeGetHeart - timePlus);
                if (VariableSystem.TimeEndGame == "")
                    timePlus = 0;
            }
        }
        static void Quit()
        {
            DateTime day = DateTime.Now;
            Debug.Log("day: " + day);
            VariableSystem.TimeEndGame = day.ToString();
            Debug.Log("Quitting the Player");
        }

        [RuntimeInitializeOnLoadMethod]
        static void RunOnStart()
        {
            Application.quitting += Quit;
        }
        private void Update()
        {
            CountTimeGetHeart();
        }
        private float _timer = 0.0f;
        private void CountTimeGetHeart()
        {
            if (VariableSystem.StoreHeart >= VariableSystem.MaxHeart)
            {
                txtCountTime.gameObject.SetActive(false);
                VariableSystem.TimeEndGame = "";
                VariableSystem.CountTimeGetHeart = 0;
                return;
            }
            txtCountTime.gameObject.SetActive(true);
            float t = VariableSystem.CountTimeGetHeart;

            int seconds = (int)(t % 60); // return the remainder of the seconds divide by 60 as an int
            t /= 60; // divide current time y 60 to get minutes
            int minutes = (int)(t % 60); //return the remainder of the minutes divide by 60 as an int

            txtCountTime.text = "(" + minutes + " : " + seconds.ToString("00") + ")";
            _timer += Time.deltaTime;
            if (_timer >= 1)
            {
                _timer = 0.0f;
                VariableSystem.CountTimeGetHeart--;
                if (VariableSystem.CountTimeGetHeart <= 0)
                {
                    VariableSystem.CountTimeGetHeart = (int)VariableSystem.TimeGetHeart;
                    VariableSystem.StoreHeart++;
                    InitHeart();
                }
            }
        }
        public void InitHeart()
        {
            txtHeart.text = VariableSystem.StoreHeart.ToString();
        }
        public void SetHeart(int amountHeart, int isIncress)
        {
            StartCoroutine(IE_SetHeart(amountHeart, isIncress));
        }
        IEnumerator IE_SetHeart(int amountHeart, int isIncress)
        {
            timeCurrent = 0;
            while (timeCurrent < timeInit)
            {
                yield return new WaitForEndOfFrame();
                timeCurrent += Time.deltaTime;

                float valueWant = amountHeart * curve.Evaluate(timeCurrent / timeInit);

                txtHeart.text = VariableSystem.StoreHeart + (int)valueWant * isIncress + "";
            }
            VariableSystem.StoreHeart += amountHeart * isIncress;
            InitHeart();
        }
    }
}
