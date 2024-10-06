using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WildWind.Movement
{

    public class Mover : IMover
    {

        [NonSerialized] private float _aileronsState;
        private float aileronsState { get => _aileronsState / 100f; set => _aileronsState = Mathf.Clamp(value, -100, 100); }

        public void Execute(MoverData moverData, Transform transform, float direction)
        {
            UpdateAileronsState(moverData.rollRate, direction);
            MoveForward(transform, moverData.speed);
            Rotate(transform, moverData.yawRate);
        }

        public float GetRotation() => aileronsState;

        private void UpdateAileronsState(float rollRate, float direction)
        {

            int signedDirection = direction == 0 ? 0 : (int)Mathf.Sign(direction);
            int desiredDirectionToRoll = Math.Sign(signedDirection - aileronsState);
            float rollAmount = desiredDirectionToRoll * rollRate * Time.deltaTime;
            if (direction != 0)
                rollAmount *= Mathf.Abs(direction);

            aileronsState = (int)(aileronsState * 100 + rollAmount);
        }

        private void Rotate(Transform transform, float yawRate) => transform.Rotate(Vector3.up * (yawRate * aileronsState) * Time.deltaTime);

        private void MoveForward(Transform transform, float speed) => transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);

        [MenuItem("Create Asset", menuItem = "Tools/Mover")]
        public static void Create()
        {
            ObjectType scriptableObject = ScriptableObject.CreateInstance<ObjectType>();
            scriptableObject.type = typeof(Mover);
            AssetDatabase.CreateAsset(scriptableObject, "Assets/Wild Wind/Class Types/IMover/" + typeof(Mover).Name + ".asset");
            AssetDatabase.SaveAssets();
        }

    }

}