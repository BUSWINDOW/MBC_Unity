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
        Application.runInBackground = true; // 채팅이 주가 아니기에 백그라운드에서 실행

        this.userName = DateTime.Now.ToShortDateString(); // 테스트용
        this.currentChannelName = "Channel1557";
        this.client = new ChatClient(this);

        client.UseBackgroundWorkerForSending = true; // true가 아닐 시, 어플이 백그라운드로 가면 연결이 끊어짐
        client.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, "1.0", new AuthenticationValues(this.userName));
        AddLine(string.Format("연결시도", this.userName));
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
        AddLine("연결됨");
        this.client.Subscribe(new string[] { this.currentChannelName } , 10);
    }

    public void OnDisconnected()
    {
        AddLine("연결 해제됨");
    }
    public void OnSubscribed(string[] channels, bool[] results)
    {
        AddLine("채팅 입장됨" + string.Join("," , channels));
        
    }

    public void OnUnsubscribed(string[] channels)
    {
        AddLine("채팅 퇴장함" + string.Join(",", channels));
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
            AddLine("채널을 찾을 수 없습니다: " + channelName);
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
