using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorController : MonoBehaviour
{
    [SerializeField] public Animator animator;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    public void SinglePunch()
    {
        int side = Random.Range(0, 2);
        animator.SetFloat("RightLeft", side);
        animator.SetTrigger("Punch");
    }

    public void GotHit()
    {
        animator.SetTrigger("Hit");
    }

    public void PlayDieAnimation()
    {
        animator.SetTrigger("DieByCombat");
    }

    public void StartGame(bool isReady)
    {
        // Idle Blend Tree
        animator.SetFloat("StartStop", isReady? 0.0f : 1.0f);
    }


    public void FinishPunch()
    {
        int side = Random.Range(0, 2);
        if (LevelManager.Instance.levelEndType == LevelEnd.PUNCHING_MACHINE)
        {
            side = 0;
        } else if (LevelManager.Instance.levelEndType == LevelEnd.CHEST)
        {
            side = 1;
        }
        
        animator.SetFloat("RightLeft", side);
        animator.SetTrigger("Finish");
    }

     public void Combo()
    {
        animator.SetTrigger("Combo");
    }
}
