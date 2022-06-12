using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : CharacterCombat
{
    PlayerController playerController;
    [SerializeField] public GameObject rightEnd, leftEnd;
    bool isCombatBegin;
    public Transform staticTarget;
    Touch touch;
    bool isOnlyTouch = false;
    Vector3 test;

    PlayerAnimatorController playerAnimatorController;

    public float radius = 5.0F;
    public float power = 10.0F;

    // To prevent double attack
    public bool isFinishHim;

    Vector2 startTouchPos;

    public override void Start()
    {
        base.Start();
        playerController = GetComponent<PlayerController>();
        playerAnimatorController = (PlayerAnimatorController)characterAnimatorController;
    }

    public override void Update()
    {
        base.Update();

        if (isCombatBegin && m_Target != null)
        {
            GameUtils.TouchInput(this, "TapToAttack", "HoldToDefense", "ReleaseToIdle");
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            HoldToDefense();
        }
    }

    void TapToAttack()
    {
        GameManager.Instance.uIController.ToggleGuideUI(-1);
        if (isReadyToCombat) Attack(m_Target);
        isOnlyTouch = false;
    }

    void HoldToDefense()
    {
        if(m_Target != null && m_Target.gameObject.CompareTag("King"))
        {
            isDefense = true;
            playerAnimatorController.animator.SetBool("IsDef", isDefense);
        }
    }

    void ReleaseToIdle()
    {
        if (isDefense) 
        {
            isDefense = false;
            playerAnimatorController.animator.SetBool("IsDef", isDefense);
        }
    }

    public void LateUpdate()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(test, radius);
        Gizmos.color = Color.blue;
    }

    override public void OnRightEndHitTarget()
    {
        base.OnRightEndHitTarget();
        if (staticTarget != null)
        {
            if (staticTarget.CompareTag("Brick"))
            {
                LogUtils.Log("Fired");
                
                if (playerController.isEnoughEnergy)
                {
                    BreakWall(rightEnd.transform.position);
                    
                } else
                {
                    staticTarget.GetComponent<ObjectShake>().Shake();
                    Die();
                }
            }
        }
    }

   protected override void OnFinishHitTargetReadyToFly()
    {
        base.OnFinishHitTargetReadyToFly();
        LogUtils.Log("Finish");
        isFinishHim = true;
        OnHitTarget();
    }

    protected override void OnFinishPosReach()
    {
        base.OnFinishPosReach();
    }

    void DisableStaticTarget()
    {
        staticTarget.transform.gameObject.SetActive(false);
        staticTarget = null;
    }

    void BreakWall(Vector3 breakPoint)
    {
        var ratio = GetComponent<PlayerAnimatorController>().currentScale;

        // Time.timeScale = 0;
        var exPoint = breakPoint;
        RaycastHit wallPoint;
        if (Physics.Raycast(breakPoint, new Vector3(0,0,10), out wallPoint, Mathf.Infinity)){
            LogUtils.Log("WallHitRay " + wallPoint.transform.tag);
            if (wallPoint.transform.CompareTag("Brick"))
            {
                exPoint = wallPoint.point + new Vector3(0, -1, -1);
                ObjectPooler.Instance.SpawnFormPool("FX_WallHit",null, Vector3.zero, exPoint, Quaternion.identity);
                playerAnimatorController.currentScale = playerAnimatorController.GetCurrentScaleBone().gameObject.transform.localScale.x + staticTarget.GetComponent<Item>().getValue() * Item.valueForPoint;
            }
        }

        Collider[] colliders = Physics.OverlapSphere(exPoint, radius);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(power * ratio, exPoint, radius * ratio, 1.0F);
            }

        }

        Invoke("DisableStaticTarget", 2f);
    }

    override public void Die()
    {
        base.Die();
        playerController.StopMoving();
        playerController.isDead = true;
        GameplayUIController.Instance.GameOver(false, 1f);
        CameraController.Instance.MoveToDeadScene(CameraController.SceneType.DEAD);
    }

    public override void ChangeCombatStatus(bool value)
    {
        base.ChangeCombatStatus(value);

        if (value)
        {
            CameraController.Instance.stress = 0.25f;

            switch (LevelManager.Instance.levelEndType)
            {
                case LevelEnd.BOSS_FIGHT:

                    var botTarget = LevelManager.Instance.finalChallenge.GetComponent<CharacterCombat>();
                    m_Target = botTarget;
                    botTarget.m_Target = this;
                    botTarget.isAutoAttack = true;

                    break;
                case LevelEnd.CHEST:
                    var chest = LevelManager.Instance.finalChallenge.GetComponent<CharacterCombat>();
                    m_Target = chest;
                    break;

                case LevelEnd.PUNCHING_MACHINE:
                    var pc = LevelManager.Instance.finalChallenge.GetComponent<CharacterCombat>();
                    m_Target = pc;
                    GameplayUIController.Instance.ShowPowerBar();
                    break;

                default:
                    break;
            }
        } else
        {
            CameraController.Instance.stress = 0.45f;
        }
        isCombatBegin = value;
    }

}
