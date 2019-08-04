using UnityEngine;

namespace AdLucem.Maths
{
	[RequireComponent(typeof(Camera))]
	public class Mandelbulb : MonoBehaviour
	{
		[HideInInspector, SerializeField]
		private Shader _mandelbulbShader;

		[SerializeField] private float _nearClipOffset = 0f;
		[SerializeField] private float _farClipOffset = 0f;

		private MeshRenderer _renderer;
		private Material material;
		private Mesh _mesh;
		private Camera _camera;

		private void Start()
		{
			_camera = GetComponent<Camera>();
			material = new Material(_mandelbulbShader);
			InitFrustumVolume();
			_renderer.material = material;
		}

		private void Update()
		{
			UpdateFrustumVolume();
		}

		private void UpdateFrustumVolume()
		{
			var vertices = _mesh.vertices;

			var nc = _camera.nearClipPlane + _nearClipOffset;
			var nf = _camera.farClipPlane + _farClipOffset;
			var fov = _camera.fieldOfView;
			var pos = _camera.transform.position;
			var fwd = _camera.transform.forward;
			var up = _camera.transform.up;
			var right = _camera.transform.right;
			var tan = Mathf.Tan(fov * Mathf.Deg2Rad * .5f);
			var ratio = _camera.pixelRect.width / _camera.pixelRect.height;

			var ncp = pos + fwd * nc;
			var nfp = pos + fwd * nf;
			var upp = up * tan * nc;
			var uppf = up * tan * nf;
			var rp = right * tan * nc * ratio;
			var rpf = right * tan * nf * ratio;

			// top front left
			var p = ncp + upp - rp;
			vertices[5] = p;
			vertices[11] = p;
			vertices[18] = p;

			// top front right
			p = ncp + upp + rp;
			vertices[4] = p;
			vertices[10] = p;
			vertices[21] = p;

			// bottom front right
			p = ncp - upp + rp;
			vertices[6] = p;
			vertices[12] = p;
			vertices[20] = p;

			// bottom front left
			p = ncp - upp - rp;
			vertices[7] = p;
			vertices[15] = p;
			vertices[19] = p;

			// top back left
			p = nfp + uppf - rpf;
			vertices[3] = p;
			vertices[9] = p;
			vertices[17] = p;

			// top back right
			p = nfp + uppf + rpf;
			vertices[2] = p;
			vertices[8] = p;
			vertices[22] = p;

			// bottom back right
			p = nfp - uppf + rpf;
			vertices[0] = p;
			vertices[13] = p;
			vertices[23] = p;

			// bottom back left
			p = nfp - uppf - rpf;
			vertices[1] = p;
			vertices[14] = p;
			vertices[16] = p;

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
			volume.hideFlags = HideFlags.HideAndDontSave;

			_mesh = volume.GetComponent<MeshFilter>().mesh;
			_renderer = volume.GetComponent<MeshRenderer>();
		}
	}
}
