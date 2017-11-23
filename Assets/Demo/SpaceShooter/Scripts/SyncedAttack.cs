using TrueSync;

/// <summary>
/// 攻击行为
/// </summary>
public class SyncedAttack : TrueSyncBehaviour
{
    /// <summary>
    /// 攻击力
    /// </summary>
    public int Damage = 10;

    public void OnSyncedTriggerEnter(TSCollision other)
    {
        SyncedHealth health = other.gameObject.GetComponent<SyncedHealth>();
        if (health != null)
        {
            health.TakeDamage(Damage);
        }
    }
}