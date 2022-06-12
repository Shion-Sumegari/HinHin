using System.Collections;
using UnityEngine;

public class CharacterCombat : MonoBehaviour
{
    #region Variables
    public AnimationEventManager characterEventManager;
    public CharacterAnimatorController characterAnimatorController;

    private bool m_combatReady;
    public bool isReadyToCombat { get { return m_combatReady; } set { m_combatReady = value; ChangeCombatStatus(m_combatReady); } }

    public bool InCombat { get; protected set; }

    public float attackCountdown = 0f;
    protected const float combatCooldown = 1f;
    protected float lastAttackTime;

    protected float combatDelay = .6f;

    protected int m_CurrentHealth;
    public int currentHealth { get { return m_CurrentHealth; } set { m_CurrentHealth = value; if (OnHealthChanged != null) OnHealthChanged(maxHealth, currentHealth, 0); } }
    public int maxHealth;
    public int currentDamage;
    public int baseHealth, baseDamage;

    public Transform HitTrans;

    [SerializeField] float attackSpeed = 1f;

    [SerializeField] public CharacterCombat m_Target;

    public bool isDead;

    public bool isDefense;

    public event System.Action<CharacterCombat> OnAttackTarget;

    protected bool m_isAutoAttack;
    public bool isAutoAttack { get { return m_isAutoAttack;} set { m_isAutoAttack = value; ChangeAutoAttackStatus(); } }

    public System.Action<int, int, int> OnHealthChanged;

    public float flyingDistance = 100f;

    public float flyingSpeed = 80f;

    public float autoAttackDelay = 1f;

    [SerializeField] Transform rayPoint;

    protected Coroutine autoAttackCoroutine;

    private int comboCount = 0;

    bool isStun;

    Coroutine flyCoroutine;
    #endregion

    // Start is called before the first frame update
    public virtual void Start()
    {
        characterEventManager.OnRightHandHitTarget += OnRightEndHitTarget;
        characterEventManager.OnFinishHitTargetReadyToFly += OnFinishHitTargetReadyToFly;
        characterEventManager.OnLeftHandHitTarget += OnLeftEndHitTarget;
        characterEventManager.OnFinishPosReach += OnFinishPosReach;
        characterEventManager.OnRightHandAttackEvent += OnRightHandAttack;
        characterEventManager.OnLeftHandAttackEvent += OnLeftHandAttack;
        characterAnimatorController = GetComponent<CharacterAnimatorController>();
        ResetStats();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        CombatCowndownCaculating();
    }

    public void ResetStats()
    {
        if (!gameObject.CompareTag("Player"))
        {
            characterAnimatorController.StartGame(true);
        }
        isDead = false;
        m_CurrentHealth = maxHealth;
    }

    void CombatCowndownCaculating()
    {
        attackCountdown -= Time.deltaTime;

        if (Time.time - lastAttackTime > combatCooldown)
        {
            //Debug.Log("Not in combat");
            InCombat = false;
        }
    }

    protected virtual void OnFinishHitTargetReadyToFly()
    {
        
    }

    public void OnRightHandAttack()
    {
        if (m_Target != null && m_Target.isDefense)
        {
            m_Target.Dodge(1.0f);
        }
    }

    public void OnLeftHandAttack()
    {
        if (m_Target != null && m_Target.isDefense)
        {
            m_Target.Dodge(0f);
        }
    }

    public virtual void OnRightEndHitTarget()
    {
        if (isDead || (m_Target != null && m_Target.isDefense)) return;
        OnHitTarget();
    }

    private void OnLeftEndHitTarget()
    {
        if (isDead || (m_Target != null && m_Target.isDefense)) return;
            OnHitTarget();
    }

    public void Dodge(float side)
    {
        characterAnimatorController.animator.SetFloat("RightLeft", side);
        characterAnimatorController.animator.SetTrigger("Dodge");
    }

    public virtual void Die()
    {
        isAutoAttack = false;

        LogUtils.Log(gameObject.name + " Died");
        if(m_Target != null)
        {
            m_Target.characterAnimatorController.animator.SetTrigger("Victory");
        } else if(!gameObject.CompareTag("Player"))
        {
            PlayerController.Instance.GetComponent<PlayerAnimatorController>().animator.SetTrigger("Victory");
        }
        // Fly away
        if ((transform.CompareTag("Boss") || transform.CompareTag("King")) && flyCoroutine == null)
        {
            flyCoroutine = StartCoroutine(MoveObject(transform));
            return;
        } else if (transform.CompareTag("CombatChest"))
        {
            GameplayUIController.Instance.GameOver(true, 1.5f);
        }

        characterAnimatorController.PlayDieAnimation();
    }

    public virtual void ChangeCombatStatus(bool isReady)
    {
        characterAnimatorController.StartGame(isReady);
        var statUI = GetComponent<StatsUI>();
        if (statUI != null && LevelManager.Instance.levelEndType == LevelEnd.BOSS_FIGHT)
        {
            LogUtils.Log("StatsUI: " + transform.name + " / " + isReady);
            statUI.isEnable = isReady;
        } 
    }

    
    public void ChangeAutoAttackStatus()
    {
        if (autoAttackCoroutine != null) StopCoroutine(autoAttackCoroutine);
        if (m_isAutoAttack)
        {
            StartCoroutine(AutoAttack());
        } 
    }

    IEnumerator AutoAttack()
    {
        yield return new WaitForSeconds(autoAttackDelay);

        while (m_isAutoAttack && m_Target != null)
        {
            Attack(m_Target);
            yield return new WaitForEndOfFrame();
        }
    }

    protected void OnHitTarget()
    {
        
        CameraController.Instance.Shake();
        if (m_Target != null)
        {
            m_Target.GotHit(currentDamage);
        }
    }

    public void GotHit(int damage)
    {
        if (isDead) return;

        ObjectPooler.Instance.SpawnFormPool("FX_Hit", null, Vector3.zero, HitTrans.position, Quaternion.identity);

        if (isDefense) damage = 1;

        m_CurrentHealth -= damage;

        if (m_CurrentHealth <= 0)
        {
            isDead = true;
            m_CurrentHealth = 0;
            Die();
        } else
        {
            characterAnimatorController.GotHit();
            isDead = false;
        }

        if (OnHealthChanged != null)
            OnHealthChanged(maxHealth, m_CurrentHealth, -damage);
    }

    protected virtual void OnFinishPosReach()
    {

    }

    public void Attack(CharacterCombat targetCombat)
    {
        m_Target = targetCombat;
        if (attackCountdown <= 0f)
        {
            if (!targetCombat.isDead)
            {
                attackCountdown = 3f / attackSpeed;
                if(transform.CompareTag("Player") && currentDamage >= targetCombat.m_CurrentHealth)
                {
                    isReadyToCombat = false;
                    targetCombat.isAutoAttack = false;
                    characterAnimatorController.FinishPunch();
                    CameraController.Instance.MoveToFinishPunchScene(CameraController.SceneType.FINISHPUNCH);
                    LogUtils.Log(gameObject.name + " Attack dame Finish: " + currentDamage + " / " + targetCombat.m_CurrentHealth);
                } else
                {
                    LogUtils.Log(gameObject.name + " Attack dame: " + currentDamage + " / " + targetCombat.m_CurrentHealth);
                    if (transform.CompareTag("King") && !isStun)
                    {
                        comboCount++;
                        characterAnimatorController.Combo();
                        if(comboCount > 1)
                        {
                            comboCount = 0;
                            if(!isDead) StartCoroutine(Stun());
                        }
                    }
                    if(!transform.CompareTag("King"))
                    {
                        characterAnimatorController.SinglePunch();
                    }
                    
                }

                if (targetCombat.gameObject.CompareTag("PunchingMachine") && GameplayUIController.Instance.isPowerBarRolling)
                {
                    GameplayUIController.Instance.isPowerBarRolling = false;
                }
                
                if (OnAttackTarget != null)
                    OnAttackTarget(m_Target);
                InCombat = true;
                lastAttackTime = Time.time;
            }
        }
    }

    IEnumerator Stun()
    {
        float stunTime = 5f;
        var timeElapsed = 0f;
        isStun = true;
        characterAnimatorController.animator.SetBool("isStun", isStun);
        
        while (timeElapsed < stunTime)
        {
            
            timeElapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        isStun = false;
        characterAnimatorController.animator.SetBool("isStun", isStun);
        if(GetComponentInChildren<AnimationEventManager>() != null)
        {
           if(GetComponentInChildren<AnimationEventManager>().stunFX != null)
            {
                GetComponentInChildren<AnimationEventManager>().stunFX.SetActive(false);
                GetComponentInChildren<AnimationEventManager>().firstStun = 0;
            }
        }
    }

    IEnumerator MoveObject(Transform target)
    {
        yield return new WaitUntil(() => { return PlayerController.Instance.playerCombat.isFinishHim; });
        
        flyingDistance *= PlayerController.Instance.GetComponent<PlayerAnimatorController>().currentScale * (GameManager.Instance.userData._finishPunchPower / GameConfigs.MIN_FINISH_POWER);
        Animator characterAniamtor = target.GetComponent<CharacterAnimatorController>().animator;
        characterAniamtor.SetTrigger("ZBossDieLastHitFly");
        yield return new WaitForSeconds(0.05f);

        var targetZ = target.position.z + flyingDistance;
        RaycastHit hit;
        float point = 1.0f;
        while (targetZ - target.position.z > 1f)
        {
            if( rayPoint != null && Physics.Raycast(rayPoint.position, -Vector3.up, out hit, Mathf.Infinity))
            {
                if (hit.transform.gameObject.CompareTag("PointPlane"))
                {
                    var pointController = hit.transform.gameObject.GetComponent<PointPlaneController>();
                    if (!pointController.isReached)
                    {
                        pointController.isReached = true;
                        point = pointController.pointX;
                    }
                }
            }

            float newSpeed = flyingSpeed;

            if (targetZ - target.position.z < (15f)) { newSpeed = flyingSpeed / 5.5f; }
            else
            if (targetZ - target.position.z < (42f))
            {
                newSpeed = flyingSpeed * (targetZ - target.position.z) / flyingDistance;
                if (newSpeed < flyingSpeed / 4) newSpeed = flyingSpeed / 4;
            }
            
            target.position += new Vector3(0, 0, newSpeed * Time.deltaTime);
            if (targetZ - target.position.z < (15f))
            {
                characterAniamtor.SetTrigger("ZBossSlideEnd");
            }
            else if(targetZ - target.position.z < (32f))
            {
                characterAniamtor.SetTrigger("ZBossDieDrop");
            } 
            yield return new WaitForEndOfFrame();
        }

        LogUtils.Log("Extra Point: " + point);

        ObjectPooler.Instance.SpawnFormPool("FX_Confetti", null, Vector3.zero, rayPoint.position, Quaternion.Euler(-90, 0, 0));
        ObjectPooler.Instance.SpawnFormPool("FX_Confetti", null, Vector3.zero, rayPoint.position + new Vector3(-10, 0,0), Quaternion.Euler(-90,0,0));
        ObjectPooler.Instance.SpawnFormPool("FX_Confetti", null, Vector3.zero, rayPoint.position + new Vector3(+10, 0, 0), Quaternion.Euler(-90, 0, 0));

        GameplayUIController.Instance.GameOver(true, 1.2f);
    }

}
