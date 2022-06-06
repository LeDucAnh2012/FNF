using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace FridayNightFunkin.GamePlay
{

    public class CalibrationNote : MonoBehaviour
    {
        public float strumTime;
        private float _scrollSpeed;
        public bool isDestroyNot = false;
        public float Speed
        {
            set => _scrollSpeed = value / 100;
            get => _scrollSpeed * 100;
        }
        public int type;
        [ShowInInspector] public decimal length;
        public bool mustHit;
        public bool dummy;
        public GameObject _renderer;
        private void Start()
        {
        }
        // Update is called once per frame
        private void Update()
        {
            if (CalibrationManager.instance != null)
                if (!CalibrationManager.instance.runCalibration || dummy) return;

            Vector3 oldPos = transform.position;
            oldPos.y = (float)(4.45 + (CalibrationManager.instance.stopwatch?.ElapsedMilliseconds - (strumTime + CalibrationManager.instance.currentVisualOffset)) * (0.45f * (_scrollSpeed))) * -1;
            transform.position = oldPos;
            if (mustHit)
            {
                if (CalibrationManager.instance.stopwatch.ElapsedMilliseconds -
                    (strumTime + CalibrationManager.instance.currentVisualOffset) >= 135)
                {
                    CalibrationManager.instance.NoteMiss(type);
                }
            }
            else
            {
                Color newColor = Color.white;
                newColor.a = .4f;
                _renderer.GetComponent<SpriteRenderer>().color = newColor;

                //if (CalibrationManager.instance.stopwatch.ElapsedMilliseconds >= (strumTime + CalibrationManager.instance.currentVisualOffset))
                //{
                //    CalibrationManager.instance.NoteHit(type);
                //}
                if (this.transform.position.y <= CalibrationManager.instance.staticNotes[type].position.y)
                {
                    CalibrationManager.instance.NoteHit(type);
                    // DestroyNote();
                }
            }

        }
        //public void DestroyNote()
        //{
        //    if (this.length > 0)
        //        if (CalibrationManager.instance.stopwatch.ElapsedMilliseconds >= (strumTime + CalibrationManager.instance.currentVisualOffset + (float)length / 100))
        //        {
        //            CalibrationManager.instance.NoteHit(type, true);
        //        }
        //}
    }

}
