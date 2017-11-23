using System;
using Proton;
using TrueSync;
using UnityEngine;
using UnityEngine.UI;

[System.SerializableAttribute]
public class BoundaryFP
{
    public FP xMin, xMax, zMin, zMax;
}

/// <summary>
/// 玩家角色控制器
/// </summary>
public class SyncedPlayerController : TrueSyncBehaviour
{
    // 移动速度
    [SerializeField] private FP speed;

    // 转向速度
    [SerializeField] private FP rotateSpeed;

    // 移动范围界限
    [SerializeField] private BoundaryFP boundary;

    // 机身倾斜系数
    [SerializeField] private FP tilt;

    // 子弹 Prefab 和枪口位置
    [SerializeField] private GameObject playerShot;

    [SerializeField] private GameObject enemyShot;

    private GameObject _currentShot;

    [SerializeField] private Transform shotSpawn;

    // 开火间隔
    [SerializeField] private FP fireDelta = 0.25;

    // 引擎特效粒子系统
    [SerializeField] private ParticleSystem particleSystem1;

    [SerializeField] private ParticleSystem particleSystem2;

    private SyncedInputManager _syncedInputManager;
    private SyncedHealth _syncedHealth;

    private AudioSource _audioSource;

    private FP _myTime = 0;
    private FP _nextFire = 0.25;

    private FP _movX;
    private FP _movY;
    private FP _rotX;
    private FP _rotY;

    // 死亡次数
    [AddTracking] private int _death;

    private GUIStyle _style = new GUIStyle();

    public override void OnSyncedStart()
    {
        tsTransform.position = new TSVector(TSRandom.Range(-5, 5), 0, TSRandom.Range(-5, 5));

        GameObject inputManagerObject = GameObject.FindWithTag("InputManager");
        if (inputManagerObject == null)
        {
            Debug.LogError("场景中缺失 SyncedInputManager 对象！");
            return;
        }

        _syncedInputManager = inputManagerObject.GetComponent<SyncedInputManager>();
        if (_syncedInputManager == null)
        {
            Debug.LogError("场景中缺失 SyncedInputManager 组件！");
        }

        if (owner.id.Equals(localOwner.id))
        {
            _currentShot = playerShot;
        }
        else
        {
            _currentShot = enemyShot;
        }

        _syncedHealth = GetComponent<SyncedHealth>();

        // 如果该对象为本地玩家对象，则将其生命值显示到 UI 控件上
        if (owner.id.Equals(localOwner.id))
        {
            GameObject uiCameraObject = GameObject.Find("UICamera");
            if (uiCameraObject != null)
            {
                UIManager uiManager = uiCameraObject.GetComponent<UIManager>();
                if (uiManager != null)
                {
                    uiManager.PlayerHealth = _syncedHealth;
                }
                else
                {
                    Debug.LogError("Can not get the reference of 'HealthText'!");
                }
            }
            else
            {
                Debug.LogError("GameObject 'UICamera' is missing in the current scene!");
            }
        }

        _audioSource = GetComponent<AudioSource>();

        _death = 0;

        _style.normal.textColor = Color.white;
        _style.fontSize = 24;
    }

    public override void OnSyncedInput()
    {
        if (RealSyncManager.IsBattleStart)
        {
            // 将飞船的位置和转向信息保存到 SyncedData 中以发送往 Server 同步到所有 Client
            TrueSyncInput.SetFP(0, _syncedInputManager.DeltaMovement.x);
            TrueSyncInput.SetFP(1, _syncedInputManager.DeltaMovement.y);
            TrueSyncInput.SetFP(2, _syncedInputManager.DeltaRotation.x);
            TrueSyncInput.SetFP(3, _syncedInputManager.DeltaRotation.y);
        }
    }

    public override void OnSyncedUpdate()
    {
        _movX = TrueSyncInput.GetFP(0);
        _movY = TrueSyncInput.GetFP(1);
        _rotX = TrueSyncInput.GetFP(2);
        _rotY = TrueSyncInput.GetFP(3);

        // 移动
        Move();

        // 开火
        Fire();
    }

    /// <summary>
    /// 移动
    /// </summary>
    private void Move()
    {
        if (!(_movX == 0 && _movY == 0))
        {
            bool gotIt = true;
        }

        tsRigidBody.velocity = new TSVector(_movX * speed, 0, _movY * speed);

        // 限制角色移动范围
        tsRigidBody.position = new TSVector(
            TSMath.Clamp(tsRigidBody.position.x, boundary.xMin, boundary.xMax),
            0,
            TSMath.Clamp(tsRigidBody.position.z, boundary.zMin, boundary.zMax)
        );

        // 左右平移时稍微倾斜一下机身（绕 z 轴）
        tsRigidBody.rotation = TSQuaternion.Euler(0, 0, tsRigidBody.velocity.x * -tilt);

        // 旋转方向
        if (_rotX == 0 && _rotY == 0)
        {
            tsTransform.rotation = TSQuaternion.Lerp(tsTransform.rotation, TSQuaternion.identity,
                TrueSyncManager.DeltaTime * rotateSpeed);
        }
        else
        {
            TSVector joystickKnobPos = new TSVector(_rotX, 0, _rotY);
            TSQuaternion targetRot = TSQuaternion.LookRotation(joystickKnobPos);
            tsTransform.rotation =
                TSQuaternion.Lerp(tsTransform.rotation, targetRot, TrueSyncManager.DeltaTime * rotateSpeed);
        }

        // 旋转粒子系统
        float r = (float) tsTransform.rotation.eulerAngles.y * Mathf.Deg2Rad;
        if (null != particleSystem1)
        {
            particleSystem1.startRotation = r;
        }
        if (null != particleSystem2)
        {
            particleSystem2.startRotation = r;
        }
    }

    /// <summary>
    /// 开火
    /// </summary>
    private void Fire()
    {
        _myTime += TrueSyncManager.DeltaTime;

        bool canFire = (_rotX > 0.8 || _rotX < -0.8 || _rotY > 0.8 ||
                        _rotY < -0.8) && _myTime > _nextFire;

        if (canFire)
        {
            _nextFire = _myTime + fireDelta;

            // 生成子弹
            TrueSyncManager.SyncedInstantiate(_currentShot,
                new TSVector(shotSpawn.position.x, shotSpawn.position.y, shotSpawn.position.z),
                tsTransform.rotation);

            _nextFire = _nextFire - _myTime;
            _myTime = 0;

            _audioSource.Play();
        }
    }

    /// <summary>
    /// 重生
    /// </summary>
    public void Respawn()
    {
        // 死亡次数加一
        _death++;
        // 重置生命值
        _syncedHealth.Reset();
        // 设置随机出生位置
        tsTransform.position = new TSVector(TSRandom.Range(-5, 5), 0, TSRandom.Range(-5, 5));
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 100 + 30 * owner.Id, 300, 30), "player: " + owner.name + ", deaths: " + _death, _style);
    }
}