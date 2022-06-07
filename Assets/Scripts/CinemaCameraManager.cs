using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemaCameraManager : MonoBehaviour
{

    InputManager inputManager;
    [SerializeField] CinemachineVirtualCamera regularCamera;
    [SerializeField] CinemachineVirtualCamera aimCamera;
    CinemachineBasicMultiChannelPerlin channelPerlin;
    AimCrossHair aimCrossHair;
    public LayerMask aimColladerMask;

    [SerializeField] Transform playerTransform;
    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public GameObject CinemachineCameraTarget;

    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 70.0f;

    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -30.0f;

    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float CameraAngleOverride = 0.0f;

    [Tooltip("For locking the camera position on all axis")]
    public bool LockCameraPosition = false;


    public float cameraLookSpeed = 1f;
    public float cameraPivotSpeed = 1.5f;

    // cinemachine
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    private const float _threshold = 0.01f;

    public bool isAimMode = false;

    public float intensity = 5f;
    public float ShakeTime = .1f;

    private void Awake()
    {
        aimCrossHair = FindObjectOfType<AimCrossHair>();
        inputManager = FindObjectOfType<InputManager>();
    }
    private void Start()
    {
        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
    }

    public void CameraRotation()
    {
        if (inputManager.sqrMagnitude >= _threshold)
        {
            _cinemachineTargetYaw += inputManager.cameraInputX * cameraLookSpeed;
            _cinemachineTargetPitch += inputManager.cameraInputY * cameraPivotSpeed;
        }


        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
    }

    public void AimMode(bool Active)
    {
        
        aimCamera.gameObject.SetActive(Active);
        if (Active)
        {
            isAimMode = true;
            aimCrossHair.Enable();
            cameraLookSpeed = .5f;
            cameraPivotSpeed = 1f;
            Vector3 mouseWorldPosition = Vector3.zero;
            Vector2 screenCenterPoint = new Vector2(Screen.width/2f,Screen.height/2f);
            Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
            if(Physics.Raycast(ray,out RaycastHit raycastHit,999f,aimColladerMask)){
                //transform.position = raycastHit.point;
                mouseWorldPosition = raycastHit.point;
                inputManager.mouseWorldPosition = mouseWorldPosition;
            }
        }
        else
        {
            isAimMode = false;
            aimCrossHair.Disable();
            cameraLookSpeed = 1f;
            cameraPivotSpeed = 1.5f;
        }
    }
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }


    public void ShakeCamera(){
        if(isAimMode){
            channelPerlin = aimCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
          
        }else{
           channelPerlin =   regularCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
        channelPerlin.m_AmplitudeGain = intensity;
        
       StartCoroutine(Shake());
    }

     IEnumerator Shake()
    {
        yield return new WaitForSeconds(ShakeTime);
        channelPerlin.m_AmplitudeGain = 0f;
    }
}

