using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FPCharcaterController : MonoBehaviour
{
    private CharacterController characterController; // 角色控制器组件
    private Transform characterTransform; // 角色的Transform组件
    public float SprintingSpeed; // 奔跑速度
    public float WalkSpeed; // 行走速度
    private float Gravity = 9.8f; // 重力常数
    private Vector3 movementDirection; // 移动方向
    public float JumpHeight; // 跳跃高度
    public float CrouchHeight = 1f; // 蹲伏高度
    public bool isCrouches = false; // 是否在蹲伏状态
    public float OriginHeight; // 角色的原始高度
    public float sprintingSpeedWhenCrouched; // 蹲伏时的奔跑速度
    public float WalkSpeedWhenCrouched; // 蹲伏时的行走速度
    public Animator animator; // 动画控制器
    int speedID; // 动画速度参数的Hash ID
    float animSpeed = 0; // 当前动画速度
    bool Onoroff = false; // 瞄准状态
    float aiming = 0; // 瞄准动画参数
    public GameObject crosshair;

    private void Start()
    {
        // 初始化组件
        characterController = GetComponent<CharacterController>();
        characterTransform = transform;
        OriginHeight = characterController.height; // 获取角色的初始高度
        speedID = Animator.StringToHash("Movement"); // 获取动画参数的Hash ID
    }

    private void Update()
    {
        float tmp_CurrentSpeed = WalkSpeed; // 默认行走速度
        if (characterController.isGrounded) // 检查是否在地面上
        {
            var tmp_horizontal = Input.GetAxis("Horizontal"); // 获取水平输入
            var tmp_vertical = Input.GetAxis("Vertical"); // 获取垂直输入

            // 计算移动方向
            movementDirection = characterTransform.TransformDirection(new Vector3(tmp_horizontal, 0, tmp_vertical));

            if (Input.GetKeyDown(KeyCode.Space)) // 检查跳跃输入
                movementDirection.y = JumpHeight; // 设置跳跃高度
            if (Input.GetKeyDown(KeyCode.C)) // 检查蹲伏输入
            {
                var tmp_CurrentHeight = isCrouches ? OriginHeight : CrouchHeight; // 切换高度
                StartCoroutine(DoCrouch(tmp_CurrentHeight)); // 启动蹲伏协程
                isCrouches = !isCrouches; // 切换蹲伏状态
            }
            if (Input.GetKey(KeyCode.LeftShift)) // 检查奔跑输入
            {
                animSpeed = 2; // 设置奔跑动画速度
                animator.SetFloat(speedID, animSpeed); // 更新动画速度
            }
            else if (tmp_horizontal != 0 || tmp_vertical != 0) // 检查移动输入
            {
                animSpeed = 0.5f; // 设置行走动画速度
                animator.SetFloat(speedID, animSpeed); // 更新动画速度
            }
            else
            {
                animSpeed = 0; // 设置静止动画速度
                animator.SetFloat(speedID, 0); // 更新动画速度
            }
            if (isCrouches) // 检查是否在蹲伏状态
            {
                // 设置蹲伏状态下的速度
                tmp_CurrentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintingSpeedWhenCrouched : WalkSpeedWhenCrouched;
            }
            else
            {
                // 设置正常状态下的速度
                tmp_CurrentSpeed = Input.GetKey(KeyCode.LeftShift) ? SprintingSpeed : WalkSpeed;
            }
            if (Input.GetMouseButtonDown(1)) // 检查瞄准输入
            {
                animator.SetFloat(speedID, 0); // 重置动画速度
                Onoroff = !Onoroff; // 切换瞄准状态
                aiming = Onoroff ? 1 : 0; // 更新瞄准动画参数
                crosshair.SetActive(!crosshair.activeSelf);
            }
        }
        movementDirection.y -= Gravity * Time.deltaTime; // 应用重力
        characterController.Move(tmp_CurrentSpeed * Time.deltaTime * movementDirection); // 移动角色
        animator.SetFloat("Aiming", aiming); // 更新瞄准动画参数
        //characterController.SimpleMove(tmp_MovemenDirection*Time.deltaTime*MovemenSpeed);
    }

    private IEnumerator DoCrouch(float target)
    {
        float tmp_CurrentHeight = 0f;
        while(Mathf.Abs(characterController.height-target)>0.1f)
        {
            yield return null;
            characterController.height = 
                Mathf.SmoothDamp(characterController.height, target, ref tmp_CurrentHeight,Time.deltaTime*5);
        }
    }
}
