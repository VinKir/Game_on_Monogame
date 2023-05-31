using ProjectX.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectX.Components
{
    public class Rigidbody : IComponent
    {
        public float mass;

        GameObject gameObject;
        public GameObject boundEntity { get => gameObject; set => gameObject = value; }
    }
}
