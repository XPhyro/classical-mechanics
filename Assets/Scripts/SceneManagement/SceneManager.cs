using MaterialUI;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SceneManagement
{
    public class SceneManager : MonoBehaviour
    {
        public static SceneManager Singleton;

        [SerializeField]
        private NavDrawerConfig nawDrawer;

        [SerializeField]
        private Text barText;

        [SerializeField]
        private Transform listGo;

        [SerializeField]
        private GameObject sceneItemPrefab;

        [SerializeField]
        private SceneProperties[] sceneProperties;

        private void Start()
        {
            if(Singleton)
            {
                Destroy(this);
            }
            else
            {
                Singleton = this;
            }

            DontDestroyOnLoad(gameObject);

            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnSceneChanged;

            foreach(var s in sceneProperties)
            {
                var go = Instantiate(sceneItemPrefab, listGo);
                go.GetComponent<SceneItem>().Properties = s;
            }
        }

        private void OnSceneChanged(Scene changedScene, Scene loadedScene)
        {
            var es = FindObjectOfType<EventSystem>();

            QualitySettings.SetQualityLevel(5, true);

            if(!es)
            {
                var go = new GameObject();
                go.name = "EventSystem";
                go.AddComponent<EventSystem>();
                go.AddComponent<StandaloneInputModule>();
            }
            else if(!es.gameObject.activeSelf)
            {
                es.gameObject.SetActive(true);
            }
        }

        private IEnumerator SetupLoading(AsyncOperation operation)
        {
            while(!operation.isDone)
            {
                barText.text = "Loading: " + Math.Round(operation.progress * 1e2, 2).ToString() + " / 100";
                yield return new WaitForSecondsRealtime(Time.deltaTime);
            }
            barText.text = sceneProperties[UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex - 1].Name;
        }

        public void ChangeScene(int id)
        {
            nawDrawer.Close();
            var loadingOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(id);
            StartCoroutine(SetupLoading(loadingOperation));
        }

        public void Exit()
        {
            Application.Quit();
        }
    }
}
