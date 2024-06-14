using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Mathematics;

public class FogWallInteractable : Interactable
{
    [Header("Fog")]
    [SerializeField] GameObject[] fogGameObjects;

    [Header("Collision")]
    [SerializeField] Collider fogWallCollider;

    [Header("I.D")]
    public int fogWallID;

    [Header("Sound")]
    private AudioSource fogWallAudioSource;
    [SerializeField] AudioClip fogWallSFX;

    [Header("Active")]
    public NetworkVariable<bool> isActive = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    protected override void Awake()
    {
        base.Awake();

        fogWallAudioSource = gameObject.GetComponent<AudioSource>();
    }

    public override void Interact(PlayerManager player)
    {
        base.Interact(player);

        // 1 face the fog wall
       // Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward);
       // player.transform.rotation = targetRotation;

        // disable collision with the fog wall (on all cients so client on one players screen doesint get stuck and then teleport in on resync)
        AllowPlayerTroughWallCollidersServerRpc(player.NetworkObjectId);
        // walk trough fog wall
        player.playerAnimatorManager.PlayTargetActionAnimation("Pass_Trough_Fog_01", true);
        // reanable collision with fog wall
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        OnIsActiveChanged(false, isActive.Value);
        isActive.OnValueChanged += OnIsActiveChanged;
        WorldObjectManager.instance.AddFogWallToList(this);

    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        isActive.OnValueChanged -= OnIsActiveChanged;
        WorldObjectManager.instance.RemoveFogWallFromList(this);
    }

    private void OnIsActiveChanged(bool oldStatus, bool newStatus)
    {
        if(isActive.Value)
        {
            foreach(var fogObject in fogGameObjects)
            {
                fogObject.SetActive(true);
            }
            
        }
        else
        {
            foreach (var gameObject in fogGameObjects)
            {
                gameObject.SetActive(false);
            }
        }
    }

    // when a server rpc does not require ownership a now owner can activate the funcion (client player does not own fog wall as they are not the host)
    [ServerRpc(RequireOwnership = false)]

    private void AllowPlayerTroughWallCollidersServerRpc(ulong playerObjectID)
    {
        if (IsServer)
        {
            AllowPlayerTroughWallCollidersClientRpc(playerObjectID);
        }
    }

    [ClientRpc]
    private void AllowPlayerTroughWallCollidersClientRpc(ulong playerObjectID)
    {
        PlayerManager player = NetworkManager.Singleton.SpawnManager.SpawnedObjects[playerObjectID].GetComponent<PlayerManager>();

        fogWallAudioSource.PlayOneShot(fogWallSFX);

        if(player != null)
        {
            StartCoroutine(DisableCollisionForTime(player));
        }
    }

    private IEnumerator DisableCollisionForTime(PlayerManager player)
    {
        // make this function the same time as the walking trough fog wall animation lenght
        Physics.IgnoreCollision(player.characterController, fogWallCollider, true);

        yield return new WaitForSeconds(4);
        Physics.IgnoreCollision(player.characterController, fogWallCollider, false);
    }
}
