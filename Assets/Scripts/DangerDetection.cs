using System;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class DangerDetection : MonoBehaviour
{
    
   [SerializeField] private float rayRadius = 0.002f;
   [SerializeField] private float maxDistance = 10f;

   private int layerMaskNumber;

   private ObjectBehaviour currentTarget;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        layerMaskNumber = LayerMask.GetMask("Object");
    }

    // Update is called once per frame
    void Update()
    {
       DetectObject();
       
        if (Input.GetMouseButtonDown(0))
        {
            if (currentTarget != null)
                currentTarget.ValidateAnswer(true); // joueur dit "danger"
        }

       /* if (Input.GetMouseButtonDown(1))
        {
            if (currentTarget != null)
                currentTarget.ValidateAnswer(false); // joueur dit "pas danger"
        }*/
    }

    private void DetectObject()
    {
         RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward * maxDistance, Color.green);
        if(Physics.SphereCast(transform.position, rayRadius, transform.forward, out hit, maxDistance, layerMaskNumber))
        {
            //Debug.Log(hit.collider.gameObject.name);
            ObjectBehaviour obj = hit.collider.gameObject.GetComponent<ObjectBehaviour>();

            if(obj != null && obj != currentTarget)
            {
                ClearCurrentTarget(); //on a changer d'objet
                currentTarget = obj;
                currentTarget.Highlight(true);
            }
        }
    }

    private void ClearCurrentTarget()
    {
        if(currentTarget != null)
        {
            currentTarget.Highlight(false);
            currentTarget = null;
        }
    }
}
