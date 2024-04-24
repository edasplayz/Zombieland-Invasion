using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//  performs identical to to ui_stat bar except this bar appers and disapers in world space (will always face camera)
public class UI_Character_HP_Bar : UI_StatBar
{
    private CharacterManager character;

    [SerializeField] bool displayCharacterNameOnDamage = false;
    [SerializeField] float deaultTimeBeforeBarHides = 3;

}
