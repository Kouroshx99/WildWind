using System;

namespace UnityEngine
{

    public class ObjectType : ScriptableObject
    {

        [SerializeField] private string _type;
        public Type type
        {

            get
            {

                return Type.GetType(_type);

            }
            set
            {

                _type = value.ToString();

            }

        }

    }

}
