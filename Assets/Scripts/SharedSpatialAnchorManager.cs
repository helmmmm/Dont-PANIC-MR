using Meta.XR.BuildingBlocks;
using Oculus.Platform;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using Oculus.Platform.Models;
using UnityEngine;
using System;
using static UnityEngine.Rendering.DebugUI.Table;

public class SharedSpatialAnchorManager : NetworkBehaviour
{
    [SerializeField] private SharedSpatialAnchorCore _sharedSpatialAnchor;
    [SerializeField] private Transform _hostTransform;
    [SerializeField] private GameObject _spatialAnchorPrefab;

    private List<OVRSpatialAnchor> _createdAnchors = new List<OVRSpatialAnchor>();
    public List<OVRSpaceUser> _ovrUsers = new List<OVRSpaceUser>();
    //public NetworkVariable<List<OVRSpaceUser>> _ovrUsersNetworkVariable = new NetworkVariable<List<OVRSpaceUser>>(new List<OVRSpaceUser>());

    private NetworkManagerUI _networkManagerUI;


    private void Start()
    {
        _hostTransform = Camera.main.transform;
        _networkManagerUI = FindObjectOfType<NetworkManagerUI>();
    }


    public void CreateAnchor()
    {
        // instantiate a spatial anchor 0.5f in front of the host camera
        _sharedSpatialAnchor.InstantiateSpatialAnchor(_spatialAnchorPrefab, _hostTransform.position + new Vector3(0, 0, 0.5f), Quaternion.identity);

        // add the instantiated spatial anchor to the create anchors list        
        foreach (OVRSpatialAnchor anchor in FindObjectsOfType<OVRSpatialAnchor>())
        {
            if (_createdAnchors.Contains(anchor))
            {
                continue;
            }
            _createdAnchors.Add(anchor);
            Debug.Log("Anchors: " + _createdAnchors);
        }
    }

    private List<OVRSpaceUser> GetOVRSpaceUsers()
    {
        Users.GetLoggedInUser().OnComplete(message => 
        { 
            if (message == null)
            {
                Debug.LogError("Message is null");
            }
            else if (message.GetUser() == null)
            {
                Debug.LogError("GetUser is null");
            }
            else if (message.GetUser().ID == null)
            {
                Debug.LogError("User ID is null");
            }
            else
            {
                OVRSpaceUser user = new OVRSpaceUser(message.GetUser().ID);
                Debug.Log("[DPM] GetLoggedInUser userId: " + user.Id);
                _ovrUsers.Add(user);
            }
        });
        return _ovrUsers;
    }

    IEnumerator ShareAnchors()
    {
        yield return new WaitForSeconds(0.5f);
        _sharedSpatialAnchor.ShareSpatialAnchors(_createdAnchors, _ovrUsers);
    }
        
    public void LoadAnchors()
    {
        _sharedSpatialAnchor.LoadAndInstantiateAnchors(_spatialAnchorPrefab, _createdAnchors.Select(anchor => anchor.Uuid).ToList());
        Debug.Log("[DPM] Anchors Loaded: " + _createdAnchors.Count);
    }

    public void OnAnchorCreated(OVRSpatialAnchor _, OVRSpatialAnchor.OperationResult result)
    {
        if (result == OVRSpatialAnchor.OperationResult.Success)
        {
            GetOVRSpaceUsers();
            StartCoroutine(ShareAnchors());
            Debug.Log("[DPM] Anchor Created Successfully");
        }
    }

    public void OnAnchorShared(List<OVRSpatialAnchor> _, OVRSpatialAnchor.OperationResult result)
    {
        if (result == OVRSpatialAnchor.OperationResult.Success)
        {
            Debug.Log("[DPM] Anchor Shared Successfully");
            LoadAnchors();
        }
        else
        {
            LogWarning($"Failed to share the spatial anchor. {result}");
            Debug.Log($"Failed to share the spatial anchor. {result}");
        }
    }

    private void LogWarning(string msg)
    {
        AlertViewHUD.PostMessage(msg, AlertViewHUD.MessageType.Error);
        Debug.LogWarning($"[{nameof(SharedSpatialAnchorErrorHandler)}] {msg}");
    }
}




