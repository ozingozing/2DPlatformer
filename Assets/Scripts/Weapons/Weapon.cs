using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	private Animator baseAnimator;
	private Animator weaponAnimator;

    protected PlayerAttackState state;

	protected virtual void Start()
    {
        baseAnimator = transform.Find("Base").GetComponent<Animator>();
        weaponAnimator = transform.Find("Weapon").GetComponent<Animator>();

        gameObject.SetActive(false);
    }

    public virtual void EnterWeapon()
    {
        gameObject.SetActive(true);
        baseAnimator.SetBool("attack", true);
        weaponAnimator.SetBool("attack", true);
    }

    public virtual void ExitWeapon()
    {
		baseAnimator.SetBool("attack", false);
		weaponAnimator.SetBool("attack", false);
	    gameObject.SetActive(false);
    }

	#region Animation Tiggers
	public virtual void AnimationFinishTrigger()
    {
        state.AnimationFinishTrigger();
    }
    #endregion

    public void InitializeWeapon(PlayerAttackState state)
    {
        this.state = state;
    }
}
