using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "UIConfig", menuName = "UI/UIConfig")]
public class UIConfig : ScriptableObject
{
    [Header("Inventory")]
    public GameObject itemIcon;

    [Header("Domino")]
    public GameObject dominoBase;
    public GameObject sigil;

    [Header("Sigils menu")]
    public GameObject sigilFilterButton;
    public GameObject sigilCell;
    public GameObject numberFilterButton;
    public List<Sprite> numberFiltersSprites;
}
