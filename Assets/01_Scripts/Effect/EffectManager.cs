using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// contains pools for effects
/// </summary>
public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance => instance;
    private static EffectManager instance = null;
    
    private DeadCirclePool dCirclePool;
    private DustPool dustPool;
    private DashLinePool dashLinePool;
    private HoodColorHandler hoodColorHandler;
    private Camera cam;
    public static Camera mainCam => instance.cam;

    [SerializeField] private RippleHandler rippleHandler;

    public static DustVisualization GetDust() => instance.dustPool.Pool.Get();
    public static DashLineVisualization GetDashLine() => instance.dashLinePool.Pool.Get();
    public static DeathCircle GetCircle() => instance.dCirclePool.Pool.Get();
    public static void ChangeCloth() => instance.hoodColorHandler.OnDash();
    public static void CreateRipple(Vector2 pos) => instance.rippleHandler.Ripple(pos);

    // subscribers require different parameters so it would be messy.
    // public Action DashStart;
    // public Action DashEnd;
    
    void Awake() 
    {
        if (instance != this)
        {
            if (instance == null) instance = this;
            else Destroy(this);
        }
        
        DontDestroyOnLoad(this);
        
        // get components
        dCirclePool = GetComponent<DeadCirclePool>();
        dustPool = GetComponent<DustPool>();
        dashLinePool = GetComponent<DashLinePool>();
        hoodColorHandler = GetComponent<HoodColorHandler>();
        cam = Camera.main;
    }

    #region Camera effects
    
    public static Tweener ShakeCam(float duration, float strength) 
        => instance.cam.DOShakePosition(duration, strength);
    
    public static Tweener MoveCam(Vector2 posWS, float time)
        => instance.cam.transform.DOMove(new Vector3(posWS.x, posWS.y, -10), time);
    
    public void Ripple(Vector2 center, float duration, float strength)
    {
        // TODO: Ripple when dashing.
    }

    #endregion
}