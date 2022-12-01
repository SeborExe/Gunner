using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGen : MonoBehaviour
{
    [SerializeField] float speed = 5f;

    List<FireGenPoint> points = new List<FireGenPoint>();

    private FireGenPoint pointToMove;

    private void Start()
    {
        foreach (FireGenPoint point in FindObjectsOfType<FireGenPoint>())
        {
            points.Add(point);
        }

        int randomPointIndex = UnityEngine.Random.Range(0, points.Count);
        pointToMove = points[randomPointIndex];
    }

    private void Update()
    {
        if (pointToMove != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, pointToMove.transform.position, Time.deltaTime * speed);
        }
    }
}
