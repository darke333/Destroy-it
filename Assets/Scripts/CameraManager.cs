using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    [HideInInspector]
    public Camera camera;

    [SerializeField] Transform LoadingCameraPos;
    [SerializeField] Transform ShootingCameraPos;
    public float ChangePosTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
    }

    public void ChangeCameraPos()
    {
        if (camera.transform.position == LoadingCameraPos.position && camera.transform.rotation == LoadingCameraPos.rotation)
        {
            MoveCamera(ShootingCameraPos);
            return;
        }

        if (camera.transform.position == ShootingCameraPos.position && camera.transform.rotation == ShootingCameraPos.rotation)
        {
            MoveCamera(LoadingCameraPos);
            return;
        }
    }

    void MoveCamera(Transform target)
    {
        float rotateSpeed = Quaternion.Angle(target.transform.rotation, target.transform.rotation) / ChangePosTime;
        float movingSpeed = Vector3.Distance(target.transform.position, target.transform.position) / ChangePosTime;

        camera.transform.DORotateQuaternion(target.transform.rotation, ChangePosTime);
        camera.transform.DOMove(target.transform.position, ChangePosTime);

        //camera.transform.position = target.transform.position;
        //camera.transform.rotation = target.transform.rotation;

    }

}
