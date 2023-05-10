using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.IO;
using System;
using System.Text;
using System.Security.Cryptography;
using AesEncryptionNS.Con;
public class GameManager : MonoBehaviour
{
    public static GameManager GM;
    public ExcelGameData GD;

    // 카메라 줌
    public float minZoom;
    public float maxZoom;
    public bool isZoom;

    // DB에서 뽑은 정보 리스트
    public List<EnemyList> enemyList;
    public List<EliteList> eliteList;

    // 메인DB 및 데이터 저장 경로
    public MainDB data;
    string filePath;

    void Awake()
    {
        var obj = FindObjectsOfType<GameManager>();
        if (obj.Length == 1) DontDestroyOnLoad(gameObject);
        else Destroy(gameObject);

        GM = this;
        filePath = Application.persistentDataPath + "/MainDB.txt";
        Debug.Log(filePath);

        Screen.SetResolution(
            (int)data.graphic_Resolution[data.graphic_ResolutionIndex].x,
            (int)data.graphic_Resolution[data.graphic_ResolutionIndex].y,
            data.isFullScreen);

        LoadData();
    }

    void Start()
    {
        
    }

    void Update()
    {

    }

    public void SavaData()
    {
        string key = data.key;
        var save = JsonUtility.ToJson(data);

        save = Program.Encrypt(save, key);
        File.WriteAllText(filePath, save);
    }   // Json 저장
    public void LoadData()
    {
        if (!File.Exists(filePath)) { ResetMainDB(); return; }

        string key = data.key;
        var load = File.ReadAllText(filePath);

        load = Program.Decrypt(load, key);
        data = JsonUtility.FromJson<MainDB>(load);
    }   // Json 로딩
    public void ResetMainDB()
    {
        data = new MainDB();

        // =======================================================

        // 설정창 - 그래픽 파트
        {
            data.graphic_Resolution[0] = new Vector2(1920, 1080);
            data.graphic_Resolution[1] = new Vector2(1680, 1050);
            data.graphic_Resolution[2] = new Vector2(1600, 900);
            data.graphic_Resolution[3] = new Vector2(1440, 900);
            data.graphic_Resolution[4] = new Vector2(1280, 1024);
            data.graphic_Resolution[5] = new Vector2(1280, 960);
            data.graphic_Resolution[6] = new Vector2(1280, 720);
        }   // 그래픽 해상도 초기화
        data.graphic_ResolutionIndex = 0;
        data.isFullScreen = true;
        data.isLightControl = true;

        // 설정창 - 오디오 파트
        data.masterAudioValue = 0f;
        data.BGMAudioValue = 0f;
        data.SFXAudioValue = 0f;

        // 설정창 - 게임플레이 파트
        data.isKorean = true;

        // =======================================================

        // 아이템 데이터 초기화
        for (int i = 0; i < data.itemCount.Length; i++)
            data.itemCount[i] = 0;

        // 적 체크 데이터 초기화
        for (int i = 0; i < data.isEnemyConfirmeds.Length; i++)
            data.isEnemyConfirmeds[i] = false;
            


    SavaData();
    }

    [Serializable]
    public class MainDB
    {
        // AES 암호화 키
        [HideInInspector]
        public string
            key = "m#XhYd*FJbNkWzOvLqI@cPeT";



        // =======================================================

        // 설정창 - 그래픽 파트
        [HideInInspector]
        public Vector2[] graphic_Resolution = new Vector2[7];   // 그래픽 해상도 설정
        public int graphic_ResolutionIndex;                     // 그래픽 해상도 설정 인덱스
        public bool isFullScreen;                               // 전체화면 여부
        public bool isLightControl;                             // 광원효과 여부

        // 설정창 - 오디오 파트
        public float masterAudioValue;                          // 마스터 볼륨값
        public float BGMAudioValue;                             // 배경음 볼륨값
        public float SFXAudioValue;                             // 효과음 볼륨값

        // 설정창 - 게임플레이 파트
        public bool isKorean;

        // =======================================================

        // 만능책 - 도감
        public bool[] isEnemyConfirmeds = new bool[28];         // 저치한 경험이 있는 적 체크

        // 만능책 - 인벤토리
        public ushort[] itemCount = new ushort[33];             // 아이템 데이터 저장
    }
}

[Serializable]
public struct EnemyList
{
    public string krName;
    public int code;

    public string enName;
    public string tear;

    public string krArea;
    public string enArea;

    public float hp;
    public float speed;

    public int dmg;
    public float delay;
    public float atkLength;

    public float peDis0;
    public float peDis1;
}
[Serializable]
public struct EliteList
{
    public string name;
    public int code;

    public float hp;
    public float speed;
    public float jumpValue;

    public int atkDamage;
    public float atkDelay;
    public float atkLength;

    public int skill_1_Damage;
    public float Skill_1_Delay;
    public float Skill_1_Length;

    public int Skill_2_Damage;
    public float Skill_2_Delay;
    public float Skill_2_Length;
}

namespace AesEncryptionNS.Con
{
    public class Program
    {
        public static string Decrypt(string textToDecrypt, string key)
        {
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.PKCS7;
            rijndaelCipher.KeySize = 256;
            rijndaelCipher.BlockSize = 256;
            byte[] encryptedData = Convert.FromBase64String(textToDecrypt);
            byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
            byte[] keyBytes = new byte[32];
            int len = pwdBytes.Length;
            if (len > keyBytes.Length) { len = keyBytes.Length; }
            Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = keyBytes;
            rijndaelCipher.IV = keyBytes;
            byte[] plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
            return Encoding.UTF8.GetString(plainText);
        }

        public static string Encrypt(string textToEncrypt, string key)
        {
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.PKCS7;
            rijndaelCipher.KeySize = 256;
            rijndaelCipher.BlockSize = 256;
            byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
            byte[] keyBytes = new byte[32];
            int len = pwdBytes.Length;
            if (len > keyBytes.Length) { len = keyBytes.Length; }
            Array.Copy(pwdBytes, keyBytes, len);
            rijndaelCipher.Key = keyBytes;
            rijndaelCipher.IV = keyBytes;
            ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
            byte[] plainText = Encoding.UTF8.GetBytes(textToEncrypt);
            return Convert.ToBase64String(transform.TransformFinalBlock(plainText, 0, plainText.Length));
        }
    }
}
