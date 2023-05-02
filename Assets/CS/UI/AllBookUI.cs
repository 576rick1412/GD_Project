using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AllBookUI : MonoBehaviour
{
    [Header("상단 텍스트")]
    public TextMeshProUGUI mainTitleTMP;            // 최상단 현재창 텍스트
    public int allBookIndex;                        // 만능책 현재창을 정하는 인덱스값
    public GameObject[] windowObjects;              // 윈도우 오브젝트 배열

    public Guide guide;
    public Inventory inventory;
    void Start()
    {
        allBookIndex = 0;           // 만능책 현재창을 정하는 인덱스값


        guide.charaIndex = 0;       // 도감 캐릭터 선택 인덱스
        inventory.itemIndex = 0;    // 인벤토리 아이템 인덱스 


        for (int i = 0; i < guide.charaCards.Length; i++)
            guide.charaCards[i].GetComponent<GuideCard>()._idx = i;

        UI_Setting();
    }

    void Update()
    {
        UI_Setting();
    }

    void UI_Setting()
    {
        if(GameManager.GM.data.isKorean)
        {
            switch (allBookIndex)
            {
                #region 한국어 도감
                case 0:
                    mainTitleTMP.text = "도감";

                    guide.titleTMP.text = "도감";

                    // 도감 파트 - 캐릭터 정보
                    guide.charaInfoTitleTMP.text = "캐릭터 정보";
                    guide.enemyNameTMP.text  =               GameManager.GM.GD.EnemyInfo[guide.charaIndex].KR_name;
                    guide.enemyTearTMP.text  = "등급 : "   + GameManager.GM.GD.EnemyInfo[guide.charaIndex].tear;
                    guide.enemyAreaTMP.text  = "지역 : "   + GameManager.GM.GD.EnemyInfo[guide.charaIndex].KR_area;
                    guide.enemyDmgTMP.text   = "데미지 : " + GameManager.GM.GD.EnemyInfo[guide.charaIndex].dmg;
                    guide.enemyDelayTMP.text = "딜레이 : " + GameManager.GM.GD.EnemyInfo[guide.charaIndex].delay;

                    // 도감 파트 - 캐릭터 애니메이션
                    guide.charaInfoModeTitleTMP.text = "캐릭터 애니메이션";
                    guide.bIdleTMP.text = "정지";
                    guide.bMoveTMP.text = "걷기";
                    guide.bAttackTMP.text = "공격";
                    guide.bHitTMP.text = "피격";
                    guide.bSkill_1_TMP.text = "스킬 1";
                    guide.bSkill_2_TMP.text = "스킬 2";

                    break;
                #endregion
                #region 한국어 인벤토리
                case 1:
                    mainTitleTMP.text = "인벤토리";

                    inventory.infoTitleTMP.text = "인벤토리";
                    // inventory.modeButtonTMP.text = "";
                    inventory.itemInfoTitleTMP.text = "아이템 정보";
                    // inventory.itemInfoIcon
                    inventory.itemNameTMP.text = GameManager.GM.GD.ItemDB_KR[inventory.itemIndex].name;
                    inventory.itemCountTMP.text = "Count : " + GameManager.GM.data.itemCount[inventory.itemIndex];
                    inventory.itemDesTitleTMP.text = "아이템 정보";
                    inventory.itemDesTMP.text = GameManager.GM.GD.ItemDB_KR[inventory.itemIndex].mainDes;
                    inventory.itemDropInfoTMP.text = "획득 정보";
                    inventory.itemDropTMP.text = GameManager.GM.GD.ItemDB_KR[inventory.itemIndex].dropDes;

                    break;
                #endregion
            }

        }
        else
        {
            switch (allBookIndex)
            {
                #region 영어 도감
                case 0:
                    mainTitleTMP.text = "Guide";

                    guide.titleTMP.text = "Guide";

                    // 도감 파트 - 캐릭터 정보
                    guide.charaInfoTitleTMP.text = "Character Infomatin";
                    guide.enemyNameTMP.text  =              GameManager.GM.GD.EnemyInfo[guide.charaIndex].EN_name;
                    guide.enemyTearTMP.text  = "tear : "  + GameManager.GM.GD.EnemyInfo[guide.charaIndex].tear;
                    guide.enemyAreaTMP.text  = "Area : "  + GameManager.GM.GD.EnemyInfo[guide.charaIndex].EN_area;
                    guide.enemyDmgTMP.text   = "DMG : "   + GameManager.GM.GD.EnemyInfo[guide.charaIndex].dmg;
                    guide.enemyDelayTMP.text = "Delay : " + GameManager.GM.GD.EnemyInfo[guide.charaIndex].delay;

                    // 도감 파트 - 캐릭터 애니메이션
                    guide.charaInfoModeTitleTMP.text = "Character Animation";
                    guide.bIdleTMP.text = "Idle";
                    guide.bMoveTMP.text = "Move";
                    guide.bAttackTMP.text = "Attack";
                    guide.bHitTMP.text = "Hit";
                    guide.bSkill_1_TMP.text = "Skill 1";
                    guide.bSkill_2_TMP.text = "Skill 2";

                    break;
                #endregion
                #region 한국어 인벤토리
                case 1:
                    mainTitleTMP.text = "Inventory";






                    break;
                    #endregion
            }

        }

        switch (allBookIndex)
        {
            #region 공용 도감
            case 0:

                // 전체 적용 설정값
                if (guide.charaIndex > 8)
                {
                    guide.oSkill_1.SetActive(false);
                    guide.oSkill_2.SetActive(false);
                }
                else
                {
                    guide.oSkill_1.SetActive(true);
                    guide.oSkill_2.SetActive(true);
                }

                Destroy(guide.tempCharaObject);
                guide.tempCharaObject = Instantiate(guide.charaObjects[guide.charaIndex], guide.charaSpawnPoint.position, Quaternion.identity);
                guide.tempCharaAnim = guide.tempCharaObject.GetComponent<Animator>();

                guide.tempCharaObject.transform.SetParent(guide.charaSpawnPoint);
                guide.tempCharaObject.transform.localScale = new Vector3(1, 1, 1);

                break;
                #endregion
        }
    }

    // 만능책 전역 함수
    public void Select_Window(int i)
    {
        allBookIndex = i;

        foreach(var obj in windowObjects)
            obj.SetActive(false);

        windowObjects[i].SetActive(true);
    }

    // 도감 함수
    public void Select_GuideCard(int i) 
    { 
        guide.charaIndex = i;
        UI_Setting();
    }
    public void CharaAnime(int i)
    {
        guide.tempCharaAnim.SetInteger("action", i);
    }

    [System.Serializable]
    public struct ModeSet
    {
        public string KR_Str;
        public string EN_Str;
    }   // 한 / 영 구조체

    [System.Serializable]
    public struct Guide
    {
        public TextMeshProUGUI titleTMP;                // 도감 그리드 타이틀 텍스트

        [HideInInspector]
        public int charaIndex;                          // 도감 캐릭터 인덱스

        [Header("도감 파트 - 캐릭터 정보")]
        public TextMeshProUGUI charaInfoTitleTMP;       // 캐릭터 정보 타이틀 텍스트
        public GameObject[] charaCards;                 // 캐릭터 카드 오브젝트
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
        public GameObject oSkill_1;                     // 스킬 1 버튼 오브젝트
        public GameObject oSkill_2;                     // 스킬 2 버튼 오브젝트
        public RectTransform charaSpawnPoint;           // 캐릭터 오브젝트 스폰 포인트
        public GameObject[] charaObjects;               // 캐릭터 오브젝트 배열

        [HideInInspector]
        public GameObject tempCharaObject;              // 현재 스폰중인 캐릭터 오브젝트
        [HideInInspector]
        public Animator tempCharaAnim;                  // 현재 스폰중인 캐릭터 애니메이션 컴포넌트
    }

    [System.Serializable]
    public struct Inventory
    {
        [HideInInspector]
        public int itemIndex;                           // 인벤토리 아이템 인덱스

        [Header("인벤토리 - 아이템 배열")]
        public GameObject[] consums;                    // 소비 아이템 배열
        public GameObject[] materials;                  // 재료 아이템 배열
        public GameObject[] equipments;                 // 장비 아이템 배열
        
        [Header("인벤토리 파트 - 아이템 목록")]
        public TextMeshProUGUI infoTitleTMP;            // 캐릭터 정보 타이틀 텍스트
        public TextMeshProUGUI modeButtonTMP;           // 인벤토리 모드 설정 버튼 텍스트

        [Header("인벤토리 파트 - 아이템 정보")]
        public TextMeshProUGUI itemInfoTitleTMP;        // 아이템 정보 타이틀 텍스트
        public Image itemInfoIcon;                      // 아이템 아이콘 이미지
        public TextMeshProUGUI itemNameTMP;             // 아이템 이름 텍스트
        public TextMeshProUGUI itemCountTMP;            // 아이템 개수 텍스트
        public TextMeshProUGUI itemDesTitleTMP;         // 아이템 설명 타이틀 텍스트
        public TextMeshProUGUI itemDesTMP;              // 아이템 설명 텍스트
        public TextMeshProUGUI itemDropInfoTMP;         // 아이템 드랍 정보 타이틀 텍스트
        public TextMeshProUGUI itemDropTMP;             // 아이템 드랍 정보 텍스트
    }
}
