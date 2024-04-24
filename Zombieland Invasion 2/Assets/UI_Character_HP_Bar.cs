using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading;

//  performs identical to to ui_stat bar except this bar appers and disapers in world space (will always face camera)
public class UI_Character_HP_Bar : UI_StatBar
{
    private CharacterManager character;
    private AICharacterManager aiCharacter;
    private PlayerManager playerCharacter;

    [SerializeField] bool displayCharacterNameOnDamage = false;
    [SerializeField] float deaultTimeBeforeBarHides = 3;
    [SerializeField] float hideTimer = 0;
    [SerializeField] int currentDamageTaken = 0;
    [SerializeField] TextMeshProUGUI characterName;
    [SerializeField] TextMeshProUGUI characterDamage;
    [HideInInspector] public int oldHealthValue = 0;

    protected override void Awake()
    {
        base.Awake();

        character = GetComponentInParent<CharacterManager>();
        if(character != null )
        {
            aiCharacter = character as AICharacterManager;
            playerCharacter = character as PlayerManager;
        }
        
    }

    protected override void Start()
    {
        base.Start();

        gameObject.SetActive(false);
    }

    public override void SetStat(int newValue)
    {
        if(displayCharacterNameOnDamage)
        {
            characterName.enabled = true;
            if(aiCharacter != null)
            {
                characterName.text = aiCharacter.characterName;
            }
            if(playerCharacter != null)
            {
                characterName.text = playerCharacter.playerNetworkManager.characterName.Value.ToString();
            }

        }

        // call this here incase max health changes from a character effect/buuf ect
        slider.maxValue = character.characterNetworkManager.maxHealth.Value;

        // to do tun secondary bar logic (yellow bat that appears behind hp when damaged)

        // total the damage taken whilst the bar is active
        currentDamageTaken = Mathf.RoundToInt(currentDamageTaken + (oldHealthValue - newValue));

        if(currentDamageTaken < 0)
        {
            currentDamageTaken = Mathf.Abs(currentDamageTaken);
            characterDamage.text = "+ " + currentDamageTaken.ToString();
        }
        else
        {
            characterDamage.text = "- " + currentDamageTaken.ToString();
        }

        slider.value = newValue;

        if(character.characterNetworkManager.currentHealth.Value != character.characterNetworkManager.maxHealth.Value)
        {
            hideTimer = deaultTimeBeforeBarHides;
            gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        transform.LookAt(transform.position + Camera.main.transform.forward);

        if(hideTimer > 0)
        {
            hideTimer -= Time.deltaTime;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        
    }



}
