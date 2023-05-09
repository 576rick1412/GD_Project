using UnityEngine;
using System.Threading.Tasks;

// 뒤끝 SDK namespace 추가
using BackEnd;

public class BackendManager : MonoBehaviour
{
    void Awake()
    {
        var bro = Backend.Initialize(true); // 뒤끝 초기화

        // 뒤끝 초기화에 대한 응답값

        // 성공일 경우 statusCode 204 Success
        if (bro.IsSuccess()) Debug.Log("초기화 성공 : " + bro);

        // 실패일 경우 statusCode 400대 에러 발생 
        else Debug.LogError("초기화 실패 : " + bro);

        Test();
    }

    // 동기 함수를 비동기에서 호출하게 해주는 함수(유니티 UI 접근 불가)
    async void Test()
    {
        await Task.Run(() => {
            BackendLogin.Instance.CustomLogin("user1", "1234"); // 뒤끝 로그인 함수

            // [추가] chartId의 차트 정보 불러오기
            // [변경 필요] <파일 ID>을 뒤끝 콘솔 > 차트 관리 > 아이템차트에서 등록한 차트의 파일 ID값으로 변경해주세요.
            BackendChart.Instance.EnemyChartGet(); // 예시 : "64584"

            Debug.Log("테스트를 종료합니다.");
        });
    }
}
