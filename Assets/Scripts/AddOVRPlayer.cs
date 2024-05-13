using Oculus.Platform;
using Oculus.Platform.Models;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using static Oculus.Platform.PlatformInternal;

public class AddOVRPlayer : NetworkBehaviour
{
    private SharedSpatialAnchorManager _sharedSpatialAnchorManager;
    private void Start()
    {
        Debug.Log("[DPM] AddOVRPlayer Start");


        _sharedSpatialAnchorManager = FindObjectOfType<SharedSpatialAnchorManager>();

        if (IsClient && !IsServer)
        {
            Oculus.Platform.Users.GetLoggedInUser().OnComplete((Message<Oculus.Platform.Models.User> message) =>
            {
                ulong userId = message.GetUser().ID;
                SendClientOVRUserIdServerRpc(userId);
                Debug.Log("[DPM] GetLoggedInUser userId: " + userId);
            });
        }
    }

    [ServerRpc]
    private void SendClientOVRUserIdServerRpc(ulong userId)
    {
        OVRSpaceUser user = new OVRSpaceUser(userId);
        _sharedSpatialAnchorManager._ovrUsers.Add(user);
        Debug.Log($"[DPM] ServerRPC called and added to list the userId: {userId}");
    }
}
