using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackPartController : MonoBehaviour
{

    private Rigidbody rigidbody;
    private MeshRenderer meshRenderer;
    private StackController stackController;
    private Collider collider;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        stackController = transform.parent.GetComponent<StackController>();
        collider = GetComponent<Collider>();
    }

    public void Shatter()
    {
        rigidbody.isKinematic = false;
        collider.enabled = false;

        Vector3 forcePoint = transform.parent.position;
        float paretXpos = transform.parent.position.x;
        float xPos = meshRenderer.bounds.center.x;

        Vector3 subDir = (paretXpos - xPos < 0) ? Vector3.right : Vector3.left;
        Vector3 dir = (Vector3.up * 1.5f + subDir).normalized;

        float force = Random.Range(20, 35);
        float torque = Random.Range(110, 180);

        rigidbody.AddForceAtPosition(dir * force, forcePoint, ForceMode.Impulse);
        rigidbody.AddTorque(Vector3.left * torque);
        rigidbody.velocity = Vector3.down;
    }

    public void RemoveAllChilds()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).SetParent(null);
            i--;
        }
    }
  
}
