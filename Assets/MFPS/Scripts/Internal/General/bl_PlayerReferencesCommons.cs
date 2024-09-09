﻿using Photon.Pun;
using System;
using UnityEngine;

public abstract class bl_PlayerReferencesCommon : MonoBehaviour
{
    /// <summary>
    /// Called when this player dies
    /// </summary>
    public Action onDie;

    /// <summary>
    /// 
    /// </summary>
    public abstract Collider[] AllColliders
    {
        get;
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract Animator PlayerAnimator
    {
        get;
        set;
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract Transform BotAimTarget
    {
        get;
        set;
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract Team PlayerTeam
    {
        get;
    }

    private PhotonView _networkView = null;
    /// <summary>
    /// Player network view
    /// </summary>
    public PhotonView NetworkView
    {
        get
        {
            if (_networkView == null) _networkView = GetComponent<PhotonView>();
            return _networkView;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="list"></param>
    /// <param name="ignore"></param>
    public abstract void IgnoreColliders(Collider[] list, bool ignore);

    /// <summary>
    /// Determine if this player is death
    /// Since when the player dies it still exists in the game as a ragdoll for a short period of time
    /// </summary>
    /// <returns></returns>
    public abstract bool IsDeath();

    /// <summary>
    /// Get the health of this player
    /// </summary>
    /// <returns></returns>
    public abstract int GetHealth();

    /// <summary>
    /// Get the max health of this player
    /// </summary>
    /// <returns></returns>
    public virtual int GetMaxHealth() { return 100; }

    /// <summary>
    /// Is a local player?
    /// </summary>
    public virtual bool IsLocal()
    {
        return NetworkView.IsMine;
    }

    /// <summary>
    /// Is this player a real player or a bot?
    /// </summary>
    /// <returns></returns>
    public virtual bool IsRealPlayer()
    {
        return true;
    }

    /// <summary>
    /// Is this player in the same team of the local player?
    /// </summary>
    /// <returns></returns>
    public virtual bool IsTeamMateOfLocalPlayer()
    {
        return bl_RoomSettings.Instance != null && bl_RoomSettings.Instance.isOneTeamMode ? false : PlayerTeam == bl_MFPS.LocalPlayer.Team;
    }
}