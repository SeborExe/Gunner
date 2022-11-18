using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using echo17.EndlessBook;
using UnityEngine.UI;
using System;

public class EndlessBookController : MonoBehaviour
{
    [SerializeField] EndlessBook endlessBook;
    [SerializeField] AudioSource audioSource;
    [SerializeField] Button nextButton;
    [SerializeField] Button previousButton;

    private void OnEnable()
    {
        nextButton.onClick.AddListener(NextPage);
        previousButton.onClick.AddListener(PreviousPage);
        CheckPreviousPage();
    }

    private void OnDisable()
    {
        nextButton.onClick.RemoveListener(NextPage);
        previousButton.onClick.RemoveListener(PreviousPage);
    }

    private void NextPage()
    {
        if (!endlessBook.IsLastPageGroup)
        {
            endlessBook.TurnToPage(endlessBook.CurrentLeftPageNumber + 2, EndlessBook.PageTurnTimeTypeEnum.TimePerPage, 1f);
            audioSource.Play();

            previousButton.gameObject.SetActive(true);
            Invoke(nameof(CheckNextPage), 1.1f);
        }
    }

    private void PreviousPage()
    {
        if (!endlessBook.IsFirstPageGroup)
        {
            endlessBook.TurnToPage(endlessBook.CurrentLeftPageNumber - 2, EndlessBook.PageTurnTimeTypeEnum.TimePerPage, 1f);
            audioSource.Play();

            nextButton.gameObject.SetActive(true);
            Invoke(nameof(CheckPreviousPage), 1.1f);
        }
    }

    private void CheckNextPage()
    {
        if (endlessBook.IsLastPageGroup)
        {
            nextButton.gameObject.SetActive(false);
        }
    }

    private void CheckPreviousPage()
    {
        if (endlessBook.IsFirstPageGroup)
        {
            previousButton.gameObject.SetActive(false);
        }
    }
}
