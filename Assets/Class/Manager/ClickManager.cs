using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ClickManager : MonoBehaviour
{
    public Transform characterTransform;

    float clickableTime = 0f;
    void Awake()
    {
        clickableTime = Time.time;
    }

    // Use this for initialization
    void Start()
    {
        DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time > clickableTime)
        {
            // Construct a ray from the current mouse coordinates
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {
                float moveValue = 0;
                if (characterTransform.position.z < hit.point.z)
                {
                    moveValue = -3;
                }
                else
                {
                    moveValue = 3;
                }

                
                float targetPosZ = hit.collider.gameObject.transform.position.z + moveValue;

                hit.collider.gameObject.transform.DOMoveZ(targetPosZ, 0.5f).SetEase(Ease.InSine);
            }

            

            clickableTime = Time.time + 0.5f;
        }
    }
}
