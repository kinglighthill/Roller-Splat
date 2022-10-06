using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public Rigidbody rb;
    public float speed = 15;
    public int minSwipeRecognition = 500;

    public bool isMoving;
    public Vector3 moveDirection;
    public Vector3 nextCollisionPosition;
    public Vector2 swipePosLastFrame;
    public Vector2 swipePosCurrentFrame;
    public Vector2 currentSwipe;

    private Color solveColor;

    private void Start()
    {
        solveColor = Random.ColorHSV(0.5f, 1);
        GetComponent<MeshRenderer>().material.color = solveColor;
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            rb.velocity = speed * moveDirection;
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position - (Vector3.up / 2), 0.05f);
        int i = 0;
        while(i < hitColliders.Length)
        {
            GroundPieceController groundPiece = hitColliders[i].transform.GetComponent<GroundPieceController>();

            if (groundPiece && !groundPiece.isColoured)
            {
                groundPiece.ChangeColor(solveColor);
            }
            i++;
        }

        if (nextCollisionPosition != Vector3.zero)
        {
            if (Vector3.Distance(transform.position, nextCollisionPosition) < 1)
            {
                isMoving = false;
                moveDirection = Vector3.zero;
                nextCollisionPosition = Vector3.zero;
            }
        }

        if (isMoving) return;

        //float v = Input.GetAxis("Vertical");
        //float h = Input.GetAxis("Horizontal");

        //if (v > 0)
        //{
        //    SetDestination(Vector3.forward);
        //}

        //if (v < 0)
        //{
        //    SetDestination(Vector3.back);
        //}

        //if (h > 0)
        //{
        //    SetDestination(Vector3.right);
        //}

        //if (h < 0)
        //{
        //    SetDestination(Vector3.left);
        //}


        if (Input.GetMouseButton(0))
        {
            swipePosCurrentFrame = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Debug.Log(swipePosCurrentFrame.ToString());

            if (swipePosCurrentFrame != Vector2.zero)
            {
                currentSwipe = swipePosCurrentFrame - swipePosLastFrame;

                if (currentSwipe.sqrMagnitude < minSwipeRecognition)
                {
                    return;
                }

                currentSwipe.Normalize();

                float y = 0f;
                float x = 0f;

                if (currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
                    //y = currentSwipe.y;
                    SetDestination(currentSwipe.y > 0 ? Vector3.forward : Vector3.back);
                }

                if (currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    //x = currentSwipe.x;
                    SetDestination(currentSwipe.x > 0 ? Vector3.right : Vector3.left);
                }

                //SetDestination(Mathf.Abs(y) > Mathf.Abs(x) ? y > 0 ? Vector3.forward : Vector3.back : x > 0 ? Vector3.right : Vector3.left);

                swipePosLastFrame = swipePosCurrentFrame;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            swipePosLastFrame = Vector2.zero;
            currentSwipe = Vector2.zero;

        }
    }

    private void SetDestination(Vector3 direction)
    {
        moveDirection = direction;

        RaycastHit hit;

        if(Physics.Raycast(transform.position, direction, out hit, 100f))
        {
            nextCollisionPosition = hit.point;
        }

        isMoving = true;
    }
}
