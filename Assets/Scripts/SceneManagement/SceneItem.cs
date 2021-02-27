using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace SceneManagement
{
    public class SceneItem : MonoBehaviour
    {
        private SceneProperties _properties;
        public SceneProperties Properties
        {
            get
            {
                return _properties;
            }

            set
            {
                _properties = value;

                SetupItem();
            }
        }

        [SerializeField]
        private Button topicButton;

        [SerializeField]
        private Button sceneButton;

        [SerializeField]
        private Text nameText;

        [SerializeField]
        private GameObject vLayoutGo;

        [SerializeField]
        private GameObject newTextPrefab;

        private void SetupItem()
        {
            nameText.text = Properties.Name;
            topicButton.onClick.AddListener(delegate { ToggleTopicList(); });
            sceneButton.onClick.AddListener(delegate { InformSceneManager(); });

            foreach(var t in Properties.Topics)
            {
                var go = Instantiate(newTextPrefab, vLayoutGo.transform);
                go.GetComponent<Text>().text = Regex.Replace(t.ToString(), @"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))", " $0");
            }

            StartCoroutine(CloseList());
        }

        private IEnumerator CloseList()
        {
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

            vLayoutGo.SetActive(false);
        }

        private void ToggleTopicList()
        {
            vLayoutGo.SetActive(!vLayoutGo.activeSelf);
        }

        private void InformSceneManager()
        {
            SceneManager.Singleton.ChangeScene(transform.GetSiblingIndex());
        }
    }
}
