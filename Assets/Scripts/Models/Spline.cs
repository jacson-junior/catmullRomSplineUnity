using UnityEngine;

namespace Splines {
	interface ISpline {
		Vector3 GetPoint(float t, Vector3[] points);
		Vector3 GetPoint(float t, Vector3[] points, out Vector3 tangent, out Vector3 normal, out Quaternion orientation);
	}

	namespace CatmullRom {
		public struct Spline : ISpline {
			public Vector3 GetPoint(float t, Vector3[] points)
			{
				//(0.5*(2p1)) + (0.5*(p2-p0))*t + (0.5*(2p0 - 5p1 + 4p2 - p3))*t² + (0.5*(-p0 + 3p1 - 3p2 + p3))*t³
				return (0.5f * (2f * points[1])) 
				+ ((0.5f * (points[2] - points[0])) * t)
				+ ((0.5f * (2f * points[0] - 5f * points[1] + 4f * points[2] - points[3])) * t * t) 
				+ ((0.5f * (-points[0] + 3f * points[1] - 3f * points[2] + points[3])) * t * t * t);
			}

			public Vector3 CalculateTangent(float t, Vector3[] points)
			{
				return 1.5f*(t*t)*(-points[0] + 3*points[1] - 3*points[2] + points[3]) + t*(2*points[0] - 5*points[1] + 4*points[2] - points[3]) + 0.5f*(points[2] - points[0]);
			}

			Vector3 CalculateNormal(Vector3 tangent, Vector3 up)
			{
				Vector3 binormal = Vector3.Cross(up, tangent);
				return Vector3.Cross(tangent, binormal);
			}

			public Vector3 GetPoint(float t, Vector3[] points, out Vector3 tangent, out Vector3 normal, out Quaternion orientation)
			{	
				tangent = CalculateTangent(t, points);
				normal = CalculateNormal(tangent, Vector3.up);
				orientation = Quaternion.LookRotation(tangent, normal);
		
				return GetPoint(t, points);
			}
		}
	}

	namespace Belzier {
		public struct Spline : ISpline {
			public Vector3 GetPoint(float t, Vector3[] points)
			{
				float t2 = t * t;
				float t3 = t2 * t;
				float it = (1 - t);
				float it2 = it * it;
				float it3 = it * it * it;
				return points[0] * (it3) +
					points[1] * (3 * it2 * t) +
					points[2] * (3 * it * t2) +
					points[3] * t3;
			}

			Vector3 CalculateTangent(Vector3[] points, float t, float t2, float it2)
			{
				return (points[0] * -it2 +
					points[1] * (t * (3 * t - 4) + 1) +
					points[2] * (-3 * t2 + t * 2) +
					points[3] * t2).normalized;
			}

			Vector3 CalculateNormal(Vector3 tangent, Vector3 up)
			{
				Vector3 binormal = Vector3.Cross(up, tangent);
				return Vector3.Cross(tangent, binormal);
			}

			public Vector3 GetPoint(float t, Vector3[] points, out Vector3 tangent, out Vector3 normal, out Quaternion orientation)
			{	
				float t2 = t * t;
				float it = (1 - t);
				float it2 = it * it;
				
				tangent = CalculateTangent(points, t, t2, it2);
				normal = CalculateNormal(tangent, Vector3.up);
				orientation = Quaternion.LookRotation(tangent, normal);
		
				return GetPoint(t, points);
			}
		}
	}
}
