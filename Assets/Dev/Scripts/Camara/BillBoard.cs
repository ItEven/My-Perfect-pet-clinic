using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BillBoard : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    void Start() {
        if (mainCamera == null) {
            mainCamera = Camera.main;
        }
    }
    void LateUpdate() {
        transform.LookAt(mainCamera.transform);
        transform.Rotate(0, 180, 0);
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (mainCamera == null) {
            mainCamera = Camera.main;
        }
    }
    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


}
