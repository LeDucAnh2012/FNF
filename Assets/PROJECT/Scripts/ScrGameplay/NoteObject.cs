using FridayNightFunkin.UI.GamePlayUI;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FridayNightFunkin.GamePlay
{
    public class NoteObject : MonoBehaviour
    {
        private float _scrollSpeed;
        public SpriteRenderer _sprite;

        public float strumTime;
        private Song _song;
        public bool mustHit;
        public bool susNote;
        public int type;
        public bool dummyNote = true;
        public bool lastSusNote = false;
        public int layer;
        public bool isAlt;
        public float currentStrumTime;
        public float currentStopwatch;

        public float susLength;

        private LTDescr _tween;
        public float ScrollSpeed
        {
            get => _scrollSpeed * 100;
            set => _scrollSpeed = value / 100;
        }

        [SerializeField] private Sprite[] sprHold;
        [SerializeField] private Sprite[] sprHoldEnd;

        // Start is called before the first frame update

        public void GenerateHold(bool isLastSusNote)
        {

            _song = Song.instance;

            var noteTransform = _sprite.transform;
            _sprite.flipY = OptionsV2.Downscroll;


            if (lastSusNote)
            {
                _sprite.drawMode = SpriteDrawMode.Sliced;
                noteTransform.localScale = aspect > 0.5f ? new Vector3(0.8f, 0.65f, 1) : new Vector3(0.65f, 0.65f, 1);
                _sprite.size = new Vector2(0.5f, 0.44f * -(float)(Song.instance.stepCrochet / 100 * 1.84 * (ScrollSpeed + _song.speedDifference * 100)));


            }
            else
            {
                _sprite.drawMode = SpriteDrawMode.Simple;
                Vector3 oldScale = aspect > 0.5f ? new Vector3(0.8f, 0.65f, 1) : new Vector3(0.65f, 0.65f, 1);

                oldScale.y *= -(float)(Song.instance.stepCrochet / 100 * 1.84 * (ScrollSpeed + _song.speedDifference * 100));

                noteTransform.localScale = oldScale;
            }

            _sprite.sprite = sprHold[type];
            /*if (!prevNote.susNote)
            {
                return;
            }

            var prevNoteTransform = prevNote.transform;
            Vector3 oldScale = prevNoteTransform.localScale;
            oldScale.y *= -((float) (Song.instance.stepCrochet / 100 * 1.8 * ScrollSpeed) / 1.76f);
            prevNoteTransform.localScale = oldScale;*/

        }
        float aspect;
        private void Start()
        {
            aspect = UIGameplayManager.instance.GetAspectCamera();
        }
        // Update is called once per frame
        void Update()
        {
            _song = Song.instance;
            if (dummyNote)
                return;

            if (OptionsV2.Middlescroll)
            {
                if (Song.modeOfPlay == 2)
                {
                    _sprite.enabled = !mustHit;
                }
                else
                {
                    _sprite.enabled = mustHit;
                }
            }

            var oldPos = transform.position;
            var yPos = Song.instance.player1NoteSprites[type].transform.position.y;

            if (_song.songSetupDone & !_song.songStarted)
            {
                oldPos.y = (yPos - (_song.stopwatch.ElapsedMilliseconds - (strumTime + Player.visualOffset)) * (0.5f * (_scrollSpeed + Song.instance.speedDifference))) * -1;
                if (lastSusNote)
                    oldPos.y += ((float)(Song.instance.stepCrochet / 100 * 1.8 * (ScrollSpeed + _song.speedDifference * 100)) / 2.76f) * (_scrollSpeed + Song.instance.speedDifference) * -1;
                //Debug.Log("========> old pos.y + " + oldPos.y);
                if (OptionsV2.Downscroll)
                {
                    oldPos.y -= 4.45f * 2 * -1;
                    // oldPos.y = -oldPos.y;
                }
                transform.position = oldPos;

                //_tween ??= gameObject.LeanScale(transform.localScale * 1.10f, .35f).setLoopPingPong();

            }
            //else if (_song.songSetupDone & _song.songStarted)
            //{
            //    if (_tween != null)
            //    {
            //        LeanTween.cancel(_tween.id);
            //        _tween = null;
            //    }
            //}


            var color = _song.player1NoteSprites[type].color;

            //if (susNote)
            //    color.a = 0.75f;

            if (!mustHit)
                color.a = 0.25f;
            _sprite.color = color;


            oldPos.y = (float)(yPos - (_song.stopwatch.ElapsedMilliseconds - (strumTime + Player.visualOffset)) * (0.5f * (_scrollSpeed + Song.instance.speedDifference))) * -1;
            // Debug.Log("....old pos y + " + oldPos.y);
            /*
            if (lastSusNote)
                oldPos.y += ((float) (Song.instance.stepCrochet / 100 * 1.85 *  (ScrollSpeed + _song.speedDifference * 100)) / 1.76f) * (_scrollSpeed + Song.instance.speedDifference);
            */
            if (OptionsV2.Downscroll)
            {
                oldPos.y -= 4.45f * 2 * -1;
                //  oldPos.y = -oldPos.y;
            }
            transform.position = oldPos;

            if (!_song.musicSources[0].isPlaying) return;

            if (!mustHit)
            {
                if (this.transform.position.y <= _song.player1Left.transform.position.y)
                {
                    Song.instance.NoteHit(this,null);
                    CameraMovement.instance.focusOnPlayerOne = layer == 1;
                }
            }
            else
            {
                if (this.transform.position.y >= _song.player1Left.transform.position.y - 1f) return;

                if (!Player.playAsEnemy)
                {
                    //  if (!(strumTime + Player.visualOffset - _song.stopwatch.ElapsedMilliseconds < Player.maxHitRoom)) return;
                    Song.instance.NoteMiss(this);
                    CameraMovement.instance.focusOnPlayerOne = layer == 1;
                    _song.player1NotesObjects[type].Remove(this);
                    if (this.transform.position.y < _song.player1Left.transform.position.y + 2f)
                        if (susNote)
                        {
                            _song.holdNotesPool.Release(gameObject);
                        }
                        else
                        {
                            switch (type)
                            {
                                case 0:
                                    _song.leftNotesPool.Release(gameObject);
                                    break;
                                case 1:
                                    _song.downNotesPool.Release(gameObject);
                                    break;
                                case 2:
                                    _song.upNotesPool.Release(gameObject);
                                    break;
                                case 3:
                                    _song.rightNotesPool.Release(gameObject);
                                    break;
                            }
                        }
                }
                else
                {
                    if (strumTime + Player.visualOffset >= _song.stopwatch.ElapsedMilliseconds) return;
                    Song.instance.NoteHit(this,null);
                    CameraMovement.instance.focusOnPlayerOne = layer == 1;
                }
            }
        }
    }
}