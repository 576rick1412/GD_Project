using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class MainUI : MonoBehaviour
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
    public TextMeshProUGUI optionSaveTMP;           // 옵션 저장 텍스트
    public TextMeshProUGUI optionBackTMP;           // 옵션 나가기 텍스트

    [Header("설정창 - 그래픽 파트")]
    public TextMeshProUGUI graphicTitleTMP;         // 그래픽 타이틀 텍스트
    public TextMeshProUGUI resolutionTMP;           // 해상도 텍스트
    public TextMeshProUGUI nowResolutionTMP;        // 현재 해상도 텍스트
    public TextMeshProUGUI fullScreenTMP;           // 전체화면 텍스트
    public TextMeshProUGUI nowFullScreenTMP;        // 현재 전체화면 텍스트
    public TextMeshProUGUI lightTMP;                // 광원효과 텍스트
    public TextMeshProUGUI nowLightTMP;             // 현재 광원효과 텍스트
    bool isFullScreen;                              // 전체화면 여부
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

    [Header("설정창 - 데이터 파트")]
    public TextMeshProUGUI dataTitleTMP;            // 데이터 타이틀 텍스트
    public TextMeshProUGUI GameDataDeletePartTMP;   // 모든 데이터 초기화 텍스트
    public TextMeshProUGUI GameDataDeleteButtonTMP; // 모든 데이터 초기화 버튼 텍스트
    public TextMeshProUGUI OptionResetPartTMP;      // 옵션 설정 초기화 텍스트
    public TextMeshProUGUI OptionResetButtonTMP;    // 옵션 설정 초기화 버튼 텍스트



    [Header("설정창 - 게임플레이 파트")]
    public TextMeshProUGUI gameplayTitleTMP;        // 게임플레이 타이틀 텍스트
    public TextMeshProUGUI languagePartTMP;         // 언어설정 텍스트
    public TextMeshProUGUI nowLanguageTMP;          // 현재 언어 설정값 텍스트
    bool isKorean;

    void Start()
    {
        setWindow = null;
        setActiveValue = false;

        OptionLoad();   // 옵션 설정 불러오기
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

    public void OptionWindow(bool isControl)
    {
        OptionLoad();   // GM에 있는 옵션 데이터 불러오기

        if (isControl)
        {
            WindowControl(ref optionWindow);
        }

        // 언어로 나뉜 텍스트 전부
        if (isKorean)
        {
            optionTitleTMP.text = "설정";
            optionSaveTMP.text = "저장하기";
            optionBackTMP.text = "나가기";

            // 그래픽 파트
            graphicTitleTMP.text = "그래픽";
            resolutionTMP.text = "해상도";
            fullScreenTMP.text = "전체화면";
            if (isFullScreen)
            {
                nowFullScreenTMP.text = "켜기";
            }   // 전체화면  켜기
            else
            {
                nowFullScreenTMP.text = "끄기";
            }                // 전체화면  끄기
            lightTMP.text = "광원효과";
            if (isLightControl)
            {
                nowLightTMP.text = "켜기";
            }   // 광원효과  켜기
            else
            {
                nowLightTMP.text = "끄기";
            }                  // 광원효과  끄기

            // 오디오 파트
            audioTitleTMP.text = "오디오";
            masterAudioTMP.text = "전체";
            bgmAudioTMP.text = "배경음";
            sfxAudioTMP.text = "효과음";

            // 데이터 파트
            dataTitleTMP.text = "데이터";
            GameDataDeletePartTMP.text = "모든 데이터 초기화";
            GameDataDeleteButtonTMP.text = "초기화";
            OptionResetPartTMP.text = "옵션 설정 초기화";
            OptionResetButtonTMP.text = "초기화";

            // 게임플레이 파트
            gameplayTitleTMP.text = "게임 플레이";
            languagePartTMP.text = "언어";
            nowLanguageTMP.text = "한국어";
        }
        else
        {
            optionTitleTMP.text = "Option";
            optionSaveTMP.text = "Save";
            optionBackTMP.text = "Back";

            // 그래픽 파트
            graphicTitleTMP.text = "Graphic";
            resolutionTMP.text = "Resolution";
            fullScreenTMP.text = "FullScreen";
            if (isFullScreen)
            {
                nowFullScreenTMP.text = "ON";
            }   // FullScreen  ON
            else
            {
                nowFullScreenTMP.text = "OFF";
            }                // FullScreen  OFF
            lightTMP.text = "Lighting";
            if (isLightControl)
            {
                nowLightTMP.text = "ON";
            }   // Lighting  ON
            else
            {
                nowLightTMP.text = "OFF";
            }                  // Lighting  OFF

            // 오디오 파트
            audioTitleTMP.text = "Audio";
            masterAudioTMP.text = "Master";
            bgmAudioTMP.text = "BGM";
            sfxAudioTMP.text = "SFX";

            // 데이터 파트
            dataTitleTMP.text = "Data";
            GameDataDeletePartTMP.text = "Reset All Data";
            GameDataDeleteButtonTMP.text = "Reset";
            OptionResetPartTMP.text = "Reset Option Settings";
            OptionResetButtonTMP.text = "Reset";

            // 게임플레이 파트
            gameplayTitleTMP.text = "Game Play";
            languagePartTMP.text = "Language";
            nowLanguageTMP.text = "English";
        }

        { 
            nowResolutionTMP.text =
                GameManager.GM.data.graphic_Resolution[graphic_ResolutionIndex].x +
                "x" +
                GameManager.GM.data.graphic_Resolution[graphic_ResolutionIndex].y;
        }   // 현재 해상도 텍스트
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
    public void OptionSave(bool isClose)
    {
        // 그래픽 파트
        GameManager.GM.data.isFullScreen            = isFullScreen;
        GameManager.GM.data.graphic_ResolutionIndex = graphic_ResolutionIndex;
        GameManager.GM.data.isLightControl          = isLightControl;

        // 오디오 파트
        GameManager.GM.data.masterAudioValue        = masterSlider.value;
        GameManager.GM.data.BGMAudioValue           = BGMSlider.value;
        GameManager.GM.data.SFXAudioValue           = SFXSlider.value;

        // 게임 플레이 파트
        GameManager.GM.data.isKorean                = isKorean;

       // if (isClose) CloseButton();
    }

    public void OptionLoad()
    {
        // 그래픽 파트
        isFullScreen = GameManager.GM.data.isFullScreen;
        graphic_ResolutionIndex = GameManager.GM.data.graphic_ResolutionIndex;
        isLightControl = GameManager.GM.data.isLightControl;

        // 오디오 파트
        masterSlider.value = GameManager.GM.data.masterAudioValue;
        BGMSlider.value    = GameManager.GM.data.BGMAudioValue;
        SFXSlider.value    = GameManager.GM.data.SFXAudioValue;

        // 게임 플레이 파트
        isKorean            = GameManager.GM.data.isKorean;
    }

    // 설정창 - 그래픽 파트
    public void ResolutionButton(bool isPlus)
    {
        if (isPlus)
        {
            graphic_ResolutionIndex++;

            // 만약 해상도 범위를 벗어났다면 실행
            if (graphic_ResolutionIndex >= GameManager.GM.data.graphic_Resolution.Length)
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
                graphic_ResolutionIndex = GameManager.GM.data.graphic_Resolution.Length - 1;
            }
        }

        Screen.SetResolution(
            (int)GameManager.GM.data.graphic_Resolution[graphic_ResolutionIndex].x,
            (int)GameManager.GM.data.graphic_Resolution[graphic_ResolutionIndex].y,
            isFullScreen);
        
        nowResolutionTMP.text =
            GameManager.GM.data.graphic_Resolution[graphic_ResolutionIndex].x + 
            "x" +
            GameManager.GM.data.graphic_Resolution[graphic_ResolutionIndex].y;

    }   // 해상도 설정
    public void FullScreenButton()
    {
        isFullScreen = !isFullScreen;

        Screen.SetResolution(
            (int)GameManager.GM.data.graphic_Resolution[graphic_ResolutionIndex].x,
            (int)GameManager.GM.data.graphic_Resolution[graphic_ResolutionIndex].y,
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

    // 설정창 - 데이터 파트
    public void GameDataDelete()
    {
        GameManager.GM.ResetMainDB();
        Destroy(GameManager.GM.gameObject);
        LoadingManager.LoadScene("TitleScene");
    }
    public void OptionReset()
    {
        // 그래픽 파트
        isFullScreen = true;
        graphic_ResolutionIndex = 0;
        Screen.SetResolution(
            (int)GameManager.GM.data.graphic_Resolution[graphic_ResolutionIndex].x,
            (int)GameManager.GM.data.graphic_Resolution[graphic_ResolutionIndex].y,
            isFullScreen);
        isLightControl = true;

        // 오디오 파트
        masterSlider.value = 0;
        BGMSlider.value = 0;
        SFXSlider.value = 0;

        // 데이터 파트

        // 게임 플레이 파트
        isKorean = true;

        OptionSave(false);
        OptionWindow(false);
    }

    // 설정창 - 게임 플레이 파트
    public void LanguageButton()
    {
        isKorean = !isKorean;

        if(isKorean)
        {
            nowLanguageTMP.text = "한국어";
        }
        else
        {
            nowLanguageTMP.text = "English";
        }

        // OptionWindow(false);
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
