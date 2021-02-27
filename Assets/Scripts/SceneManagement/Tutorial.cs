using MaterialUI;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

namespace SceneManagement
{
    public class Tutorial : MonoBehaviour
    {
        [SerializeField]
        private Button dontShowButton;

        private const string saveFilePath = "/StartMenuSave.berkekocaoglu";

        private void Start()
        {
            if(!File.Exists(Application.persistentDataPath + saveFilePath))
            {
                StartCoroutine("OpenDialogAfter");
            }
            else
            {
                var file = File.Open(Application.persistentDataPath + saveFilePath, FileMode.Open);
                if((bool)new BinaryFormatter().Deserialize(file))
                {
                    Destroy(gameObject);
                }
                else
                {
                    StartCoroutine("OpenDialogAfter");
                }
                file.Close();
            }

            dontShowButton.onClick.AddListener(delegate { OnDontShowButtonClicked(); });
        }

        private IEnumerator OpenDialogAfter()
        {
            yield return new WaitForSecondsRealtime(2f);
            GetComponent<DialogBoxConfig>().Open();
        }

        private void OnDontShowButtonClicked()
        {
            var file = File.Open(Application.persistentDataPath + saveFilePath, FileMode.OpenOrCreate);
            new BinaryFormatter().Serialize(file, true);
            file.Close();
        }
    }
}
