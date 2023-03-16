using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class MainSceneUI : MonoBehaviour
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

    [Header("����â - �׷��� ��Ʈ")]
    public TextMeshProUGUI graphicTitleTMP;         // �׷��� Ÿ��Ʋ �ؽ�Ʈ
    public TextMeshProUGUI resolutionTMP;           // �ػ� �ؽ�Ʈ
    public TextMeshProUGUI nowResolutionTMP;        // ���� �ػ� �ؽ�Ʈ
    public TextMeshProUGUI fullScreenTMP;           // ��üȭ�� �ؽ�Ʈ
    public TextMeshProUGUI nowFullScreenTMP;        // ���� ��üȭ�� �ؽ�Ʈ
    public TextMeshProUGUI lightTMP;                // ����ȿ�� �ؽ�Ʈ
    public TextMeshProUGUI nowLightTMP;             // ���� ����ȿ�� �ؽ�Ʈ
    bool isFullScreen;                              // ��üȭ�� ����
    Vector2[] graphic_Resolution = new Vector2[7];  // �׷��� �ػ� ����
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
        }   // �׷��� �ػ� �ʱ�ȭ
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

    public void OptionWindow()
    {
        WindowControl(ref optionWindow);

        if (GameManager.GM.data.isKorean)
        {
            optionTitleTMP.text = "����";

            // �׷��� ��Ʈ
            graphicTitleTMP.text = "�׷���";
            resolutionTMP.text = "�ػ�";
            fullScreenTMP.text = "��üȭ��";
            lightTMP.text = "����ȿ��";

            // ����� ��Ʈ
            audioTitleTMP.text = "�����";
            masterAudioTMP.text = "��ü";
            bgmAudioTMP.text = "�����";
            sfxAudioTMP.text = "ȿ����";
        }
        else
        {
            optionTitleTMP.text = "Option";

            // �׷��� ��Ʈ
            graphicTitleTMP.text = "Graphic";
            resolutionTMP.text = "Resolution";
            fullScreenTMP.text = "FullScreen";
            lightTMP.text = "Lighting";

            // ����� ��Ʈ
            audioTitleTMP.text = "Audio";
            masterAudioTMP.text = "Master";
            bgmAudioTMP.text = "BGM";
            sfxAudioTMP.text = "SFX";
        }

        // ��ü�ϸ� ����
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

        // ����ȿ�� ����
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
    public void OptionReset()
    {
        // �׷���
        isFullScreen = true;
        Screen.SetResolution(
            (int)graphic_Resolution[0].x, 
            (int)graphic_Resolution[0].y,
            isFullScreen);
        graphic_ResolutionIndex = 0;
        isLightControl = true;

        // �����
        masterSlider.value = 0;
        BGMSlider.value = 0;
        SFXSlider.value = 0;
    }

    // ����â - �׷��� ��Ʈ
    public void ResolutionButton(bool isPlus)
    {
        if (isPlus)
        {
            graphic_ResolutionIndex++;

            // ���� �ػ� ������ ����ٸ� ����
            if (graphic_ResolutionIndex >= graphic_Resolution.Length)
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

    }   // �ػ� ����
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
