using UnityEngine;

namespace CoLib
{

public static class QuaternionUtils
{
	#region Public static methods

	public static Quaternion Squad(Quaternion p1, Quaternion p2, Quaternion p3, Quaternion p4, float t)
	{
		Quaternion a1 = SquadIntermediate(p1, p2, p3);
		Quaternion a2 = SquadIntermediate(p2, p3, p4);
		return SlerpNoInvert(SlerpNoInvert(p2,p3, t), SlerpNoInvert(a1,a2,t), 2 * t * (1 - t));	
	}

	public static void Log(this Quaternion a)
	{
		float a0 = a.w;
		a.w = 0.0f;
		if (Mathf.Abs(a0) < 1.0f) {
			float angle = Mathf.Acos(a0);
			float sinAngle = Mathf.Sin(angle);
			if (Mathf.Abs(sinAngle) >= 1.0e-15f)
			{
				float coeff = angle/sinAngle;
				a.x *= coeff;
				a.y *= coeff;
				a.z *= coeff;
			}
		}
	}

	public static Quaternion Logged(this Quaternion a)
	{
		Quaternion b = a;
		float b0 = b.w;
		b.w = 0.0f;
		if (Mathf.Abs(b0) < 1.0f) {
			float angle = Mathf.Acos(b0);
			float sinAngle = Mathf.Sin(angle);
			if (Mathf.Abs(sinAngle) >= 1.0e-15f)
			{
				float coeff = angle/sinAngle;
				b.x *= coeff;
				b.y *= coeff;
				b.z *= coeff;
			}
		}
		return b;
	}

	public static void Scale(this Quaternion a, float s)
	{
		a.w *= s;
		a.x *= s;
		a.y *= s;
		a.z *= s;
	}

	public static Quaternion Scaled(this Quaternion a,float s)
	{
		Quaternion result = a;
		result.w *= s;
		result.x *= s;
		result.y *= s;
		result.z *= s;
		return result;
	}

	public static void Exp(this Quaternion a)
	{
		float angle = Mathf.Sqrt(a.x * a.x + a.y * a.y + a.z * a.z);
		float sinAngle = Mathf.Sin(angle);
		a.w = Mathf.Cos(angle);

		if (Mathf.Abs(sinAngle) >= 1.0e-15f) {
			float coeff = sinAngle/angle;

			a.x *= coeff;
			a.y *= coeff;
			a.z *= coeff;
		}
	}

	public static Quaternion Exped(this Quaternion a)
	{
		Quaternion result = a;
		float angle = Mathf.Sqrt(result.x * result.x + result.y * result.y + result.z * result.z);
		float sinAngle = Mathf.Sin(angle);
		result.w = Mathf.Cos(angle);
		if (Mathf.Abs(sinAngle) >= 1.0e-15f) {
			float coeff = sinAngle/angle;
			result.x *= coeff;
			result.y *= coeff;
			result.z *= coeff;
		}
		return result;
	}


	public static void Normalize(this Quaternion a)
	{
		float length = a.Length();
		if (length > 1.0e-15f) {
			float invlen = 1.0f / length;
			a.w *= invlen;
			a.x *= invlen;
			a.y *= invlen;
			a.z *= invlen;
		} else {
			length = 0.0f;
			a.w = 0.0f;
			a.x = 0.0f;
			a.y = 0.0f;
			a.z = 0.0f;
		}
	}

	public static Quaternion Normalized(this Quaternion a)
	{
		Quaternion result = Quaternion.identity;
		float length = result.Length();
		if (length > 1.0e-15f) {
			float invlen = 1.0f / length;
			result.w *= invlen;
			result.x *= invlen;
			result.y *= invlen;
			result.z *= invlen;
		} else {
			length = 0.0f;
			result.w = 0.0f;
			result.x = 0.0f;
			result.y = 0.0f;
			result.z = 0.0f;
		}
		return result;
	}

	public static float Length(this Quaternion a)
	{
		return Mathf.Sqrt(a.w * a.w + a.x * a.x + a.y * a.y + a.z * a.z);
	}

	public static Quaternion Added(this Quaternion a, Quaternion b)
	{
		Quaternion r;
		r.w = a.w + b.w;
		r.x = a.x + b.x;
		r.y = a.y + b.y;
		r.z = a.z + b.z;
		return r;
	}

	public static Quaternion Subtracted(this Quaternion a, Quaternion b)
	{
		Quaternion r;
		r.w = a.w - b.w;
		r.x = a.x - b.x;
		r.y = a.y - b.y;
		r.z = a.z - b.z;
		return r;
	}
	
	public static Quaternion SlerpNoInvert(Quaternion fro, Quaternion to, float factor)
	{
		float dot = Quaternion.Dot(fro,to);

		if (Mathf.Abs(dot) > 0.9999f) { 
			return fro;
		}
		
		float theta = Mathf.Acos(dot);
		float sinT = 1.0f / Mathf.Sin(theta);
		float newFactor = Mathf.Sin(factor * theta) * sinT;
		float invFactor = Mathf.Sin((1.0f - factor) * theta) * sinT;

		return new Quaternion(
			invFactor * fro.x + newFactor * to.x,
			invFactor * fro.y + newFactor * to.y,
			invFactor * fro.z + newFactor * to.z,
			invFactor * fro.w + newFactor * to.w
		);
	}

	public static Quaternion FromToRotation(Vector3 from, Vector3 to)
	{
		from.Normalize();
		to.Normalize();
		Quaternion result;

     	Vector3 H = (from + to).normalized;


     	result.w = Vector3.Dot(from, H);

     	result.x = from.y * H.z - from.z * H.y;
     	result.y = from.z * H.x - from.x * H.z;
     	result.z = from.x * H.y - from.y * H.x;
     	return result;
	}

	#endregion

	#region Private methods

	private static Quaternion SquadIntermediate(Quaternion previous, Quaternion current, Quaternion next)
	{
		Quaternion q1inv = Quaternion.Inverse(current);
		Quaternion c1 = q1inv * next;
		Quaternion c2 = q1inv * previous;
		c1.Log();
		c2.Log();
		Quaternion c3 = c2.Added(c1);
		c3.Scale(-0.25f);
		c3.Exp();
		Quaternion r = current * c3;
		r.Normalize();
		return r;
	}


	#endregion
}

}
