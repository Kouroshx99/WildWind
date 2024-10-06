using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WildWind.Movement
{
    
    public interface IMover
    {

        public void Execute(MoverData moverData,Transform transform,float direction);

        public float GetRotation();

    }

}