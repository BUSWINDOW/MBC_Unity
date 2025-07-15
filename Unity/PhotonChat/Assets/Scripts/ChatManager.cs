using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Chat;
using Photon.Realtime;
using ExitGames.Client.Photon;
using AuthenticationValues = Photon.Chat.AuthenticationValues; // ���� ê ����
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.UIElements;


public class ChatManager : MonoBehaviourPun , IChatClientListener
{
    private ChatClient chatClient;
    private string userName;
    private string currentChannelName;

    public Text currentChannelText;
    public Text outputText;
    public InputField inputFieldChat;
    void Start()
    {
        Application.runInBackground = true; // ä���� �ְ� �ƴϱ⿡ ��׶��忡�� ����

        this.userName = DateTime.Now.ToShortDateString(); // �׽�Ʈ��
        this.currentChannelName = "Channel 1557";
        chatClient = new ChatClient(this);

        chatClient.UseBackgroundWorkerForSending = true; // true�� �ƴ� ��, ������ ��׶���� ���� ������ ������
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, "1.0", new AuthenticationValues(this.userName));
        AddLine(string.Format("����õ�", this.userName));
    }
    public void AddLine(string lineString)
    {
        this.outputText.text += lineString + "\r\n";
    }

    private void OnApplicationQuit()
    {
        if(chatClient != null)
        {
            chatClient.Disconnect();
        }
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        switch (level)
        {
            case ExitGames.Client.Photon.DebugLevel.ERROR:
                Debug.LogError(message);
                break;
            case ExitGames.Client.Photon.DebugLevel.WARNING:
                Debug.LogWarning(message);
                break;
            default:
                Debug.Log(message);
                break;
        }
    }
    public void OnConnected()
    {
        //���� ���ῡ ���� �ߴٸ�
        AddLine("���� ���� ����");
        this.chatClient.Subscribe(new string[] { this.currentChannelName }, 10);
    }
    public void OnDisconnected()
    {
        //���� ������ �������ٸ�
        AddLine("���� ���� ����");
    }
    public void OnChatStateChange(ChatState state)
    {
        //���� Ŭ���̾�Ʈ ���¸� ���
        Debug.Log("State Change : " + state);
    }


    public void OnSubscribed(string[] channels, bool[] results)
    {
        AddLine("ä�� ����" + string.Join(",", channels));
    }

    public void OnUnsubscribed(string[] channels)
    {
        AddLine("ä�� ����" + string.Join(",", channels));
    }


    
    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        // chatClient.Service()�� �� ȣ�� �� �̰� ȣ����
        // Update�� ����
        if (channelName.Equals(this.currentChannelName))
        {
            this.ShowChannel(channelName);
        }
    }
    public void ShowChannel(string channelName)
    {
        if (string.IsNullOrEmpty(channelName))
        {
            return; // ä�� �̸��� ����ִٸ� ����
        }
        ChatChannel channel = null;
        bool found = this.chatClient.TryGetChannel(channelName, out channel);
        // ä�� Ȯ�� �� Ȯ�εǸ� channel ������ ä�γ��� �Ҵ�
        if (!found)
        {
            //ä���� Ȯ�ε��� ������
            Debug.Log("ä�� Ȯ�� ����" + channelName);
            return;
        }
        // return�����ʰ� ������� �Դٸ� = ä���� Ȯ�� �ƴٸ�
        this.currentChannelName = channelName;
        //ä�ο� ����� ��� ä�� �޼����� �ҷ��´�
        //���� �̸��� ä�� ������ ���� �ҷ�������
        this.currentChannelText.text = channel.ToStringMessages();
        Debug.Log("ä�� : " + this.currentChannelName);
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        //���� �޼����� ���� ��� ���
        //�ӼӸ�

        Debug.Log("�ӼӸ� : " + sender + " : " + message);
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        Debug.Log("����" + user + " : " + status + " , " + message);
    }

    private void Update()
    {
        this.chatClient.Service();
    }

    public void OnUserSubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }



    //�ν������� InputField���� �Է� ���� �޼����� ������ ���� �Լ��� �����Ѵ�.
    public void OnEnterSend()
    {
        if (Input.GetKey(KeyCode.Return)||Input.GetKey(KeyCode.KeypadEnter))
        {
            this.SendMessageChat(this.inputFieldChat.text);
            this.inputFieldChat.text = ""; // �޼��� ���� �� �Է�â ����
        }
    }
    private void SendMessageChat(string inputLine)
    {
        if(string.IsNullOrEmpty(inputLine)) 
        {
            return; // �Է��� ����ִٸ� ����
        }
        this.chatClient.PublishMessage(this.currentChannelName, inputLine);
    }
}
