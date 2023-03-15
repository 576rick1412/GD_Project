using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    static string moveTo;
    static string tip;
    [SerializeField] Image progressBar;

    public TextMeshProUGUI tip_TMP;

    void Start()
    {
        // 팁
        //tip = Random.Range();

        tip_TMP   .text = tip;

        StartCoroutine(LoadSceneProcess());
    }

    public static void LoadScene(string sceneName)
    {
        moveTo = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(moveTo);
        op.allowSceneActivation = false;

        float fakePoint = 0.1f; // 게이지 시작 분기점
        float timer = 0f;       // 게이지 올라가는 시간
        while (!op.isDone)
        {
            yield return null;

            if (op.progress < fakePoint)
            {
                progressBar.fillAmount = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime * 0.5f;
                progressBar.fillAmount = Mathf.Lerp(fakePoint, 1f, timer);
                if (progressBar.fillAmount >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
