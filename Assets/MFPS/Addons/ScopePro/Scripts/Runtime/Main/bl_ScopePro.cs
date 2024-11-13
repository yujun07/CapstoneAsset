using UnityEngine;

namespace MFPS.Addon.ScopePro
{
    public class bl_ScopePro : bl_MonoBehaviour
    {
        public MeshDisplayMode meshDisplayMode = MeshDisplayMode.None;
        public Camera RenderCamera;
        public Camera PlayerCamera;
        public Material ScopeRTMaterial;
        public GameObject NormalScopeMesh;
        public GameObject RTScopeMesh;

        void Start()
        {
            if (NormalScopeMesh != null) { NormalScopeMesh.SetActive(true); }
            if (RTScopeMesh != null) { RTScopeMesh.SetActive(false); }
            RenderCamera.gameObject.SetActive(false);
        }

        public void OnAim(bool isAiming)
        {
            RenderCamera.gameObject.SetActive(isAiming);
            NormalScopeMesh.SetActive(!isAiming);
            RTScopeMesh.SetActive(isAiming);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnSlowUpdate()
        {
            ApplyProps();
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            ApplyProps();
        }
#endif

        public void ApplyProps()
        {
            if (PlayerCamera != null && RenderCamera != null)
            {
                RenderCamera.transform.position = PlayerCamera.transform.position;
                RenderCamera.transform.rotation = PlayerCamera.transform.rotation;
            }
        }

        [System.Serializable]
        public enum MeshDisplayMode
        {
            None,
            SwapOnAim,
            ShowAlways,
        }
    }
}