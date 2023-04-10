using System;
using UnityEngine;

namespace Cinemachine.Examples
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    [SaveDuringPlay] // Enable SaveDuringPlay for this class
    public class MouseScrollZoom2D : MonoBehaviour
    {
        [Range(0, 10)]
        public float ZoomMultiplier = 1f;   // 줌 속도 배율

        float minZoom;
        float maxZoom;

        CinemachineVirtualCamera m_VirtualCamera;
        float m_OriginalOrthoSize;

        void Awake()
        {
            m_VirtualCamera = GetComponent<CinemachineVirtualCamera>();
            m_OriginalOrthoSize = m_VirtualCamera.m_Lens.OrthographicSize;

#if UNITY_EDITOR
            // This code shows how to play nicely with the VirtualCamera's SaveDuringPlay functionality
            SaveDuringPlay.SaveDuringPlay.OnHotSave -= RestoreOriginalOrthographicSize;
            SaveDuringPlay.SaveDuringPlay.OnHotSave += RestoreOriginalOrthographicSize;
#endif
        }

#if UNITY_EDITOR
        void OnDestroy()
        {
            SaveDuringPlay.SaveDuringPlay.OnHotSave -= RestoreOriginalOrthographicSize;
        }

        void RestoreOriginalOrthographicSize()
        {
            m_VirtualCamera.m_Lens.OrthographicSize = m_OriginalOrthoSize;
        }
#endif

        void OnValidate()
        {
            maxZoom = Mathf.Max(minZoom, maxZoom);
        }

        void Start()
        {
            minZoom = GameManager.GM.minZoom;
            maxZoom = GameManager.GM.maxZoom;
        }

        void Update()
        {
            float zoom = m_VirtualCamera.m_Lens.OrthographicSize + Input.mouseScrollDelta.y * ZoomMultiplier  *-1; 
            m_VirtualCamera.m_Lens.OrthographicSize = Mathf.Clamp(zoom, minZoom, maxZoom);
        }

        public void MoveCanera(bool isZoom, float moveValue)
        {
            int value = isZoom ? 1 : -1;    // 줌 방향 설정

            float zoom = m_VirtualCamera.m_Lens.OrthographicSize + moveValue * value;
            m_VirtualCamera.m_Lens.OrthographicSize = Mathf.Clamp(zoom, minZoom, maxZoom);

            if (isZoom)
            {
                if (m_VirtualCamera.m_Lens.OrthographicSize >= maxZoom)
                    GameManager.GM.isZoom = false;
            }
            else
            {
                if (m_VirtualCamera.m_Lens.OrthographicSize <= minZoom)
                    GameManager.GM.isZoom = false;
            }
        }
    }
}
