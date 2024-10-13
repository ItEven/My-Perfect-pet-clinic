using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;

public static class CameraExtension
{
    public static Vector2 GetCameraSizeInUnityUnit(this Camera cam)
    {
        float height = cam.orthographicSize * 2;
        float width = height * cam.aspect;
        Vector2 camSize = new Vector2(width, height);
        return camSize;
    }

    public static void SetCameraWidthUnit(this Camera cam, float width)
    {
        float height = width / cam.aspect;
        cam.orthographicSize = height / 2;
    }

    public static void SetCameraHeightUnit(this Camera cam, float height)
    {
        cam.orthographicSize = height / 2;
    }

    public static float GetFrustumHeight(this Camera cam, float distance)
    {
        var frustumHeight = 2.0f * distance * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        return frustumHeight;
    }

    public static Vector2 GetFrustumSize(this Camera cam, float distance)
    {
        float height = GetFrustumHeight(cam, distance);
        return new Vector2(GetFrustumWidth(cam, distance), height);
    }

    public static float GetFrustumWidth(this Camera cam, float frustumHeight)
    {
        var frustumWidth = frustumHeight * cam.aspect;
        return frustumWidth;
    }

    public static float GetDisatance(this Camera cam, float frustumHeight)
    {
        var distance = frustumHeight * 0.5f / Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        return distance;
    }

    public static float GetFOV(this Camera cam, float frustumHeight, float distance)
    {
        var fieldOfView = 2.0f * Mathf.Atan(frustumHeight * 0.5f / distance) * Mathf.Rad2Deg;
        return fieldOfView;
    }

    //public static Rect GetViewpPortRect(this Camera cam, float distance)
    //{
    //    float heigth = GetFrustumHeight(cam, distance);
    //    float width = GetFrustumWidth(cam, distance);
    //    Vector2 pos = cam.transform.position;
    //    return new Rect(cam.transform.localPosition - new Vector3(width, heigth, 0) * 0.5f, new Vector2(width, heigth));
    //}
}
