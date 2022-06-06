using FridayNightFunkin.Data;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static SongConfig;

namespace FridayNightFunkin.GamePlay
{
    public class GameplayController : MonoBehaviour
    {
        public static GameplayController instance;

        private void Awake()
        {
            if (instance == null)
                instance = this;
        }
        public void OnClickBackHome()
        {
            SceneManager.LoadScene(0);
        }
    }
}