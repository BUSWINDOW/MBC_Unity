using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Chat;
using Photon.Realtime;
using ExitGames.Client.Photon;
using AuthenticationValues = Photon.Chat.AuthenticationValues; // 포톤 챗 인증
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
        Application.runInBackground = true; // 채팅이 주가 아니기에 백그라운드에서 실행

        this.userName = DateTime.Now.ToShortDateString(); // 테스트용
        this.currentChannelName = "Channel 1557";
        chatClient = new ChatClient(this);

        chatClient.UseBackgroundWorkerForSending = true; // true가 아닐 시, 어플이 백그라운드로 가면 연결이 끊어짐
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, "1.0", new AuthenticationValues(this.userName));
        AddLine(string.Format("연결시도", this.userName));
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
        //서버 연결에 성공 했다면
        AddLine("서버 연결 성공");
        this.chatClient.Subscribe(new string[] { this.currentChannelName }, 10);
    }
    public void OnDisconnected()
    {
        //서버 연결이 끊어졌다면
        AddLine("서버 연결 끊김");
    }
    public void OnChatStateChange(ChatState state)
    {
        //현재 클라이언트 상태를 출력
        Debug.Log("State Change : " + state);
    }


    public void OnSubscribed(string[] channels, bool[] results)
    {
        AddLine("채널 입장" + string.Join(",", channels));
    }

    public void OnUnsubscribed(string[] channels)
    {
        AddLine("채널 퇴장" + string.Join(",", channels));
    }


    
    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        // chatClient.Service()가 매 호출 시 이걸 호출함
        // Update와 연동
        if (channelName.Equals(this.currentChannelName))
        {
            this.ShowChannel(channelName);
        }
    }
    public void ShowChannel(string channelName)
    {
        if (string.IsNullOrEmpty(channelName))
        {
            return; // 채널 이름이 비어있다면 리턴
        }
        ChatChannel channel = null;
        bool found = this.chatClient.TryGetChannel(channelName, out channel);
        // 채널 확인 및 확인되면 channel 변수에 채널내용 할당
        if (!found)
        {
            //채널이 확인되지 않으면
            Debug.Log("채널 확인 실패" + channelName);
            return;
        }
        // return되지않고 여기까지 왔다면 = 채널이 확인 됐다면
        this.currentChannelName = channelName;
        //채널에 저장된 모든 채팅 메세지를 불러온다
        //유저 이름과 채팅 내용이 전부 불러와진다
        this.currentChannelText.text = channel.ToStringMessages();
        Debug.Log("채널 : " + this.currentChannelName);
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        //개인 메세지를 보낼 경우 사용
        //귓속말

        Debug.Log("귓속말 : " + sender + " : " + message);
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        Debug.Log("상태" + user + " : " + status + " , " + message);
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



    //인스펙터의 InputField에서 입력 받은 메세지를 보내기 위해 함수를 구현한다.
    public void OnEnterSend()
    {
        if (Input.GetKey(KeyCode.Return)||Input.GetKey(KeyCode.KeypadEnter))
        {
            this.SendMessageChat(this.inputFieldChat.text);
            this.inputFieldChat.text = ""; // 메세지 전송 후 입력창 비우기
        }
    }
    private void SendMessageChat(string inputLine)
    {
        if(string.IsNullOrEmpty(inputLine)) 
        {
            return; // 입력이 비어있다면 리턴
        }
        this.chatClient.PublishMessage(this.currentChannelName, inputLine);
    }
}
