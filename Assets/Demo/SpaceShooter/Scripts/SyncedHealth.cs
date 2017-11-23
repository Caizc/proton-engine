using System;
using TrueSync;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 生命值/耐久度
/// </summary>
public class SyncedHealth : TrueSyncBehaviour
{
    [SerializeField] private GameObject explosion;
    [SerializeField] private GameObject playerExplosion;

    /// <summary>
    /// 当前角色生命值/物品耐久度
    /// </summary>
    [AddTracking] public int Health = 100;

    /// <summary>
    /// 是否已死亡/损毁
    /// </summary>
    [AddTracking] public bool IsDead;

    // 临时保存最大生命值/耐久度
    private int _maxHealth;

    public override void OnSyncedStart()
    {
        IsDead = false;
        _maxHealth = Health;
    }

    public override void OnSyncedUpdate()
    {
        if (IsDead)
        {
            // 如果已死亡/损毁，同步销毁本对象
            TrueSyncManager.SyncedDestroy(this.gameObject);
        }
    }

    /// <summary>
    /// 受到伤害扣生命值/物品耐久度降低
    /// </summary>
    /// <param name="damage">伤害值</param>
    public void TakeDamage(int damage)
    {
        Health -= damage;

        PlayFX();

        if (Health <= 0)
        {
            Health = 0;
            Death();
        }
    }

    /// <summary>
    /// 重置
    /// </summary>
    public void Reset()
    {
        Health = _maxHealth;
    }

    /// <summary>
    /// 播放视觉特效和声音特效
    /// </summary>
    private void PlayFX()
    {
        if (null != explosion)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }
    }

    /// <summary>
    /// 死亡/损毁
    /// </summary>
    private void Death()
    {
        if (this.gameObject.tag == "Player")
        {
            // 如果该对象是玩家角色，则播放玩家角色爆炸特效，并进行重生处理
            if (null != playerExplosion)
            {
                Instantiate(playerExplosion, transform.position, transform.rotation);
            }

            SyncedPlayerController playerController = GetComponent<SyncedPlayerController>();
            if (playerController != null)
            {
                playerController.Respawn();
            }
        }
        else
        {
            // 否则，标记本对象已死亡/损毁
            IsDead = true;
        }
    }
}