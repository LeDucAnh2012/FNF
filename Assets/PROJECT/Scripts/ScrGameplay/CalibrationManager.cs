using FridayNightFunkin.Data;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using DG.Tweening;
using static SongConfig;
using UnityEngine.Events;

namespace FridayNightFunkin.GamePlay
{
    public class CalibrationManager : MonoBehaviour
    {
        public static CalibrationManager instance;

        public bool runCalibration;

        public AudioSource audioSource;

        [ShowInInspector] public List<List<CalibrationNote>> notes;
        public CalibrationNote dummyNote;
        public Sprite[] noteSprites;


        public Transform[] staticNotes;
        public GameObject calibrationNote;
        public GameObject[] calibrationNoteHold;

        [Header("Input Offset")] public Canvas inputCanvas;
        public AudioClip inputCalibrationClip;
        public TextAsset inputChart;

        public int x = 0;
        public TypePlay typePlay;
        public TypeSong typeSong;
        public float delay;
        public bool isTest = false;
        [Space]
        public float currentVisualOffset = 0f;
        public float offsetHit = 0f;
        public float currentInputOffset = 0f;

        public Stopwatch stopwatch;
        public FNFSong song;
        private void Awake()
        {
            if (instance == null)
                instance = this;
        }
        private void Start()
        {
            if (isTest)
            {
                TypeSong _ts = typeSong;
                TypePlay _t = typePlay;
                switch (_ts)
                {
                    case TypeSong.Song:
                        inputCalibrationClip = DataSong.GetSongByID(x);
                        break;
                    case TypeSong.Voice:
                        inputCalibrationClip = DataSong.GetVoiceByID(x);
                        break;
                }

                inputChart = DataSong.GetJsonFile(x, _t);
                delay = DataSong.GetDelay(x);
            }
            else
            {
                x = VariableSystem.CurrentSong;

                TypeSong _ts = typeSong;
                TypePlay _t = typePlay;
                switch (_ts)
                {
                    case TypeSong.Song:
                        inputCalibrationClip = DataSong.GetSongByID(x);
                        break;
                    case TypeSong.Voice:
                        inputCalibrationClip = DataSong.GetVoiceByID(x);
                        break;
                }

                inputChart = DataSong.GetJsonFile(x, _t);
                delay = DataSong.GetDelay(x);
            }
            notes = new List<List<CalibrationNote>>
            {
                new List<CalibrationNote>(),
                new List<CalibrationNote>(),
                new List<CalibrationNote>(),
                new List<CalibrationNote>()
            };
            CalibrateInputs();
        }

        public void CalibrateInputs()
        {
            stopwatch?.Stop();
            foreach (List<CalibrationNote> noteList in notes)
            {
                for (var index = noteList.Count - 1; index >= 0; index--)
                {
                    CalibrationNote note = noteList[index];
                    noteList.Remove(note);
                    print("Removing note");
                    Destroy(note.gameObject);
                }
            }

            stopwatch = new Stopwatch();
            stopwatch.Start();

            PlaySong();
            SpawnNode();

            runCalibration = true;
        }
        private void PlaySong()
        {
            audioSource.clip = inputCalibrationClip;
            audioSource.Play();
        }
        private void SpawnNode()
        {
            song = new FNFSong(inputChart.text, FNFSong.DataReadType.AsRawJson);

            foreach (FNFSong.FNFSection section in song.Sections)
            {
                foreach (FNFSong.FNFNote note in section.Notes)
                {
                    var noteType = ((int)note.Type);
                    if (noteType > 3) return;
                    if (note.Length <= 0)
                    {

                        GameObject newNote = Instantiate(calibrationNote);

                        newNote.transform.position = new Vector3(staticNotes[noteType].position.x, 0, 0);
                        newNote.GetComponentInChildren<SpriteRenderer>().sprite = noteSprites[noteType];
                        newNote.GetComponentInChildren<SpriteRenderer>().transform.localScale = staticNotes[noteType].transform.GetChild(0).gameObject.transform.localScale;
                        CalibrationNote noteScript = newNote.GetComponent<CalibrationNote>();
                        noteScript.strumTime = (float)note.Time;
                        noteScript.Speed = song.Speed;
                        noteScript.type = noteType;
                        noteScript.mustHit = section.MustHitSection;
                        noteScript.length = note.Length;
                        notes[noteType].Add(noteScript);

                    }
                    else
                    {
                        // spawn note length
                        GameObject newNote = Instantiate(calibrationNoteHold[noteType]);

                        newNote.transform.position = new Vector3(staticNotes[noteType].position.x, 0, 0);
                        CalibrationNoteHold noteScript = newNote.GetComponent<CalibrationNoteHold>();
                        noteScript.strumTime = (float)note.Time;
                        noteScript.Speed = song.Speed;
                        noteScript.type = noteType;
                        noteScript.mustHit = section.MustHitSection;
                        noteScript.length = note.Length;
                    }
                }
            }
        }

        private void Update()
        {
            for (var index = 0; index < Player.primaryKeyCodes.Count; index++)
            {
                KeyCode key = Player.primaryKeyCodes[index];
                CalibrationNote note = dummyNote;
                if (notes[index].Count != 0)
                {
                    note = notes[index][0];
                }

                if (Input.GetKeyDown(key))
                {
                    if (CanHitNote(note))
                    {
                        UnityEngine.Debug.Log("click");
                        NoteHit(index);
                    }
                    else
                    {
                        AnimateNote(1, index, "ActivePress", true);
                    }
                }

                if (Input.GetKeyUp(key))
                {
                    AnimateNote(1, index, "ActivePress", false);
                }
            }
            for (var index = 0; index < Player.secondaryKeyCodes.Count; index++)
            {
                KeyCode key = Player.secondaryKeyCodes[index];
                CalibrationNote note = dummyNote;
                if (notes[index].Count != 0)
                {
                    note = notes[index][0];
                }

                if (Input.GetKeyDown(key))
                {
                    if (CanHitNote(note))
                    {
                        UnityEngine.Debug.Log("click");
                        NoteHit(index);
                    }
                    else
                    {
                        AnimateNote(1, index, "ActivePress", true);
                    }
                }

                if (Input.GetKeyUp(key))
                {
                    AnimateNote(1, index, "ActivePress", false);
                }
            }
        }

        public void AnimateNote(int player, int type, string animationName, bool stateAnim)
        {
            staticNotes[type].gameObject.GetComponentInChildren<Animator>().SetBool(animationName, stateAnim);
        }

        public bool CanHitNote(CalibrationNote note)
        {
            if (note.transform.position.y <= staticNotes[note.type].position.y)
                return true;
            return false;
            //float noteDiff = (note.strumTime + currentVisualOffset - stopwatch.ElapsedMilliseconds) + currentInputOffset;

            //return noteDiff <= 135 * Time.timeScale & noteDiff >= -135 * Time.timeScale;
        }

        public void NoteHit(int type)
        {
            CalibrationNote note = notes[type][0];
            notes[type].Remove(note);
            if (note.mustHit)
            {
                offsetHit = stopwatch.ElapsedMilliseconds - (note.strumTime + currentVisualOffset) - currentInputOffset;
                offsetHit = -offsetHit;
                UnityEngine.Debug.Log("Offet Hit: " + offsetHit);
            }
            Destroy(note.gameObject);
            //CalibrationNote note = notes[type][0];
            //if (note.length == 0)
            //    notes[type].Remove(note);
            //if (note.mustHit)
            //{
            //    offsetHit = stopwatch.ElapsedMilliseconds - (note.strumTime + currentVisualOffset) - currentInputOffset;
            //    offsetHit = -offsetHit;
            //    UnityEngine.Debug.Log("Offet Hit: " + offsetHit);
            //}
            //if (note.length == 0)
            //    Destroy(note.gameObject);
            //else
            //{
            //    note._renderer.SetActive(false);
            //    Vector2 sizeNoteHold = note.noteHole.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().size;
            //    note.noteHole.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().size -= new Vector2(0, 0.5f);
            //    if (sizeNoteHold.y <= 0)
            //    {
            //        notes[type].Remove(note);
            //        Destroy(note.gameObject);
            //        Destroy(note.noteHole.gameObject);
            //        callBack?.Invoke();
            //    }
            //}
        }
        public void NoteMiss(int type)
        {
            CalibrationNote note = notes[type][0];
            notes[type].Remove(note);
            Destroy(note.gameObject);
        }
    }
}