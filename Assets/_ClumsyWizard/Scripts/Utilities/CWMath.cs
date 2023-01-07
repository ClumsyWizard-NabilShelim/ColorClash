using System.Collections;
using UnityEngine;

namespace ClumsyWizard.Utilities
{
    public class CWMath : MonoBehaviour
    {
		public static Vector2 GetDirectionFromAngle(float angleInDegrees, Transform rotateAround)
		{
			angleInDegrees -= rotateAround.eulerAngles.z;
			return GetDirectionFromAngle(angleInDegrees);
		}

		public static Vector2 GetDirectionFromAngle(float angleInDegrees)
		{
			return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad)).normalized;
		}
	}
}