using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImageSprite : MonoBehaviour
{
    [SerializeField]
    private float activeTime = 0.1f;
    private float timeActivated;
    private float alpha;
	[SerializeField]
	private float alphaSet = 0.8f;
    [SerializeField]
    private float alphaDecay = 0.5f;

    private Transform player;

    private SpriteRenderer spriteRenderer;
    private SpriteRenderer playerSpriteRenderer;

    private Color color;

	private void OnEnable()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerSpriteRenderer = player.GetComponent<SpriteRenderer>();

        alpha = alphaSet;
        spriteRenderer.sprite = playerSpriteRenderer.sprite;
        transform.position = player.position;
        transform.rotation = player.rotation;
        timeActivated = Time.time;
	}

	private void Update()
	{
        alpha -= alphaDecay * Time.deltaTime * 0.5f;
        color = new Color(1f, 1f, 1f, alpha);
        spriteRenderer.color = color;

        if(Time.time > (timeActivated + activeTime))
        {
            //Add back to pool
            PlayerAfterImagePool.Instance.AddToPool(gameObject);
        }
	}
}
