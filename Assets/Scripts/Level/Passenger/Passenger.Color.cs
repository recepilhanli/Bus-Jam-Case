using System.Collections;
using System.Collections.Generic;
using Game.Utils;
using UnityEngine;
using UnityEngine.AI;


namespace Game.Level
{

    public partial class Passenger
    {
        [Header("Appearance")]
        public Renderer _renderer;
        [SerializeField] private ColorList _color;

        public ColorList Color
        {
            get => _color;
            set
            {
                _color = value;
                ColorHelper.SetRendererColor(_renderer, _color);
            }
        }

        [ContextMenu("Mark Passenger")]
        public void MarkPassenger()
        {
            Material[] sharedMaterials = _renderer.sharedMaterials;
            if (sharedMaterials.Length > 2) return;
            sharedMaterials = new Material[2] { sharedMaterials[0], GameManager.instance.markPassengerMaterial };
            _renderer.sharedMaterials = sharedMaterials;
        }

        [ContextMenu("Unmark Passenger")]
        public void UnmarkPassenger()
        {
            Material[] sharedMaterials = _renderer.sharedMaterials;
            if (sharedMaterials.Length < 2) return;
            sharedMaterials = new Material[1] { sharedMaterials[0] };
            _renderer.sharedMaterials = sharedMaterials;
        }

        [ContextMenu("Update Color")]
        public void UpdateColor() => ColorHelper.SetRendererColor(_renderer, _color);
    }

}