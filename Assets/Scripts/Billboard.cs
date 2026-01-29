using UnityEngine;

public class Billboard : MonoBehaviour
{
    Camera mainCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       mainCamera = Camera.main; 
    }

    // Update is called once per frame
    void LateUpdate() //called after Cinemachine
    {
        if(mainCamera == null) return;
        transform.LookAt(transform.position + mainCamera.transform.forward, mainCamera.transform.up);
    }
}
