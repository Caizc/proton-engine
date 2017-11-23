using TrueSync;
using UnityEngine;

/// <summary>
/// 匀速移动
/// </summary>
public class SyncedMover : TrueSyncBehaviour
{
    [SerializeField] private FP speed = 20;

    private TSRigidBody _rigidbody;

    public override void OnSyncedStart()
    {
        _rigidbody = GetComponent<TSRigidBody>();

        if (_rigidbody != null)
        {
            _rigidbody.velocity = tsTransform.forward * speed;
        }
        else
        {
            Debug.LogError("Missing TSRigidBody on current GameObject!");
        }
    }
}