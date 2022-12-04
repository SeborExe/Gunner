using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGen : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] GameObject bullet;
    [SerializeField] int numberOfProjectiles;
    [SerializeField] private float ammoSpeed = 8f;
    [SerializeField] private float timeBetweenShoots = 2f;
    [SerializeField] ParticleSystem bloodParticle;

    private int cycles = 8;
    private float radius = 5f;
    private Vector2 startPoint;

    private bool shoot = true;

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
        
        if (Vector3.Distance(transform.position, pointToMove.transform.position) <= Mathf.Epsilon && shoot)
        {
            StartCoroutine(SpawnProjectiles(numberOfProjectiles));
        }
    }

    private IEnumerator SpawnProjectiles(int numberOfProjectiles)
    {
        float angleStep = 360f / numberOfProjectiles;
        float angle = 0f;

        startPoint = transform.position;
        shoot = false;

        for (int a = 0; a < cycles; a++)
        {
            for (int i = 0; i <= numberOfProjectiles - 1; i++)
            {
                float projectileDirXposition = startPoint.x + Mathf.Sin((angle * Mathf.PI) / 180) * radius;
                float projectileDirYposition = startPoint.y + Mathf.Cos((angle * Mathf.PI) / 180) * radius;

                Vector2 projectileVector = new Vector2(projectileDirXposition, projectileDirYposition);
                Vector2 projectileMoveDIrection = (projectileVector - startPoint).normalized * speed;

                IFireable projectile = (IFireable)PoolManager.Instance.ReuseComponent(bullet, startPoint, Quaternion.identity);
                projectile.GetGameObject().SetActive(true);
                projectile.GetGameObject().GetComponent<Rigidbody2D>().velocity = new Vector2(projectileMoveDIrection.x, projectileMoveDIrection.y);

                angle += angleStep;
            }
            yield return new WaitForSeconds(timeBetweenShoots);
        }

        Die();
    }

    private void Die()
    {
        Instantiate(bloodParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
