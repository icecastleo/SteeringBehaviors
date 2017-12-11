using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStateUI : MonoBehaviour {

    Character character;

    [SerializeField]
    GameObject HpBar = null;
    Slider HpBarSlider;

    float HpBarShowTime = 6f;
    float HpBarCountdown = 0f;

	// Use this for initialization
	void Start () {
        character = GetComponentInParent<Character>();

        HpBar.transform.localPosition += new Vector3(0, character.GetComponent<SpriteRenderer>().sprite.bounds.size.y * 0.6f, 0);
        HpBar.GetComponent<RectTransform>().sizeDelta = new Vector2(character.GetComponent<SpriteRenderer>().sprite.bounds.size.x, character.GetComponent<SpriteRenderer>().sprite.bounds.size.x / 20);

        HpBarSlider = HpBar.GetComponent<Slider>();
        HpBarSlider.value = character.Hp * 100 / character.MaxHp;
    }
	
	// Update is called once per frame
	void Update () {

        if (HpBarShowTime > 0)
        {
            float temp = HpBarSlider.value;

            HpBarSlider.value = character.Hp * 100 / character.MaxHp;

            if (temp != HpBarSlider.value)
            {
                HpBarCountdown = HpBarShowTime;
            }

            if (HpBarCountdown > 0)
            {
                HpBar.SetActive(true);
            } else
            {
                HpBar.SetActive(false);
            }

            HpBarCountdown -= Time.deltaTime;
        }
    }
}
