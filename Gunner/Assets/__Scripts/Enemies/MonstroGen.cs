using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonstroGen : MonoBehaviour
{
    [Header("Gen Stats")]
    [SerializeField] float speed = 5f;
    [SerializeField] ParticleSystem bloodParticle;

    [Header("Ammo Stats")]
    [SerializeField] AmmoDetailsSO ammoDetails;
    [SerializeField] int numberOfProjectiles;
    [SerializeField] private float timeBetweenShoots = 2f;
    [SerializeField] private int cycles = 8;

    private float radius = 5f;
    private Vector2 startPoint;

    private bool shoot = true;

    private FireGenPoint pointToMove;

    public void SetUp(FireGenPoint pointToMove)
    {
        this.pointToMove = pointToMove;
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
        GameObject ammoPrefab = ammoDetails.ammoPrefabArray[UnityEngine.Random.Range(0, ammoDetails.ammoPrefabArray.Length)];

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
                Vector2 projectileMoveDIrection = (projectileVector - startPoint).normalized * UnityEngine.Random.Range(ammoDetails.ammoSpeedMin, ammoDetails.ammoSpeedMax);

                IFireable projectile = (IFireable)PoolManager.Instance.ReuseComponent(ammoPrefab, startPoint, Quaternion.identity);
                projectile.InitializeAmmo(ammoDetails, 0, 0, UnityEngine.Random.Range(ammoDetails.ammoSpeedMin, ammoDetails.ammoSpeedMax), startPoint, gameObject);
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
