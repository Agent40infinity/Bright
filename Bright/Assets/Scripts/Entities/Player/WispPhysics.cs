using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WispPhysics : MonoBehaviour
{
    public float moveSpeed;
    public float restSpeed;
    private float playerDistance;
    public float minFollow;
    public float maxFollow;

    public float shootDistance;
    public float shootSpeed;

    public bool gotTarget;
    public Vector2 target;

    public Player player;
    public GameObject parent;

    public ShootState shootState;

    public void Start()
    {
        transform.parent = null;
    }

    public void Follow()
    {
        playerDistance = Vector2.Distance(transform.position, player.transform.position);

        if (playerDistance > maxFollow)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }
        else if (playerDistance <= maxFollow && playerDistance > minFollow)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, restSpeed * Time.deltaTime);
        }
    }

    public void ShootOut(Vector2 direction)
    {
        Vector2 position = transform.position;

        switch (shootState)
        {
            case ShootState.Out:
                switch (gotTarget)
                {
                    case false:
                        Vector2 playerPosition = player.transform.position;
                        target = playerPosition + direction * shootDistance;
                        gotTarget = true;
                        break;
                }
                
                transform.position = Vector2.MoveTowards(transform.position, target, shootSpeed * Time.deltaTime);

                float distanceToTarget = Vector2.Distance(position, target);
                if (distanceToTarget <= 0.1f)
                {
                    shootState = ShootState.Return;
                }

                break;
            case ShootState.Return:
                Vector2 returnTarget = player.transform.position;
                transform.position = Vector2.MoveTowards(position, returnTarget, shootSpeed * Time.deltaTime);

                float distanceToReturn = Vector2.Distance(position, returnTarget);
                if (distanceToReturn <= 1)
                {
                    gotTarget = false;
                    shootState = ShootState.Out;
                    player.returnState = ReturnState.Idle;
                }
                break;
        }
    }
}

public enum ShootState
{ 
    Return,
    Out
}
