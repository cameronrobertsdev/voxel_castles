using System;
using UnityEngine;

namespace CDR
{

    [Serializable]
    public class WeightPoint
    {
        public WeightPoint(float weight, Vector3 position)
        {
            this.weight = weight;
            this.position = position;
        }

        public float weight;
        public Vector3 position;

        public bool Equals(WeightPoint other)
        {
            if (other is null)
            {
                return false;
            }

            return this.position == other.position && this.weight == other.weight;
        }

        public override bool Equals(object obj) => base.Equals(obj);

        public override int GetHashCode() => base.GetHashCode();

        public static bool operator ==(WeightPoint wp1, WeightPoint wp2) => wp1.Equals(wp2);
        public static bool operator !=(WeightPoint wp1, WeightPoint wp2) => !wp1.Equals(wp2);
    }
}