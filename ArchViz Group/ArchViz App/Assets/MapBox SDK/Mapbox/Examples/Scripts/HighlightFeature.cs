namespace Mapbox.Examples
{
	using UnityEngine;
	using System.Collections.Generic;

	public class HighlightFeature : MonoBehaviour
	{
		static Material _highlightMaterial;

		private List<Material> _materials = new List<Material>();

		MeshRenderer _meshRenderer;

		void Start()
		{
			if (_highlightMaterial == null)
			{
				_highlightMaterial = Instantiate(GetComponent<MeshRenderer>().material);
				_highlightMaterial.color = new Color(0.35f, 0.25f, 0.25f);
			}

			_meshRenderer = GetComponent<MeshRenderer>();

            foreach (var item in _meshRenderer.sharedMaterials)
			{
				_materials.Add(item);
			}
		}

		public void OnMouseEnter()
		{
			_meshRenderer.sharedMaterial = _highlightMaterial;
		}

		public void OnMouseExit()
		{
			_meshRenderer.materials = _materials.ToArray();
        }
	}
}