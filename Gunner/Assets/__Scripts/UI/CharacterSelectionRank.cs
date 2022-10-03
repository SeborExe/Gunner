using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionRank : MonoBehaviour
{
    [SerializeField] TMP_Text rankText;
    [SerializeField] SpriteRenderer rankImage;

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
        }
        else if (animator.runtimeAnimatorController.name == "TheScientist")
        {
            rankText.text = Rank.GetRank("scientist").ToString();
        }
        else if (animator.runtimeAnimatorController.name == "TheThief")
        {
            rankText.text = Rank.GetRank("thief").ToString();
        }

        if (rankText.text == "0")
        {
            rankText.text = "";
            rankImage.color = Color.black;
        }
    }

    public TMP_Text GetRankText()
    {
        return rankText;
    }
}
