using System;
using System.Net.Sockets;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnityMainThreadDispatcher : MonoBehaviour
{
    private static UnityMainThreadDispatcher instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void DispatchToMainThread(Action action)
    {
        if (instance != null)
        {
            instance.Invoke(action.Method.Name, 0);
        }
    }
}

public class ChatClient : MonoBehaviour
{
    private TcpClient client;
    private NetworkStream stream;
    private byte[] buffer = new byte[1024];

    public TMP_InputField inputField;
    public TMP_Text outputText;

    private void Start()
    {
        try
        {
            client = new TcpClient("218.50.158.112", 80); // 서버 IP 및 포트 입력
            stream = client.GetStream();

            // Start receiving messages in a separate thread
            System.Threading.Thread receiveThread = new System.Threading.Thread(ReceiveMessages);
            receiveThread.Start();

            outputText.text = "서버와 연결되었습니다";
            outputText.color = Color.green;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Connection error: {ex.Message}");
        }
    }

    public void Send()
    {
            SendMessage(inputField.text);
            inputField.text = ""; // Clear input field
    }

    private void ReceiveMessages()
    {
        while (true)
        {
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            if (bytesRead <= 0)
            {
                break;
            }

            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Debug.Log($"Received: {message}");

            
        }
    }

    private void SendMessage(string message)
    {
        byte[] messageBytes = Encoding.UTF8.GetBytes(message);
        stream.Write(messageBytes, 0, messageBytes.Length);
    }

    private void OnDestroy()
    {
        client.Close();
    }
}

