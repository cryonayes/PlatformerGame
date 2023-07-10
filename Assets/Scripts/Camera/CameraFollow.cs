using UnityEngine;

namespace Camera
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform player;
        public UnityEngine.Camera cam;
        public float horizontalMargin, verticalMargin, depth = -10, smoothTime = 0.25f;
        private Vector3 _target, _lastPosition, _currentVelocity;
    
        private void LateUpdate()
        {
            SetTarget();
            MoveCamera();
        }

        private void SetTarget()
        {
            var position = player.position;
            var movementDelta = position - _lastPosition;
            var screenPos = cam.WorldToScreenPoint(position);
            var bottomLeft = cam.ViewportToScreenPoint(new Vector3(horizontalMargin,verticalMargin,0));
            var topRight = cam.ViewportToScreenPoint(new Vector3(1-horizontalMargin, 1-verticalMargin, 0));
        
            if (screenPos.x < bottomLeft.x || screenPos.x > topRight.x)
                _target.x += movementDelta.x;
            if (screenPos.y < bottomLeft.y || screenPos.y > topRight.y)
                _target.y += movementDelta.y;
            _target.z = depth;
            _lastPosition = player.position;
        }

        private void MoveCamera()
        {
            transform.position = Vector3.SmoothDamp(transform.position, _target, ref _currentVelocity, smoothTime);
        }
    }
}
