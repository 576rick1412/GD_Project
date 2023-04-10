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

    // ī�޶� ��
    public float minZoom;
    public float maxZoom;
    public bool isZoom;

    // ����DB �� ������ ���� ���
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
    }

    void Start()
    {
        LoadData();
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
    }   // Json ����
    public void LoadData()
    {
        if (!File.Exists(filePath)) { ResetMainDB(); return; }

        string key = data.key;
        var load = File.ReadAllText(filePath);

        load = Program.Decrypt(load, key);
        data = JsonUtility.FromJson<MainDB>(load);
    }   // Json �ε�
    public void ResetMainDB()
    {
        data = new MainDB();

        // ����â - �׷��� ��Ʈ
        {
            data.graphic_Resolution[0] = new Vector2(1920, 1080);
            data.graphic_Resolution[1] = new Vector2(1680, 1050);
            data.graphic_Resolution[2] = new Vector2(1600, 900);
            data.graphic_Resolution[3] = new Vector2(1440, 900);
            data.graphic_Resolution[4] = new Vector2(1280, 1024);
            data.graphic_Resolution[5] = new Vector2(1280, 960);
            data.graphic_Resolution[6] = new Vector2(1280, 720);
        }   // �׷��� �ػ� �ʱ�ȭ
        data.graphic_ResolutionIndex = 0;
        data.isFullScreen = true;
        data.isLightControl = true;

        // ����â - ����� ��Ʈ
        data.masterAudioValue = 0f;
        data.BGMAudioValue = 0f;
        data.SFXAudioValue = 0f;

        // ����â - �����÷��� ��Ʈ
        data.isKorean = true;


        SavaData();
    }

    [Serializable]
    public class MainDB
    {
        // AES ��ȣȭ Ű
        [HideInInspector]
        public string
            key = "m#XhYd*FJbNkWzOvLqI@cPeT";


        // ����â - �׷��� ��Ʈ
        [HideInInspector]
        public Vector2[] graphic_Resolution = new Vector2[7];   // �׷��� �ػ� ����
        public int graphic_ResolutionIndex;                     // �׷��� �ػ� ���� �ε���
        public bool isFullScreen;                               // ��üȭ�� ����
        public bool isLightControl;                             // ����ȿ�� ����

        // ����â - ����� ��Ʈ
        public float masterAudioValue;                          // ������ ������
        public float BGMAudioValue;                             // ����� ������
        public float SFXAudioValue;                             // ȿ���� ������

        // ����â - �����÷��� ��Ʈ
        public bool isKorean;
    }
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
