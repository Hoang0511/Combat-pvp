using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MovementSystem
{
    [Serializable]
    public class PlayerCameraRecenteringData 
    {
        [field: SerializeField][field:Range(0f,360f)] public float MinumumAngle { get;private set; }
        [field: SerializeField][field:Range(0f,360f)] public float MaximumAngle { get;private set; }
        [field: SerializeField][field:Range(-1f,20f)] public float WaitTime { get;private set; }
        [field: SerializeField][field:Range(-1f,20f)] public float RecenteringTime { get;private set; }

        public bool IsWithinRange(float angle)
        {
            return angle >= MinumumAngle && angle <= MaximumAngle;
        }
    }
}