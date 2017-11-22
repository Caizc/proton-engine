using Proton;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 游戏大厅界面
/// </summary>
public class LobbyPanel : UIPanel
{
    #region UI 组件

    private Text _idText;
    private Text _winText;
    private Text _lostText;
    private Transform _content;
    private GameObject _roomItem;
    private Button _newButton;
    private Button _refreshButton;
    private Button _logoutButton;

    #endregion

    #region 生命周期方法

    /// <summary>
    /// 初始化
    /// </summary>
    public override void Init(params object[] args)
    {
        base.Init(args);

        SkinPath = "LobbyPanel";
        Layer = PanelLayerEnum.Panel;
    }

    /// <summary>
    /// 显示界面
    /// </summary>
    public override void OnShowing()
    {
        base.OnShowing();

        Transform skinTransform = Skin.transform;
        Transform achievementTransform = skinTransform.Find("AchievementPanel");
        Transform roomListTransform = skinTransform.Find("RoomListPanel");

        // 获取玩家成就栏组件
        _idText = achievementTransform.Find("IDText").GetComponent<Text>();
        _winText = achievementTransform.Find("WinText").GetComponent<Text>();
        _lostText = achievementTransform.Find("LostText").GetComponent<Text>();

        // 获取房间列表栏组件
        Transform scrollRectTransform = roomListTransform.Find("ScrollRect");
        _content = scrollRectTransform.Find("Content");
        _roomItem = _content.Find("RoomItem").gameObject;

        _newButton = roomListTransform.Find("NewBtn").GetComponent<Button>();
        _refreshButton = roomListTransform.Find("RefreshBtn").GetComponent<Button>();
        _logoutButton = roomListTransform.Find("LogoutBtn").GetComponent<Button>();

        // 为按钮添加点击事件监听
        _newButton.onClick.AddListener(OnNewClick);
        _refreshButton.onClick.AddListener(OnRefreshClick);
        _logoutButton.onClick.AddListener(OnLogoutClick);

        // 向消息分发器注册事件监听回调方法
        NetworkManager.Instance.ServerConn.AddEventListener(ProtocolType.GET_ACHIEVEMENT, RecvAchievementCallback);
        NetworkManager.Instance.ServerConn.AddEventListener(ProtocolType.GET_ROOM_LIST, RecvRoomListCallback);

        // 向服务端查询“玩家成就”和“房间列表”
        ProtocolBytes proto = new ProtocolBytes();
        proto.AddString(ProtocolType.GET_ACHIEVEMENT);
        NetworkManager.Instance.ServerConn.Send(proto);

        proto = new ProtocolBytes();
        proto.AddString(ProtocolType.GET_ROOM_LIST);
        NetworkManager.Instance.ServerConn.Send(proto);
    }

    /// <summary>
    /// 关闭界面
    /// </summary>
    public override void OnClosing()
    {
        base.OnClosing();

        // 从消息分发器移除事件监听回调方法
        NetworkManager.Instance.ServerConn.RemoveEventListener(ProtocolType.GET_ACHIEVEMENT, RecvAchievementCallback);
        NetworkManager.Instance.ServerConn.RemoveEventListener(ProtocolType.GET_ROOM_LIST, RecvRoomListCallback);
    }

    #endregion

    /// <summary>
    /// 获取到玩家成就后的回调方法
    /// </summary>
    /// <param name="proto">协议消息</param>
    private void RecvAchievementCallback(Protocol proto)
    {
        ProtocolBytes responseProto = (ProtocolBytes) proto;

        int start = 0;

        // 协议名称，暂时无用，但需要从字节流中先读取出来
        string unused = responseProto.GetString(start, ref start);

        int winCount = responseProto.GetInt(start, ref start);
        int lostCount = responseProto.GetInt(start, ref start);

        // 更新 UI 信息
        _idText.text = "玩家: " + PlayerManager.Instance.CurrentPlayer.Id;
        _winText.text = "Win: " + winCount;
        _lostText.text = "Lost: " + lostCount;
    }

    /// <summary>
    /// 获取到房间列表后的回调方法
    /// </summary>
    /// <param name="proto">协议消息</param>
    private void RecvRoomListCallback(Protocol proto)
    {
        // 首先清空房间列表
        ClearRoomList();

        ProtocolBytes responseProto = (ProtocolBytes) proto;

        int start = 0;

        // 协议名称，暂时无用，但需要从字节流中先读取出来
        string unused = responseProto.GetString(start, ref start);

        // 房间数量
        int roomCount = responseProto.GetInt(start, ref start);
        for (int i = 0; i < roomCount; i++)
        {
            // 遍历获取每个房间的玩家人数和状态
            int playerCount = responseProto.GetInt(start, ref start);
            int status = responseProto.GetInt(start, ref start);

            // 构建房间列表项
            GenerateRoomItem(i, playerCount, status);
        }
    }

    /// <summary>
    /// 清空房间列表
    /// </summary>
    private void ClearRoomList()
    {
        // 销毁房间列表中的所有项目
        for (int i = 0; i < _content.childCount; i++)
        {
            if (_content.GetChild(i).name.Contains("Clone"))
            {
                Destroy(_content.GetChild(i).gameObject);
            }
        }
    }

    /// <summary>
    /// 产生一个房间列表项
    /// </summary>
    /// <param name="index">房间序号</param>
    /// <param name="count">玩家人数</param>
    /// <param name="status">房间状态，1：准备中；2：战斗中</param>
    private void GenerateRoomItem(int index, int count, int status)
    {
        // TODO: 弄清楚 sizeDelta 的详细使用方法
        _content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, (index + 1) * 110);

        // 实例化一个房间列表项
        GameObject roomItem = Instantiate(_roomItem);
        roomItem.transform.SetParent(_content);
        roomItem.SetActive(true);

        // 设置房间信息
        Transform roomItemTransform = roomItem.transform;
        Text nameText = roomItemTransform.Find("NameText").GetComponent<Text>();
        Text countText = roomItemTransform.Find("CountText").GetComponent<Text>();
        Text statusText = roomItemTransform.Find("StatusText").GetComponent<Text>();

        nameText.text = "房间号: " + (index + 1);
        countText.text = "人数: " + count;

        if (status == 1)
        {
            statusText.color = Color.black;
            statusText.text = "状态: 准备中";
        }
        else
        {
            statusText.color = Color.red;
            statusText.text = "状态: 战斗中";
        }

        // 为“加入”按钮添加点击事件
        Button joinButton = roomItemTransform.Find("JoinBtn").GetComponent<Button>();
        // 将房间序号作为按钮点击事件的参数传入
        joinButton.onClick.AddListener(delegate() { OnJoinButtonClick(index); });
    }

    /// <summary>
    /// 加入房间
    /// </summary>
    /// <param name="index">房间序号</param>
    private void OnJoinButtonClick(int index)
    {
        ProtocolBytes proto = new ProtocolBytes();
        proto.AddString(ProtocolType.ENTER_ROOM);
        proto.AddInt(index);

        NetworkManager.Instance.ServerConn.Send(proto, RecvEnterRoomCallback);

        Debug.Log("[请求加入房间] 房间号: " + (index + 1));
    }

    /// <summary>
    /// 接收到进入房间消息后的回调方法
    /// </summary>
    /// <param name="proto">协议消息</param>
    private void RecvEnterRoomCallback(Protocol proto)
    {
        // TODO: GO ON

        ProtocolBytes responseProto = (ProtocolBytes) proto;

        int start = 0;

        // 协议名称，暂时无用，但需要从字节流中先读取出来
        string unused = responseProto.GetString(start, ref start);

        int result = responseProto.GetInt(start, ref start);
        if (result == 0)
        {
            // UIManager.Instance.ShowAlertPanel("成功进入房间");

            // 打开房间界面
            UIManager.Instance.OpenPanel<RoomPanel>("");

            // 关闭游戏大厅界面
            Close();
        }
        else
        {
            UIManager.Instance.ShowAlertPanel("进入房间失败！");
        }
    }

    /// <summary>
    /// 创建房间
    /// </summary>
    private void OnNewClick()
    {
        ProtocolBytes proto = new ProtocolBytes();
        proto.AddString(ProtocolType.CREATE_ROOM);

        NetworkManager.Instance.ServerConn.Send(proto, RecvCreateRoomCallback);

        Debug.Log("[请求创建房间]");
    }

    /// <summary>
    /// 接收到创建房间消息后的回调方法
    /// </summary>
    /// <param name="proto">协议消息</param>
    private void RecvCreateRoomCallback(Protocol proto)
    {
        ProtocolBytes responseProto = (ProtocolBytes) proto;

        int start = 0;

        // 协议名称，暂时无用，但需要从字节流中先读取出来
        string unused = responseProto.GetString(start, ref start);

        int result = responseProto.GetInt(start, ref start);
        if (result == 0)
        {
            // 打开房间界面
            UIManager.Instance.OpenPanel<RoomPanel>("");

            // 关闭游戏大厅界面
            Close();
        }
        else
        {
            UIManager.Instance.ShowAlertPanel("创建房间失败！");
        }
    }

    /// <summary>
    /// 刷新房间列表
    /// </summary>
    private void OnRefreshClick()
    {
        ProtocolBytes proto = new ProtocolBytes();
        proto.AddString(ProtocolType.GET_ROOM_LIST);

        NetworkManager.Instance.ServerConn.Send(proto);

        Debug.Log("[请求刷新房间列表]");
    }

    /// <summary>
    /// 用户登出
    /// </summary>
    private void OnLogoutClick()
    {
        ProtocolBytes proto = new ProtocolBytes();
        proto.AddString(ProtocolType.LOGOUT);

        NetworkManager.Instance.ServerConn.Send(proto, RecvLogoutCallback);

        Debug.Log("[请求用户登出]");
    }

    /// <summary>
    /// 接收到用户登出消息后的回调方法
    /// </summary>
    /// <param name="proto">协议消息</param>
    private void RecvLogoutCallback(Protocol proto)
    {
        UIManager.Instance.ShowAlertPanel("登出成功！");

        // 关闭网络
        NetworkManager.Instance.Shutdown();

        // 重新加载游戏
        GameObject gameControllerObj = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerObj == null)
        {
            Debug.LogError("[空引用异常] 场景中找不到 GameController 对象！");
        }
        else
        {
            GameController gameControllerScript = gameControllerObj.GetComponent<GameController>();
            gameControllerScript.Reload();
        }
    }
}