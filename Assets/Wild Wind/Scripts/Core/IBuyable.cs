using System.Collections;
using UnityEngine;

namespace WildWind.Core
{

    public interface IBuyable
    {

        int price { get; set; }

        bool Buy(ref int balance);

    }

}