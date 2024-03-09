using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Burst;
using Unity.Jobs;
using Unity.Collections;
using System.Diagnostics;
using UnityEngine.Events;
public class Player : MonoBehaviour
{
    [Header("Requires")]
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private CharacterController carCon;
    [Header("Configs")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float cameraMoveSpeed;
    [SerializeField] private float cameraMoveSpeedWindows;

    [SerializeField] private float gravity;
    [SerializeField] private UnityEvent whenOnMove;


    private float rotateX;
    private bool isLook;
    private float3 velocity;
    public static Player Instance;
    public Mode mode;
    private void Awake()
    {
        Instance = this;
#if UNITY_EDITOR
        Cursor.lockState = CursorLockMode.Locked;
        isLook = true;
#elif UNITY_STANDALONE_WIN
isLook = true;
    Cursor.lockState = CursorLockMode.Locked;
#endif
    }
    public void CursorLockModeNone()
    {
#if UNITY_EDITOR
        isLook = false;
        Cursor.lockState = CursorLockMode.None;
#elif UNITY_STANDALONE_WIN
isLook = false;
    Cursor.lockState = CursorLockMode.None;
#endif
    }
    public void CursorLockModeLocked()
    {
#if UNITY_EDITOR
        Cursor.lockState = CursorLockMode.Locked;
        isLook = true;

#elif UNITY_STANDALONE_WIN
    Cursor.lockState = CursorLockMode.Locked;
    isLook = true;
#endif
    }
    public enum Mode
    {
        Free,
        PickCardBoard,
        Builder,
        Cashier
    }
    // NativeArray<float2> cartesius;
    // Update is called once per frame
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isLook)
            {
                Cursor.lockState = CursorLockMode.None;
                isLook = false;

            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                isLook = true;

            }

        }
        if (!carCon.isGrounded && mode != Mode.Builder)
        {
            velocity.y += -1f;
            velocity = carCon.isGrounded ? float3.zero : velocity;
            carCon.Move(velocity * Time.deltaTime * gravity);
        }
#if UNITY_EDITOR
        WindowsMove();

#elif UNITY_STANDALONE_WIN
    WindowsMove();
#elif UNITY_ANDROID
    AndroidMove();
#endif

    }
    private float2 Cartesius(float2 centerPos, float2 polar)
    {
        float centerX = centerPos.x;
        float centerY = centerPos.y;
        float radius = polar.x;
        float angleInDegree = polar.y; // dalam derajat

        float angleInRadian = angleInDegree * Mathf.Deg2Rad;
        float x = radius * math.cos(angleInRadian) + centerX;
        float y = radius * math.sin(angleInRadian) + centerY;
        return new float2(x, y);
    }
    public void Up()
    {

        Vector3 a = transform.position;
        transform.position = new Vector3(a.x, 3.7f, a.z);
    }
    public void Down()
    {

        if (transform.position.y > 2f)
        {
            Vector3 a = transform.position;
            transform.position = new Vector3(a.x, .2f, a.z);
        }
    }
    public void ChangeMode(PlayerChangeMode playerChangeMode)
    {
        mode = playerChangeMode.mode;
    }
    private void OnApplicationQuit()
    {
        DG.Tweening.DOTween.KillAll();
    }
    private void AndroidMove()
    {
        if (JoyStick.isMove)
        {

            // Stopwatch timer = new Stopwatch();
            // timer.Start();
            // cartesius = new NativeArray<float2>(1, Allocator.TempJob);
            float2 polar = new float2(walkSpeed * Time.deltaTime, JoyStick.degrees);
            float2 cartesius = Cartesius(Vector2.zero, polar);
            // CartesiusJob cartesiusJob = new CartesiusJob { centerPos = centerPos, polar = polar, result = cartesius };
            // JobHandle cartesiusHandle = cartesiusJob.Schedule();
            // cartesiusHandle.Complete();
            // transform.Translate(new Vector3(cartesius.x, 0f, cartesius.y), Space.Self);
            // from brackeys in youtube vvvv
            carCon.Move(transform.right * cartesius.x + transform.forward * cartesius.y);

            // cartesius.Dispose();
            // timer.Stop();
            // UnityEngine.Debug.Log(timer.ElapsedTicks);
            whenOnMove?.Invoke();
        }
        if (PlayerCameraController.isRotate)
        {
            rotateX += ((-1f * PlayerCameraController.delta.y) * cameraMoveSpeed) * Time.deltaTime;
            rotateX = math.clamp(rotateX, -90f, 90f);
            playerCamera.transform.localRotation = Quaternion.Euler(rotateX, playerCamera.transform.localRotation.y, playerCamera.transform.localRotation.z);
            transform.Rotate(((Vector3.up * PlayerCameraController.delta.x) * cameraMoveSpeed) * Time.deltaTime, Space.Self);
        }
    }
    private void WindowsMove()
    {

        float x = 0;
        float y = 0;
        bool isMove = false;
        int hitCount = 0;

        if (Input.GetKey(KeyCode.W))
        {
            y += walkSpeed;
            isMove = true;
            hitCount++;
        }
        if (Input.GetKey(KeyCode.S))
        {
            y -= walkSpeed;
            isMove = true;
            hitCount++;

        }
        if (Input.GetKey(KeyCode.A))
        {
            x -= walkSpeed;
            isMove = true;
            hitCount++;

        }
        if (Input.GetKey(KeyCode.D))
        {
            x += walkSpeed;
            isMove = true;
            hitCount++;

        }
        if (hitCount > 1)
        {
            x /= 2;
            y /= 2;
        }
        if (isMove)
        {
            Vector3 movepos = transform.right * x + transform.forward * y;

            carCon.Move(movepos * Time.deltaTime);
            isMove = false;
            whenOnMove?.Invoke();
        }
        float mouseX = Input.GetAxis("Mouse X") * cameraMoveSpeedWindows * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * cameraMoveSpeedWindows * Time.deltaTime;
        if (isLook)
        {
            rotateX -= mouseY;
            rotateX = math.clamp(rotateX, -90f, 90f);
            playerCamera.transform.localRotation = Quaternion.Euler(rotateX, playerCamera.transform.localRotation.y, playerCamera.transform.localRotation.z);
            transform.Rotate(Vector3.up * mouseX, Space.Self);
        }




    }

}
