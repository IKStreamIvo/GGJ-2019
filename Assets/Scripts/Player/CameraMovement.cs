using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
    [SerializeField] private Transform target;
    [SerializeField] private float followSpeed;
    [SerializeField] private Vector2 followOffset;

    private void FixedUpdate() {
        Vector3 followPos = target.position;
        followPos += (Vector3)followOffset;
        followPos.z = transform.position.z;
        transform.position = Vector3.MoveTowards(transform.position, followPos, followSpeed * Time.fixedDeltaTime);    
    }
}
