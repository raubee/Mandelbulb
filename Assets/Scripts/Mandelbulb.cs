using UnityEngine;

namespace AdLucem.Maths
{
	public class Mandelbulb : MonoBehaviour
	{
		[HideInInspector, SerializeField]
		private Shader _mandelbulbShader;

		private MeshRenderer _renderer;
		private Material material;
		private Mesh _mesh;

		private void Start()
		{
			InitFrustumVolume();
			material = new Material(_mandelbulbShader);
			_renderer.material = material;
		}

		private void Update()
		{
			UpdateFrustumVolume();
		}

		
		private void UpdateFrustumVolume()
		{
			var vertices = _mesh.vertices;

			var size = 2f;
			var cam = Camera.main;
			var h = cam.nearClipPlane * Mathf.Tan(cam.fieldOfView / 2);
			var localOffSet = cam.transform.localPosition;
			localOffSet.y += h;

			var f_tfr = cam.transform.position;
			f_tfr.SubVec4(cam.transform.localToWorldMatrix * localOffSet);

			// tfr
			vertices[5] = f_tfr;
			vertices[11] = f_tfr;
			vertices[18] = f_tfr;

			// tfl
			var tfl_p = new Vector3(size, size, -size);
			vertices[4] = tfl_p;
			vertices[10] = tfl_p;
			vertices[21] = tfl_p;

			// bfl
			var bfl_p = new Vector3(size, -size, -size);
			vertices[6] = bfl_p;
			vertices[12] = bfl_p;
			vertices[20] = bfl_p;

			// bfr
			var bfr_p = new Vector3(-size, -size, -size);
			vertices[7] = bfr_p;
			vertices[15] = bfr_p;
			vertices[19] = bfr_p;

			// tbr
			var tbr_p = new Vector3(-size, size, size);
			vertices[3] = tbr_p;
			vertices[9] = tbr_p;
			vertices[17] = tbr_p;

			// tbl
			var tbl_p = new Vector3(size, size, size);
			vertices[2] = tbl_p;
			vertices[8] = tbl_p;
			vertices[22] = tbl_p;

			// bbl
			var bbl_p = new Vector3(size, -size, size);
			vertices[0] = bbl_p;
			vertices[13] = bbl_p;
			vertices[23] = bbl_p;

			// bbr
			var bbr_p = new Vector3(-size, -size, size);
			vertices[1] = bbr_p;
			vertices[14] = bbr_p;
			vertices[16] = bbr_p;

			_mesh.vertices = vertices;
		}

		/// <summary>
		/// Create frustum volume  
		/// Add a polyhedron that represent the camera frustum
		/// It is used to perform the volumetric ray-marching
		/// </summary>
		private void InitFrustumVolume()
		{
			var volume = GameObject.CreatePrimitive(PrimitiveType.Cube);
			volume.name = "Marching volume";

			_mesh = volume.GetComponent<MeshFilter>().mesh;
			_renderer = volume.GetComponent<MeshRenderer>();
		}
	}

	public static class ExtensionVector3
	{
		public static void SubVec4(this Vector3 target, Vector4 vector)
		{
			target.x -= vector.x;
			target.y -= vector.y;
			target.z -= vector.z;
		}
	}
}
