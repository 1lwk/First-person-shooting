using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPMouseLook : MonoBehaviour
{
    private Transform cameraTransform; // 摄像机的变换组件
    [SerializeField] private Transform characterTransform; // 角色的变换组件
    private Vector3 cameraRotation; // 摄像机的旋转角度
    public float MouseSensitivity; // 鼠标灵敏度
    public Vector2 MaxMinAngle = new Vector2(-65, 65); // 摄像机垂直旋转的最大最小角度

    // Start is called before the first frame update
    void Start()
    {
        // 获取摄像机的变换组件
        cameraTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        // 获取鼠标输入
        var tmp_MouseX = Input.GetAxis("Mouse X");
        var tmp_MouseY = Input.GetAxis("Mouse Y");

        // 计算摄像机旋转角度
        cameraRotation.x -= tmp_MouseY * MouseSensitivity; // 垂直旋转（上下）
        cameraRotation.y += tmp_MouseX * MouseSensitivity; // 水平旋转（左右）

        // 限制垂直旋转角度在指定范围内
        cameraRotation.x = Mathf.Clamp(cameraRotation.x, MaxMinAngle.x, MaxMinAngle.y);

        // 设置角色和摄像机的旋转
        characterTransform.rotation = Quaternion.Euler(cameraRotation.x, cameraRotation.y, 0);
        cameraTransform.rotation = Quaternion.Euler(cameraRotation.x, cameraRotation.y, 0);
    }
}
