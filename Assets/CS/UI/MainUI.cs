using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class MainUI : MonoBehaviour
{
    public GameObject setWindow;   // ���� ���� UI ����
    public bool setActiveValue;    // ���� ���� UI�� ���� Ȯ��

    [Header("��������Ȯ��â")]
    public GameObject gameEndWindow;
    public TextMeshProUGUI alarmTMP;
    public TextMeshProUGUI endWindowYesTMP;
    public TextMeshProUGUI endWindowNoTMP;

    [Header("���Ӽ���â")]
    public GameObject optionWindow;

    [Header("����â")]
    public TextMeshProUGUI optionTitleTMP;          // �ɼ� Ÿ��Ʋ �ؽ�Ʈ
    public TextMeshProUGUI optionSaveTMP;           // �ɼ� ���� �ؽ�Ʈ
    public TextMeshProUGUI optionBackTMP;           // �ɼ� ������ �ؽ�Ʈ

    [Header("����â - �׷��� ��Ʈ")]
    public TextMeshProUGUI graphicTitleTMP;         // �׷��� Ÿ��Ʋ �ؽ�Ʈ
    public TextMeshProUGUI resolutionTMP;           // �ػ� �ؽ�Ʈ
    public TextMeshProUGUI nowResolutionTMP;        // ���� �ػ� �ؽ�Ʈ
    public TextMeshProUGUI fullScreenTMP;           // ��üȭ�� �ؽ�Ʈ
    public TextMeshProUGUI nowFullScreenTMP;        // ���� ��üȭ�� �ؽ�Ʈ
    public TextMeshProUGUI lightTMP;                // ����ȿ�� �ؽ�Ʈ
    public TextMeshProUGUI nowLightTMP;             // ���� ����ȿ�� �ؽ�Ʈ
    bool isFullScreen;                              // ��üȭ�� ����
    int graphic_ResolutionIndex;                    // �׷��� �ػ� ���� �ε���
    bool isLightControl;                            // ����ȿ�� ����

    [Header("����â - ����� ��Ʈ")]
    public TextMeshProUGUI audioTitleTMP;           // ����� Ÿ��Ʋ �ؽ�Ʈ
    public AudioMixer audioMixer;                   // ����� �ͼ� �ؽ�Ʈ
    public TextMeshProUGUI masterAudioTMP;          // ��ü ����� ���� �ؽ�Ʈ
    public Slider masterSlider;                     // ��ü ����� �����̴�
    public TextMeshProUGUI bgmAudioTMP;             // ����� ���� �ؽ�Ʈ
    public Slider BGMSlider;                        // ��ü ����� �����̴� 
    public TextMeshProUGUI sfxAudioTMP;             // ȿ���� ���� �ؽ�Ʈ
    public Slider SFXSlider;                        // ��ü ����� �����̴�

    [Header("����â - ������ ��Ʈ")]
    public TextMeshProUGUI dataTitleTMP;            // ������ Ÿ��Ʋ �ؽ�Ʈ
    public TextMeshProUGUI GameDataDeletePartTMP;   // ��� ������ �ʱ�ȭ �ؽ�Ʈ
    public TextMeshProUGUI GameDataDeleteButtonTMP; // ��� ������ �ʱ�ȭ ��ư �ؽ�Ʈ
    public TextMeshProUGUI OptionResetPartTMP;      // �ɼ� ���� �ʱ�ȭ �ؽ�Ʈ
    public TextMeshProUGUI OptionResetButtonTMP;    // �ɼ� ���� �ʱ�ȭ ��ư �ؽ�Ʈ



    [Header("����â - �����÷��� ��Ʈ")]
    public TextMeshProUGUI gameplayTitleTMP;        // �����÷��� Ÿ��Ʋ �ؽ�Ʈ
    public TextMeshProUGUI languagePartTMP;         // ���� �ؽ�Ʈ
    public TextMeshProUGUI nowLanguageTMP;          // ���� ��� ������ �ؽ�Ʈ
    bool isKorean;

    void Start()
    {
        setWindow = null;
        setActiveValue = false;

        OptionLoad();   // �ɼ� ���� �ҷ�����
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
            alarmTMP.text = "������ �����Ͻðڽ��ϱ�?";
            endWindowYesTMP.text = "Ȯ��";
            endWindowNoTMP.text = "���";
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
        OptionLoad();   // GM�� �ִ� �ɼ� ������ �ҷ�����

        if (isControl)
        {
            WindowControl(ref optionWindow);
        }

        // ���� ���� �ؽ�Ʈ ����
        if (isKorean)
        {
            optionTitleTMP.text = "����";
            optionSaveTMP.text = "�����ϱ�";
            optionBackTMP.text = "������";

            // �׷��� ��Ʈ
            graphicTitleTMP.text = "�׷���";
            resolutionTMP.text = "�ػ�";
            fullScreenTMP.text = "��üȭ��";
            if (isFullScreen)
            {
                nowFullScreenTMP.text = "�ѱ�";
            }   // ��üȭ��  �ѱ�
            else
            {
                nowFullScreenTMP.text = "����";
            }                // ��üȭ��  ����
            lightTMP.text = "����ȿ��";
            if (isLightControl)
            {
                nowLightTMP.text = "�ѱ�";
            }   // ����ȿ��  �ѱ�
            else
            {
                nowLightTMP.text = "����";
            }                  // ����ȿ��  ����

            // ����� ��Ʈ
            audioTitleTMP.text = "�����";
            masterAudioTMP.text = "��ü";
            bgmAudioTMP.text = "�����";
            sfxAudioTMP.text = "ȿ����";

            // ������ ��Ʈ
            dataTitleTMP.text = "������";
            GameDataDeletePartTMP.text = "��� ������ �ʱ�ȭ";
            GameDataDeleteButtonTMP.text = "�ʱ�ȭ";
            OptionResetPartTMP.text = "�ɼ� ���� �ʱ�ȭ";
            OptionResetButtonTMP.text = "�ʱ�ȭ";

            // �����÷��� ��Ʈ
            gameplayTitleTMP.text = "���� �÷���";
            languagePartTMP.text = "���";
            nowLanguageTMP.text = "�ѱ���";
        }
        else
        {
            optionTitleTMP.text = "Option";
            optionSaveTMP.text = "Save";
            optionBackTMP.text = "Back";

            // �׷��� ��Ʈ
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

            // ����� ��Ʈ
            audioTitleTMP.text = "Audio";
            masterAudioTMP.text = "Master";
            bgmAudioTMP.text = "BGM";
            sfxAudioTMP.text = "SFX";

            // ������ ��Ʈ
            dataTitleTMP.text = "Data";
            GameDataDeletePartTMP.text = "Reset All Data";
            GameDataDeleteButtonTMP.text = "Reset";
            OptionResetPartTMP.text = "Reset Option Settings";
            OptionResetButtonTMP.text = "Reset";

            // �����÷��� ��Ʈ
            gameplayTitleTMP.text = "Game Play";
            languagePartTMP.text = "Language";
            nowLanguageTMP.text = "English";
        }

        { 
            nowResolutionTMP.text =
                GameManager.GM.data.graphic_Resolution[graphic_ResolutionIndex].x +
                "x" +
                GameManager.GM.data.graphic_Resolution[graphic_ResolutionIndex].y;
        }   // ���� �ػ� �ؽ�Ʈ
    }

    // ��������â
    public void EndGameButton()
    {
        GameManager.GM.SavaData();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // ����â
    public void OptionSave(bool isClose)
    {
        // �׷��� ��Ʈ
        GameManager.GM.data.isFullScreen            = isFullScreen;
        GameManager.GM.data.graphic_ResolutionIndex = graphic_ResolutionIndex;
        GameManager.GM.data.isLightControl          = isLightControl;

        // ����� ��Ʈ
        GameManager.GM.data.masterAudioValue        = masterSlider.value;
        GameManager.GM.data.BGMAudioValue           = BGMSlider.value;
        GameManager.GM.data.SFXAudioValue           = SFXSlider.value;

        // ���� �÷��� ��Ʈ
        GameManager.GM.data.isKorean                = isKorean;

       // if (isClose) CloseButton();
    }

    public void OptionLoad()
    {
        // �׷��� ��Ʈ
        isFullScreen = GameManager.GM.data.isFullScreen;
        graphic_ResolutionIndex = GameManager.GM.data.graphic_ResolutionIndex;
        isLightControl = GameManager.GM.data.isLightControl;

        // ����� ��Ʈ
        masterSlider.value = GameManager.GM.data.masterAudioValue;
        BGMSlider.value    = GameManager.GM.data.BGMAudioValue;
        SFXSlider.value    = GameManager.GM.data.SFXAudioValue;

        // ���� �÷��� ��Ʈ
        isKorean            = GameManager.GM.data.isKorean;
    }

    // ����â - �׷��� ��Ʈ
    public void ResolutionButton(bool isPlus)
    {
        if (isPlus)
        {
            graphic_ResolutionIndex++;

            // ���� �ػ� ������ ����ٸ� ����
            if (graphic_ResolutionIndex >= GameManager.GM.data.graphic_Resolution.Length)
            {
                graphic_ResolutionIndex = 0;
            }
        }
        else
        {
            graphic_ResolutionIndex--;

            // ���� �ػ� ������ ����ٸ� ����
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

    }   // �ػ� ����
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
                nowFullScreenTMP.text = "�ѱ�";
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
                nowFullScreenTMP.text = "����";
            }
            else
            {
                nowFullScreenTMP.text = "OFF";
            }
        }
    }              // ��üȭ�� ����
    public void LightButton()
    {
        isLightControl = !isLightControl;

        if (isLightControl)
        {
            if (GameManager.GM.data.isKorean)
            {
                nowLightTMP.text = "�ѱ�";
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
                nowLightTMP.text = "����";
            }
            else
            {
                nowLightTMP.text = "OFF";
            }
        }
    }                   // ����ȿ�� ����

    // ����â - ����� ��Ʈ
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

    // ����â - ������ ��Ʈ
    public void GameDataDelete()
    {
        GameManager.GM.ResetMainDB();
        Destroy(GameManager.GM.gameObject);
        LoadingManager.LoadScene("TitleScene");
    }
    public void OptionReset()
    {
        // �׷��� ��Ʈ
        isFullScreen = true;
        graphic_ResolutionIndex = 0;
        Screen.SetResolution(
            (int)GameManager.GM.data.graphic_Resolution[graphic_ResolutionIndex].x,
            (int)GameManager.GM.data.graphic_Resolution[graphic_ResolutionIndex].y,
            isFullScreen);
        isLightControl = true;

        // ����� ��Ʈ
        masterSlider.value = 0;
        BGMSlider.value = 0;
        SFXSlider.value = 0;

        // ������ ��Ʈ

        // ���� �÷��� ��Ʈ
        isKorean = true;

        OptionSave(false);
        OptionWindow(false);
    }

    // ����â - ���� �÷��� ��Ʈ
    public void LanguageButton()
    {
        isKorean = !isKorean;

        if(isKorean)
        {
            nowLanguageTMP.text = "�ѱ���";
        }
        else
        {
            nowLanguageTMP.text = "English";
        }

        // OptionWindow(false);
    }

    // â ����
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
    // â �ݱ�
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
