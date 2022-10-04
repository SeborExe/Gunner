using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionRank : MonoBehaviour
{
    [SerializeField] TMP_Text rankText;
    [SerializeField] SpriteRenderer rankImage;
    [SerializeField] Sprite wonGameSprite;

    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        SetRank();
    }

    private void SetRank()
    {
        if (animator.runtimeAnimatorController.name == "TheGeneral")
        {
            rankText.text = Rank.GetRank("general").ToString();
            SetIfWon();
        }
        else if (animator.runtimeAnimatorController.name == "TheScientist")
        {
            rankText.text = Rank.GetRank("scientist").ToString();
            SetIfWon();
        }
        else if (animator.runtimeAnimatorController.name == "TheThief")
        {
            rankText.text = Rank.GetRank("thief").ToString();
            SetIfWon();
        }

        if (rankText.text == "0")
        {
            rankText.text = "";
            rankImage.color = Color.black;
        }
    }

    private void SetIfWon()
    {
        if (rankText.text == "6")
        {
            rankText.text = "W";
            rankImage.sprite = wonGameSprite;
        }
    }

    public TMP_Text GetRankText()
    {
        return rankText;
    }
}
