//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;

//public class PlayerCombatController : MonoBehaviour
//{
//    [SerializeField]
//    private bool combatEnabled;
//    [SerializeField]
//    private float inputTimer, attack1Radius, attack1Damage;
//    [SerializeField]
//    private float stunDamageAmount = 1f;
//    [SerializeField]
//    private Transform attack1HitBoxPos;
//    [SerializeField]
//    private LayerMask whatIsDamageable;


//	private bool gotInput, isAttacking, isFirstAttack;
//	private float lastInputTime = Mathf.NegativeInfinity;

//    private AttackDetails attackDetails;

//    private Animator anim;

//    private PlayerController PC;
//    private PlayerStats playerStats;

//	private void Start()
//	{
//        anim = GetComponent<Animator>();
//        anim.SetBool("canAttack", combatEnabled);
//        PC = GetComponent<PlayerController>();
//        playerStats = GetComponent<PlayerStats>();
//	}

//	private void Update()
//	{
//		CheckCombatInput();
//        CheckAttacks();
//	}

//	private void CheckCombatInput()
//    {
//        if(Input.GetMouseButtonDown(0))
//        {
//            Debug.Log("공격!");
//            if(combatEnabled)
//            {
//                //Attempt combat
//                gotInput = true;
//                lastInputTime = Time.time;
//            }
//        }
//    }

//    private void CheckAttacks()
//    {
//        if(gotInput)
//        {
//            //Perform Attack1
//            if(!isAttacking)
//            {
//                gotInput = false;
//                isAttacking = true;
//                isFirstAttack = !isFirstAttack;
//                anim.SetBool("attack1", true);
//                anim.SetBool("firstAttack", isFirstAttack);
//                anim.SetBool("isAttacking", isAttacking);
//            }
//        }

//        if(Time.time > lastInputTime + inputTimer)
//        {
//            //Wait for new input
//            gotInput = false;
//        }
//    }

//    private void CheckAttackHitBox()
//    {
//        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attack1HitBoxPos.position, attack1Radius,
//            whatIsDamageable);

//        attackDetails.damageAmount = attack1Damage;
//        attackDetails.position = transform.position;
//        attackDetails.stunDamageAmount = stunDamageAmount;

//        foreach(Collider2D collider in detectedObjects)
//        {
//            //SendMessage 이거 개편함 다른스크립트 참조 없이 함수 이름만 알면 호출 됨 개쩜
//            collider.transform.parent.SendMessage("Damage", attackDetails);
//            //TODO: Instantiate hit particle
//        }
//    }

//    private void FinishAttack1()
//    {
//        isAttacking = false;
//        anim.SetBool("isAttacking", isAttacking);
//        anim.SetBool("attack1", false);
//    }

//    private void Damage(AttackDetails attackDetails)
//    {
//        if(!PC.GetDashStatus())
//        {
//			int direction;

//            playerStats.DecreaseHealth(attackDetails.damageAmount);

//			if (attackDetails.position.x < transform.position.x)
//			{
//				direction = 1;
//			}
//			else
//			{
//				direction = -1;
//			}

//			PC.Knockback(direction);
//		}
        
//    }

//}
