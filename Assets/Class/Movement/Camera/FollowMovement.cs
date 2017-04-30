using UnityEngine;
using System.Collections;
using DG.Tweening;

public class FollowMovement : MonoBehaviour
{
    public Camera camera;
    public Transform sourceTransform;
    public Transform targetTransform;

    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (sourceTransform)
        {
            Vector3 point = camera.WorldToViewportPoint(sourceTransform.position);
            Vector3 offset = sourceTransform.position - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));

            Vector3 destination = targetTransform.position + offset;
            targetTransform.position = Vector3.SmoothDamp(targetTransform.position, destination, ref velocity, dampTime);
        }
    }
}
