using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Will
{
    public class SceneController : MonoBehaviour
    {
        private int _sceneCount = 3;
        private int _sceneIndex = 0;

        void OnGUI()
        {
            if (GUI.Button(new Rect(800, 100, 200, 50), "Change Scene"))
            {
                _sceneIndex += 1;
                if (_sceneIndex > (_sceneCount - 1))
                {
                    _sceneIndex = 0;
                }

                SceneManager.LoadScene(_sceneIndex);
            }
        }


        private static SceneController _instance;
        public static SceneController Instance { get { return _instance; } }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
        }


    }


}

