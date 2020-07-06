using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TextSpeech;

public class VoiceController : MonoBehaviour
{
    string LANG_CODE = "en-US";

    [SerializeField]
    private Text uiText;
    public GameObject listeningIcon;
   
    public string correctCommand;

    private void Start()
    {
        Setup(LANG_CODE);
        listeningIcon.SetActive(false);
        SpeechToText.instance.onResultCallback = onFinalSpeechResult;


    }


    #region speech to text

    public void StartListening()
    {
        SpeechToText.instance.StartRecording();
        listeningIcon.SetActive(true);
        //Invoke("StopListening", 2f);
    }

    public void StopListening()
    {
        SpeechToText.instance.StopRecording();
        listeningIcon.SetActive(false);
    }

    void onFinalSpeechResult(string result)
    {
        if (result.Contains(correctCommand))
        {
            // do something
        }
        

        uiText.text = result;
    }

    #endregion

    void Setup(string code)
    {
        SpeechToText.instance.Setting(code);
    }
}
