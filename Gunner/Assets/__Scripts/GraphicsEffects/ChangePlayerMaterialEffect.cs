using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChangeMaterial_", menuName = "Scriptable Objects/Items/Effects/Change Material")]
public class ChangePlayerMaterialEffect : ItemEffect
{
    [SerializeField] Material materialToSet;

    public override void ActiveEffect()
    {
        Material playerMaterial = GameManager.Instance.GetPlayer().spriteRenderer.material;
        MaterialsManager materialsManager = MaterialsManager.Instance;

        if (playerMaterial.name == materialsManager.basicMaterial)
        {
            GameManager.Instance.GetPlayer().spriteRenderer.material = materialToSet;
        }

        else
        {
            if (playerMaterial == materialToSet) return;


            //Pixel and Wind
            if ((materialToSet == materialsManager.pixelizeMaterial && playerMaterial == materialsManager.windMaterial) ||
                (materialToSet == materialsManager.windMaterial && playerMaterial == materialsManager.pixelizeMaterial))
            {
                GameManager.Instance.GetPlayer().spriteRenderer.material = materialsManager.pixelizeAndWindMaterial;
            }

            //Pixel and Blure
            else if ((materialToSet == materialsManager.pixelizeMaterial && playerMaterial == materialsManager.blureMaterial) ||
                (materialToSet == materialsManager.blureMaterial && playerMaterial == materialsManager.pixelizeMaterial))
            {
                GameManager.Instance.GetPlayer().spriteRenderer.material = materialsManager.blureAndPixelizeMaterial;
            }

            //Blure and Wind
            else if ((materialToSet == materialsManager.blureMaterial && playerMaterial == materialsManager.windMaterial) ||
                (materialToSet == materialsManager.windMaterial && playerMaterial == materialsManager.blureMaterial))
            {
                GameManager.Instance.GetPlayer().spriteRenderer.material = materialsManager.windAndBlureMaterial;
            }
        }
  
        base.ActiveEffect();
    }
}
