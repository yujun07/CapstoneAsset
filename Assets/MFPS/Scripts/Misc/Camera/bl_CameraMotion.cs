using MFPSEditor;
using System.Collections;
using UnityEngine;

public class bl_CameraMotion : bl_CameraMotionBase
{
    [ScriptableDrawer] public bl_CameraMotionSettings motionSettings;

    #region Private members
    private bool wiggleMotion = true;
    private bool breathingMotion = false;
    private float m_breatingAmplitude;
    private float m_startBreathingTime;
    private Vector3 breathRotation = Vector3.zero;
    private float sideTilt = 0;
    private Quaternion rotationMotion, extraRotation;
    private bool isSliding = false;
    private bl_PlayerReferences playerRefs;
    #endregion

    /// <summary>
    /// 
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        playerRefs = GetComponentInParent<bl_PlayerReferences>();
        m_breatingAmplitude = motionSettings.breatheAmplitude;
        rotationMotion = CachedTransform.localRotation;
    }

    /// <summary>
    /// 
    /// </summary>
    protected override void OnEnable()
    {
        base.OnEnable();
        bl_EventHandler.onPlayerLand += OnSmallImpact;
        wiggleMotion = bl_GameData.Instance.playerCameraWiggle;
        bl_EventHandler.onLocalPlayerStateChanged += OnPlayerStateChanged;
    }

    /// <summary>
    /// 
    /// </summary>
    protected override void OnDisable()
    {
        base.OnDisable();
        bl_EventHandler.onPlayerLand -= OnSmallImpact;
        bl_EventHandler.onLocalPlayerStateChanged -= OnPlayerStateChanged;
    }

    /// <summary>
    /// 
    /// </summary>
    public override void OnUpdate()
    {
        if (!bl_RoomMenu.Instance.isCursorLocked) return;

        Breathe();

        Wiggle();

        CachedTransform.localRotation = rotationMotion * extraRotation;
    }

    /// <summary>
    /// 
    /// </summary>
    void Wiggle()
    {
        if (!wiggleMotion) return;

        float rollAngle = -bl_GameInput.MouseX * motionSettings.tiltAngle;
        if (isSliding) rollAngle = 0;
        rollAngle = Mathf.Clamp(rollAngle, -motionSettings.tiltAngle, motionSettings.tiltAngle);

        if (motionSettings.sideMoveEffector > 0)
        {
            sideTilt = Mathf.Lerp(sideTilt, -bl_GameInput.Horizontal * motionSettings.sideMoveEffector, Time.deltaTime * (motionSettings.smooth * 2));
            rollAngle += sideTilt;
        }

        breathRotation.z = bl_GameInput.Aim() ? rollAngle * motionSettings.aimMultiplier : rollAngle;
        rotationMotion = Quaternion.Slerp(rotationMotion, Quaternion.Euler(breathRotation), Time.smoothDeltaTime * motionSettings.smooth);
    }

    /// <summary>
    /// 
    /// </summary>
    void Breathe()
    {
        if (!breathingMotion) return;

        float theta = (Time.time - m_startBreathingTime) / motionSettings.breathePeriod;
        float vertical = m_breatingAmplitude * Mathf.Sin(theta);
        float horizontal = m_breatingAmplitude * Mathf.Cos(theta * 0.5f);
        breathRotation = (Vector3.up * vertical) + (Vector3.right * horizontal);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="rotation"></param>
    public override void AddCameraRotation(Quaternion rotation)
    {
        extraRotation = rotation;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="active"></param>
    public override void SetActiveBreathing(bool active, float amplitude = 0)
    {
        breathingMotion = active;

        if (!breathingMotion)
        {
            m_breatingAmplitude = motionSettings.breatheAmplitude;
            breathRotation = Vector3.zero;
        }
        else
        {
            m_breatingAmplitude = amplitude;
            m_startBreathingTime = Time.time;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void OnSmallImpact(float impactAmount)
    {
        StartCoroutine(FallEffect());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator FallEffect()
    {
        Vector3 target = Vector3.zero;
        Quaternion rot;
        float d = 0;

        while (d <= 1)
        {
            d += Time.deltaTime / motionSettings.downMotionDuration;

            target.x = (motionSettings.downPitchMovement.Evaluate(d) * motionSettings.DownAmount);
            rot = Quaternion.Euler(target);
            CachedTransform.localRotation = rotationMotion * extraRotation * rot;
            yield return null;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void OnPlayerStateChanged(PlayerState from, PlayerState to)
    {
        isSliding = to == PlayerState.Sliding;
    }
}