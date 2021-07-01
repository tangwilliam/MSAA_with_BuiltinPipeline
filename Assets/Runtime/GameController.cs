using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TA
{
    public class GameController : MonoBehaviour
    {

        private CameraSetup _cameraSetup;

        private Text _MSAAText;

        private void Start()
        {
            _cameraSetup = GameObject.Find("Main Camera").GetComponent<CameraSetup>();
            _MSAAText = GameObject.Find("MSAAText").GetComponent<Text>();

            if (_cameraSetup == null || _MSAAText == null)
            {
                Debug.LogError("Demo 必备的 GameObject 不存在，初始化失败！");
            }

            _MSAAText.text = "MSAA" + _cameraSetup.MSAA.ToString();
        }

        public void ModifyMSAA()
        {
            int msaa = (int)_cameraSetup.MSAA;
            int power = Mathf.RoundToInt( Mathf.Log(msaa, 2));
            power = (power >= 3) ? 0 : power + 1;
            msaa = Mathf.RoundToInt(Mathf.Pow(2, power));

            Debug.Log("Current MSAA = " + msaa.ToString()); //test

            _cameraSetup.MSAA = (CameraSetup.MSAAEnum)msaa;

            _MSAAText.text = "MSAA" + _cameraSetup.MSAA.ToString();

            _cameraSetup.UpdateRT();
            
    }

    }

}

