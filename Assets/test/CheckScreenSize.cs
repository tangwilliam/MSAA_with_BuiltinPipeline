using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Profiling;

public class CheckScreenSize : MonoBehaviour
{
    public int m_CheckInterval = 2;

    private int m_CheckCounter = 0;
    private bool m_ShouldCheck = false;
    private bool m_ShouldLog = true;

    private Text m_CheckingButtonText;
    private Text m_LoggingButtonText;

    private static string s_SampleName = "CheckScreenSize";

    public void EnableChecking()
    {
        m_ShouldCheck = !m_ShouldCheck;
        UpdateCheckingButton();
    }
    public void EnableLogging()
    {
        m_ShouldLog = !m_ShouldLog;
        UpdateLoggingButton();
    }

    private void UpdateCheckingButton()
    {
        m_CheckingButtonText.text = m_ShouldCheck ? "Checking" : "No Checking";

    }
    private void UpdateLoggingButton()
    {
        m_LoggingButtonText.text = m_ShouldLog ? "Logging" : "No Logging";

    }

    // Start is called before the first frame update
    void Start()
    {
        m_CheckCounter = 0;

        m_CheckingButtonText = GameObject.Find("CheckButtonText").GetComponent<Text>();
        m_LoggingButtonText = GameObject.Find("LogButtonText").GetComponent<Text>();
        UpdateCheckingButton();
        UpdateLoggingButton();

    }

    // Update is called once per frame
    void Update()
    {
        m_CheckCounter++;
        if(m_CheckCounter % m_CheckInterval == 0)
        {
            if (m_ShouldCheck)
            {
                Profiler.BeginSample(s_SampleName);

                int width = Screen.width;
                int height = Screen.height;

                Profiler.EndSample();

                if (m_ShouldLog)
                {
                    Debug.Log("Current width = " + width.ToString());
                }
            }
        }

        
    }
}
