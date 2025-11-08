using UnityEngine;

namespace EclipseRealms.Core.Player
{
    /// <summary>
    /// Minimal top-down camera follow helper.
    /// </summary>
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(0f, 12f, -10f);
        [SerializeField] private float followSmoothing = 8f;

        private void LateUpdate()
        {
            if (target == null)
            {
                return;
            }

            Vector3 desired = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, desired, Time.deltaTime * followSmoothing);
            transform.LookAt(target);
        }

        public void SetTarget(Transform followTarget)
        {
            target = followTarget;
        }
    }
}
