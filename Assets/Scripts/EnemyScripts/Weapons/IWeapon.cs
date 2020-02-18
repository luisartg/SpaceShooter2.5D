using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.EnemyScripts.Weapons
{
    public interface IWeapon
    {
        void Shoot(Vector2 direction);
    }
}
