using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UsefulExtensions.Transform
{
    public static class TransformExtensions
    {
        public static IEnumerable<UnityEngine.Transform> GetChildren(this UnityEngine.Transform transform)
        {
            if (transform == null)
                throw new ArgumentNullException(nameof(transform));

            return GetChildrenInternal();

            IEnumerable<UnityEngine.Transform> GetChildrenInternal()
            {
                for (var i = 0; i < transform.childCount; i++)
                    yield return transform.GetChild(i);
            }
        }

        public static void DestroyChildren(this UnityEngine.Transform transform)
        {
            if (transform == null)
                throw new ArgumentNullException(nameof(transform));

            for (var i = 0; i < transform.childCount; i++)
            {
                Object.Destroy(transform.GetChild(i).gameObject);
            }
        }

        public static void DisableChildren(this UnityEngine.Transform transform)
        {
            if (transform == null)
                throw new ArgumentNullException(nameof(transform));

            for (var i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        public static IEnumerable<UnityEngine.Transform> GetActiveSelfChildren(this UnityEngine.Transform transform)
        {
            if (transform == null)
                throw new ArgumentNullException(nameof(transform));

            return GetChildrenInternal();

            IEnumerable<UnityEngine.Transform> GetChildrenInternal()
            {
                for (var i = 0; i < transform.childCount; i++)
                    if (transform.GetChild(i).gameObject.activeSelf)
                        yield return transform.GetChild(i);
            }
        }

        public static IEnumerable<UnityEngine.Transform> GetActiveInHierarchyChildren(this UnityEngine.Transform transform)
        {
            if (transform == null)
                throw new ArgumentNullException(nameof(transform));

            return GetChildrenInternal();

            IEnumerable<UnityEngine.Transform> GetChildrenInternal()
            {
                for (var i = 0; i < transform.childCount; i++)
                    if (transform.GetChild(i).gameObject.activeInHierarchy)
                        yield return transform.GetChild(i);
            }
        }

        public static TComponent GetComponentInSibling<TComponent>(this Component component) where TComponent : Component =>
            component.gameObject.GetComponentInSibling<TComponent>();

        public static TComponent GetComponentInSibling<TComponent>(this GameObject gameObject) where TComponent : Component
        {
            var self = gameObject.transform;
            var parent = self.parent;
            if (parent == null) return null;

            for (int i = 0; i < parent.childCount; i++)
            {
                var sibling = parent.GetChild(i);
                if (sibling == self) continue;

                var component = sibling.GetComponent<TComponent>();
                if (component != null) return component;
            }

            return null;
        }

        public static UnityEngine.Transform GetTopmostParent(this UnityEngine.Transform transform)
        {
            var parent = transform;
            while (parent.parent != null)
            {
                parent = parent.parent;
            }

            return parent;
        }

        public static void FindAllChildsRecursively(UnityEngine.Transform parentTransform, List<UnityEngine.Transform> allFoundTransforms)
        {
            allFoundTransforms.Add(parentTransform);

            foreach (UnityEngine.Transform child in parentTransform)
            {
                FindAllChildsRecursively(child, allFoundTransforms);
            }
        }
        
        public static void FindAllSpriteRendererChildrenRecursively(UnityEngine.Transform parentTransform, List<SpriteRenderer> allFoundTransforms)
        {
            if(parentTransform.TryGetComponent(out SpriteRenderer spriteRenderer))
                allFoundTransforms.Add(spriteRenderer);

            foreach (UnityEngine.Transform child in parentTransform)
            {
                FindAllSpriteRendererChildrenRecursively(child, allFoundTransforms);
            }
        }

        public static UnityEngine.Vector3 TransformPointUnscaled(this UnityEngine.Transform transform, UnityEngine.Vector3 position)
        {
            var localToWorldMatrix = Matrix4x4.TRS(transform.position, transform.rotation, UnityEngine.Vector3.one);
            return localToWorldMatrix.MultiplyPoint3x4(position);
        }

        public static UnityEngine.Vector3 InverseTransformPointUnscaled(this UnityEngine.Transform transform, UnityEngine.Vector3 position)
        {
            var worldToLocalMatrix = Matrix4x4.TRS(transform.position, transform.rotation, UnityEngine.Vector3.one).inverse;
            return worldToLocalMatrix.MultiplyPoint3x4(position);
        }

        public static UnityEngine.Vector2 GetWorldToViewportPoint(this UnityEngine.Transform transform,
            UnityEngine.Camera camera = null)
        {
            if (camera == null)
                camera = UnityEngine.Camera.main;
            
            return camera.WorldToViewportPoint(transform.position);
        }

        public static void SetChildAsLastChild(this UnityEngine.Transform transform,
            UnityEngine.Transform parent)
        {
            transform.SetParent(parent);
            transform.SetSiblingIndex(parent.childCount - 1);
        }
    }

}
