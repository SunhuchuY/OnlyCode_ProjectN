using BackEnd;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BackendInitialized : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        var bro = Backend.Initialize(true); // 뒤끝 초기화

        // 뒤끝 초기화에 대한 응답값
        if (bro.IsSuccess())
        {
            //성공일 경우 statusCode 204 Success
            Debug.Log("초기화 성공 : " + bro);
        }
        else
        {
            // 실패일 경우 statusCode 400대 에러 발생 
            Debug.Log("초기화 실패 : " + bro);
        }

        // SendQueue 초기화
        SendQueue.StartSendQueue(true);
        StartCoroutine(UpdateQueuePolling());
    }

    private void Update()
    {
        // 뒤끝 비동기 함수 사용 시, 메인쓰레드에서 콜백을 처리해주는 Dispatch
        Backend.AsyncPoll();
    }

    private IEnumerator UpdateQueuePolling()
    {
        while (true)
        {
            if (SendQueue.IsInitialize == false)
                break;

            SendQueue.Poll();
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator OnApplicationQuit()
    {
        if (SendQueue.IsInitialize)
        {
            while (SendQueue.UnprocessedFuncCount > 0)
            {
                SendQueue.Poll();
                yield return null;
            }

            SendQueue.StopSendQueue();
        }
    }
}