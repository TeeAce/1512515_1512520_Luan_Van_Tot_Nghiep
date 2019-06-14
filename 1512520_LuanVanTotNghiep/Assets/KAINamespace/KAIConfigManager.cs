using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KAINamespace
{
    public class KAIConfigManager : MonoBehaviour
    {
        public static KAIConfigManager Instance;

        public static string AssetPath = "Input/";
        public static string FilePath = "/Resources/Input/";

        public ConfigRecorgnizeInput ConfigRecorgnizeData = new ConfigRecorgnizeInput();


        private void Start()
        {
            MakeInstance();

            LoadRecorgnizeInput();
        }

        public void LoadRecorgnizeInput()
        {
            ConfigRecorgnizeData.ReadFileFromPath(Application.dataPath + FilePath + "ConfigRecorgnizeInput.csv");

            //RecorgnizeInputStruct data = ConfigQuizData.records[0].GetRecorgnizeInputStruct();
            //Debug.Log(string.Format("x:{0}, y:{1}, width:{2}, height:{3}", data.x, data.y, data.width, data.height));
        }

        private void MakeInstance()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != this)
                Destroy(this.gameObject);
        }
    }
}
