using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monstro : MonoBehaviour
{
    [Header("FireGen")]
    [SerializeField] Transform[] fireGens;

    List<FireGenPoint> points = new List<FireGenPoint>();

    private void Start()
    {
        SetUp();
    }

    private void SetUp()
    {
        foreach (FireGenPoint point in FindObjectsOfType<FireGenPoint>())
        {
            points.Add(point);
        }

        if (points.Count > 0)
        {
            foreach (FireGenPoint point in points)
            {
                InstantiateMonstroGen(point);
            }
        }
    }

    public void InstantiateMonstroGen(FireGenPoint pointToMove, int amountToSummon = 1)
    {
        for (int i = 0; i < amountToSummon; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, fireGens.Length);
            Transform monstroGenTransform = Instantiate(fireGens[randomIndex], transform.position, Quaternion.identity);
            monstroGenTransform.GetComponent<MonstroGen>().SetUp(pointToMove);
        }
    }
}
