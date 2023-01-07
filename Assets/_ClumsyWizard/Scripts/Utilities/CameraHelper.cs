using UnityEngine;

namespace ClumsyWizard.Utilities
{
	public static class CameraHelper
	{
		private static Camera cam;

		public static Camera Camera
		{
			get
			{
				if (cam == null)
					cam = Camera.main;

				return cam;
			}
		}

		public static Vector3 GetMouseScreenPosition(Vector3 offset)
		{
			Vector3 mousePos = Input.mousePosition + offset;
			return mousePos;
		}

		public static Ray ShootRayFromMouse()
		{
			if (Camera.orthographic)
			{
				Vector3 mousePos = Camera.ScreenToWorldPoint(Input.mousePosition);
				return new Ray(mousePos, Camera.transform.forward * 1000.0f);
			}
			else
            {
				return Camera.ScreenPointToRay(Input.mousePosition);
			}
		}

		public static Vector3 GetWorldToScreenPosition(Vector3 worldPosition)
		{
			return Camera.WorldToScreenPoint(worldPosition);
		}
	}
}
