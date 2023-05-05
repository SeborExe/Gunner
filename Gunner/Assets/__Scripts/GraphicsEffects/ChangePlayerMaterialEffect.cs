using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChangeMaterial_", menuName = "Scriptable Objects/Items/Effects/Change Material")]
public class ChangePlayerMaterialEffect : ItemEffect
{
    [SerializeField] Material materialToSet;

    private string windMat = "WindEffect_Mat (Instance)";
    private string pixelMat = "Pixelize_Mat (Instance)";
    private string blureMat = "SpriteBlure_Mat (Instance)";
    private string pixelWindMat = "PixelizeWind_Mat (Instance)";
    private string pixelBlureMat = "PixelateBlure_Mat (Instance)";
    private string windBlureMat = "BlureWind_Mat (Instance)";

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
            if ((materialToSet == materialsManager.pixelizeMaterial && playerMaterial.name == windMat) ||
                (materialToSet == materialsManager.windMaterial && playerMaterial.name == pixelMat))
            {
                GameManager.Instance.GetPlayer().spriteRenderer.material = materialsManager.pixelizeAndWindMaterial;
            }

            //Pixel and Blure
            else if ((materialToSet == materialsManager.pixelizeMaterial && playerMaterial.name == blureMat) ||
                (materialToSet == materialsManager.blureMaterial && playerMaterial.name == pixelMat))
            {
                GameManager.Instance.GetPlayer().spriteRenderer.material = materialsManager.blureAndPixelizeMaterial;
            }

            //Blure and Wind
            else if ((materialToSet == materialsManager.blureMaterial && playerMaterial.name == windMat) ||
                (materialToSet == materialsManager.windMaterial && playerMaterial.name == blureMat))
            {
                GameManager.Instance.GetPlayer().spriteRenderer.material = materialsManager.windAndBlureMaterial;
            }

            //Blure, Wind and pixelize
            else if ((materialToSet == materialsManager.blureMaterial && playerMaterial.name == pixelWindMat) ||
                (materialToSet == materialsManager.windMaterial && playerMaterial.name == pixelBlureMat) ||
                (materialToSet == materialsManager.pixelizeMaterial && playerMaterial.name == windBlureMat))
            {
                GameManager.Instance.GetPlayer().spriteRenderer.material = materialsManager.blurePixelizeAndWindMaterial;
            }
        }
  
        base.ActiveEffect();
    }
}
