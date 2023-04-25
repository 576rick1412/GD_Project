
[System.Serializable]
public class ExcelItemDataBase
{
    public byte code;       // 번호
    public string name;     // 이름


    public byte tear;       // 등급
    public string type;     // 분류


    public byte value;      // 값
    public float cool;      // 쿨타임


    public string mainDes;  // 설명
    public string dropDes;  // 드랍 정보
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
