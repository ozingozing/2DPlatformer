using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatDummyController : MonoBehaviour
{
    [SerializeField]
    private float maxHealth, knockbackSpeed_X, knockbackSpeed_Y, knockbackDuration,
        knockbackDeathSpeed_X, knockbackDeathSpeed_Y, deathTorque;
    [SerializeField]
    private bool apllyKnockback;
    [SerializeField]
    private GameObject hitParticle;

    private float currentHealth, knockbackStart;

    private int playerFacingDirection;
    private bool playerOnLeft, knockback;

    private PlayerController playerController;
    private GameObject aliveGO, brokenTopGO, brokenBottomGO;
    private Rigidbody2D rbAlive, rbBrokenTop, rbBrokenBottom;
    private Animator aliveAnim;

	private void Start()
	{
        currentHealth = maxHealth;

        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        aliveGO = transform.GetChild(0).gameObject;
        brokenTopGO = transform.GetChild(1).gameObject;
        brokenBottomGO = transform.GetChild(2).gameObject;

        aliveAnim = aliveGO.GetComponent<Animator>();
        rbAlive = aliveGO.GetComponent<Rigidbody2D>();
        rbBrokenTop = brokenTopGO.GetComponent<Rigidbody2D>();
        rbBrokenBottom = brokenBottomGO.GetComponent<Rigidbody2D>();

        aliveGO.SetActive(true);
        brokenTopGO.SetActive(false);
        brokenBottomGO.SetActive(false);
	}

	private void Update()
	{
		CheckKnockback();
	}

    private void Knockback()
    {
        knockback = true;
        knockbackStart = Time.time;
        rbAlive.velocity = new Vector2(knockbackSpeed_X * playerFacingDirection, knockbackSpeed_Y);
    }

    private void CheckKnockback()
    {
        if(Time.time >= knockbackStart + knockbackDuration && knockback)
        {
            knockback = false;
            rbAlive.velocity = new Vector2(0, rbAlive.velocity.y);
        }
    }

    private void Die()
    {
        aliveGO.SetActive(false);
        brokenTopGO.SetActive(true);
        brokenBottomGO.SetActive(true);

        brokenTopGO.transform.position = aliveGO.transform.position;
        brokenBottomGO.transform.position = aliveGO.transform.position;

        rbBrokenBottom.velocity = new Vector2(knockbackSpeed_X * playerFacingDirection, knockbackSpeed_Y);
        rbBrokenTop.velocity = new Vector2(knockbackDeathSpeed_X * playerFacingDirection, knockbackDeathSpeed_Y);
        rbBrokenTop.AddTorque(deathTorque * -playerFacingDirection, ForceMode2D.Impulse);
    }
}
