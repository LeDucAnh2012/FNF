using System;
using UnityEngine;
using DG.Tweening;
using System.Collections;

namespace FridayNightFunkin.GamePlay
{
    public class CalibrationNoteHold : MonoBehaviour
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
        public decimal length;
        public bool mustHit;
        public bool dummy;
        public SpriteRenderer _renderer;
        public AnimationCurve curve;
        private void Start()
        {
            _renderer.drawMode = SpriteDrawMode.Tiled;
            _renderer.size = new Vector2(_renderer.size.x, (float)length / 100);
        }
        // Update is called once per frame
        private void Update()
        {
            Vector3 oldPos = transform.position;

            float lengthNote = (float)length * (0.45f * _scrollSpeed);
            oldPos.y = (float)(4.45 +
                (CalibrationManager.instance.stopwatch?.ElapsedMilliseconds -
                (strumTime + CalibrationManager.instance.currentVisualOffset)) *
                (0.45f * (_scrollSpeed)) + lengthNote) * -1;

            transform.position = oldPos;

            _renderer.size = new Vector2(0.5f, (float)length * (0.45f * (_scrollSpeed)));

            if (!mustHit)
            {
                Color newColor = Color.white;
                newColor.a = .4f;
                transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = newColor;
                _renderer.color = newColor;
                if (this.transform.position.y - (float)length * (0.45f * (_scrollSpeed)) <= CalibrationManager.instance.staticNotes[type].position.y)
                {
                    if (count == 1)
                        HitNoteHold();
                }
                if (CalibrationManager.instance.stopwatch.ElapsedMilliseconds -
                     (strumTime + CalibrationManager.instance.currentVisualOffset) >= 135)
                {
                    Destroy(gameObject);
                }
            }

        }
        int count = 1;
        public void HitNoteHold()
        {
            count++;
            Debug.Log("time = " + (float)length * (0.45f * (_scrollSpeed)));
            StartCoroutine(CounterSizeNoteHold());
        }
        IEnumerator CounterSizeNoteHold()
        {
            float timeCurrent = 0;
            float sizeY_render = _renderer.size.y;
            Debug.Log("time + " + (float)length * (0.45f * (_scrollSpeed)));
            while (timeCurrent < ((float)length * (0.45f * (_scrollSpeed))))
            {
                yield return new WaitForEndOfFrame();
                timeCurrent += Time.deltaTime;
                float valueWant = sizeY_render - sizeY_render * curve.Evaluate(timeCurrent / ((float)length * (0.45f * (_scrollSpeed))));
                _renderer.size = new Vector2(_renderer.size.x, valueWant);
            }
            if (_renderer.size.y <= 0)
                Destroy(gameObject);
        }
    }
}
