using UnityEngine;
using System.Collections;

namespace UnityEngine
{
    public struct OrientedPoint
    {
        public Vector3 position;
        public Quaternion rotation;
        public Matrix4x4 scale;

        public OrientedPoint(Vector3 position, Quaternion rotation, Matrix4x4 scale)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }

        public Vector3 LocalToWorld(Vector3 point)
        {
            return  (new Vector4(position.x, position.y, position.z, 1) + (scale*(rotation * point)));
        }

        public Vector3 WorldToLocal(Vector3 point)
        {
            return Matrix4x4.Inverse(scale) * (Quaternion.Inverse(rotation) * (point - position));
        }

        public Vector3 LocalToWorldDirection(Vector3 dir)
        {
            return scale * (rotation * dir);
        }
    }
}
