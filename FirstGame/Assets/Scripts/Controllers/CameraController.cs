using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public enum SceneType
    {
        RACING,
        COMBAT,
        POINT,
        FLYING,
        ZOOMING,
        FINISHPUNCH,
        DEAD
    }

    public SceneType m_SceneType;

    [SerializeField] Transform target;

    [SerializeField] [Range(0, 1f)] float smoothSpeed;

    [SerializeField] float cameraSpeed = 1f;

    [SerializeField] Vector3 currentOffset, startOffset, combatOffset, finishPunchOffset, flyingOffset, punchMachinePointViewOffset, deadOffset;

    [SerializeField] float currentPitch = 2f;
    [SerializeField] float startPitch = 2f;
    [SerializeField] float combatPitch = 3f;
    [SerializeField] float punchMachinePitch = 9f;
    [SerializeField] float raceToCombatTime = 2.5f;
    [SerializeField] float timeCombatToFinish = 1f;
    [SerializeField] float timeFinishtToFlying = 1f;

    [SerializeField] float currrentZoom = 10f;
    [SerializeField] float combatZoom = 3.2f;
    [SerializeField] float startZoom = 4f;
    [SerializeField] float deadZoom = 4f;

    [SerializeField] float yOffset = 15f;

    private Vector3 velocity = Vector3.zero;

    public float stress;

    public bool isFollowing = true;

    public bool isCombatEnd = true;

    #region For Modifier
    public bool isFollowingByNewOffsetFixed = false;

    public float timeScale = 0.5f;

    public float timeScaleSpeed = 1f;

    public bool isLooking = false;
    #endregion
    Coroutine movingCoroutine;

    #region SingleTon

    public static CameraController Instance;

    public float t;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        } else
        {
            Instance = this;
        }
    }

    #endregion

    private void Start()
    {
        currentOffset = startOffset;
        currrentZoom = startZoom;
        currentPitch = startPitch;
    }

    private void LateUpdate()
    {
        if (isFollowing && target != null)
        {
            isLooking = true;
            Vector3 newPos = target.position - currentOffset * currrentZoom;
            Vector2 vec2 = Vector2.Lerp(new Vector2(transform.position.x, transform.position.z), new Vector2(newPos.x, newPos.z), smoothSpeed);
            Vector3 smoothPos = Vector3.Lerp(transform.position, new Vector3(vec2.x, target.position.y + yOffset, vec2.y), smoothSpeed);
            transform.position = smoothPos;
            transform.LookAt(target.position + Vector3.up * currentPitch);
        } else if (isFollowingByNewOffsetFixed && target != null)
        {
            isLooking = true;
            Vector3 newPos = target.position - currentOffset * currrentZoom;
            Vector3 smoothPos = Vector3.Lerp(transform.position, newPos, smoothSpeed);
            transform.position = smoothPos;
            transform.LookAt(target.position + Vector3.up * currentPitch);
        }

        //transform.position = Vector3.SmoothDamp(transform.position, newPos, ref velocity, smoothSpeed);
    }

    private void Update()
    {
        
    }

    public void Shake()
    {
        GameManager.Instance.effectsManager.VibrationWithDelay(100, 0f);
        
        var receiver = GetComponent<StressReceiver>();
        receiver.InduceStress(stress);
    }


    public void MoveToStartScene(SceneType sceneType)
    {
        if (movingCoroutine != null) StopCoroutine(movingCoroutine);
        target = PlayerController.Instance.transform;
        isFollowing = true;
        isFollowingByNewOffsetFixed = false;

        currentOffset = startOffset;
        currentPitch = startPitch;
        currrentZoom = startZoom;

        var newPos = target.position - startOffset * startZoom;
        transform.position = newPos + new Vector3(0, yOffset, 0);
        transform.LookAt(target.position + Vector3.up * startPitch);
    }

    void MoveToPointCountScene(SceneType sceneType)
    {
        isLooking = false;

        Invoke("MyMove", 0.5f);
    }

    void MyMove()
    {
        MoveToNewScene(LevelManager.Instance.finalChallenge.transform, punchMachinePointViewOffset, punchMachinePitch, startZoom, 1f, SceneType.POINT, new Vector3(0, 0, 0f), 0f);
    }

    public void MoveToDeadScene(SceneType sceneType)
    {
        isLooking = false;
        MoveToNewScene(target, deadOffset, currentPitch, deadZoom, 1f, sceneType, new Vector3(0, 0, 1f), 0f);
    }

    public void MoveToCombatPosition(SceneType sceneType)
    {
        isLooking = false;
        var zOffset = LevelManager.Instance.levelEndType == LevelEnd.PUNCHING_MACHINE ? -5f : -3f;
        if(LevelManager.Instance.finalChallenge.CompareTag("Boss") || LevelManager.Instance.finalChallenge.CompareTag("King"))
        {
            LevelManager.Instance.finalChallenge.GetComponentInChildren<Animator>().SetTrigger("Taunt");
        }
        MoveToNewScene(LevelManager.Instance.finalChallenge.transform, combatOffset, combatPitch, combatZoom, raceToCombatTime, sceneType, new Vector3(0, 1, zOffset), 0);
    }

    public void MoveToFinishPunchScene(SceneType sceneType)
    {
        isLooking = false;
        MoveToNewScene(LevelManager.Instance.finalChallenge.transform, finishPunchOffset, currentPitch, currrentZoom, timeCombatToFinish, sceneType, Vector3.zero, 0.1f);
    }

    public void MoveToFlyingScene(SceneType sceneType)
    {
        isLooking = true;
        MoveToNewScene(LevelManager.Instance.finalChallenge.transform, flyingOffset, currentPitch, 3.5f, timeFinishtToFlying, sceneType, new Vector3(0, 0, 1f), 0f);
    }

    public void MoveToNewScene(Transform targetPosition, Vector3 newOffset, float newPitch, float newZoom, float duration, SceneType scenetype, Vector3 fixedChange, float callbackDelay)
    {
        m_SceneType = scenetype;
        var calback_method = "";
        switch (scenetype)
        {
            case SceneType.COMBAT:
                calback_method = "CombatReadyCallBack";
                break;

            case SceneType.FINISHPUNCH:
                calback_method = "PunchReadyCallBack";
                break;

            case SceneType.FLYING:
                calback_method = "ReadyToFollowFlyingObject";
                break;

            case SceneType.RACING:
                isFollowing = true;
                break;
            default:

                break;
        }

        if (movingCoroutine != null) StopCoroutine(movingCoroutine);
        movingCoroutine = StartCoroutine(MoveToNewAngeles(targetPosition, newOffset, newPitch, newZoom, duration, calback_method, fixedChange, callbackDelay));
    }

    void CombatReadyCallBack()
    {
        LogUtils.Log("ReadyToAttack");
        PlayerController.Instance.playerCombat.isReadyToCombat = true;
        if (LevelManager.Instance.levelEndType == LevelEnd.BOSS_FIGHT)
        {
            LevelManager.Instance.finalChallenge.GetComponent<CharacterCombat>().isReadyToCombat = true;
        }
        GameManager.Instance.uIController.ToggleGuideUI(1);
    }

    void PunchReadyCallBack()
    {
        LogUtils.Log("PunchReadyPull");
        float durationSlow = LevelManager.Instance.levelEndType == LevelEnd.PUNCHING_MACHINE ? 0.4f : 0.25f;
        GameManager.Instance.effectsManager.Slowmotion(0.2f, durationSlow, 8f, 0f);
        Invoke("ChangeToFlyPosition", 0.2f);
    }

    IEnumerator MoveToNewAngeles(Transform targetPosition, Vector3 newOffset, float newPitch, float newZoom, float duration, string calback_method, Vector3 fixedChange, float callbackDelay)
    {
        isFollowing = false;

        yield return new WaitForEndOfFrame();
        Vector3 newFocusPos = targetPosition.position + fixedChange;
        float elapsedTime = 0f;
        Vector3 oldOffset = currentOffset;
        float oldPitch = currentPitch;

        LogUtils.Log("Move Dura: " + duration);

        Vector3 oldPos = transform.position;
        Quaternion oldRot = transform.rotation;

        while (elapsedTime < duration)
        {
            var timeRatio = elapsedTime / duration;
            if (isLooking)
            {
                var newPos = newFocusPos - newOffset * newZoom;
                transform.position = Vector3.Lerp(oldPos, newPos, timeRatio);
                transform.LookAt(target.position + Vector3.up * newPitch);
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            } else {
                Vector3 newTargetPos = Vector3.Lerp(target.position, newFocusPos, timeRatio);
                Vector3 newPos = newTargetPos - Vector3.Lerp(oldOffset, newOffset, timeRatio) * currrentZoom;
                transform.position = Vector3.Lerp(oldPos, newPos, timeRatio);

                var newLookPos = newTargetPos + Vector3.up * Mathf.Lerp(oldPitch, newPitch, timeRatio);
                var newRot = Quaternion.LookRotation(newLookPos - transform.position);
                transform.rotation = Quaternion.Lerp(oldRot, newRot, timeRatio);
                elapsedTime += Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
            //LogUtils.Log("Move Ratio: " + timeRatio);
        }

        currentOffset = newOffset;
        currentPitch = newPitch;
        currrentZoom = newZoom;
        target = targetPosition;

        LogUtils.Log("Move Cam Completed");

        Invoke(calback_method, callbackDelay);
    }

    void ChangeToFlyPosition()
    {
        if (LevelManager.Instance.levelEndType == LevelEnd.BOSS_FIGHT) MoveToFlyingScene(SceneType.FLYING);
        else if (LevelManager.Instance.levelEndType == LevelEnd.PUNCHING_MACHINE) MoveToPointCountScene(SceneType.POINT);
    }

    void ReadyToFollowFlyingObject()
    {
        isFollowingByNewOffsetFixed = true;
    }
}
