using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableRankText : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<CharacterSelectionRank>(out CharacterSelectionRank selectionRank))
        {
            selectionRank.GetRankText().enabled = true;
            selectionRank.GetCharacterName().enabled = true;
        }
    }
}
