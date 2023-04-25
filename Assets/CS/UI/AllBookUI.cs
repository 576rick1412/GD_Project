using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AllBookUI : MonoBehaviour
{
    [Header("엑셀DB")]
    public ExcelGameData EGD;                       // 엑셀 DB

    [Header("상단 텍스트")]
    public TextMeshProUGUI mainTitleTMP;            // 최상단 현재창 텍스트

    [Header("도감 파트")]   // guide
    public TextMeshProUGUI guideTitleTMP;           // 도감 그리드 타이플 텍스트
    public byte guideIndex;                         // 도감 캐릭터 인덱스

    public TextMeshProUGUI guideSettingButtonTMP;   // 도감 그리드 세팅 버튼 텍스트
    public ModeSet[] guideModeSet;                  // 도감 그리드 세팅 버튼 텍스트 저장
    byte guideModeStringSet;                        // 도감 그리드 세팅 인덱스

    [Header("도감 파트 - 캐릭터 정보")]
    public TextMeshProUGUI charaInfoTitleTMP;       // 캐릭터 정보 타이틀 텍스트
    public TextMeshProUGUI enemyNameTMP;            // 캐릭터 이름 텍스트
    public TextMeshProUGUI enemyTearTMP;            // 캐릭터 티어 텍스트
    public TextMeshProUGUI enemyAreaTMP;            // 캐릭터 출몰지역 텍스트
    public TextMeshProUGUI enemyDmgTMP;             // 캐릭터 데미지 텍스트
    public TextMeshProUGUI enemyDelayTMP;           // 캐릭터 딜레이 텍스트

    [Header("도감 파트 - 캐릭터 애니메이션")]
    public TextMeshProUGUI charaInfoModeTitleTMP;   // 캐릭터 모드 타이틀 텍스트
    public TextMeshProUGUI bIdleTMP;                // 캐릭터 기본 버튼 텍스트
    public TextMeshProUGUI bMoveTMP;                // 캐릭터 이동 버튼 텍스트
    public TextMeshProUGUI bAttackTMP;              // 캐릭터 공격 버튼 텍스트
    public TextMeshProUGUI bHitTMP;                 // 캐릭터 피격 버튼 텍스트
    public TextMeshProUGUI bSkill_1_TMP;            // 캐릭터 스킬 1 버튼 텍스트
    public TextMeshProUGUI bSkill_2_TMP;            // 캐릭터 스킬 2 버튼 텍스트
    
    void Start()
    {
        guideModeStringSet = 0;     // 도감 설정 인덱스
        guideIndex = 0;             // 도감 캐릭터 선택 인덱스

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
                guideTitleTMP.text = "도감";

                guideSettingButtonTMP.text =
                    guideModeSet[guideModeStringSet].KR_Str;

                // 도감 파트 - 캐릭터 정보
                charaInfoTitleTMP.text = "캐릭터 정보";
                enemyNameTMP.text  = EGD.EnemyInfo[guideIndex].KR_name;     
                enemyTearTMP.text  =   "등급 : " + EGD.EnemyInfo[guideIndex].tear;     
                enemyAreaTMP.text  =   "지역 : " + EGD.EnemyInfo[guideIndex].KR_area;      
                enemyDmgTMP.text   = "데미지 : " + EGD.EnemyInfo[guideIndex].dmg;
                enemyDelayTMP.text = "딜레이 : " + EGD.EnemyInfo[guideIndex].delay;

                // 도감 파트 - 캐릭터 애니메이션
                charaInfoModeTitleTMP.text = "캐릭터 애니메이션";
                bIdleTMP.text = "정지";
                bMoveTMP.text = "걷기"; ;
                bAttackTMP.text = "공격"; ;
                bHitTMP.text = "피격"; ;
                bSkill_1_TMP.text = "스킬 1"; ;
                bSkill_2_TMP.text = "스킬 2"; ;
            }
        }
        else
        {
            {
                guideTitleTMP.text = "Guide";

                guideSettingButtonTMP.text =
                    guideModeSet[guideModeStringSet].EN_Str;

                // 도감 파트 - 캐릭터 정보
                charaInfoTitleTMP.text = "Character Infomatin";
                enemyNameTMP.text  = EGD.EnemyInfo[guideIndex].EN_name;
                enemyTearTMP.text  = "tear : "   + EGD.EnemyInfo[guideIndex].tear;
                enemyAreaTMP.text  = "Area : "   + EGD.EnemyInfo[guideIndex].EN_area;
                enemyDmgTMP.text   = "DMG : " + EGD.EnemyInfo[guideIndex].dmg;
                enemyDelayTMP.text = "Delay : " + EGD.EnemyInfo[guideIndex].delay;

                // 도감 파트 - 캐릭터 애니메이션
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
    }   // 한 / 영 구조체
}
