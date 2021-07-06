using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Will
{
    public class DiscardContentsGameController : MonoBehaviour
    {

        private DiscardContents _cameraSetup;

        private Text _MSAAText;
        private Text _discardColorBufferText;

        private void Start()
        {
            _cameraSetup = GameObject.Find("Main Camera").GetComponent<DiscardContents>();
            _MSAAText = GameObject.Find("MSAAText").GetComponent<Text>();
            _discardColorBufferText = GameObject.Find("DiscardColorBufferText").GetComponent<Text>();

            if (_cameraSetup == null || _MSAAText == null)
            {
                Debug.LogError("Demo 必备的 GameObject 不存在，初始化失败！");
            }

            _MSAAText.text = "MSAA" + _cameraSetup.MSAA.ToString();
            _discardColorBufferText.text = "DiscardColor: " + _cameraSetup.DiscardColorBuffer.ToString();
        }

        public void ModifyMSAA()
        {
            int msaa = (int)_cameraSetup.MSAA;
            int power = Mathf.RoundToInt(Mathf.Log(msaa, 2));
            power = (power >= 3) ? 0 : power + 1;
            msaa = Mathf.RoundToInt(Mathf.Pow(2, power));

            Debug.Log("Current MSAA = " + msaa.ToString()); //test

            _cameraSetup.MSAA = (DiscardContents.MSAAEnum)msaa;

            _MSAAText.text = "MSAA" + _cameraSetup.MSAA.ToString();

            _cameraSetup.UpdateRT();

        }

        public void ModifyDiscardColorBuffer()
        {
            _cameraSetup.DiscardColorBuffer = !_cameraSetup.DiscardColorBuffer;
            _discardColorBufferText.text = "DiscardColor: " + _cameraSetup.DiscardColorBuffer.ToString();

            _cameraSetup.UpdateRT();
        }

    }

}

