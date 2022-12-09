using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DestroyedEvent))]
[DisallowMultipleComponent]
public class Destroyed : MonoBehaviour
{
    private DestroyedEvent destroyedEvent;
    [SerializeField] ParticleSystem playerDeadParticles;

    private void Awake()
    {
        destroyedEvent = GetComponent<DestroyedEvent>();
    }

    private void OnEnable()
    {
        destroyedEvent.OnDestroyed += DestroyedEvent_OnDestroyed;
    }

    private void OnDisable()
    {
        destroyedEvent.OnDestroyed -= DestroyedEvent_OnDestroyed;
    }

    private void DestroyedEvent_OnDestroyed(DestroyedEvent destroyedEvent, DestroyEventArgs destroyEventArgs)
    {
        if (destroyEventArgs.playerDied)
        {
            Instantiate(playerDeadParticles, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }
        else
        {
            if (destroyedEvent.TryGetComponent<DevilHearthStats>(out DevilHearthStats devilHearth))
            {
                devilHearth.StopAllCoroutines();
            }

            Destroy(gameObject);
        }
    }
}
