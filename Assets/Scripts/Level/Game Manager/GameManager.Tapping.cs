using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace Game.Level
{

    public partial class GameManager
    {
        private const int TAP_LAYER_MASK = 1 << 6;

        public Camera mainCamera;
        public PlayerInput playerInput;

        private void InitTapping()
        {
            playerInput.actions.FindAction("Tap").performed += OnTap;
#if UNITY_EDITOR
            playerInput.actions.FindAction("Press").performed += OnPress; //mouse
#endif

        }

        public void OnTap(CallbackContext context)
        {
            Vector2 tapPosition = context.ReadValue<Vector2>();
            Ray ray = mainCamera.ScreenPointToRay(tapPosition);
            SendRaycast(in ray);
        }

#if UNITY_EDITOR
        public void OnPress(CallbackContext context)
        {
            if (Mouse.current == null) return;
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = mainCamera.ScreenPointToRay(mousePosition);
            SendRaycast(in ray);
        }
#endif

        private void SendRaycast(in Ray ray)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, TAP_LAYER_MASK))
            {
                GameObject gameObject = hit.collider.gameObject;
                CheckCell(gameObject);
            }
        }

        private void CheckCell(GameObject gameObject)
        {
            GridCell cell = GridCell.GetActualType(gameObject);

            if (cell.isInPrimaryGrid) return;

            Passenger passenger = cell.passenger;
            if (!passenger) return;
            else if (!cell.hasSpaceToMove)
            {
                Debug.LogError($"Cell at {cell.position} has no space to move the passenger");
                onPlayerAttemptedToMovePassenger?.Invoke(passenger, false);
                return;
            }
            
            cell.RemovePassenger();

            bool success = MovePassengerToPrimaryGrid(passenger);
            onPlayerAttemptedToMovePassenger?.Invoke(passenger, success);

            Debug.Assert(cell != null, $"Passenger not found for GameObject: {gameObject.name}");
        }

    }

}