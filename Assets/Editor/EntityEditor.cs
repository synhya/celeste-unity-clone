﻿
using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Entity))]
public class EntityEditor : Editor
{
    protected SerializedProperty hitboxSize, hitboxOffset;

    protected virtual  void OnEnable()
    {
        hitboxSize = serializedObject.FindProperty("HitboxSize");
        hitboxOffset = serializedObject.FindProperty("HitboxBottomLeftOffset");
    }

    protected virtual void OnSceneGUI()
    {
        var e = target as Entity;
        
        e.PositionWS = Vector2Int.RoundToInt(e.transform.position);
        var rect = new Rect
        {
            x = e.PositionWS.x + e.HitBoxBottomLeftOffset.x,
            y = e.PositionWS.y + e.HitBoxBottomLeftOffset.y,
            width = e.HitboxSize.x,
            height = e.HitboxSize.y
        };
        Handles.DrawSolidRectangleWithOutline(rect, Color.clear, Color.green);
        
        // snap position to int
        if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
        {
            e.transform.position = Vector3Int.RoundToInt(e.transform.position);
        }
    }
}

