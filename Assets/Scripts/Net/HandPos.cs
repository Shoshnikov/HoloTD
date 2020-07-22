using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using Photon.Pun;
using System.Runtime.Versioning;

public class HandPos : MonoBehaviour
{
    private Vector3 userHandPos;
    private Quaternion userHandRot;
    [SerializeField] private PhotonView photonView;
    private void Awake()
    {
        InteractionManager.InteractionSourceUpdated += HandPosition;
    }

    private void HandPosition(InteractionSourceUpdatedEventArgs interactionSourceUpdatedEventArgs)
    {
        interactionSourceUpdatedEventArgs.state.sourcePose.TryGetPosition(out userHandPos);
        interactionSourceUpdatedEventArgs.state.sourcePose.TryGetRotation(out userHandRot);
        SendHand();
    }

    private void SendHand()
    {
        photonView.RPC("MessagePosition", RpcTarget.All, userHandPos.x, userHandPos.y, userHandPos.z);
        photonView.RPC("MessageRotation", RpcTarget.All, userHandRot.x, userHandRot.y, userHandRot.z);
    }

    [PunRPC] private void MessagePosition(float x, float y, float z)
    {
        Debug.Log($"Position {x} {y} {z}");
    }

    [PunRPC]
    private void MessageRotation(float x, float y, float z)
    {
        Debug.Log($"Rotation {x} {y} {z}");
    }
}
