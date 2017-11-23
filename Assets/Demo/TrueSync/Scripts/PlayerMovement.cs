using System.Collections;
using System.Collections.Generic;
using TrueSync;
using UnityEngine;

public class PlayerMovement : TrueSyncBehaviour
{
    [SerializeField] private FP accellRate;

    [SerializeField] private FP steerRate;

    [AddTracking] public int deaths = 0;

    public override void OnSyncedInput()
    {
        FP accell = Input.GetAxis("Vertical");
        FP steer = Input.GetAxis("Horizontal");

        TrueSyncInput.SetFP(0, accell);
        TrueSyncInput.SetFP(1, steer);
    }

    public override void OnSyncedUpdate()
    {
        FP accell = TrueSyncInput.GetFP(0);
        FP steer = TrueSyncInput.GetFP(1);

        accell *= accellRate * TrueSyncManager.DeltaTime;
        steer *= steerRate * TrueSyncManager.DeltaTime;

        tsTransform.Translate(0, 0, accell, Space.Self);
        tsTransform.Rotate(0, steer, 0);
    }

    public override void OnSyncedStart()
    {
        // 设置非当前玩家为灰色
        if (owner.name != PlayerManager.Instance.CurrentPlayer.Id)
        {
            tsTransform.GetComponentInParent<MeshRenderer>().materials[0].color = Color.gray;
        }

        tsTransform.position = new TSVector(TSRandom.Range(-5, 5), 0, TSRandom.Range(-5, 5));
    }

    public void Respawn()
    {
        tsTransform.position = new TSVector(TSRandom.Range(-5, 5), 0, TSRandom.Range(-5, 5));
        deaths++;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 100 + 30 * owner.Id, 300, 30), "player: " + owner.Id + ", deaths: " + deaths);
    }
}