using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Image spellIconImage; // this is the background image

    // update the active spell icon
    public void UpdateActiveSpellIcon(Sprite newIcon)
    {
        spellIconImage.sprite = newIcon;
    }

    public void OnSpellButtonClicked(Spell spell)
    {
        //UpdateActiveSpellIcon(spell.spellIcon);
    }

}
