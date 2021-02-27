using MaterialUI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SceneManagement
{
    public class Options : MonoBehaviour
    {
        [SerializeField]
        private SelectionBoxConfig qualitySelectionBox;
        [SerializeField]
        private SelectionBoxConfig resolutionSelectionBox;
        [SerializeField]
        private Toggle fullscreenToggle;

        private void Awake()
        {
            var s = new string[Screen.resolutions.Length];
            for(int i = 0; i < s.Length; i++)
            {
                s[i] = Screen.resolutions[i].width + "x" + Screen.resolutions[i].height;
            }
            resolutionSelectionBox.listItems = s;
        }

        private void Start()
        {
            UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnSceneChanged;
            SetupListeners();
        }

        private void OnSceneChanged(Scene changedScene, Scene loadedScene)
        {
            Destroy(this);
        }

        private void SetupListeners()
        {
            qualitySelectionBox.ItemPicked += OnQualityChanged;
            resolutionSelectionBox.ItemPicked += SetResolution;
            fullscreenToggle.onValueChanged.AddListener(delegate { SetResolution(resolutionSelectionBox.currentSelection); });
        }

        private void OnQualityChanged(int index)
        {
            QualitySettings.SetQualityLevel(index);
        }

        private void SetResolution(int index)
        {
            if(index == -1)
            {
                return;
            }

            Screen.SetResolution(Screen.resolutions[index].width, Screen.resolutions[index].height, fullscreenToggle.isOn);
        }
    }
}
