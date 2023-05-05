using System;
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
    [SerializeField] TMP_Text characterNameText;

    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        SetRank();
    }

    private void SetRank()
    {
        if (animator.runtimeAnimatorController.name == "TheAstronaut")
        {
            rankText.text = Rank.GetRank("astronaut").ToString();
            SetCharacterName("The Astronaut");
            SetIfWon();
        }
        else if (animator.runtimeAnimatorController.name == "TheScientist")
        {
            rankText.text = Rank.GetRank("scientist").ToString();
            SetCharacterName("The Scientist");
            SetIfWon();
        }
        else if (animator.runtimeAnimatorController.name == "TheThief")
        {
            rankText.text = Rank.GetRank("thief").ToString();
            SetCharacterName("The Thief");
            SetIfWon();
        }
        else if (animator.runtimeAnimatorController.name == "TheKnight")
        {
            rankText.text = Rank.GetRank("knight").ToString();
            SetCharacterName("The Knight");
            SetIfWon();
        }

        if (rankText.text == "0")
        {
            rankText.text = "";
            rankImage.color = Color.black;
        }
    }

    private void SetCharacterName(string name)
    {
        characterNameText.text = name;
    }

    private void SetIfWon()
    {
        if (rankText.text == "7")
        {
            rankText.text = "W";
            rankImage.sprite = wonGameSprite;
        }
    }

    public TMP_Text GetRankText()
    {
        return rankText;
    }

    public TMP_Text GetCharacterName()
    {
        return characterNameText;
    }
}
