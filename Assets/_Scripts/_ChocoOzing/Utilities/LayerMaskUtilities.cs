using System.Collections;
using UnityEngine;

namespace Ozing.Utilities
{
	public class LayerMaskUtilities
	{
		public static bool IsLayerInMask(int layer, LayerMask mask) => ((1 << layer) & mask) != 0;

		public static bool IsLayerInColliderGameObject(RaycastHit2D hit, LayerMask mask) =>
			IsLayerInMask(hit.collider.gameObject.layer, mask);
	}
}