using UnityEngine;
using System.Collections;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
public class FowardMovement : MonoBehaviour
{
    public float speed = 3.0f;

    private enum Mode { none, move, rotate };
    private Mode mode = Mode.none;

    private Vector3 moveValue = Vector3.forward;

    private Vector3 rotateValue = new Vector3(0, -90, 0);

    void OnEnable()
    {
        DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
    }

    // Use this for initialization
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        switch (mode)
        {
            case Mode.none:
                break;
            case Mode.move:
                transform.Translate(moveValue * Time.deltaTime * speed);
                break;
            case Mode.rotate:
                break;
            default:
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "UnPassable" && mode == Mode.move)
        {
            mode = Mode.rotate;
            rotateValue += new Vector3(0, 180, 0);
            StartCoroutine("Rotate");
        }
        else if (mode == Mode.none)
        {
            mode = Mode.move;
        }
    }

    IEnumerator Rotate()
    {
        Tween tween = transform.DORotate(rotateValue, 0.2f);
        yield return tween.WaitForCompletion();
        mode = Mode.move;
    }
}
