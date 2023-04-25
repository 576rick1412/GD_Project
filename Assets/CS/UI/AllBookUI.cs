using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AllBookUI : MonoBehaviour
{
    [Header("����DB")]
    public ExcelGameData EGD;                       // ���� DB

    [Header("��� �ؽ�Ʈ")]
    public TextMeshProUGUI mainTitleTMP;            // �ֻ�� ����â �ؽ�Ʈ

    [Header("���� ��Ʈ")]   // guide
    public TextMeshProUGUI guideTitleTMP;           // ���� �׸��� Ÿ���� �ؽ�Ʈ
    public byte guideIndex;                         // ���� ĳ���� �ε���

    public TextMeshProUGUI guideSettingButtonTMP;   // ���� �׸��� ���� ��ư �ؽ�Ʈ
    public ModeSet[] guideModeSet;                  // ���� �׸��� ���� ��ư �ؽ�Ʈ ����
    byte guideModeStringSet;                        // ���� �׸��� ���� �ε���

    [Header("���� ��Ʈ - ĳ���� ����")]
    public TextMeshProUGUI charaInfoTitleTMP;       // ĳ���� ���� Ÿ��Ʋ �ؽ�Ʈ
    public TextMeshProUGUI enemyNameTMP;            // ĳ���� �̸� �ؽ�Ʈ
    public TextMeshProUGUI enemyTearTMP;            // ĳ���� Ƽ�� �ؽ�Ʈ
    public TextMeshProUGUI enemyAreaTMP;            // ĳ���� ������� �ؽ�Ʈ
    public TextMeshProUGUI enemyDmgTMP;             // ĳ���� ������ �ؽ�Ʈ
    public TextMeshProUGUI enemyDelayTMP;           // ĳ���� ������ �ؽ�Ʈ

    [Header("���� ��Ʈ - ĳ���� �ִϸ��̼�")]
    public TextMeshProUGUI charaInfoModeTitleTMP;   // ĳ���� ��� Ÿ��Ʋ �ؽ�Ʈ
    public TextMeshProUGUI bIdleTMP;                // ĳ���� �⺻ ��ư �ؽ�Ʈ
    public TextMeshProUGUI bMoveTMP;                // ĳ���� �̵� ��ư �ؽ�Ʈ
    public TextMeshProUGUI bAttackTMP;              // ĳ���� ���� ��ư �ؽ�Ʈ
    public TextMeshProUGUI bHitTMP;                 // ĳ���� �ǰ� ��ư �ؽ�Ʈ
    public TextMeshProUGUI bSkill_1_TMP;            // ĳ���� ��ų 1 ��ư �ؽ�Ʈ
    public TextMeshProUGUI bSkill_2_TMP;            // ĳ���� ��ų 2 ��ư �ؽ�Ʈ
    
    void Start()
    {
        guideModeStringSet = 0;     // ���� ���� �ε���
        guideIndex = 0;             // ���� ĳ���� ���� �ε���

        UI_Setting();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.N))
        {
            guideIndex++;
        }

        UI_Setting();
    }

    void UI_Setting()
    {
        if(GameManager.GM.data.isKorean)
        {
            {
                guideTitleTMP.text = "����";

                guideSettingButtonTMP.text =
                    guideModeSet[guideModeStringSet].KR_Str;

                // ���� ��Ʈ - ĳ���� ����
                charaInfoTitleTMP.text = "ĳ���� ����";
                enemyNameTMP.text  = EGD.EnemyInfo[guideIndex].KR_name;     
                enemyTearTMP.text  =   "��� : " + EGD.EnemyInfo[guideIndex].tear;     
                enemyAreaTMP.text  =   "���� : " + EGD.EnemyInfo[guideIndex].KR_area;      
                enemyDmgTMP.text   = "������ : " + EGD.EnemyInfo[guideIndex].dmg;
                enemyDelayTMP.text = "������ : " + EGD.EnemyInfo[guideIndex].delay;

                // ���� ��Ʈ - ĳ���� �ִϸ��̼�
                charaInfoModeTitleTMP.text = "ĳ���� �ִϸ��̼�";
                bIdleTMP.text = "����";
                bMoveTMP.text = "�ȱ�"; ;
                bAttackTMP.text = "����"; ;
                bHitTMP.text = "�ǰ�"; ;
                bSkill_1_TMP.text = "��ų 1"; ;
                bSkill_2_TMP.text = "��ų 2"; ;
            }
        }
        else
        {
            {
                guideTitleTMP.text = "Guide";

                guideSettingButtonTMP.text =
                    guideModeSet[guideModeStringSet].EN_Str;

                // ���� ��Ʈ - ĳ���� ����
                charaInfoTitleTMP.text = "Character Infomatin";
                enemyNameTMP.text  = EGD.EnemyInfo[guideIndex].EN_name;
                enemyTearTMP.text  = "tear : "   + EGD.EnemyInfo[guideIndex].tear;
                enemyAreaTMP.text  = "Area : "   + EGD.EnemyInfo[guideIndex].EN_area;
                enemyDmgTMP.text   = "DMG : " + EGD.EnemyInfo[guideIndex].dmg;
                enemyDelayTMP.text = "Delay : " + EGD.EnemyInfo[guideIndex].delay;

                // ���� ��Ʈ - ĳ���� �ִϸ��̼�
                charaInfoModeTitleTMP.text = "Character Animation";
                bIdleTMP.text = "Idle";
                bMoveTMP.text = "Move";
                bAttackTMP.text = "Attack";
                bHitTMP.text = "Hit";
                bSkill_1_TMP.text = "Skill 1";
                bSkill_2_TMP.text = "Skill 2";


            }
        }
    }
    
    public void GuideSettingButton(byte i)
    {
        guideModeStringSet += i;

        if(i < 0)
        {
            if(guideModeStringSet < 0)
            {
                guideModeStringSet = 3;
            }
        }
        else
        {
            if(guideModeStringSet > 3)
            {
                guideModeStringSet = 0;
            }
        }
    }

    [System.Serializable]
    public struct ModeSet
    {
        public string KR_Str;
        public string EN_Str;
    }   // �� / �� ����ü
}
