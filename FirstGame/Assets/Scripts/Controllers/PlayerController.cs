using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Touch touch;

    bool m_IsRunning;
    public bool isRunning { get { return m_IsRunning; } set { m_IsRunning = value; if (animator != null) animator.SetBool("isRunning", m_IsRunning); } } 

    [SerializeField] float currentMovementSpeed, rotationTime, speedModifier, rotationSpeed;

    [SerializeField] bool isFinishRace;

    [SerializeField] Vector3 finishPos, maxX, minX;

    [SerializeField] Animator animator;

    [SerializeField] [Range(0, 40f)] float angleOffset = 25f;

    [SerializeField] Vector3 leftRotationPoint, rightRotaionPoint;

    [SerializeField] float startMovementSpeed;

    GameObject currentScaleBone;

    private Coroutine rotateCoroutine;

    PlayerAnimatorController playerAnimatorController;

    public PlayerCombat playerCombat;

    public bool isEnoughEnergy;

    Coroutine moveCoroutine;

    public bool isDead;

    public float timeToCombat;

    public float t;

    public bool isCombatEnd = true;

    public bool isStarted;

    Vector2 oldTouchPos;

    #region SingleTon

    public static PlayerController Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void LateUpdate()
    {
        if (!isDead && (Input.touchCount > 0 || Input.GetKeyDown(KeyCode.Space)) && !m_IsRunning && !playerCombat.isReadyToCombat && !isStarted)
        {
            touch = Input.GetTouch(0);
            int id = touch.fingerId;
            if (!GameManager.IsPointerOverUIElement())
            {
                LogUtils.Log("Touch");
                // ui not touched
                isStarted = true;
                // Move player by touching
                GameplayUIController.Instance.GameStart();
                if (!m_IsRunning) MovePlayer();
                GameManager.Instance.uIController.ToggleGuideUI(-1);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        float newDis = 0f;
        
        if (m_IsRunning)
        {
            if (!isDead && !isFinishRace)
            {
                if (Input.touchCount > 0)
                {
                    touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                    {
                        newDis = touch.deltaPosition.x;
                        float x = transform.position.x;

                        // kiểm tra điều kiện di chuyển của user
                        if ((x <= maxX.x) && (x >= minX.x) || (x > maxX.x && newDis < 0) || (x < minX.x && newDis > 0))
                        {
                            float newX = transform.position.x + newDis * speedModifier;

                            // Ngăn player bị drag ra khỏi ground khi di chuyển
                            if (newX > maxX.x)
                            {
                                newX = maxX.x;
                            }
                            else if (newX < minX.x)
                            {
                                newX = minX.x;
                            }
                            transform.position = new Vector3(newX, transform.position.y, transform.position.z); // Di chuyển player

                            // Quay player về rotation chuẩn khi di chuyển
                        }
                        //Debug.Log("Move + " + touch.deltaPosition.x);
                    }
                }
            }
            RotateByMove(newDis);
        }
    }

    public void BeginLevel()
    {
        GameManager.Instance.uIController.ToggleGuideUI(0);
        GameManager.Instance.effectsManager.ChangeTimeScale(1f, 0f, 0f);

        transform.position = LevelManager.Instance.startPoint;

        playerAnimatorController = GetComponent<PlayerAnimatorController>();

        playerAnimatorController.currentScale = LevelManager.Instance.levelController.startScale;
        
        playerCombat = GetComponent<PlayerCombat>();

        if (isDead)
        {
            LogUtils.Log("Restart Trigger");
            playerAnimatorController.animator.SetLayerWeight(2, 0f);
            playerAnimatorController.animator.SetLayerWeight(5, 0f);
        }

        transform.eulerAngles += 180f * Vector3.up;
        playerAnimatorController.animator.SetFloat("StartStop", 1.0f);
        playerAnimatorController.animator.SetTrigger("Restart");
        currentMovementSpeed = startMovementSpeed;
        isDead = false;
        isRunning = false;
        isStarted = false;
        isFinishRace = false;
        playerCombat.isReadyToCombat = false;
        isCombatEnd = false;
        playerCombat.m_Target = null;
        playerCombat.staticTarget = null;
        playerCombat.ResetStats();
        playerCombat.isFinishHim = false;
        
        finishPos = new Vector3(0, transform.position.y, LevelManager.Instance.endPoint.z);

        StopAllCoroutines();
    }

    #region PLAYER_MOVEMENT

    void MovePlayer()
    {
        transform.rotation = Quaternion.identity;
        playerAnimatorController.animator.SetFloat("StartStop", 0.0f);
        isRunning = true;
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(Move());
    }

    public void StopMoving()
    {
        isRunning = false;
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
    }

    IEnumerator Move()
    {
        while (!isFinishRace)
        {
            isFinishRace = Mathf.Abs(finishPos.z - transform.position.z) < 45f;
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + currentMovementSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        currentMovementSpeed = 0;
        StartCoroutine(MoveToEndPoint());
    }

    IEnumerator MoveToEndPoint()
    {
        m_IsRunning = true;
        transform.LookAt(finishPos);
        float elapsedTime = 0f;
        Vector3 startPos = transform.position;
        CameraController.Instance.MoveToCombatPosition(CameraController.SceneType.COMBAT);
        while (elapsedTime < timeToCombat)
        {
            transform.position = Vector3.Lerp(startPos, new Vector3(finishPos.x, transform.position.y, finishPos.z), (elapsedTime / timeToCombat));
            //LogUtils.Log((elapsedTime / timeToCombat) + " / " + transform.position.z + "/" + finishPos.z);
            elapsedTime += Time.fixedDeltaTime;
            
            yield return new WaitForFixedUpdate();
        }

        transform.position = finishPos;
        StopMoving();
    }

    public float zLookOffset;
    void RotateByMove(float newDis)
    {
        //transform.LookAt(new Vector3(transform.position.x + newDis, transform.position.y, transform.position.z + zLookOffset));

        if (rotateCoroutine != null) StopCoroutine(rotateCoroutine);
        rotateCoroutine = StartCoroutine(Rotate(newDis));
    }

    IEnumerator Rotate(float inputX)
    {
        if (inputX != 0)
        {
            var elapsedTime = 0f;
            while (rotationTime > elapsedTime)
            {
                var oldRot = transform.rotation;
                var newRot = Quaternion.LookRotation(inputX < 0 ? leftRotationPoint : rightRotaionPoint);
                transform.rotation = Quaternion.Lerp(oldRot, newRot, elapsedTime / rotationTime);

                elapsedTime += Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }
            elapsedTime = 0f;
            while (rotationTime > elapsedTime)
            {
                var oldRot = transform.rotation;
                var newRot = Vector3.zero;
                transform.rotation = Quaternion.Lerp(oldRot, Quaternion.identity, elapsedTime / rotationTime);

                elapsedTime += Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }
        } else
        {
            var elapsedTime = 0f;
            while (rotationTime > elapsedTime)
            {
                var oldRot = transform.rotation;
                var newRot = Vector3.zero;
                transform.rotation = Quaternion.Lerp(oldRot, Quaternion.identity, elapsedTime / rotationTime);

                elapsedTime += Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }
        }
    }

    #endregion

    void PickItem(Item item)
    {
        int value = item.getValue();
        GameManager.Instance.effectsManager.VibrationWithDelay(75, 0f);
        if(item.itemType == Item.Type.DIAMOND)
        {
            ObjectPooler.Instance.SpawnFormPool("FX_Diamond", transform, new Vector3(0, 7f, -0.5f), transform.position, Quaternion.identity);
            LevelManager.Instance.currentPickupDiamond++;
        } else if(item.itemType == Item.Type.KEY)
        {

        } else
        {
            playerAnimatorController.PickUpItem(item);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("other: " + other.tag);

        if (other.CompareTag("Pickup_Item"))
        {
            Item item = other.GetComponent<Item>();
            PickItem(item);
        }

        if (other.CompareTag("Obsacle") && !isDead)
        {
            StopAllCoroutines();
            isDead = true;
            ObjectPooler.Instance.SpawnFormPool("FX_Hit", null, Vector3.zero, playerCombat.HitTrans.position, Quaternion.identity);
            playerAnimatorController.animator.SetLayerWeight(5, 1f);
            playerAnimatorController.animator.SetTrigger("ObsacleHitDie");
            GameManager.Instance.effectsManager.Slowmotion(0.5f, 1.5f, 8f, 0f);
            playerCombat.Die();
            CameraController.Instance.Shake();
            LogUtils.Log("Obsacle Hit");
        }

        if (other.CompareTag("Brick"))
        {
            WallController wallController = other.GetComponent<WallController>();
            var boxCollider = wallController.GetComponent<BoxCollider>();
            if (other.GetType() == typeof(SphereCollider))
            {
                // Change Camera position
                LogUtils.Log("Camera change");

                // Check requirements for wall
                int value = other.GetComponent<Item>().getValue();
                float newScale = playerAnimatorController.GetCurrentScaleBone().gameObject.transform.localScale.x + value * Item.valueForPoint;
                Debug.Log(newScale);
                if (newScale < playerAnimatorController.minScale)
                {
                    // Die by punching the wall
                    isEnoughEnergy = false;
                    boxCollider.size = boxCollider.size + new Vector3(0, 0, -21f);
                } else
                {
                    // Break the wall
                    isEnoughEnergy = true;
                }
                wallController.IsBreakable(isEnoughEnergy);
                playerCombat.staticTarget = wallController.transform;
                

            } else if (other.GetType() == typeof(BoxCollider))
            {
                if (!isEnoughEnergy)
                {
                    playerAnimatorController.animator.SetLayerWeight(2, 1f);
                    playerAnimatorController.animator.SetTrigger("PunchToDie");
                }
                    
                else
                {
                    GameManager.Instance.effectsManager.Slowmotion(0.4f, 0.5f, 4f, 0f);
                    playerAnimatorController.animator.SetTrigger("AttackWhileRunning");
                }
            }
        }
    }
}
