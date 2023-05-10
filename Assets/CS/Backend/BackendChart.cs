using System.Collections.Generic;
using System.Text;
using UnityEngine;

// 뒤끝 SDK namespace 추가
using BackEnd;

public class BackendChart
{
    private static BackendChart _instance = null;

    public static BackendChart Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new BackendChart();
            }

            return _instance;
        }
    }

    public void EnemyChartGet()
    {
        string chartId = "79721";

        Debug.Log($"{chartId}의 차트 불러오기를 요청합니다.");
        var bro = Backend.Chart.GetChartContents(chartId);

        if (bro.IsSuccess() == false)
        {
            Debug.LogError($"{chartId}의 차트를 불러오는 중, 에러가 발생했습니다. : " + bro);
            return;
        }

        Debug.Log("차트 불러오기에 성공했습니다. : " + bro);
        
        foreach (LitJson.JsonData gameData in bro.FlattenRows())
        {
            EnemyList temp = new EnemyList();

            temp.code        = int.Parse(gameData["Code"     ].ToString());

            temp.krName                = gameData["KR_name"  ].ToString();
            temp.enName                = gameData["EN_name"  ].ToString();
            temp.tear                  = gameData["tear"     ].ToString();

            temp.krArea                = gameData["KR_area"  ].ToString();
            temp.enArea                = gameData["EN_area"  ].ToString();

            temp.hp        = float.Parse(gameData["HP"       ].ToString());
            temp.speed     = float.Parse(gameData["Speed"    ].ToString());

            temp.dmg         = int.Parse(gameData["Dmg"      ].ToString());
            temp.delay     = float.Parse(gameData["Delay"    ].ToString());
            temp.atkLength = float.Parse(gameData["AtkLength"].ToString());

            temp.peDis0    = float.Parse(gameData["PEdis0"   ].ToString());
            temp.peDis1    = float.Parse(gameData["PEdis1"   ].ToString());

            GameManager.GM.enemyList.Add(temp); 
        }
    }
    public void EliteChartGet()
    {
        string chartId = "79830";

        Debug.Log($"{chartId}의 차트 불러오기를 요청합니다.");
        var bro = Backend.Chart.GetChartContents(chartId);

        if (bro.IsSuccess() == false)
        {
            Debug.LogError($"{chartId}의 차트를 불러오는 중, 에러가 발생했습니다. : " + bro);
            return;
        }

        Debug.Log("차트 불러오기에 성공했습니다. : " + bro);

        foreach (LitJson.JsonData gameData in bro.FlattenRows())
        {
            EliteList temp = new EliteList();

            temp.code             = int.Parse(gameData["Code"          ].ToString());
            temp.name                       = gameData["Name"          ].ToString();
                                                                       
            temp.hp             = float.Parse(gameData["HP"            ].ToString());
            temp.speed          = float.Parse(gameData["Speed"         ].ToString());
            temp.jumpValue      = float.Parse(gameData["JumpValue"     ].ToString());

            temp.atkDamage        = int.Parse(gameData["AtkDamage"     ].ToString());
            temp.atkDelay       = float.Parse(gameData["AtkDelay"      ].ToString());
            temp.atkLength      = float.Parse(gameData["AtkLength"     ].ToString());

            temp.skill_1_Damage   = int.Parse(gameData["Skill_1_Damage"].ToString());
            temp.Skill_1_Delay  = float.Parse(gameData["Skill_1_Delay" ].ToString());
            temp.Skill_1_Length = float.Parse(gameData["Skill_2_Length"].ToString());

            temp.Skill_2_Damage   = int.Parse(gameData["Skill_2_Damage"].ToString());
            temp.Skill_2_Delay  = float.Parse(gameData["Skill_2_Delay" ].ToString());
            temp.Skill_2_Length = float.Parse(gameData["Skill_2_Length"].ToString());

            GameManager.GM.eliteList.Add(temp);
        }
    }

    public void ItemChartGet()
    {
        string chartId = "79675";

        Debug.Log($"{chartId}의 차트 불러오기를 요청합니다.");
        var bro = Backend.Chart.GetChartContents(chartId);

        if (bro.IsSuccess() == false)
        {
            Debug.LogError($"{chartId}의 차트를 불러오는 중, 에러가 발생했습니다. : " + bro);
            return;
        }

        Debug.Log("차트 불러오기에 성공했습니다. : " + bro);
        foreach (LitJson.JsonData gameData in bro.FlattenRows())
        {
            StringBuilder content = new StringBuilder();
            content.AppendLine("code : " + int.Parse(gameData["code"].ToString()));

            content.AppendLine("name : " + gameData["name"].ToString());
            content.AppendLine("tear : " + int.Parse(gameData["tear"].ToString()));
            content.AppendLine("type : " + gameData["type"].ToString());

            content.AppendLine("value : " + int.Parse(gameData["value"].ToString()));
            content.AppendLine("cool : " + float.Parse(gameData["cool"].ToString()));
            content.AppendLine("mainDes : " + gameData["mainDes"].ToString());
            content.AppendLine("dropDes : " + gameData["dropDes"].ToString());

            content.AppendLine("count : " + int.Parse(gameData["count"].ToString()));

            Debug.Log(content.ToString());
        }
    }
}
