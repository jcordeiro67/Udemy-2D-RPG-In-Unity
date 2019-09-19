using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleChar : MonoBehaviour {

	public bool isPlayer;
	public string charName;
	public string[] movesAvailiable;
	public int currentHP, maxHP, currentMP, maxMP, strength, defence, wpnPower, armrPower;
	public bool hasDied;
	public bool shouldFade;
	public float fadeSpeed;
	public Color fadeColor;
	public SpriteRenderer spriteRenderer;
	public Sprite aliveSprite, deadSprite;

    // Start is called before the first frame update
    void Start() {
		if (spriteRenderer == null) {
			spriteRenderer = GetComponent<SpriteRenderer>();
		}
    }

    // Update is called once per frame
    void Update() {
		if (shouldFade) {
			spriteRenderer.color = new Color(Mathf.MoveTowards(spriteRenderer.color.r, fadeColor.r, fadeSpeed * Time.deltaTime), 
				Mathf.MoveTowards(spriteRenderer.color.g, fadeColor.g, fadeSpeed * Time.deltaTime),
				Mathf.MoveTowards(spriteRenderer.color.b, fadeColor.b, fadeSpeed * Time.deltaTime), 
				Mathf.MoveTowards(spriteRenderer.color.a, fadeColor.a, fadeSpeed * Time.deltaTime));
			if (spriteRenderer.color.a == fadeColor.a) {
				gameObject.SetActive(false);
			}
		}
    }

	public void EnemyFade() {

		shouldFade = true;

	}
}
