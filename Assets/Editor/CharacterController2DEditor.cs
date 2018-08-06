using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace CC2D
{
    // [CustomEditor(typeof(CharacterController2D))]
    public class CharacterController2DEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            CharacterController2D cc = (CharacterController2D)target;
            Movementparameters ccp = cc.movementParameters;

            GUILayout.Label("Movement constraints");

            GUILayout.BeginHorizontal();

            ccp.affectedByGravity = EditorGUILayout.Toggle("Enable gravity", ccp.affectedByGravity);

            if (ccp.affectedByGravity)
            {
                ccp.gravityAcceleration = EditorGUILayout.Vector2Field("", ccp.gravityAcceleration);
            }

            GUILayout.EndHorizontal();

            ccp.movementAcceleration = EditorGUILayout.FloatField("Movement acceleration", ccp.movementAcceleration, GUILayout.ExpandWidth(false));
        }
    }
}
