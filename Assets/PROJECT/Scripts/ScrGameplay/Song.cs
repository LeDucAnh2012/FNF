using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using FridayNightFunkin;
using FridayNightFunkin.Data;
using FridayNightFunkin.UI.GamePlayUI;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using static SongConfig;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;
using DG.Tweening;
using QFSW.MOP2;
using UnityEngine.Events;

namespace FridayNightFunkin.GamePlay
{
    public class Song : MonoBehaviour
    {

        #region Variables
        public bool isTest = false;
        public AudioSource soundSource;
        public AudioClip startSound;
        [Space] public AudioSource[] musicSources;
        public AudioSource vocalSource;
        public AudioSource oopsSource;
        public AudioClip musicClip;
        public AudioClip vocalClip;
        public TextAsset inputChart;
        public AudioClip menuClip;
        public AudioClip[] noteMissClip;
        public bool hasVoiceLoaded;
        //public HybInstance modInstance;


        [Space] public bool songSetupDone = false;

        [Space] public GameObject[] defaultSceneObjects;

        [Space] public GameObject ratingObject;
        public GameObject liteRatingObjectP1;
        public GameObject liteRatingObjectP2;
        public Sprite sickSprite;
        public Sprite goodSprite;
        public Sprite badSprite;
        public Sprite shitSprite;
        public Text playerOneScoringText;
        public Text playerOneMissText;
        public Text txtNameSong;
        public Text txtDifficul;
        //        public Text playerTwoScoringText;

        [Space]
        public float ratingLayerTimer;
        private float _ratingLayerDefaultTime = 2.2f;
        private int _currentRatingLayer;
        public PlayerStat playerOneStats;
        public PlayerStat playerTwoStats;

        public Stopwatch stopwatch;
        public Stopwatch beatStopwatch;
        [Space] public Camera mainCamera;
        public Camera gamePlayCamera;
        public float beatZoomTime;
        private float _defaultZoom;
        public float defaultGameZoom;

        [Space] public float notesOffset;
        public float noteDelay;
        [Range(-1f, 1f)] public float speedDifference;

        [Space] public Canvas battleCanvas;
        public Canvas menuCanvas;

        //public GameObject songListScreen;

        //[Space] public GameObject menuScreen;

        [Header("Death Mechanic")] public Camera deadCamera;
        //public GameObject deadBoyfriend;
        //public Animator deadBoyfriendAnimator;
        public AudioClip deadNoise;
        public AudioClip deadTheme;
        public AudioClip deadConfirm;
        //public Image deathBlackout;
        public bool isDead;
        public bool respawning;





        private float _currentInterval;
        [Space]
        public SpriteRenderer[] player1NoteSprites;
        public Sprite[] player1NoteSpritesNormal;
        public Sprite[] player1NoteSpritesPress;
        public List<List<NoteObject>> player1NotesObjects;
        //public Animator[] player1NotesAnimators;
        public Transform player1Left;
        public Transform player1Down;
        public Transform player1Up;
        public Transform player1Right;
        [Space]
        public List<List<NoteObject>> player2NotesObjects;
        private List<NoteBehaviour> _noteBehaviours = new List<NoteBehaviour>();

        [Header("Prefabs")] public GameObject downArrow;

        [Space] public Sprite holdNoteEnd;
        public Sprite holdNoteSprite;
        [Header("Object Pools")]
        public ObjectPool leftNotesPool;
        public ObjectPool downNotesPool;
        public ObjectPool upNotesPool;
        public ObjectPool rightNotesPool;
        public ObjectPool holdNotesPool;

        [Header("Characters")]
        public string[] characterNames;
        [Space] public GameObject girlfriendObject;
        public bool altDance;

        [FormerlySerializedAs("enemyObj")] [Header("Enemy")] public GameObject opponentObject;
        public string enemyName;
        public float enemyIdleTimer = .3f;
        private float _currentEnemyIdleTimer;
        public float enemyNoteTimer = .25f;
        private Vector3 _enemyDefaultPos;
        private readonly float[] _currentEnemyNoteTimers = { 0, 0, 0, 0 };
        private readonly float[] _currentDemoNoteTimers = { 0, 0, 0, 0 };
        private LTDescr _enemyFloat;



        [FormerlySerializedAs("bfObj")] [Header("Boyfriend")] public GameObject boyfriendObject;
        public float boyfriendIdleTimer = .3f;
        public Sprite boyfriendPortraitNormal;
        public Sprite boyfriendPortraitDead;
        private float _currentBoyfriendIdleTimer;

        private FNFSong _song;

        public static Song instance;


        [Header("Health")] public float health = 100;

        private const float MAXHealth = 200;
        public float healthLerpSpeed;
        public GameObject healthBar;
        public RectTransform boyfriendHealthIconRect;
        public Image boyfriendHealthIcon;
        public Image boyfriendHealthBar;
        public RectTransform enemyHealthIconRect;
        public Image enemyHealthIcon;
        public Image enemyHealthBar;

        [Space] public GameObject songDurationObject;
        public Text songDurationText;
        public Image songDurationBar;

        [Space] public GameObject startSongTooltip;

        [Space] public NoteObject lastNote;
        public float stepCrochet;
        public float beatsPerSecond;
        public int currentBeat;
        public bool beat;

        private float _bfRandomDanceTimer;
        private float _enemyRandomDanceTimer;

        private bool _portraitsZooming;
        private bool _cameraZooming;

        public string songsFolder;
        public string selectedSongDir;

        public static string difficulty;
        public static int modeOfPlay;

        public bool songStarted;

        [Header("Subtitles")]

        public bool usingSubtitles;

        #endregion
        private void Awake()
        {
            instance = this;
            SetNoteClick();
            LoadText();
        }
        private void LoadText()
        {
            txtNameSong.text = "SONG: " + DataSong.GetNameSongByID(VariableSystem.CurrentSong);
            txtDifficul.text = DataSong.GetTypePlay(VariableSystem.CurrentSong).ToString();
        }
        private void SetNoteClick()
        {
            float aspect = UIGameplayManager.instance.GetAspectCamera();
            float posY = player1Left.transform.position.y;

            float posXLeft = aspect > 0.5f ? -2 : -1.75f;
            float posXDown = aspect > 0.5f ? -0.65f : -0.55f;

            foreach (SpriteRenderer s in player1NoteSprites)
                s.transform.localScale = aspect > 0.5f ? new Vector3(0.8f, 0.8f, 1) : new Vector3(0.65f, 0.8f, 1);

            player1Left.transform.position = new Vector3(posXLeft, posY, 0);
            player1Down.transform.position = new Vector3(posXDown, posY, 0);
            player1Up.transform.position = new Vector3(-posXDown, posY, 0);
            player1Right.transform.position = new Vector3(-posXLeft, posY, 0);
        }
        private void Start()
        {
            if (!isTest)
            {
                int x = VariableSystem.CurrentSong;

                TypePlay _t = DataSong.GetTypePlay(x);

                musicClip = DataSong.GetSongByID(x);
                vocalClip = DataSong.GetVoiceByID(x);

                inputChart = DataSong.GetJsonFile(x, _t);
            }

            battleCanvas.enabled = true;
            _defaultZoom = gamePlayCamera.orthographicSize;
            playerOneScoringText.text = "SCORE: " + playerOneStats.currentScore;
            playerOneMissText.text = "MISS: " + playerOneStats.missedHits;
            OptionsV2.SongDuration = true;

            modeOfPlay = 1;
            Player.playAsEnemy = false;
            Player.twoPlayers = false;

            PlaySong(difficulty);
        }

        #region Song Gameplay

        public void PlaySong(string difficulty = "", string directory = "")
        {
            _currentRatingLayer = 0;

            currentBeat = 0;

            GenerateSong();
        }

        public void GenerateSong()
        {
            _song = new FNFSong(inputChart.text, FNFSong.DataReadType.AsRawJson);

            beatsPerSecond = 60 / (float)_song.Bpm;
            stepCrochet = (60 / (float)_song.Bpm * 1000 / 4);

            if (player1NotesObjects != null)
            {
                foreach (List<NoteObject> list in player1NotesObjects)
                    list.Clear();

                player1NotesObjects.Clear();
            }

            if (player2NotesObjects != null)
            {
                foreach (List<NoteObject> list in player2NotesObjects)
                    list.Clear();

                player2NotesObjects.Clear();
            }

            leftNotesPool.ReleaseAll();
            downNotesPool.ReleaseAll();

            upNotesPool.ReleaseAll();
            rightNotesPool.ReleaseAll();

            holdNotesPool.ReleaseAll();

            //GENERATE PLAYER ONE NOTES
            player1NotesObjects = new List<List<NoteObject>>
            {
            new List<NoteObject>(), new List<NoteObject>(), new List<NoteObject>(), new List<NoteObject>()
            };

            //GENERATE PLAYER TWO NOTES
            player2NotesObjects = new List<List<NoteObject>>
            {
            new List<NoteObject>(), new List<NoteObject>(), new List<NoteObject>(), new List<NoteObject>()
            };



            if (_song == null)
            {
                Debug.LogError("Error with song data");
                return;
            }

            foreach (FNFSong.FNFSection section in _song.Sections)
            {
                foreach (FNFSong.FNFNote note in section.Notes)
                {
                    _noteBehaviours.Add(new NoteBehaviour(section, note));
                }
            }

            for (int i = 0; i < 4; i++)
            {
                player1NotesObjects[i] = player1NotesObjects[i].OrderBy(s => s.strumTime).ToList();
                player2NotesObjects[i] = player2NotesObjects[i].OrderBy(s => s.strumTime).ToList();
            }

            songSetupDone = false;
            songStarted = false;

            /*
             * Reset the stopwatch entirely.
             */
            stopwatch = new Stopwatch();

            if (isDead)
            {
                isDead = false;
                respawning = false;

                deadCamera.enabled = false;


            }
            if (OptionsV2.SongDuration)
            {
                float time = musicClip.length - musicSources[0].time;

                int seconds = (int)(time % 60); // return the remainder of the seconds divide by 60 as an int
                time /= 60; // divide current time y 60 to get minutes
                int minutes = (int)(time % 60); //return the remainder of the minutes divide by 60 as an int

                songDurationText.text = minutes + ":" + seconds.ToString("00");

                //songDurationBar.fillAmount = 0;
            }

            /*
             * Stops any current music playing and sets it to not loop.
             */
            musicSources[0].loop = false;
            musicSources[0].Stop();

            /*
             * Now we can fully start the song in a coroutine.
             */
            // StartCoroutine(nameof(SongStart), startSound.length);
            SongStart();
        }



        void SongStart()
        {
            stopwatch.Start();
            soundSource.clip = startSound;
            soundSource.Play();



            //if (!OptionsV2.LiteMode)
            //    mainCamera.orthographicSize = 4;

            /*
             * Start the beat stopwatch.
             *
             * This is used to precisely calculate when a beat happens based
             * on the BPM or BPS.
             */



            /*
             * Sets the voices and music audio sources clips to what
             * they should have.
             */
            musicSources[0].clip = musicClip;
            vocalSource.clip = vocalClip;
            UIGameplayManager.instance.btnPause.interactable = false;

            StartCoroutine(ActionHelper.StartAction(() =>
            {
                beatStopwatch = new Stopwatch();
                beatStopwatch.Start();
                songSetupDone = true;
                songStarted = true;
                stopwatch.Restart();

            }, DataSong.GetTimeDelayVoice(VariableSystem.CurrentSong) + 0.2f));
            /*
             * In case we have more than one audio source,
             * let's tell them all to play.
             */
            StartCoroutine(ActionHelper.StartAction(() =>
            {
                foreach (AudioSource source in musicSources)
                    source.Play();
                vocalSource.Play();

                UIGameplayManager.instance.btnPause.interactable = true;
            }, 1.75f));
            /*
             * Plays the vocal audio source then tells this script and other
             * attached scripts that the song fully started.
             */
            //if (hasVoiceLoaded)


            //modInstance?.Invoke("OnSongStarted");


            ///*
            // * Start subtitles.
            // */
            //if (usingSubtitles)
            //{
            //    subtitleDisplayer.paused = false;
            //    subtitleDisplayer.StartSubtitles();
            //}
            /*
             * Restart the stopwatch for the song itself.
             */

        }


        public void GenNote(FNFSong.FNFSection section, List<decimal> note)
        {
            /*
                     * The .NET FNF Chart parsing library already has something specific
                     * to tell us if the note is a must hit.
                     *
                     * But previously I already kind of reverse engineered the FNF chart
                     * parsing process so I used the "ConvertToNote" function in the .NET
                     * library to grab "note data".
                     */
            GameObject newNoteObj;
            List<decimal> data = note;

            /*
             * It sets the "must hit note" boolean depending if the note
             * is in a section focusing on the boyfriend or not, and
             * if the note is for the other section.
             */
            bool mustHitNote = section.MustHitSection;
            if (data[1] > 3)
                mustHitNote = !section.MustHitSection;
            int noteType = Convert.ToInt32(data[1] % 4);

            /*
             * We make a spawn pos variable to later set the spawn
             * point of this note.
             */
            Vector3 spawnPos;

            /*
             * We get the length of this note's hold length.
             */
            float susLength = (float)data[2];

            /*
            if (susLength > 0)
            {
                isSusNote = true;

            }
            */

            /*
             * Then we adjust it to fit the step crochet to get the TRUE
             * hold length.
             */
            susLength /= stepCrochet;

            /*
             * It checks the type of note this is and spawns in a note gameobject
             * tailored for it then sets the spawn point for it depending on if it's
             * a note belonging to player 1 or player 2.
             *
             * If somehow this is the wrong data type, it fails and stops the song generation.
             */
            switch (noteType)
            {
                case 0: //Left
                    newNoteObj = leftNotesPool.GetObject();
                    spawnPos = player1Left.position;
                    break;
                case 1: //Down
                    newNoteObj = downNotesPool.GetObject();
                    spawnPos = player1Down.position;
                    break;
                case 2: //Up
                    newNoteObj = upNotesPool.GetObject();
                    spawnPos = player1Up.position;
                    break;
                case 3: //Right
                    newNoteObj = rightNotesPool.GetObject();
                    spawnPos = player1Right.position;
                    break;
                default:
                    Debug.LogError("Invalid note data.");
                    return;
            }

            /*
             * We then move the note to a specific position in the game world.
             */
            spawnPos += Vector3.down *
                        (Convert.ToSingle(data[0] / (decimal)notesOffset) + (_song.Speed * noteDelay));
            spawnPos.y -= (_song.Bpm / 60) * startSound.length * _song.Speed;
            newNoteObj.transform.position = spawnPos;
            newNoteObj.transform.localScale = player1NoteSprites[0].transform.localScale;
            //newNoteObj.transform.position += Vector3.down * Convert.ToSingle(secNoteData[0] / notesOffset);

            /*
             * Each note gameobject has a special component named "NoteObject".
             * It controls the note's movement based on the data provided.
             * It also allows Player 2 to hit their notes.
             *
             * Below we set this note's component data. Simple.
             *
             * DummyNote is always false if generated via a JSON.
             */
            NoteObject nObj = newNoteObj.GetComponent<NoteObject>();

            nObj.ScrollSpeed = -_song.Speed;
            nObj.strumTime = (float)data[0];
            nObj.type = noteType;
            nObj.mustHit = mustHitNote;
            nObj.dummyNote = false;
            nObj.layer = section.MustHitSection ? 1 : 2;

            /*
             * We add this new note to a list of either player 1's notes
             * or player 2's notes, depending on who it belongs to.
             */
            if (mustHitNote)
                player1NotesObjects[noteType].Add(nObj);
            else
                player2NotesObjects[noteType].Add(nObj);

            /*
             * This below is for hold notes generation. It tells the future
             * hold note what the previous note is.
             */
            lastNote = nObj;
            /*
             * Now we generate hold notes depending on this note's hold length.
             * The generation of hold notes is more or less the same as normal
             * notes. Hold notes, though, use a different gameobject as it's not
             * a normal note.
             *
             * If there's nothing, we skip.
             */
            for (int i = 0; i < Math.Floor(susLength); i++)
            {
                GameObject newSusNoteObj;
                Vector3 susSpawnPos;

                bool setAsLastSus = false;

                /*
                 * Math.floor returns the largest integer less than or equal to a given number.
                 *
                 * I uh... have no clue why this is needed or what it does but we need this
                 * in or else it won't do hold notes right so...
                 */
                newSusNoteObj = holdNotesPool.GetObject();
                if ((i + 1) == Math.Floor(susLength))
                {
                    newSusNoteObj.GetComponent<NoteObject>()._sprite.sprite = holdNoteEnd;
                    setAsLastSus = true;
                }
                else
                {
                    setAsLastSus = false;
                    newSusNoteObj.GetComponent<NoteObject>()._sprite.sprite = holdNoteSprite;
                }

                switch (noteType)
                {
                    case 0: //Left
                        susSpawnPos = player1Left.position;
                        break;
                    case 1: //Down
                        susSpawnPos = player1Down.position;
                        break;
                    case 2: //Up
                        susSpawnPos = player1Up.position;
                        break;
                    case 3: //Right
                        susSpawnPos = player1Right.position;
                        break;
                    default:
                        susSpawnPos = player1Left.position;
                        break;
                }


                susSpawnPos += Vector3.down *
                               (Convert.ToSingle(data[0] / (decimal)notesOffset) + (_song.Speed * noteDelay));
                susSpawnPos.y -= (_song.Bpm / 60) * startSound.length * _song.Speed;
                newSusNoteObj.transform.position = susSpawnPos;
                NoteObject susObj = newSusNoteObj.GetComponent<NoteObject>();
                susObj.type = noteType;
                susObj.ScrollSpeed = -_song.Speed;
                susObj.mustHit = mustHitNote;
                susObj.strumTime = (float)data[0] + (stepCrochet * i) + stepCrochet;
                susObj.susNote = true;
                susObj.dummyNote = false;
                susObj.lastSusNote = setAsLastSus;
                susObj.layer = section.MustHitSection ? 1 : 2;
                susObj.GenerateHold(lastNote);
                if (mustHitNote)
                    player1NotesObjects[noteType].Add(susObj);
                else
                    player2NotesObjects[noteType].Add(susObj);
                lastNote = susObj;
            }
        }


        //#region Pause Menu
        public void PauseSong()
        {
            stopwatch.Stop();
            beatStopwatch.Stop();

            foreach (AudioSource source in musicSources)
            {
                source.Pause();
            }

            vocalSource.Pause();
        }

        //public void ContinueSong()
        //{
        //    stopwatch.Start();
        //    beatStopwatch.Start();

        //    subtitleDisplayer.paused = false;

        //    foreach (AudioSource source in musicSources)
        //    {
        //        source.UnPause();
        //    }

        //    if (hasVoiceLoaded)
        //        vocalSource.UnPause();

        //    Pause.instance.pauseScreen.SetActive(false);
        //}

        //public void RestartSong()
        //{
        //    subtitleDisplayer.StopSubtitles();
        //    PlaySong(false, difficulty, currentSongMeta.songPath);
        //    Pause.instance.pauseScreen.SetActive(false);
        //}

        //public void QuitSong()
        //{
        //    ContinueSong();
        //    subtitleDisplayer.StopSubtitles();
        //    foreach (AudioSource source in musicSources)
        //    {
        //        source.Stop();
        //    }

        //    if (hasVoiceLoaded)
        //        vocalSource.Stop();
        //}

        //#endregion
        #endregion

        public void QuitGame()
        {
            Application.Quit();
        }



        #region Animating

        public void EnemyPlayAnimation(string animationName)
        {
            //if (enemy.idleOnly || OptionsV2.DesperateMode) return;
            //opponentAnimator.Play(animationName);
            //_currentEnemyIdleTimer = enemyIdleTimer;
        }

        private void BoyfriendPlayAnimation(string animationName)
        {
            //if (OptionsV2.DesperateMode) return;
            //boyfriendAnimator.Play("BF " + animationName);


            //_currentBoyfriendIdleTimer = boyfriendIdleTimer;
        }

        public void AnimateNote(int player, int type, string animName, bool isStateAnim)
        {
            player1NoteSprites[type].sprite = isStateAnim ? player1NoteSpritesPress[type] : player1NoteSpritesNormal[type];
            //player1NotesAnimators[type].SetBool(animName, isStateAnim);
            //switch (player)
            //{
            //    case 1: //Boyfriend

            //        player1NotesAnimators[type].Play(animName, 0, 0);
            //        player1NotesAnimators[type].speed = 0;

            //        player1NotesAnimators[type].Play(animName);
            //        player1NotesAnimators[type].speed = 1;

            //        if (animName == "Activated" & !Player.twoPlayers)
            //        {
            //            if (Player.demoMode)
            //                _currentDemoNoteTimers[type] = enemyNoteTimer;
            //            else if (Player.playAsEnemy)
            //                _currentEnemyNoteTimers[type] = enemyNoteTimer;

            //        }

            //        break;
            //    case 2: //Opponent

            //        player2NotesAnimators[type].Play(animName, 0, 0);
            //        player2NotesAnimators[type].speed = 0;

            //        player2NotesAnimators[type].Play(animName);
            //        player2NotesAnimators[type].speed = 1;

            //        if (animName == "Activated" & !Player.twoPlayers)
            //        {
            //            if (!Player.playAsEnemy)
            //                _currentEnemyNoteTimers[type] = enemyNoteTimer;
            //        }
            //        break;
            //}
        }

        #endregion

        #region Note & Score Registration

        public enum Rating
        {
            Sick = 1,
            Good = 2,
            Bad = 3,
            Shit = 4
        }

        public void UpdateScoringInfo()
        {

            if (true/*!Player.playAsEnemy || Player.twoPlayers || Player.demoMode*/)
            {
                float accuracyPercent;
                if (playerOneStats.totalNoteHits != 0)
                {
                    float sickScore = playerOneStats.totalSicks * 4;
                    float goodScore = playerOneStats.totalGoods * 3;
                    float badScore = playerOneStats.totalBads * 2;
                    float shitScore = playerOneStats.totalShits;

                    float totalAccuracyScore = sickScore + goodScore + badScore + shitScore;

                    var accuracy = totalAccuracyScore / (playerOneStats.totalNoteHits * 4);

                    accuracyPercent = (float)Math.Round(accuracy, 4);
                    accuracyPercent *= 100;
                }
                else
                {
                    accuracyPercent = 0;
                }

                playerOneScoringText.text = "SCORE: " + playerOneStats.currentScore;
                playerOneMissText.text = "MISS: " + playerOneStats.missedHits;

                //playerOneScoringText.text = $"Score: {playerOneStats.currentScore} | Combo: {playerOneStats.currentCombo} ({playerOneStats.highestCombo})\nAccuracy: {accuracyPercent}% | Misses: {playerOneStats.missedHits}";
            }

            return;
            if (Player.playAsEnemy || Player.twoPlayers || Player.demoMode)
            {
                float accuracyPercent;
                if (playerTwoStats.totalNoteHits != 0)
                {
                    float sickScore = playerTwoStats.totalSicks * 4;
                    float goodScore = playerTwoStats.totalGoods * 3;
                    float badScore = playerTwoStats.totalBads * 2;
                    float shitScore = playerTwoStats.totalShits;

                    float totalAccuracyScore = sickScore + goodScore + badScore + shitScore;

                    var accuracy = totalAccuracyScore / (playerTwoStats.totalNoteHits * 4);

                    accuracyPercent = (float)Math.Round(accuracy, 4);
                    accuracyPercent *= 100;
                }
                else
                {
                    accuracyPercent = 0;
                }

                //    playerTwoScoringText.text = $"Score: {playerTwoStats.currentScore}\nAccuracy: {accuracyPercent}%\nCombo: {playerTwoStats.currentCombo} ({playerTwoStats.highestCombo})\nMisses: {playerTwoStats.missedHits}";
            }
            else
            {
                // playerTwoScoringText.text = string.Empty;
            }
        }

        public void NoteHit(NoteObject note, UnityAction callback)
        {
            if (note == null) return;


            var player = note.mustHit ? 1 : 2;


            if (hasVoiceLoaded)
                vocalSource.mute = false;

            bool invertHealth = false;

            int noteType = note.type;
            switch (player)
            {
                case 1:
                    if (!Player.playAsEnemy || Player.twoPlayers)
                        invertHealth = false;
                    switch (noteType)
                    {
                        case 0:
                            //Left
                            BoyfriendPlayAnimation("Sing Left");
                            break;
                        case 1:
                            //Down
                            BoyfriendPlayAnimation("Sing Down");
                            break;
                        case 2:
                            //Up
                            BoyfriendPlayAnimation("Sing Up");
                            break;
                        case 3:
                            //Right
                            BoyfriendPlayAnimation("Sing Right");
                            break;
                    }
                    //AnimateNote(1, noteType, "ActivePress", true);
                    break;
                case 2:
                    if (Player.playAsEnemy || Player.twoPlayers)
                        invertHealth = true;
                    switch (noteType)
                    {
                        case 0:
                            //Left
                            EnemyPlayAnimation("Sing Left");
                            break;
                        case 1:
                            //Down
                            EnemyPlayAnimation("Sing Down");
                            break;
                        case 2:
                            //Up
                            EnemyPlayAnimation("Sing Up");
                            break;
                        case 3:
                            //Right
                            EnemyPlayAnimation("Sing Right");
                            break;
                    }
                    // AnimateNote(2, noteType, "ActivePress", true);
                    break;
            }

            bool modifyScore = true;

            if (player == 1 & Player.playAsEnemy & !Player.twoPlayers)
                modifyScore = false;
            else if (player == 2 & !Player.playAsEnemy & !Player.twoPlayers)
                modifyScore = false;



            CameraMovement.instance.focusOnPlayerOne = note.layer == 1;

            Rating rating;
            if (!note.susNote & modifyScore)
            {
                if (player == 1)
                {
                    playerOneStats.totalNoteHits++;
                }
                else
                {
                    //        playerTwoStats.totalNoteHits++;
                }

                var newRatingObject = !OptionsV2.LiteMode ? Instantiate(ratingObject) : liteRatingObjectP1;
                Vector3 ratingPos = new Vector3(1, 1, 0);
                newRatingObject.transform.position = ratingPos;
                if (OptionsV2.LiteMode & player == 2)
                {
                    newRatingObject = liteRatingObjectP2;
                    ratingPos = newRatingObject.transform.position;
                }

                ratingPos.y = OptionsV2.Downscroll ? 6 : 1;
                if (player == 2)
                {

                    if (!OptionsV2.LiteMode)
                    {
                        ratingPos.x = -ratingPos.x;
                    }
                }

                newRatingObject.transform.position = ratingPos;

                var ratingObjectScript = newRatingObject.GetComponent<RatingObject>();

                if (OptionsV2.LiteMode)
                {
                    ratingObjectScript.liteTimer = 2.15f;
                }


                /*
                 * Rating and difference calulations from FNF Week 6 update
                 */

                float noteDiff = note.transform.position.y;
                float noteClick = player1Left.transform.position.y;

                //Math.Abs(note.strumTime - stopwatch.ElapsedMilliseconds + Player.visualOffset + Player.inputOffset);

                if (noteDiff >= noteClick - 0.25f && noteDiff <= noteClick + 0.25f)
                {
                    rating = Rating.Sick;
                }
                else
                if (noteDiff >= noteClick - 0.75f && noteDiff <= noteClick + 0.75f)
                {
                    rating = Rating.Good;
                }
                else if (noteDiff >= noteClick - 1f && noteDiff <= noteClick + 1f)
                {
                    rating = Rating.Bad;
                }
                else
                {
                    rating = Rating.Shit;
                }
                switch (rating)
                {
                    case Rating.Sick:
                        {
                            ratingObjectScript.sprite.sprite = sickSprite;

                            if (!invertHealth)
                                health += 5;
                            else
                                health -= 5;
                            if (player == 1)
                            {
                                playerOneStats.currentCombo++;
                                playerOneStats.totalSicks++;
                                playerOneStats.currentScore += 10;
                            }
                            else
                            {
                                playerTwoStats.currentCombo++;
                                playerTwoStats.totalSicks++;
                                playerTwoStats.currentScore += 10;
                            }
                            break;
                        }
                    case Rating.Good:
                        {
                            ratingObjectScript.sprite.sprite = goodSprite;

                            if (!invertHealth)
                                health += 2;
                            else
                                health -= 2;

                            if (player == 1)
                            {
                                playerOneStats.currentCombo++;
                                playerOneStats.totalGoods++;
                                playerOneStats.currentScore += 5;
                            }
                            else
                            {
                                playerTwoStats.currentCombo++;
                                playerTwoStats.totalGoods++;
                                playerTwoStats.currentScore += 5;
                            }
                            break;
                        }
                    case Rating.Bad:
                        {
                            ratingObjectScript.sprite.sprite = badSprite;

                            if (!invertHealth)
                                health += 1;
                            else
                                health -= 1;

                            if (player == 1)
                            {
                                playerOneStats.currentCombo++;
                                playerOneStats.totalBads++;
                                playerOneStats.currentScore += 1;
                            }
                            else
                            {
                                playerTwoStats.currentCombo++;
                                playerTwoStats.totalBads++;
                                playerTwoStats.currentScore += 1;
                            }
                            break;
                        }
                    case Rating.Shit:
                        ratingObjectScript.sprite.sprite = shitSprite;

                        if (player == 1)
                        {
                            playerOneStats.currentCombo = 0;
                            playerOneStats.totalShits = 0;
                        }
                        else
                        {
                            playerTwoStats.currentCombo = 0;
                            playerTwoStats.totalShits = 0;
                        }
                        break;
                }

                if (player == 1)
                {
                    if (playerOneStats.highestCombo < playerOneStats.currentCombo)
                    {
                        playerOneStats.highestCombo = playerOneStats.currentCombo;
                    }
                    playerOneStats.hitNotes++;
                }
                else
                {
                    if (playerTwoStats.highestCombo < playerTwoStats.currentCombo)
                    {
                        playerTwoStats.highestCombo = playerTwoStats.currentCombo;
                    }
                    playerTwoStats.hitNotes++;
                }




                _currentRatingLayer++;
                ratingObjectScript.sprite.sortingOrder = _currentRatingLayer;
                ratingLayerTimer = _ratingLayerDefaultTime;
            }

            UpdateScoringInfo();

            if (player == 1)
            {
                callback?.Invoke();
                player1NotesObjects[noteType].Remove(note);
            }
            else
            {
                player2NotesObjects[noteType].Remove(note);
            }

            if (note.susNote)
            {
                holdNotesPool.Release(note.gameObject);
            }
            else
            {

                switch (note.type)
                {
                    case 0:
                        leftNotesPool.Release(note.gameObject);
                        break;
                    case 1:
                        downNotesPool.Release(note.gameObject);
                        break;
                    case 2:
                        upNotesPool.Release(note.gameObject);
                        break;
                    case 3:
                        rightNotesPool.Release(note.gameObject);
                        break;
                }
            }
        }

        public void NoteMiss(NoteObject note)
        {
            if (hasVoiceLoaded)
                vocalSource.mute = true;
            oopsSource.clip = noteMissClip[Random.Range(0, noteMissClip.Length)];
            oopsSource.Play();

            var player = note.mustHit ? 1 : 2;


            bool invertHealth = player == 2;

            int noteType = note.type;
            switch (player)
            {
                case 1:
                    switch (noteType)
                    {
                        case 0:
                            //Left
                            BoyfriendPlayAnimation("Sing Left Miss");
                            break;
                        case 1:
                            //Down
                            BoyfriendPlayAnimation("Sing Down Miss");
                            break;
                        case 2:
                            //Up
                            BoyfriendPlayAnimation("Sing Up Miss");
                            break;
                        case 3:
                            //Right
                            BoyfriendPlayAnimation("Sing Right Miss");
                            break;
                    }
                    break;
                default:
                    switch (noteType)
                    {
                        case 0:
                            //Left
                            EnemyPlayAnimation("Sing Left");
                            break;
                        case 1:
                            //Down
                            EnemyPlayAnimation("Sing Down");
                            break;
                        case 2:
                            //Up
                            EnemyPlayAnimation("Sing Up");
                            break;
                        case 3:
                            //Right
                            EnemyPlayAnimation("Sing Right");
                            break;
                    }
                    break;
            }

            bool modifyHealth = true;

            if (player == 1 & Player.playAsEnemy & !Player.twoPlayers)
                modifyHealth = false;
            else if (player == 2 & !Player.playAsEnemy & !Player.twoPlayers)
                modifyHealth = false;

            if (modifyHealth)
            {
                if (!invertHealth)
                    health -= 8;
                else
                    health += 8;
            }

            if (player == 1)
            {
                playerOneStats.currentScore -= 5;
                playerOneStats.currentCombo = 0;
                playerOneStats.missedHits++;
                playerOneStats.totalNoteHits++;
            }
            //else
            //{
            //    playerTwoStats.currentScore -= 5;
            //    playerTwoStats.currentCombo = 0;
            //    playerTwoStats.missedHits++;
            //    playerTwoStats.totalNoteHits++;
            //}

            UpdateScoringInfo();

        }

        #endregion




        // Update is called once per frame
        void Update()
        {
            if (songSetupDone)
            {
                //modInstance?.Invoke("Update");
                if (_noteBehaviours.Count > 0)
                    foreach (NoteBehaviour nBeh in _noteBehaviours)
                        if (nBeh.count < 1)
                            nBeh.GenerateNote();
                //stopwatch.Start();

                if (songStarted & musicSources[0].isPlaying)
                {
                    if (OptionsV2.SongDuration)
                    {
                        float t = musicClip.length - musicSources[0].time;

                        int seconds = (int)(t % 60); // return the remainder of the seconds divide by 60 as an int
                        t /= 60; // divide current time y 60 to get minutes
                        int minutes = (int)(t % 60); //return the remainder of the minutes divide by 60 as an int

                        songDurationText.text = minutes + " : " + seconds.ToString("00");

                        //songDurationBar.fillAmount = musicSources[0].time / musicClip.length;
                    }
                    if ((float)beatStopwatch.ElapsedMilliseconds / 1000 >= beatsPerSecond)
                    {
                        beatStopwatch.Restart();
                        currentBeat++;

                        //   modInstance?.Invoke("OnBeat", currentBeat);
                        if (_currentBoyfriendIdleTimer <= 0 & currentBeat % 2 == 0)
                        {
                            //  boyfriendAnimator.Play("BF Idle");
                        }

                        if (_currentEnemyIdleTimer <= 0 & currentBeat % 2 == 0)
                        {
                            //  opponentAnimator.Play("Idle");
                        }



                        if (!OptionsV2.LiteMode)
                        {

                            if (altDance)
                            {
                                //  girlfriendAnimator.Play("GF Dance Left");
                                altDance = false;
                            }
                            else
                            {
                                // girlfriendAnimator.Play("GF Dance Right");
                                altDance = true;
                            }

                            if (!_portraitsZooming)
                            {
                                _portraitsZooming = true;
                                //LeanTween.value(1.25f, 1, .15f).setOnComplete(() => { _portraitsZooming = false; })
                                //    .setOnUpdate(f =>
                                //    {
                                //        boyfriendHealthIconRect.localScale = new Vector3(-f, f, 1);
                                //        enemyHealthIconRect.localScale = new Vector3(f, f, 1);
                                //    });
                            }

                            if (!_cameraZooming)
                            {
                                if (currentBeat % 4 == 0)
                                {
                                    gamePlayCamera.DOOrthoSize(_defaultZoom, beatZoomTime).From(_defaultZoom - 0.1f).OnComplete(() => _cameraZooming = false);

                                    mainCamera.DOOrthoSize(defaultGameZoom, beatZoomTime).From(defaultGameZoom - 0.1f).OnComplete(() => _cameraZooming = false);
                                }
                            }
                        }
                    }
                }

                if (health > MAXHealth)
                    health = MAXHealth;
                if (health <= 0)
                {
                    health = 0;
                    if (!Player.playAsEnemy & !Player.twoPlayers)
                    {
                        if (isDead)
                        {
                            if (!respawning)
                            {
                                if (Input.GetKeyDown(Player.pauseKey))
                                {
                                    musicSources[0].Stop();
                                    respawning = true;

                                    //  deadBoyfriendAnimator.Play("Dead Confirm");

                                    musicSources[0].PlayOneShot(deadConfirm);

                                    //deathBlackout.rectTransform.LeanAlpha(1, 3).setDelay(1).setOnComplete(() =>
                                    //{
                                    //    //PlaySong(false, difficulty, currentSongMeta.songPath);
                                    //    PlaySong(difficulty);
                                    //});
                                }
                            }
                        }
                        else
                        {
                            isDead = true;

                            // modInstance?.Invoke("OnDeath");

                            //finish song
                            foreach (AudioSource source in musicSources)
                            {
                                source.Stop();
                            }


                            if (hasVoiceLoaded)
                                vocalSource.Stop();

                            musicSources[0].PlayOneShot(deadNoise);

                            battleCanvas.enabled = false;

                            gamePlayCamera.enabled = false;
                            //mainCamera.enabled = false;
                            //deadCamera.enabled = true;

                            beatStopwatch.Reset();
                            stopwatch.Reset();

                            vocalSource.Stop();
                            oopsSource.Stop();
                            soundSource.Stop();

                            // subtitleDisplayer.StopSubtitles();
                            // subtitleDisplayer.paused = false;

                            // run anim thua của player

                            //deadBoyfriend.transform.position = boyfriendObject.transform.position;
                            //deadBoyfriend.transform.localScale = boyfriendObject.transform.localScale;

                            deadCamera.orthographicSize = mainCamera.orthographicSize;
                            deadCamera.transform.position = mainCamera.transform.position;

                            //  deadBoyfriendAnimator.Play("Dead Start");

                            Vector3 newPos = boyfriendObject.transform.position;
                            newPos.y += 2.95f;
                            newPos.z = -10;

                            //LeanTween.move(deadCamera.gameObject, newPos, .5f).setEaseOutExpo();

                            deadCamera.transform.DOMove(newPos, 0.5f).SetEase(Ease.OutExpo).OnComplete(() =>
                            {
                                UIGameplayManager.instance.ShowObjectLose();
                                this.gameObject.SetActive(false);
                            });

                            //LeanTween.delayedCall(2.417f, () =>
                            //{
                            //    if (!respawning)
                            //    {
                            //        musicSources[0].clip = deadTheme;
                            //        musicSources[0].loop = true;
                            //        musicSources[0].Play();
                            //        deadBoyfriendAnimator.Play("Dead Loop");
                            //    }
                            //});
                        }
                    }
                }


                float healthPercent = health / MAXHealth;
                boyfriendHealthBar.fillAmount = healthPercent;
                enemyHealthBar.fillAmount = 1 - healthPercent;

                var rectTransform = enemyHealthIcon.rectTransform;
                var anchoredPosition = rectTransform.anchoredPosition;
                Vector2 enemyPortraitPos = anchoredPosition;
                enemyPortraitPos.x = -(healthPercent * 394 - (200)) - 50;

                Vector2 boyfriendPortraitPos = anchoredPosition;
                boyfriendPortraitPos.x = -(healthPercent * 394 - (200)) + 50;

                if (healthPercent >= .80f)
                {
                    //  enemyHealthIcon.sprite = enemy.portraitDead;
                    boyfriendHealthIcon.sprite = boyfriendPortraitNormal;
                }
                else if (healthPercent <= .20f)
                {
                    // enemyHealthIcon.sprite = enemy.portrait;
                    boyfriendHealthIcon.sprite = boyfriendPortraitDead;
                }
                else
                {
                    //  enemyHealthIcon.sprite = enemy.portrait;
                    boyfriendHealthIcon.sprite = boyfriendPortraitNormal;
                }



                anchoredPosition = enemyPortraitPos;
                rectTransform.anchoredPosition = anchoredPosition;
                boyfriendHealthIcon.rectTransform.anchoredPosition = boyfriendPortraitPos;
                StartCoroutine(ActionHelper.StartAction(() =>
                {
                    if (!musicSources[0].isPlaying & songStarted & !isDead & !respawning & !UIGameplayManager.instance.panelPause.gameObject.activeSelf)
                    {
                        Debug.Log("stop watch");

                        //Song is done.

                        //MenuV2.startPhase = MenuV2.StartPhase.SongList;

                        LeanTween.cancelAll();

                        stopwatch.Stop();
                        beatStopwatch.Stop();

                        //opponentAnimator.spriteAnimations = defaultEnemy.animations;

                        //if (usingSubtitles)
                        //{
                        //    subtitleDisplayer.StopSubtitles();
                        //    subtitleDisplayer.paused = false;
                        //    usingSubtitles = false;
                        //}

                        //girlfriendAnimator.Play("GF Dance Loop");
                        //boyfriendAnimator.Play("BF Idle Loop");
                        //opponentAnimator.Play("Idle Loop");

                        songSetupDone = false;
                        songStarted = false;
                        foreach (List<NoteObject> noteList in player1NotesObjects.ToList())
                        {
                            foreach (NoteObject noteObject in noteList.ToList())
                            {
                                noteList.Remove(noteObject);
                            }
                        }


                        foreach (List<NoteObject> noteList in player2NotesObjects.ToList())
                        {
                            foreach (NoteObject noteObject in noteList.ToList())
                            {
                                noteList.Remove(noteObject);

                            }
                        }

                        leftNotesPool.ReleaseAll();
                        downNotesPool.ReleaseAll();
                        upNotesPool.ReleaseAll();
                        rightNotesPool.ReleaseAll();
                        holdNotesPool.ReleaseAll();

                        battleCanvas.enabled = false;

                        healthBar.SetActive(false);

                        int overallScore = 0;

                        int currentHighScore = DataSong.GetScoreSong(VariableSystem.CurrentSong);

                        switch (modeOfPlay)
                        {
                            //Boyfriend
                            case 1:
                                overallScore = playerOneStats.currentScore;
                                break;
                            //Opponent
                            case 2:
                                overallScore = playerTwoStats.currentScore;
                                break;
                            //Local Multiplayer
                            case 3:
                                overallScore = playerOneStats.currentScore + playerTwoStats.currentScore;
                                break;
                            //Auto
                            case 4:
                                overallScore = 0;
                                break;
                        }

                        if (overallScore > currentHighScore)
                        {
                            DataSong.SetScoreSong(VariableSystem.CurrentSong, overallScore);
                        }
                        VariableSystem.TotalScore = overallScore;

                        UIGameplayManager.instance.ShowObjectWin();
                        this.gameObject.SetActive(false);
                    }

                }, 1.75f));


            }
            else
            {
                _bfRandomDanceTimer -= Time.deltaTime;
                _enemyRandomDanceTimer -= Time.deltaTime;

                if (_bfRandomDanceTimer <= 0)
                {
                    switch (Random.Range(0, 4))
                    {
                        case 1:
                            BoyfriendPlayAnimation("Sing Left");
                            break;
                        case 2:
                            BoyfriendPlayAnimation("Sing Down");
                            break;
                        case 3:
                            BoyfriendPlayAnimation("Sing Up");
                            break;
                        case 4:
                            BoyfriendPlayAnimation("Sing Right");
                            break;
                        default:
                            BoyfriendPlayAnimation("Sing Left");
                            break;
                    }

                    _bfRandomDanceTimer = Random.Range(.5f, 3f);
                }
                if (_enemyRandomDanceTimer <= 0)
                {
                    switch (Random.Range(0, 4))
                    {
                        case 1:
                            EnemyPlayAnimation("Sing Left");
                            break;
                        case 2:
                            EnemyPlayAnimation("Sing Down");
                            break;
                        case 3:
                            EnemyPlayAnimation("Sing Up");
                            break;
                        case 4:
                            EnemyPlayAnimation("Sing Right");
                            break;
                        default:
                            EnemyPlayAnimation("Sing Left");
                            break;
                    }

                    _enemyRandomDanceTimer = Random.Range(.5f, 3f);
                }
            }

            //for (int i = 0; i < _currentEnemyNoteTimers.Length; i++)
            //{
            //    if (Player.twoPlayers) continue;
            //    if (!Player.playAsEnemy)
            //    {
            //        if (player2NotesAnimators[i].GetCurrentAnimatorStateInfo(0).IsName("Activated"))
            //        {
            //            _currentEnemyNoteTimers[i] -= Time.deltaTime;
            //            if (_currentEnemyNoteTimers[i] <= 0)
            //            {
            //                AnimateNote(2, i, "Normal");
            //            }
            //        }
            //    }
            //    else
            //    {
            //        if (player1NotesAnimators[i].GetCurrentAnimatorStateInfo(0).IsName("Activated"))
            //        {
            //            _currentEnemyNoteTimers[i] -= Time.deltaTime;
            //            if (_currentEnemyNoteTimers[i] <= 0)
            //            {
            //                AnimateNote(1, i, "Normal");
            //            }
            //        }
            //    }
            //}

            if (ratingLayerTimer > 0)
            {
                ratingLayerTimer -= Time.deltaTime;
                if (ratingLayerTimer < 0)
                    _currentRatingLayer = 0;
            }

            //if (Player.demoMode)
            //    for (int i = 0; i < _currentDemoNoteTimers.Length; i++)
            //    {
            //        if (player1NotesAnimators[i].GetCurrentAnimatorStateInfo(0).IsName("Activated"))
            //        {
            //            _currentDemoNoteTimers[i] -= Time.deltaTime;
            //            if (_currentDemoNoteTimers[i] <= 0)
            //            {
            //                AnimateNote(1, i, "Normal");
            //            }
            //        }
            //    }

            //if (OptionsV2.DesperateMode) return;
            //if ((opponentAnimator.CurrentAnimation == null || !opponentAnimator.CurrentAnimation.Name.Contains("Idle")) & !songStarted)
            //{
            //    _currentEnemyIdleTimer -= Time.deltaTime;
            //    if (_currentEnemyIdleTimer <= 0)
            //    {
            //        opponentAnimator.Play("Idle Loop");
            //        _currentEnemyIdleTimer = enemyIdleTimer;
            //    }
            //}
            //else
            //{
            //    _currentEnemyIdleTimer -= Time.deltaTime;
            //}

            //if ((!boyfriendAnimator.CurrentAnimation.Name.Contains("Idle") || boyfriendAnimator.CurrentAnimation == null) & !songStarted)
            //{

            //    _currentBoyfriendIdleTimer -= Time.deltaTime;
            //    if (_currentBoyfriendIdleTimer <= 0)
            //    {
            //        boyfriendAnimator.Play("BF Idle Loop");
            //        _currentBoyfriendIdleTimer = boyfriendIdleTimer;
            //    }
            //}
            //else
            //{
            //    _currentBoyfriendIdleTimer -= Time.deltaTime;
            //}


        }
    }
}