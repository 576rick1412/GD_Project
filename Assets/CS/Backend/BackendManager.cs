using UnityEngine;
using System.Threading.Tasks;

// �ڳ� SDK namespace �߰�
using BackEnd;

public class BackendManager : MonoBehaviour
{
    void Awake()
    {
        var bro = Backend.Initialize(true); // �ڳ� �ʱ�ȭ

        // �ڳ� �ʱ�ȭ�� ���� ���䰪

        // ������ ��� statusCode 204 Success
        if (bro.IsSuccess()) Debug.Log("�ʱ�ȭ ���� : " + bro);

        // ������ ��� statusCode 400�� ���� �߻� 
        else Debug.LogError("�ʱ�ȭ ���� : " + bro);

        Test();
    }

    // ���� �Լ��� �񵿱⿡�� ȣ���ϰ� ���ִ� �Լ�(����Ƽ UI ���� �Ұ�)
    async void Test()
    {
        await Task.Run(() => {
            BackendLogin.Instance.CustomLogin("user1", "1234"); // �ڳ� �α��� �Լ�

            // [�߰�] chartId�� ��Ʈ ���� �ҷ�����
            // [���� �ʿ�] <���� ID>�� �ڳ� �ܼ� > ��Ʈ ���� > ��������Ʈ���� ����� ��Ʈ�� ���� ID������ �������ּ���.
            BackendChart.Instance.EnemyChartGet(); // ���� : "64584"

            Debug.Log("�׽�Ʈ�� �����մϴ�.");
        });
    }
}
