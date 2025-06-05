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

        private InputAction _tapAction;

#if UNITY_EDITOR
        private InputAction _pressAction;
#endif

        private void InitTapping()
        {
            _tapAction = playerInput.actions.FindAction("Tap");
            _tapAction.performed += OnTap;

#if UNITY_EDITOR
            //mouse
            _pressAction = playerInput.actions.FindAction("Press");
            _pressAction.performed += OnPress;
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

            if (cell.isInPrimaryGrid)
            {
                Debug.LogWarning($"Tapped on primary grid cell at {cell.position}. This cell is not tappable.");
                return;
            }

            Passenger passenger = cell.passenger;
            if (!passenger)
            {
                Debug.LogWarning($"Tapped on empty cell at {cell.position}. No passenger to move.");
                return;
            }

            else if (!cell.hasSpaceToMove)
            {
                Debug.LogError($"Cell at {cell.position} has no space to move the passenger");
                onPlayerAttemptedToMovePassenger?.Invoke(passenger, false);
                return;
            }

            bool success = TryMovePassengerToPrimaryGrid(passenger);
            if (success)
            {
                cell.RemovePassenger();
                CheckSecondaryGridNeighbourSpace(cell.position);
            }


            onPlayerAttemptedToMovePassenger?.Invoke(passenger, success);
        }

        public void SetEnableInput(bool enable)
        {
            if (enable) EnableInput();
            else DisableInput();
        } 

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DisableInput()
        {
            _tapAction.Disable();
#if UNITY_EDITOR
            _pressAction.Disable();
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void EnableInput()
        {
            _tapAction.Enable();
#if UNITY_EDITOR
            _pressAction.Enable();
#endif
        }

    }

}