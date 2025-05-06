using UnityEngine;
using Cinemachine;
using System.Collections;
using DG.Tweening;
using UnityEngine.U2D;

public abstract class CameraLogic : MonoBehaviour
{
    [SerializeField] protected CinemachineVirtualCamera virtualCamera;
    [SerializeField] protected PixelPerfectCamera pixelPerfectCamera; 

    protected float originalSize;
    protected int originalPPU = 100;
    protected bool isPaused;

    public virtual void Initialize(CinemachineVirtualCamera camera, PixelPerfectCamera pixelCamera)
    {
        if (camera != null) { virtualCamera = camera; }
        
        if (virtualCamera != null) { originalSize = virtualCamera.m_Lens.OrthographicSize; }

        if (pixelCamera != null) { pixelPerfectCamera = pixelCamera; }
    }

    public void Zoom(float targetSize, float duration)
    {
        if (virtualCamera == null || pixelPerfectCamera == null) return;

        float zoomFactor = originalSize / targetSize;
        int targetPPU = Mathf.RoundToInt(originalPPU * zoomFactor); 

        DOTween.To(
            () => virtualCamera.m_Lens.OrthographicSize,
            x => virtualCamera.m_Lens.OrthographicSize = x,
            targetSize,
            duration).SetEase(Ease.InOutQuad);

        DOTween.To(
            () => pixelPerfectCamera.assetsPPU,
            x => pixelPerfectCamera.assetsPPU = Mathf.RoundToInt(x),
            targetPPU,
            duration).SetEase(Ease.InOutQuad);
    }

    public void ZoomOut(float duration)
    {
        if (virtualCamera == null || pixelPerfectCamera == null) return;

        DOTween.To(
            () => virtualCamera.m_Lens.OrthographicSize,
            x => virtualCamera.m_Lens.OrthographicSize = x,
            originalSize,
            duration).SetEase(Ease.InOutQuad);

        DOTween.To(
            () => pixelPerfectCamera.assetsPPU,
            x => pixelPerfectCamera.assetsPPU = Mathf.RoundToInt(x),
            originalPPU,
            duration).SetEase(Ease.InOutQuad);
    }

    public void SetPaused(bool paused)
    {
        isPaused = paused;
    }
}
