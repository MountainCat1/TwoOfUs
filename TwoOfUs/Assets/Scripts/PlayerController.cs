using System;
using Client;
using Server;
using UnityEngine;
using Zenject;

public class PlayerController : CreatureController
{
    [Inject] private IInputMapper _inputMapper;
    [Inject] private IInputManager _inputManager;

    [SerializeField] private SimpleAnimationController animationController;

    private void Start()
    {
        _inputManager.CharacterMovement += OnCharacterMovement;
        _inputManager.CharacterMovementChanged += OnCharacterMovementChanged;
    }

    private void OnCharacterMovementChanged(Vector2 move)
    {
        if (move.magnitude < 0.1f)
        {
            animationController.PlayIdle();
        }
        else
        {
            animationController.PlayWalk();
        }
    }

    private void OnCharacterMovement(Vector2 move)
    {
        Creature.Move(move);        
    }
}