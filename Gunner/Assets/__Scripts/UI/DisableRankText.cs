using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableRankText : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<CharacterSelectionRank>(out CharacterSelectionRank selectionRank))
        {
            selectionRank.GetRankText().enabled = false;
            selectionRank.GetCharacterName().enabled = false;
        }
    }
}
