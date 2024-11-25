using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public List<Platform> platforms = new List<Platform>(); // List of all platforms

    void Start()
    {
        foreach (var platform in platforms)
        {
            platform.timer = platform.offset;
            platform.previousPosition = platform.start.position; // Initialize previous position
        }
    }

    void FixedUpdate()
    {
        foreach (var platform in platforms)
        {
            platform.timer += Time.fixedDeltaTime * platform.speed;

            // Calculate interpolation factor using PingPong
            float t = Mathf.PingPong(platform.timer, 1);
            Vector3 newPosition = Vector3.Lerp(platform.start.position, platform.stop.position, t);
            newPosition.y = platform.start.position.y; // Keep Y position constant

            // Calculate platform velocity
            platform.velocity = (newPosition - platform.previousPosition) / Time.fixedDeltaTime;
            platform.previousPosition = newPosition;

            platform.rig.MovePosition(newPosition);
        }
    }

    [System.Serializable]
    public class Platform
    {
        public Transform start;
        public Transform stop;
        public Rigidbody rig;
        public float speed;
        public float offset;

        [HideInInspector] public float timer;
        [HideInInspector] public Vector3 velocity; // Store calculated velocity
        [HideInInspector] public Vector3 previousPosition; // Store previous position
    }

}
