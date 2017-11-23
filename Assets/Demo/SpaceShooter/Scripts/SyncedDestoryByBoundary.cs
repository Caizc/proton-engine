using TrueSync;

/// <summary>
/// 销毁离开游戏边界的对象
/// </summary>
public class SyncedDestoryByBoundary : TrueSyncBehaviour
{
    public void OnSyncedTriggerExit(TSCollision other)
    {
        Destroy(other.gameObject);
    }
}
