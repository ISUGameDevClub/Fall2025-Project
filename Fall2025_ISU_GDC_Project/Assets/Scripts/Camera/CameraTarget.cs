using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] private GameObject mainCamObj;
    [SerializeField] private float groupFramingOffsetY;
    private CinemachineGroupFraming groupFraming;
    private CinemachineTargetGroup targetGroup;
    private CinemachineCamera mainCam;

    private void Start()
    {
        targetGroup = GetComponent<CinemachineTargetGroup>();
        groupFraming = mainCamObj.GetComponent<CinemachineGroupFraming>();
        mainCam = mainCamObj.GetComponent<CinemachineCamera>();

        groupFraming.CenterOffset.y = groupFramingOffsetY;
    }

    public void OnPlayerJoined(PlayerInput pi)
    {
/*        CinemachineTargetGroup.Target target = new CinemachineTargetGroup.Target();
        target.Object = pi.gameObject.transform;
        targetGroup.Targets.Add(target);

        //if theres only one player, disable the group framing, and set target to only this player
        if (targetGroup.Targets.Count == 1)
        {
            groupFraming.enabled = false;
            Unity.Cinemachine.CameraTarget targ = new Unity.Cinemachine.CameraTarget();
            targ.TrackingTarget = targetGroup.Targets[0].Object;
            mainCam.Target = targ;
        }
        else
        {
            //set target to group, enable group framing
            groupFraming.enabled = true;
            Unity.Cinemachine.CameraTarget targ = new Unity.Cinemachine.CameraTarget();
            targ.TrackingTarget = targetGroup.Transform;
            mainCam.Target = targ;
        }*/
    }

    public void SetCameraTargets(List<GameObject> playerObjects)
    {
        foreach (GameObject playerObj in playerObjects)
        {
            CinemachineTargetGroup.Target target = new CinemachineTargetGroup.Target();
            target.Object = playerObj.transform;
            targetGroup.Targets.Add(target);

            //if theres only one player, disable the group framing, and set target to only this player
            if (targetGroup.Targets.Count == 1)
            {
                groupFraming.enabled = false;
                Unity.Cinemachine.CameraTarget targ = new Unity.Cinemachine.CameraTarget();
                targ.TrackingTarget = targetGroup.Targets[0].Object;
                mainCam.Target = targ;
            }
            else
            {
                //set target to group, enable group framing
                groupFraming.enabled = true;
                Unity.Cinemachine.CameraTarget targ = new Unity.Cinemachine.CameraTarget();
                targ.TrackingTarget = targetGroup.Transform;
                mainCam.Target = targ;
            }
        }
    }
}
