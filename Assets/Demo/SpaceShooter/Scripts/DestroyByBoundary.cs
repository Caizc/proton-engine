using TrueSync;
using UnityEngine;

public class DestroyByBoundary : MonoBehaviour
{
    public void OnSyncedTriggerExit(TSCollision other)
    {
        TrueSyncManager.Destroy(other.gameObject);
    }
}