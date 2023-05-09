using System.Collections.Generic;
using System.Text;
using UnityEngine;

// �ڳ� SDK namespace �߰�
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

        Debug.Log($"{chartId}�� ��Ʈ �ҷ����⸦ ��û�մϴ�.");
        var bro = Backend.Chart.GetChartContents(chartId);

        if (bro.IsSuccess() == false)
        {
            Debug.LogError($"{chartId}�� ��Ʈ�� �ҷ����� ��, ������ �߻��߽��ϴ�. : " + bro);
            return;
        }

        Debug.Log("��Ʈ �ҷ����⿡ �����߽��ϴ�. : " + bro);
        /*
        foreach (LitJson.JsonData gameData in bro.FlattenRows())
        {
            StringBuilder content = new StringBuilder();
            content.AppendLine("Code : " + int.Parse(gameData["Code"].ToString()));

            content.AppendLine("KR_name : " + gameData["KR_name"].ToString());
            content.AppendLine("EN_name : " + gameData["EN_name"].ToString());

            content.AppendLine("tear : " + gameData["tear"].ToString());
            content.AppendLine("KR_area : " + gameData["KR_area"].ToString());
            content.AppendLine("EN_area : " + gameData["EN_area"].ToString());

            content.AppendLine("Dmg : " + int.Parse(gameData["Dmg"].ToString()));
            content.AppendLine("Delay : " + float.Parse(gameData["Delay"].ToString()));
            content.AppendLine("Length : " + float.Parse(gameData["AtkLength"].ToString()));

            Debug.Log(content.ToString());
        }
        */
    }

    public void ItemChartGet()
    {
        string chartId = "79675";

        Debug.Log($"{chartId}�� ��Ʈ �ҷ����⸦ ��û�մϴ�.");
        var bro = Backend.Chart.GetChartContents(chartId);

        if (bro.IsSuccess() == false)
        {
            Debug.LogError($"{chartId}�� ��Ʈ�� �ҷ����� ��, ������ �߻��߽��ϴ�. : " + bro);
            return;
        }

        Debug.Log("��Ʈ �ҷ����⿡ �����߽��ϴ�. : " + bro);
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
