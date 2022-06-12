using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : CharacterAnimatorController
{

    [SerializeField] float scaleSpeed;

    [SerializeField] GameObject[] scaleBones;

    [SerializeField] Transform[] scaleFXPos;

    [SerializeField] public float maxScale, minScale;

    Coroutine scaleCoroutine;

    [Range(0.8f, 1.5f)] public float startScale;

    PlayerCombat playerCombat;

    float m_currentScale;
    public float currentScale { get { return m_currentScale; } set { m_currentScale = value; StartScale(value); } }

    // Start is called before the first frame update
    protected override void Start()
    {
        playerCombat = GetComponent<PlayerCombat>();
        SetScale(startScale);
    }

    // Update is called once per frame
    protected override void Update()
    {
        
    }

    public void SetScale(float newScale)
    {
        m_currentScale = startScale;
        scaleBones[0].transform.localScale = new Vector3(newScale, newScale, newScale);
        scaleBones[1].transform.localScale = new Vector3(newScale, newScale, newScale);
    }

    public void StartScale(float targetScale)
    {
        if (scaleCoroutine != null) StopCoroutine(scaleCoroutine);
        scaleCoroutine = StartCoroutine(ScaleBones(targetScale));
        float currentDamage = playerCombat.baseDamage;
        playerCombat.currentDamage = (int) (currentDamage * targetScale);

        float maxHealth = playerCombat.baseHealth;
        playerCombat.maxHealth = (int)(maxHealth * targetScale);
        playerCombat.currentHealth = playerCombat.maxHealth;
    }

    IEnumerator ScaleBones(float targetScale)
    {
        m_currentScale = scaleBones[0].transform.localScale.x;

        ObjectPooler.Instance.SpawnFormPool((targetScale < m_currentScale) ? "FX_Food" : "FX_Train", scaleFXPos[0], Vector3.zero,
            scaleFXPos[0].position, Quaternion.identity);
        ObjectPooler.Instance.SpawnFormPool((targetScale < m_currentScale) ? "FX_Food" : "FX_Train", scaleFXPos[1], Vector3.zero,
            scaleFXPos[1].position, Quaternion.identity);

        float extraScale = targetScale - m_currentScale;

        if (targetScale >= maxScale) targetScale = maxScale;
        if (targetScale <= minScale) targetScale = minScale;

        float newScaleTemp;
        var newScaleSpeed = scaleSpeed;

        newScaleTemp = targetScale + extraScale * 2;
        
        newScaleSpeed = 2 * scaleSpeed;
        while(Mathf.Abs(m_currentScale - newScaleTemp) > 0.01f)
        {
            float newScale = Mathf.Lerp(m_currentScale, newScaleTemp, newScaleSpeed * Time.deltaTime);
            SetScale(newScale);
            m_currentScale = newScale;
            startScale = newScale;
            yield return new WaitForEndOfFrame();
        }
        while (Mathf.Abs(m_currentScale - targetScale) > 0.01f)
        {
            float newScale = Mathf.Lerp(m_currentScale, targetScale, newScaleSpeed * Time.deltaTime);
            SetScale(newScale);
            m_currentScale = newScale;
            startScale = newScale;
            yield return new WaitForEndOfFrame();
        }

    }

    public GameObject GetCurrentScaleBone()
    {
        return scaleBones[0];
    }

    public void PickUpItem(Item item)
    {
        float extraScale = item.getValue() * Item.valueForPoint;
        float newScale = GetCurrentScaleBone().gameObject.transform.localScale.x + extraScale;
        currentScale = newScale;
    }
}
