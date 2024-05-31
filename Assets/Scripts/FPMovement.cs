using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Playables;

public class FPMovement : MonoBehaviour
{
    public float gravity; // 重力值
    private Transform characterTransform; // 角色的变换组件
    private Rigidbody characterRigidbody; // 角色的刚体组件
    public float Speed; // 移动速度
    public float JumpHeight; // 跳跃高度
    private bool isGrounded; // 是否在地面上

    // Start is called before the first frame update
    void Start()
    {
        // 获取角色的变换组件
        characterTransform = transform;
        // 获取角色的刚体组件
        characterRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // 空的Update方法，主要用于响应每帧的输入（如果需要的话）
    }

    // FixedUpdate is called once per physics update
    private void FixedUpdate()
    {
        if (isGrounded)
        {
            // 获取水平和垂直输入
            var tmp_horizontal = Input.GetAxis("Horizontal");
            var tmp_vertical = Input.GetAxis("Vertical");

            // 计算当前移动方向
            var tmp_CurrentDirection = new Vector3(tmp_horizontal, 0, tmp_vertical);
            tmp_CurrentDirection = characterTransform.TransformDirection(tmp_CurrentDirection);
            tmp_CurrentDirection *= Speed;

            // 获取当前速度和速度变化量
            var tmp_CurrentVelocity = characterRigidbody.velocity;
            var tmp_VelocityChange = tmp_CurrentDirection - tmp_CurrentVelocity;
            tmp_VelocityChange.y = 0;

            // 添加力以改变速度
            characterRigidbody.AddForce(tmp_VelocityChange, ForceMode.VelocityChange);

            // 检查跳跃输入
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Jumping");
                characterRigidbody.velocity = new Vector3(tmp_CurrentVelocity.x, CalculateJumpHeightSpeed(), tmp_CurrentVelocity.z);
            }
        }
        // 应用重力
        characterRigidbody.AddForce(new Vector3(0, -gravity * characterRigidbody.mass, 0));
    }

    // 计算跳跃速度
    private float CalculateJumpHeightSpeed()
    {
        return Mathf.Sqrt(2 * gravity * JumpHeight);
    }

    // 检测角色是否接触地面
    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    // 检测角色是否离开地面
    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}
