
[System.Serializable]
public class ExcelItemDataBase
{
    public byte code;       // ��ȣ
    public string name;     // �̸�


    public byte tear;       // ���
    public string type;     // �з�


    public byte value;      // ��
    public float cool;      // ��Ÿ��


    public string mainDes;  // ����
    public string dropDes;  // ��� ����
}

[System.Serializable]
public class ExcelEnemyDataBase
{
    public byte code;

    public string KR_name;
    public string EN_name;

    public string tear;

    public string KR_area;
    public string EN_area;
     
    public byte dmg;
    public float delay;
}
