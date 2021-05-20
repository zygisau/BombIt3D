using System;
using System.Collections.Generic;
using UnityEngine;

public class BombMovement : MonoBehaviour
{
    private Transform bombTransform;
    private Rigidbody bombRigidBody;
    private Vector3 direction;
    private Vector3 lastPosition;
    private float pushSpeed = 15f;
    private BombBehaviour bombBehaviour;
    private Bomb bomb;
    private bool shouldBombMove;
    private bool isBombPushed;

    void Awake()
    {
        bombTransform = gameObject.GetComponent<Transform>();
        bombRigidBody = gameObject.GetComponent<Rigidbody>();
        bombBehaviour = gameObject.GetComponent<BombBehaviour>();
        bomb = gameObject.GetComponent<Bomb>();
    }

    void OnEnable()
    {
        direction = Vector3.zero;
        lastPosition = Vector3.zero;
        shouldBombMove = false;
        isBombPushed = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && bombBehaviour.isBombPushable() && !isBombPushed)
        {
            moveBomb(other);
        }
    }

    private void moveBomb(Collider other)
    {
        direction = other.gameObject.GetComponent<PlayerMovement>().GetDirection();
        DrawArrow.ForDebug(bombTransform.position, direction, 5, Color.green);
        shouldBombMove = true;
        isBombPushed = true;
    }

    private Vector3 extractMaxVector(Vector3 vec)
    {
        int idx = 0;
        float max = vec[idx];
        for (int i = 1; i < 3; i++)
        {
            if (vec[i] > max)
            {
                idx = i;
                max = vec[idx];
            }
        }
        Vector3 newVec = Vector3.zero;
        newVec[idx] = max;
        return newVec;
    }

    void FixedUpdate()
    {
        if (!bomb.isBombExploded() && shouldBombMove)
        {
            lastPosition = bombTransform.position;
            Vector3 dir = direction * pushSpeed;
            bombRigidBody.AddForce(dir, ForceMode.Impulse);
        }
    }
}
