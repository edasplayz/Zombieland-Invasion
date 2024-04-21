using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class AIBossCharacterManager : AICharacterManager
{
    // GIVE THIS A,I a unique if
    public int bossID = 0;

    [Header("Music")]
    [SerializeField] AudioClip bossIntroClip;
    [SerializeField] AudioClip bossBattleLoopClip;

    [Header("Status")]
    public NetworkVariable<bool> bossFightIsActive = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> hasBeenDefeated = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<bool> hasBeenAwakened = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    [SerializeField] List<FogWallInteractable> fogWalls;
    [SerializeField] string sleepAnimation;
    [SerializeField] string awakenAnimation;

    [Header("Phase Shift")]
    public float minimumHealthProcentageToShift = 50;
    [SerializeField] string phaseShiftAnimation = "Phase_Change_01";
    [SerializeField] CombatStanceState phase02CombatStanceState;

    [Header("States")]
    [SerializeField] BossSleepState sleepState;

    // when this ai is spawned check our save file (dictonary)
    // if the save file does not contain a boss monster with this i.d add it
    // if it is present cheeck if boss has been defeted
    // if the boss has been defeted disable the game object
    // if the boss has not been defeted allow this object to continue to be active

    protected override void Awake()
    {
        base.Awake();

        
        
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        bossFightIsActive.OnValueChanged += OnBossFightIsActiveChanged;
        OnBossFightIsActiveChanged(false, bossFightIsActive.Value); // so if you join when the fight is already active you will get a hp bar

        if (IsOwner)
        {
            sleepState = Instantiate(sleepState);
            currentState = sleepState;
        }

        // if this is the host world
        if (IsServer)
        {
            // if our save data does not contain information on this boss add it now 
            if(!WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
            {
                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, false);
                WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Add(bossID, false);
            }
            // otherwise load the data that already exist on this boss
            else
            {
                hasBeenDefeated.Value = WorldSaveGameManager.instance.currentCharacterData.bossesDefeated[bossID];
                hasBeenAwakened.Value = WorldSaveGameManager.instance.currentCharacterData.bossesAwakened[bossID];

                
            }

            // locate fog wall
            StartCoroutine(GetFogWallsFromWorldObjectManager());

            // if boss has been awakened enable the fog walls

            if (hasBeenAwakened.Value)
            {
                for (int i = 0; i < fogWalls.Count; i++)
                {
                    fogWalls[i].isActive.Value = true;
                }
            }

            // if the bos has been defeted disable fog walls
            if (hasBeenDefeated.Value)
            {
                for (int i = 0; i < fogWalls.Count; i++)
                {
                    fogWalls[i].isActive.Value = false;
                }
                aICharacterNetworkManager.isActive.Value = false;
            }


        }

        if (!hasBeenAwakened.Value)
        {
            characterAnimatorManager.PlayTargetActionAnimation(sleepAnimation, true);
        }
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        bossFightIsActive.OnValueChanged -= OnBossFightIsActiveChanged;
    }

    private IEnumerator GetFogWallsFromWorldObjectManager()
    {
        while (WorldObjectManager.instance.fogWalls.Count == 0)
        {
            yield return new WaitForEndOfFrame();
        }

        // locate fog wall
        // you can ether share the same ID for boss and fog wall or simply place a fogwall ID cariable here on look for it using that

        fogWalls = new List<FogWallInteractable>();

        // method 1
        foreach (var fogWall in WorldObjectManager.instance.fogWalls)
        {
            if (fogWall.fogWallID == bossID)
            {
                fogWalls.Add(fogWall);
            }
        }
    }

    public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
    {

        PlayerUiManager.instance.playerUiPopUpManager.SendBossDefeatedPopUp("Great foa felled");
        if (IsOwner)
        {
            characterNetworkManager.currentHealth.Value = 0;
            isDead.Value = true;

            bossFightIsActive.Value = false;

            foreach (var fogwall in fogWalls)
            {
                fogwall.isActive.Value = false;
            }

            // reset any flags here that need to be reset 
            // nothing yet

            // if we are not grounded, play an aerial death animation

            if (!manuallySelectDeathAnimation)
            {
                characterAnimatorManager.PlayTargetActionAnimation("Dead_01", true);
            }

            hasBeenDefeated.Value = true;

            // if our save data does not contain information on this boss add it now 
            if (!WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
            {
                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
                WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Add(bossID, true);
            }
            // otherwise load the data that already exist on this boss
            else
            {
                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Remove(bossID);
                WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Remove(bossID);
                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);
                WorldSaveGameManager.instance.currentCharacterData.bossesDefeated.Add(bossID, true);

            }

            WorldSaveGameManager.instance.SaveGame();
        }

        

        // play some death sfx

        yield return new WaitForSeconds(5);

        // award players with runes

        // disable character 
    }

    public void WakeBoss()
    {
        if(IsOwner)
        {

            if (!hasBeenAwakened.Value)
            {

                characterAnimatorManager.PlayTargetActionAnimation(awakenAnimation, true);
            }
            bossFightIsActive.Value = true;
            hasBeenAwakened.Value = true;
            currentState = idle;
            // if our save data does not contain information on this boss add it now 
            if (!WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
            {
                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);

            }
            // otherwise load the data that already exist on this boss
            else
            {
                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Remove(bossID);

                WorldSaveGameManager.instance.currentCharacterData.bossesAwakened.Add(bossID, true);


            }

            for (int i = 0; i < fogWalls.Count; i++)
            {
                fogWalls[i].isActive.Value = true;
            }

            
        }

        

    }

    private void OnBossFightIsActiveChanged(bool oldStatus, bool newStatus)
    {
        if (bossFightIsActive.Value)
        {
            WorldSoundFXManager.instance.PlayBossTrack(bossIntroClip, bossBattleLoopClip);

            GameObject bossHealthBar = Instantiate(PlayerUiManager.instance.playerUIHudManager.bossHealthBarObject, PlayerUiManager.instance.playerUIHudManager.bossHealthBarParent);

            UI_Boss_HP_Bar bossHPBar = bossHealthBar.GetComponentInChildren<UI_Boss_HP_Bar>();
            bossHPBar.EnableBossHPBar(this);
        }
        else
        {
            WorldSoundFXManager.instance.StopBossMusic();
        }

        
    }

    public void PhaseShift()
    {
        characterAnimatorManager.PlayTargetActionAnimation(phaseShiftAnimation, true);
        combatStance = Instantiate(phase02CombatStanceState);
        currentState = combatStance;
    }
}

