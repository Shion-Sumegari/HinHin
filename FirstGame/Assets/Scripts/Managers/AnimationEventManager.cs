using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventManager : MonoBehaviour
{
    public event System.Action OnRightHandHitTarget;
    public event System.Action OnLeftHandHitTarget;
    public event System.Action OnFinishPosReach;
    public event System.Action OnFinishHitTargetReadyToFly;
    public event System.Action OnLeftHandAttackEvent;
    public event System.Action OnRightHandAttackEvent;

    public int firstStun = 0;
    public GameObject stunFX;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStun()
    {
        if(firstStun == 0)
        {
            if (GetComponentInParent<CharacterCombat>() != null)
            {
                var hitTrans = GetComponentInParent<CharacterCombat>().HitTrans;
                stunFX = ObjectPooler.Instance.SpawnFormPool("FX_Stun", hitTrans, Vector3.zero, hitTrans.position, Quaternion.identity);
                firstStun++;
            }
        } 
    }

    public void OnRightHandAttack()
    {
        if(OnRightHandAttackEvent != null)
        {
            OnRightHandAttackEvent();
        }
    }

    public void OnLeftHandAttack()
    {
        if (OnLeftHandAttackEvent != null)
        {
            OnLeftHandAttackEvent();
        }

    }

    void OnRightEndHitTarget()
    {
        if(OnRightHandHitTarget != null)
        {
            LogUtils.Log("Hit");
            OnRightHandHitTarget();
        }
    }

    void OnLeftEndHitTarget()
    {
        if (OnLeftHandHitTarget != null)
        {
            LogUtils.Log("Hit");
            OnLeftHandHitTarget();
        }
    }

    void FinishReadyToPunch()
    {
        if(OnFinishPosReach != null)
        {
            OnFinishPosReach();
        }
    }

    public void BodyDrop()
    {
        CameraController.Instance.Shake();
    }

    public void PointHit()
    {
        PunchingMachineController punchingMachineController = GetComponent<PunchingMachineController>();
        if(punchingMachineController != null)
        {
            float powerHit = GameManager.Instance.userData._finishPunchPower * PlayerController.Instance.GetComponent<PlayerAnimatorController>().currentScale * GameplayUIController.Instance.currentPowerbarValue;
            GetComponent<PunchingMachineController>().PunchHit((int)powerHit);
        }
    }

    public void OnFinishHitTarget()
    {
        if(OnFinishHitTargetReadyToFly != null)
        {
            OnFinishHitTargetReadyToFly();
        }
    }

    public void OnRunningHitObsacle()
    {
        
    }

}
