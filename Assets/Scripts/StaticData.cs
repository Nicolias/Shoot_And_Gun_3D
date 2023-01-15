using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Static Data", menuName = "ScriptableObjects/Static Data")]
public class StaticData : ScriptableObject
{
    [SerializeField] private List<Sprite> _gunsOnLevelVariation;

    public Sprite GetGunSpriteByLevel(UIGun gun)
    {
        if (gun.Level > _gunsOnLevelVariation.Count - 1)
            return _gunsOnLevelVariation[_gunsOnLevelVariation.Count - 1];

        return _gunsOnLevelVariation[gun.Level];
    }
}