using System.Collections.Generic;
using System.Text;
using Proton;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 房间界面
/// </summary>
public class RoomPanel : UIPanel
{
    #region UI 组件

    private List<Transform> _playerItems = new List<Transform>();
    private Button _startButton;
    private Button _quitButton;

    #endregion

    #region 生命周期方法

    /// <summary>
    /// 初始化
    /// </summary>
    public override void Init(params object[] args)
    {
        base.Init(args);

        SkinPath = "RoomPanel";
        Layer = PanelLayerEnum.Panel;
    }

    /// <summary>
    /// 显示界面
    /// </summary>
    public override void OnShowing()
    {
        base.OnShowing();

        Transform skinTransform = Skin.transform;

        // 获取玩家项组件
        for (int i = 0; i < 6; i++)
        {
            string itemName = "PlayerItem" + i;
            Transform itemTransform = skinTransform.Find(itemName);
            _playerItems.Add(itemTransform);
        }

        _startButton = skinTransform.Find("StartBtn").GetComponent<Button>();
        _quitButton = skinTransform.Find("QuitBtn").GetComponent<Button>();

        // 为按钮添加点击事件监听
        _startButton.onClick.AddListener(OnStartClick);
        _quitButton.onClick.AddListener(OnQuitClick);

        // 向消息分发器注册事件监听回调方法
        NetworkManager.Instance.serverConn.AddEventListener(ProtocolType.GET_ROOM_INFO, OnGetRoomInfoCallback);
        NetworkManager.Instance.serverConn.AddEventListener(ProtocolType.FIGHT, OnStartFightCallback);

        // 向服务端查询“房间信息”
        ProtocolBytes proto = new ProtocolBytes();
        proto.AddString(ProtocolType.GET_ROOM_INFO);
        NetworkManager.Instance.serverConn.Send(proto);
    }

    /// <summary>
    /// 关闭界面
    /// </summary>
    public override void OnClosing()
    {
        base.OnClosing();

        // 从消息分发器移除事件监听回调方法
        NetworkManager.Instance.serverConn.RemoveEventListener(ProtocolType.GET_ROOM_INFO, OnGetRoomInfoCallback);
        NetworkManager.Instance.serverConn.RemoveEventListener(ProtocolType.FIGHT, OnStartFightCallback);
    }

    #endregion

    /// <summary>
    /// 获取到房间信息后的回调方法
    /// </summary>
    /// <param name="proto">协议消息</param>
    private void OnGetRoomInfoCallback(Protocol proto)
    {
        ProtocolBytes responseProto = (ProtocolBytes) proto;

        int start = 0;

        // 协议名称，暂时无用，但需要从字节流中先读取出来
        string unused = responseProto.GetString(start, ref start);

        // 获取房间中的玩家数
        int playerCount = responseProto.GetInt(start, ref start);

        // 为每个玩家项设置信息
        for (int i = 0; i < 6; i++)
        {
            Transform itemTransform = _playerItems[i];
            Text playerInfoText = itemTransform.Find("Text").GetComponent<Text>();

            if (i < playerCount)
            {
                // 从协议中获取玩家信息
                string id = responseProto.GetString(start, ref start);
                int team = responseProto.GetInt(start, ref start);
                int win = responseProto.GetInt(start, ref start);
                int lost = responseProto.GetInt(start, ref start);
                int isRoomOwner = responseProto.GetInt(start, ref start);

                // 组装界面显示的玩家信息
                StringBuilder sb = new StringBuilder();
                sb.Append("玩家:  ").Append(id).Append("\r\n");
                sb.Append("阵营:  ").Append(team == 1 ? "红" : "蓝").Append("\r\n");
                sb.Append("战绩:  ").Append(win).Append(" - ").Append(lost).Append("\r\n");
                if (isRoomOwner == 1)
                {
                    sb.Append("[房主]");
                }
                if (id.Equals(PlayerManager.Instance.PlayerID))
                {
                    sb.Append("[我]");
                }

                // 设置玩家信息到界面组件上
                playerInfoText.text = sb.ToString();

                // 设置玩家项的背景颜色
                if (team == 1)
                {
                    itemTransform.GetComponent<Image>().color = Color.red;
                }
                else
                {
                    itemTransform.GetComponent<Image>().color = Color.blue;
                }
            }
            else
            {
                // 设置闲置玩家位
                playerInfoText.text = "[等待玩家]";
                itemTransform.GetComponent<Image>().color = Color.gray;
            }
        }
    }

    /// <summary>
    /// 接收到开始战斗指令后的回调方法
    /// </summary>
    /// <param name="proto">协议消息</param>
    private void OnStartFightCallback(Protocol proto)
    {
        // TODO: 接收到‘开始战斗’指令

        // 对玩家当前状态再次进行校验，是否可以进入战斗状态

        Debug.Log("[战斗开始]");

        // 加载战斗场景
        UIManager.Instance.ShowAlertPanel("FIGHTING!");

        // 关闭房间界面
        Close();
    }

    /// <summary>
    /// 开始战斗
    /// </summary>
    private void OnStartClick()
    {
        ProtocolBytes proto = new ProtocolBytes();
        proto.AddString(ProtocolType.START_FIGHT);

        NetworkManager.Instance.serverConn.Send(proto, OnStartCallback);

        Debug.Log("[请求开始战斗]");
    }

    /// <summary>
    /// 点击开始战斗后的回调方法
    /// </summary>
    /// <param name="proto">协议消息</param>
    private void OnStartCallback(Protocol proto)
    {
        ProtocolBytes responseProto = (ProtocolBytes) proto;

        int start = 0;

        // 协议名称，暂时无用，但需要从字节流中先读取出来
        string unused = responseProto.GetString(start, ref start);

        int result = responseProto.GetInt(start, ref start);
        if (result != 0)
        {
            UIManager.Instance.ShowAlertPanel("战斗未就绪！阵营双方各需要至少一名玩家，且只有房主可以开始战斗！");
        }
    }

    /// <summary>
    /// 离开房间
    /// </summary>
    private void OnQuitClick()
    {
        ProtocolBytes proto = new ProtocolBytes();
        proto.AddString(ProtocolType.LEAVE_ROOM);

        NetworkManager.Instance.serverConn.Send(proto, OnQuitCallback);

        Debug.Log("[请求离开房间]");
    }

    /// <summary>
    /// 离开房间后的回调方法
    /// </summary>
    /// <param name="proto">协议消息</param>
    private void OnQuitCallback(Protocol proto)
    {
        ProtocolBytes responseProto = (ProtocolBytes) proto;

        int start = 0;

        // 协议名称，暂时无用，但需要从字节流中先读取出来
        string unused = responseProto.GetString(start, ref start);

        int result = responseProto.GetInt(start, ref start);
        if (result == 0)
        {
            // 打开游戏大厅界面
            UIManager.Instance.OpenPanel<LobbyPanel>("");
            // 关闭房间界面
            Close();
        }
        else
        {
            UIManager.Instance.ShowAlertPanel("离开房间失败！");
        }
    }
}