using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class MainSceneUI : MonoBehaviour
{
    public GameObject setWindow;   // 현재 켜진 UI 지정
    public bool setActiveValue;    // 현재 켜진 UI의 상태 확인

    [Header("게임종료확인창")]
    public GameObject gameEndWindow;
    public TextMeshProUGUI alarmTMP;
    public TextMeshProUGUI endWindowYesTMP;
    public TextMeshProUGUI endWindowNoTMP;

    [Header("게임설정창")]
    public GameObject optionWindow;

    [Header("설정창")]
    public TextMeshProUGUI optionTitleTMP;          // 옵션 타이틀 텍스트

    [Header("설정창 - 그래픽 파트")]
    public TextMeshProUGUI graphicTitleTMP;         // 그래픽 타이틀 텍스트
    public TextMeshProUGUI resolutionTMP;           // 해상도 텍스트
    public TextMeshProUGUI nowResolutionTMP;        // 현재 해상도 텍스트
    public TextMeshProUGUI fullScreenTMP;           // 전체화면 텍스트
    public TextMeshProUGUI nowFullScreenTMP;        // 현재 전체화면 텍스트
    public TextMeshProUGUI lightTMP;                // 광원효과 텍스트
    public TextMeshProUGUI nowLightTMP;             // 현재 광원효과 텍스트
    bool isFullScreen;                              // 전체화면 여부
    Vector2[] graphic_Resolution = new Vector2[7];  // 그래픽 해상도 설정
    int graphic_ResolutionIndex;                    // 그래픽 해상도 설정 인덱스
    bool isLightControl;                            // 광원효과 여부

    [Header("설정창 - 오디오 파트")]
    public TextMeshProUGUI audioTitleTMP;           // 오디오 타이틀 텍스트
    public AudioMixer audioMixer;                   // 오디오 믹서 텍스트
    public TextMeshProUGUI masterAudioTMP;          // 전체 오디오 설정 텍스트
    public Slider masterSlider;                     // 전체 오디오 슬라이더
    public TextMeshProUGUI bgmAudioTMP;             // 배경음 설정 텍스트
    public Slider BGMSlider;                        // 전체 오디오 슬라이더 
    public TextMeshProUGUI sfxAudioTMP;             // 효과음 설정 텍스트
    public Slider SFXSlider;                        // 전체 오디오 슬라이더

    void Start()
    {
        setWindow = null;
        setActiveValue = false;

        {
            graphic_Resolution[0] = new Vector2(1920, 1080);
            graphic_Resolution[1] = new Vector2(1680, 1050);
            graphic_Resolution[2] = new Vector2(1600,  900);
            graphic_Resolution[3] = new Vector2(1440,  900);
            graphic_Resolution[4] = new Vector2(1280, 1024);
            graphic_Resolution[5] = new Vector2(1280,  960);
            graphic_Resolution[6] = new Vector2(1280,  720);
        }   // 그래픽 해상도 초기화
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            EndGameWindow();
        }
    }

    public void EndGameWindow()
    {
        WindowControl(ref gameEndWindow);

        if (GameManager.GM.data.isKorean)
        {
            alarmTMP.text = "게임을 종료하시겠습니까?";
            endWindowYesTMP.text = "확인";
            endWindowNoTMP.text = "취소";
        }
        else
        {
            alarmTMP.text = "Close Game?";
            endWindowYesTMP.text = "Yes";
            endWindowNoTMP.text = "No";
        }
    }

    public void OptionWindow()
    {
        WindowControl(ref optionWindow);

        if (GameManager.GM.data.isKorean)
        {
            optionTitleTMP.text = "설정";

            // 그래픽 파트
            graphicTitleTMP.text = "그래픽";
            resolutionTMP.text = "해상도";
            fullScreenTMP.text = "전체화면";
            lightTMP.text = "광원효과";

            // 오디오 파트
            audioTitleTMP.text = "오디오";
            masterAudioTMP.text = "전체";
            bgmAudioTMP.text = "배경음";
            sfxAudioTMP.text = "효과음";
        }
        else
        {
            optionTitleTMP.text = "Option";

            // 그래픽 파트
            graphicTitleTMP.text = "Graphic";
            resolutionTMP.text = "Resolution";
            fullScreenTMP.text = "FullScreen";
            lightTMP.text = "Lighting";

            // 오디오 파트
            audioTitleTMP.text = "Audio";
            masterAudioTMP.text = "Master";
            bgmAudioTMP.text = "BGM";
            sfxAudioTMP.text = "SFX";
        }

        // 전체하면 언어설정
        if (isFullScreen)
        {
            if (GameManager.GM.data.isKorean)
            {
                nowFullScreenTMP.text = "켜기";
            }
            else
            {
                nowFullScreenTMP.text = "ON";
            }
        }
        else
        {
            if (GameManager.GM.data.isKorean)
            {
                nowFullScreenTMP.text = "끄기";
            }
            else
            {
                nowFullScreenTMP.text = "OFF";
            }
        }

        // 광원효과 언어설정
        if (isLightControl)
        {
            if (GameManager.GM.data.isKorean)
            {
                nowLightTMP.text = "켜기";
            }
            else
            {
                nowLightTMP.text = "ON";
            }
        }
        else
        {
            if (GameManager.GM.data.isKorean)
            {
                nowLightTMP.text = "끄기";
            }
            else
            {
                nowLightTMP.text = "OFF";
            }
        }
    }

    // 게임종료창
    public void EndGameButton()
    {
        GameManager.GM.SavaData();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }


    // 설정창
    public void OptionReset()
    {
        // 그래픽
        isFullScreen = true;
        Screen.SetResolution(
            (int)graphic_Resolution[0].x, 
            (int)graphic_Resolution[0].y,
            isFullScreen);
        graphic_ResolutionIndex = 0;
        isLightControl = true;

        // 오디오
        masterSlider.value = 0;
        BGMSlider.value = 0;
        SFXSlider.value = 0;
    }

    // 설정창 - 그래픽 파트
    public void ResolutionButton(bool isPlus)
    {
        if (isPlus)
        {
            graphic_ResolutionIndex++;

            // 만약 해상도 범위를 벗어났다면 실행
            if (graphic_ResolutionIndex >= graphic_Resolution.Length)
            {
                graphic_ResolutionIndex = 0;
            }
        }
        else
        {
            graphic_ResolutionIndex--;

            // 만약 해상도 범위를 벗어났다면 실행
            if (graphic_ResolutionIndex < 0)
            {
                graphic_ResolutionIndex = graphic_Resolution.Length - 1;
            }
        }

        Screen.SetResolution(
            (int)graphic_Resolution[graphic_ResolutionIndex].x,
            (int)graphic_Resolution[graphic_ResolutionIndex].y,
            isFullScreen);
        
        nowResolutionTMP.text = 
            graphic_Resolution[graphic_ResolutionIndex].x + 
            "x" + 
            graphic_Resolution[graphic_ResolutionIndex].y;

    }   // 해상도 설정
    public void FullScreenButton()
    {
        isFullScreen = !isFullScreen;

        Screen.SetResolution(
            (int)graphic_Resolution[graphic_ResolutionIndex].x,
            (int)graphic_Resolution[graphic_ResolutionIndex].y,
            isFullScreen);

        if (isFullScreen)
        {
            if (GameManager.GM.data.isKorean)
            {
                nowFullScreenTMP.text = "켜기";
            }
            else
            {
                nowFullScreenTMP.text = "ON";
            }
        }
        else
        {
            if (GameManager.GM.data.isKorean)
            {
                nowFullScreenTMP.text = "끄기";
            }
            else
            {
                nowFullScreenTMP.text = "OFF";
            }
        }
    }              // 전체화면 설정
    public void LightButton()
    {
        isLightControl = !isLightControl;

        if (isLightControl)
        {
            if (GameManager.GM.data.isKorean)
            {
                nowLightTMP.text = "켜기";
            }
            else
            {
                nowLightTMP.text = "ON";
            }
        }
        else
        {
            if (GameManager.GM.data.isKorean)
            {
                nowLightTMP.text = "끄기";
            }
            else
            {
                nowLightTMP.text = "OFF";
            }
        }
    }                   // 광원효과 설정


    // 설정창 - 오디오 파트
    public void SetMasterAudio()
    {
        audioMixer.SetFloat("Master", masterSlider.value);
    }
    public void SetBGMAudio()
    {
        audioMixer.SetFloat("BGM", BGMSlider.value);
    }
    public void SetSFXAudio()
    {
        audioMixer.SetFloat("SFX", SFXSlider.value);
    }


    // 창 띄우기
    public void WindowControl(ref GameObject temp)
    {
        if (setActiveValue == false)
        {
            setActiveValue = true;

            temp.SetActive(true);
            setWindow = temp;
        }

        else
        {
            setActiveValue = false;

            temp.SetActive(false);
            setWindow = null;
        }
    }
    // 창 닫기
    public void CloseButton()
    {
        if (setWindow == null)
        {
            return;
        }

        setActiveValue = false;

        setWindow.SetActive(false);
        setWindow = null;
    }
}
