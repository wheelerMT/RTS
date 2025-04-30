using System;
using UnityEngine;

namespace Player
{
    [Serializable]
    public class CameraConfig
    {
        [field: SerializeField] public bool EnableEdgePan { get; private set; } = true;
        [field: SerializeField] public float MousePanSpeed { get; private set; } = 5;
        [field: SerializeField] public float EdgePanSize { get; private set; } = 50;

        [field: SerializeField] public float KeyboardPanSpeed { get; private set; } = 5;

        [field: SerializeField] public float MinZoomDistance { get; private set; } = 7.5f;
        [field: SerializeField] public float ZoomSpeed { get; private set; } = 1;

        [field: SerializeField] public float RotationSpeed { get; private set; } = 1;
    }
}