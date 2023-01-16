using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class MaterialsManager : SingletonMonobehaviour<MaterialsManager>
{
    [Header("Materials")]
    public string basicMaterial = "Sprite-Lit-Default (Instance)";
    public Material pixelizeMaterial;
    public Material windMaterial;
    public Material blureMaterial;

    [Header("Combined Materials")]
    public Material pixelizeAndWindMaterial;
    public Material windAndBlureMaterial;
    public Material windAndPixelizeMaterial;
    public Material blureAndPixelizeMaterial;
    public Material blurePixelizeAndWindMaterial;

    protected override void Awake()
    {
        base.Awake();
    }
}
