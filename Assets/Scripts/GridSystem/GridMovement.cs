using System;
using System.Collections;
using System.Collections.Generic;
using Settings;
using Unity.Mathematics;
using UnityEngine;

namespace GridSystem {
    public class GridMovement : MonoBehaviour {
        private readonly DeveloperSettings _devSettings = DevSet.I.developer;
        private Camera _camera;
        
        private void Awake() {
            _camera = Camera.main;
        }

        private void Update() {
            _camera.orthographicSize =
                Math.Clamp(_camera.orthographicSize - Input.mouseScrollDelta.y, _devSettings.cameraZoomRange.x, _devSettings.cameraZoomRange.y);

            var zoomMovement = 1f;
            if (DevSet.I.developer.slowWhenZoomed) {
                zoomMovement = 0.1f + ((_camera.orthographicSize - _devSettings.cameraZoomRange.x) /
                                       (_devSettings.cameraZoomRange.y - _devSettings.cameraZoomRange.x));
                zoomMovement = Math.Clamp(zoomMovement, 0f, 1f);
            }

            var movementAxis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            transform.Translate(_devSettings.cameraMovementSpeed * Time.deltaTime * 70f * zoomMovement * movementAxis);
        }
    }  
}
