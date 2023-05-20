using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using Assets.HeroEditor4D.Common.Scripts.Enums;

public class Enemy : MonoBehaviour
{
    public AIPath aiPath;

    [SerializeField] protected Character4D _character;
    [SerializeField] protected AnimationManager _animation;

    private void Update()
    {
        Vector2 direction = aiPath.desiredVelocity.normalized;

        if (direction.magnitude > 0.01f)
        {
            if (direction.x >= 0.01f)
            {
                _character.SetDirection(Vector2.right);
            }
            else if (direction.x <= -0.01f)
            {
                _character.SetDirection(Vector2.left);
            }
            else if (direction.y >= 0.01f)
            {
                _character.SetDirection(Vector2.up);
            }
            else if (direction.y <= -0.01f)
            {
                _character.SetDirection(Vector2.down);
            }

            _animation.SetState(CharacterState.Run);
        }
        else
        {
            _animation.SetState(CharacterState.Idle);
        }
    }
}
