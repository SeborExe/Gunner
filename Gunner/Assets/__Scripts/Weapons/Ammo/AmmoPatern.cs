using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPatern : MonoBehaviour, IFireable
{
    [SerializeField] private Ammo[] ammoArray;

    private float ammoRange;
    private float ammoSpeed;
    private Vector3 fireDirectionVector;
    private float fireDirectionAngle;
    private AmmoDetailsSO ammoDetails;
    private float ammoChargeTime;

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void InitializeAmmo(AmmoDetailsSO ammoDetails, float aimAngle, float weaponAimAngle, float ammoSpeed, Vector3 weaponAimDirectionVector, GameObject sender,
        bool overrideAmmoMovement)
    {
        this.ammoDetails = ammoDetails;
        this.ammoSpeed = ammoSpeed;

        SetFireDirection(ammoDetails, aimAngle, weaponAimAngle, weaponAimDirectionVector);

        ammoRange = ammoDetails.ammoRange;

        gameObject.SetActive(true);

        foreach (Ammo ammo in ammoArray)
        {
            ammo.InitializeAmmo(ammoDetails, aimAngle, weaponAimAngle, ammoSpeed, weaponAimDirectionVector, this.gameObject, true);
        }

        if (ammoDetails.ammoChargeTime > 0f)
        {
            ammoChargeTime = ammoDetails.ammoChargeTime;
        }
        else
        {
            ammoChargeTime = 0f;
        }
    }

    private void Update()
    {
        if (ammoChargeTime > 0f)
        {
            ammoChargeTime -= Time.deltaTime;
            return;
        }

        Vector3 distanceVector = fireDirectionVector * ammoSpeed * Time.deltaTime;
        transform.position += distanceVector;

        transform.Rotate(new Vector3(0f, 0f, ammoDetails.ammoRotationSpeed * Time.deltaTime));

        ammoRange -= distanceVector.magnitude;

        if (ammoRange < 0f)
        {
            DisableAmmo();
        }
    }

    private void SetFireDirection(AmmoDetailsSO ammoDetails, float aimAngle, float weaponAimAngle, Vector3 weaponAimDirectionVector)
    {
        float randomSpread = UnityEngine.Random.Range(ammoDetails.ammoSpreadMin, ammoDetails.ammoSpreadMax);

        int spreadToogle = UnityEngine.Random.Range(0, 2) * 2 - 1;

        if (weaponAimDirectionVector.magnitude < Settings.useAimAngleDistance)
        {
            fireDirectionAngle = aimAngle;
        }
        else
        {
            fireDirectionAngle = weaponAimAngle;
        }

        fireDirectionAngle += spreadToogle * randomSpread;
        fireDirectionVector = HelperUtilities.GetDirectionVectorFromAngle(fireDirectionAngle);
    }

    private void DisableAmmo()
    {
        gameObject.SetActive(false);
    }
}
