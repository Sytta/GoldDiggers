using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraSimple : MonoBehaviour
{

    public bool lockCursor;
    public float mouseSensitivity = 10;
    public Transform Target;
    public float dstFromTarget = 1.5f;
    public Vector2 pitchMinMax = new Vector2(-10, 85);

    public float rotationSmoothTime = .12f;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    float yaw;
    float pitch;

    private Transform prevTransObject;
    [SerializeField] private Transform camera;
    [SerializeField] private float alphaValue = 0.1f; // our alpha value
    [SerializeField] private LayerMask transparentLayers;   // transparency layers.
    List<Transform> meshes = new List<Transform>();

    private bool stopMoving = false;

    void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void Update()
    {
        if (Target != null)
        {
            MakeObjectSemiTransparent();
        }
    }

    void LateUpdate()
    {
        if (Target == null) return;
        if (stopMoving) return;
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;

        transform.position = Target.position - transform.forward * dstFromTarget;

    }

   void MakeObjectSemiTransparent()
    {
        // Cast ray from camera.position to target.position and check if the specified layers are between them.
        Vector3 direction = (Target.position - camera.position).normalized;
        RaycastHit[] hits = Physics.RaycastAll(camera.position, direction, Mathf.Infinity);
        List<Transform> ms = new List<Transform>();
        foreach (RaycastHit hit in hits)
        {
            Transform objectHit = hit.transform;
            LayerMask layer = objectHit.gameObject.layer;

            if (transparentLayers == (transparentLayers | (1 << layer)))
            {
                if (objectHit.GetComponent<Renderer>() != null)
                {
                    Debug.Log("Putting " + objectHit + " to transparent");
                    Material[] mats = objectHit.GetComponent<Renderer>().materials;
                    ms.Add(objectHit);
                    foreach (Material mat in mats)
                    {   
                        mat.shader = Shader.Find("Hole Shader");
                    }
                }
            }
        }

        foreach (Transform t in meshes)
        {
            if (!ms.Contains(t))
            {
                Material[] mats = t.GetComponent<Renderer>().materials;
                foreach (Material mat in mats)
                {
                    mat.shader = Shader.Find("Standard");
                }
            }
        }

        meshes = ms;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            stopMoving = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            stopMoving = false;
        }
    }

}