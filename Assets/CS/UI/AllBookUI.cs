using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AllBookUI : MonoBehaviour
{
    [Header("��� �ؽ�Ʈ")]
    public TextMeshProUGUI mainTitleTMP;            // �ֻ�� ����â �ؽ�Ʈ
    public int allBookIndex;                        // ����å ����â�� ���ϴ� �ε�����
    public GameObject[] windowObjects;              // ������ ������Ʈ �迭

    public Guide guide;
    public Inventory inventory;
    void Start()
    {
        allBookIndex = 0;           // ����å ����â�� ���ϴ� �ε�����


        guide.charaIndex = 0;       // ���� ĳ���� ���� �ε���
        inventory.itemIndex = 0;    // �κ��丮 ������ �ε��� 


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
                #region �ѱ��� ����
                case 0:
                    mainTitleTMP.text = "����";

                    guide.titleTMP.text = "����";

                    // ���� ��Ʈ - ĳ���� ����
                    guide.charaInfoTitleTMP.text = "ĳ���� ����";
                    guide.enemyNameTMP.text  =               GameManager.GM.GD.EnemyInfo[guide.charaIndex].KR_name;
                    guide.enemyTearTMP.text  = "��� : "   + GameManager.GM.GD.EnemyInfo[guide.charaIndex].tear;
                    guide.enemyAreaTMP.text  = "���� : "   + GameManager.GM.GD.EnemyInfo[guide.charaIndex].KR_area;
                    guide.enemyDmgTMP.text   = "������ : " + GameManager.GM.GD.EnemyInfo[guide.charaIndex].dmg;
                    guide.enemyDelayTMP.text = "������ : " + GameManager.GM.GD.EnemyInfo[guide.charaIndex].delay;

                    // ���� ��Ʈ - ĳ���� �ִϸ��̼�
                    guide.charaInfoModeTitleTMP.text = "ĳ���� �ִϸ��̼�";
                    guide.bIdleTMP.text = "����";
                    guide.bMoveTMP.text = "�ȱ�";
                    guide.bAttackTMP.text = "����";
                    guide.bHitTMP.text = "�ǰ�";
                    guide.bSkill_1_TMP.text = "��ų 1";
                    guide.bSkill_2_TMP.text = "��ų 2";

                    break;
                #endregion
                #region �ѱ��� �κ��丮
                case 1:
                    mainTitleTMP.text = "�κ��丮";

                    inventory.infoTitleTMP.text = "�κ��丮";
                    // inventory.modeButtonTMP.text = "";
                    inventory.itemInfoTitleTMP.text = "������ ����";
                    // inventory.itemInfoIcon
                    inventory.itemNameTMP.text = GameManager.GM.GD.ItemDB_KR[inventory.itemIndex].name;
                    inventory.itemCountTMP.text = "Count : " + GameManager.GM.data.itemCount[inventory.itemIndex];
                    inventory.itemDesTitleTMP.text = "������ ����";
                    inventory.itemDesTMP.text = GameManager.GM.GD.ItemDB_KR[inventory.itemIndex].mainDes;
                    inventory.itemDropInfoTMP.text = "ȹ�� ����";
                    inventory.itemDropTMP.text = GameManager.GM.GD.ItemDB_KR[inventory.itemIndex].dropDes;

                    break;
                #endregion
            }

        }
        else
        {
            switch (allBookIndex)
            {
                #region ���� ����
                case 0:
                    mainTitleTMP.text = "Guide";

                    guide.titleTMP.text = "Guide";

                    // ���� ��Ʈ - ĳ���� ����
                    guide.charaInfoTitleTMP.text = "Character Infomatin";
                    guide.enemyNameTMP.text  =              GameManager.GM.GD.EnemyInfo[guide.charaIndex].EN_name;
                    guide.enemyTearTMP.text  = "tear : "  + GameManager.GM.GD.EnemyInfo[guide.charaIndex].tear;
                    guide.enemyAreaTMP.text  = "Area : "  + GameManager.GM.GD.EnemyInfo[guide.charaIndex].EN_area;
                    guide.enemyDmgTMP.text   = "DMG : "   + GameManager.GM.GD.EnemyInfo[guide.charaIndex].dmg;
                    guide.enemyDelayTMP.text = "Delay : " + GameManager.GM.GD.EnemyInfo[guide.charaIndex].delay;

                    // ���� ��Ʈ - ĳ���� �ִϸ��̼�
                    guide.charaInfoModeTitleTMP.text = "Character Animation";
                    guide.bIdleTMP.text = "Idle";
                    guide.bMoveTMP.text = "Move";
                    guide.bAttackTMP.text = "Attack";
                    guide.bHitTMP.text = "Hit";
                    guide.bSkill_1_TMP.text = "Skill 1";
                    guide.bSkill_2_TMP.text = "Skill 2";

                    break;
                #endregion
                #region �ѱ��� �κ��丮
                case 1:
                    mainTitleTMP.text = "Inventory";






                    break;
                    #endregion
            }

        }

        switch (allBookIndex)
        {
            #region ���� ����
            case 0:

                // ��ü ���� ������
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

    // ����å ���� �Լ�
    public void Select_Window(int i)
    {
        allBookIndex = i;

        foreach(var obj in windowObjects)
            obj.SetActive(false);

        windowObjects[i].SetActive(true);
    }

    // ���� �Լ�
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
    }   // �� / �� ����ü

    [System.Serializable]
    public struct Guide
    {
        public TextMeshProUGUI titleTMP;                // ���� �׸��� Ÿ��Ʋ �ؽ�Ʈ

        [HideInInspector]
        public int charaIndex;                          // ���� ĳ���� �ε���

        [Header("���� ��Ʈ - ĳ���� ����")]
        public TextMeshProUGUI charaInfoTitleTMP;       // ĳ���� ���� Ÿ��Ʋ �ؽ�Ʈ
        public GameObject[] charaCards;                 // ĳ���� ī�� ������Ʈ
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
        public GameObject oSkill_1;                     // ��ų 1 ��ư ������Ʈ
        public GameObject oSkill_2;                     // ��ų 2 ��ư ������Ʈ
        public RectTransform charaSpawnPoint;           // ĳ���� ������Ʈ ���� ����Ʈ
        public GameObject[] charaObjects;               // ĳ���� ������Ʈ �迭

        [HideInInspector]
        public GameObject tempCharaObject;              // ���� �������� ĳ���� ������Ʈ
        [HideInInspector]
        public Animator tempCharaAnim;                  // ���� �������� ĳ���� �ִϸ��̼� ������Ʈ
    }

    [System.Serializable]
    public struct Inventory
    {
        [HideInInspector]
        public int itemIndex;                           // �κ��丮 ������ �ε���

        [Header("�κ��丮 - ������ �迭")]
        public GameObject[] consums;                    // �Һ� ������ �迭
        public GameObject[] materials;                  // ��� ������ �迭
        public GameObject[] equipments;                 // ��� ������ �迭
        
        [Header("�κ��丮 ��Ʈ - ������ ���")]
        public TextMeshProUGUI infoTitleTMP;            // ĳ���� ���� Ÿ��Ʋ �ؽ�Ʈ
        public TextMeshProUGUI modeButtonTMP;           // �κ��丮 ��� ���� ��ư �ؽ�Ʈ

        [Header("�κ��丮 ��Ʈ - ������ ����")]
        public TextMeshProUGUI itemInfoTitleTMP;        // ������ ���� Ÿ��Ʋ �ؽ�Ʈ
        public Image itemInfoIcon;                      // ������ ������ �̹���
        public TextMeshProUGUI itemNameTMP;             // ������ �̸� �ؽ�Ʈ
        public TextMeshProUGUI itemCountTMP;            // ������ ���� �ؽ�Ʈ
        public TextMeshProUGUI itemDesTitleTMP;         // ������ ���� Ÿ��Ʋ �ؽ�Ʈ
        public TextMeshProUGUI itemDesTMP;              // ������ ���� �ؽ�Ʈ
        public TextMeshProUGUI itemDropInfoTMP;         // ������ ��� ���� Ÿ��Ʋ �ؽ�Ʈ
        public TextMeshProUGUI itemDropTMP;             // ������ ��� ���� �ؽ�Ʈ
    }
}
