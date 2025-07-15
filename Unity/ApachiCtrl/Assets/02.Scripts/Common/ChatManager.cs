using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviourPun, IChatClientListener
{
    ChatClient client;
    private string userName;
    private string currentChannelName;

    public InputField input;
    public Text outputText;
    public Text channelText;
    void Start()
    {
        Application.runInBackground = true; // ä���� �ְ� �ƴϱ⿡ ��׶��忡�� ����

        this.userName = DateTime.Now.ToShortDateString(); // �׽�Ʈ��
        this.currentChannelName = "Channel1557";
        this.client = new ChatClient(this);

        client.UseBackgroundWorkerForSending = true; // true�� �ƴ� ��, ������ ��׶���� ���� ������ ������
        client.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, "1.0", new AuthenticationValues(this.userName));
        AddLine(string.Format("����õ�", this.userName));
    }
    private void AddLine(string lineString)
    {
        this.channelText.text += lineString + "\r\n";
    }
    private void OnApplicationQuit()
    {
        if (this.client != null)
        {
            this.client.Disconnect();
        }
    }
    public void DebugReturn(DebugLevel level, string message)
    {
        switch (level)
        {
            case DebugLevel.ERROR:
                Debug.LogError(message);
                break;
            case DebugLevel.WARNING:
                Debug.LogWarning(message);
                break;
            default:
                Debug.Log(message);
                break;
        }
    }

    public void OnChatStateChange(ChatState state)
    {
        AddLine("Chat state changed: " + state.ToString());
    }

    public void OnConnected()
    {
        AddLine("�����");
        this.client.Subscribe(new string[] { this.currentChannelName } , 10);
    }

    public void OnDisconnected()
    {
        AddLine("���� ������");
    }
    public void OnSubscribed(string[] channels, bool[] results)
    {
        AddLine("ä�� �����" + string.Join("," , channels));
        
    }

    public void OnUnsubscribed(string[] channels)
    {
        AddLine("ä�� ������" + string.Join(",", channels));
    }

    void Update()
    {
        this.client.Service();
    }
    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        if (channelName.Equals(this.currentChannelName))
        {
            this.ShowChannel(channelName);
        }
        
    }
    private void ShowChannel(string channelName)
    {
        if (string.IsNullOrEmpty(channelName))
            return;
        ChatChannel channel = null;
        bool found = this.client.TryGetChannel(channelName, out channel);
        if (!found)
        {
            AddLine("ä���� ã�� �� �����ϴ�: " + channelName);
            return;
        }
        this.currentChannelName = channelName;
        this.outputText.text = channel.ToStringMessages();
    }

    public void OnSendEnter()
    {
        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            this.SendMessageChat(this.input.text);
            this.input.text = string.Empty;
        }
    }
    private void SendMessageChat(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return;
        }
        this.client.PublishMessage(this.currentChannelName, text);
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {

    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {

    }



    public void OnUserSubscribed(string channel, string user)
    {

    }

    public void OnUserUnsubscribed(string channel, string user)
    {

    }


}
