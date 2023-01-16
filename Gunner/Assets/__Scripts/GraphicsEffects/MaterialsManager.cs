using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class MaterialsManager : SingletonMonobehaviour<MaterialsManager>
{
    [Header("Materials")]
    public Material pixelizeMaterial;
    public Material windMaterial;
    public Material blureMaterial;

    protected override void Awake()
    {
        base.Awake();
    }
}
