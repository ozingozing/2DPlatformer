using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AggressiveWeapon : Weapon
{
	protected Movement Movement
	{ get => movement ?? core.GetCoreComponent(ref movement); }
	private Movement movement;

	protected SO_AggressiveWeaponData aggressiveWeaponData;

	private List<IDamageable> detectedDamageables = new List<IDamageable>();
	private List<IKnockbackable> detectedKnockbackables = new List<IKnockbackable>();

	protected override void Awake()
	{
		base.Awake();

		if(weaponData.GetType() == typeof(SO_AggressiveWeaponData))
		{
			//다운 캐스팅 - 기본 클래스 객체를 파생 클래스 인스턴스로 변형
			aggressiveWeaponData = (SO_AggressiveWeaponData)weaponData;
		}
		else
		{
			Debug.Log("Wrong data for the weapon");
		}
	}

	public override void AnimationActionTrigger()
	{
		base.AnimationActionTrigger();
		
		CheckMeleeAttack();
	}

	private void CheckMeleeAttack()
	{
		//그냥 배열을 foreach에 돌리면 참조값을 들고와서 도중에 사라지면 애러가 걸림
		//그래서 .ToList()로 값을 직접 들고옴
		WeaponAttackDetails details = aggressiveWeaponData.AttackDetails[attackCounter];

		foreach (IDamageable item in detectedDamageables.ToList())
		{
			item.Damage(details.damageAmount);
		}

		foreach (IKnockbackable item in detectedKnockbackables.ToList())
		{
			item.Knockback(details.knockbackAngle, details.knockbackStrength, Movement.FacingDirection);
		}
	}

	public void AddToDetected(Collider2D collision)
	{

		IDamageable damageable = collision.GetComponent<IDamageable>();

        if (damageable != null)
        {
			detectedDamageables.Add(damageable);
        }

		IKnockbackable knockbackable = collision.GetComponent<IKnockbackable>();

		if(knockbackable != null)
		{
			detectedKnockbackables.Add(knockbackable);
		}
    }

	public void RemoveFromDetected(Collider2D collision)
	{
		IDamageable damageable = collision.GetComponent<IDamageable>();

		if (damageable != null)
		{
			detectedDamageables.Remove(damageable);
		}

		IKnockbackable knockbackable = collision.GetComponent<IKnockbackable>();

		if (knockbackable != null)
		{
			detectedKnockbackables.Remove(knockbackable);
		}
	}
}
