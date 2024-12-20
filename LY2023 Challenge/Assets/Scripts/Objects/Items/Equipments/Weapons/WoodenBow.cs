using UnityEngine;

namespace LY2023Challenge
{
    [CreateAssetMenu(fileName = "WoodenBow", menuName = "Items/Equipments/Weapons/Wooden Bow")]
    public class WoodenBow : Weapon
    {
        public override string Name
        {
            get => "Wooden Bow";
        }

        public override int BuyingPrice
        {
            get => 500;
        }
        public override int SellingPrice
        {
            get => 200;
        }

        public override int MaxLevel
        {
            get => 12;
        }

        public override WeaponType Type
        {
            get => WeaponType.Bow;
        }

        public override float Weight
        {
            get => 5f;
        }

        public override float PhysicalDamage
        {
            get
            {
                float value = 0f;

                // Level 1
                value += 20f;
                // Level 2-4
                value += 3f * Mathf.Max(0, Mathf.Min(3, this.Level - 1));
                // Level 5-7
                value += 4f * Mathf.Max(0, Mathf.Min(3, this.Level - 4));
                // Level 8-10
                value += 5f * Mathf.Max(0, Mathf.Min(3, this.Level - 7));
                // Level 11-12
                value += 6f * Mathf.Max(0, Mathf.Min(3, this.Level - 10));

                return value;
            }
        }
        public override float PhysicalPierce
        {
            get
            {
                float value = 0f;

                // Level 1
                value += 3f;
                // Level 2-4
                value += 0.5f * Mathf.Max(0, Mathf.Min(3, this.Level - 1));
                // Level 5-7
                value += 0.75f * Mathf.Max(0, Mathf.Min(3, this.Level - 4));
                // Level 8-10
                value += 1f * Mathf.Max(0, Mathf.Min(3, this.Level - 7));
                // Level 11-12
                value += 1.5f * Mathf.Max(0, Mathf.Min(3, this.Level - 10));

                return value;
            }
        }

        public override float PhysicalLifeSteal
        {
            get
            {
                float value = 0f;

                // Level 1
                value += 0.01f;
                // Level 2-3
                value += 0.004f * Mathf.Max(0, Mathf.Min(2, this.Level - 1));
                // Level 4-5
                value += 0.001f * Mathf.Max(0, Mathf.Min(2, this.Level - 3));

                return value;
            }
        }

        public override float MagicLifeSteal
        {
            get
            {
                float value = 0f;

                // Level 1
                value += 0.001f;
                // Level 2-3
                value += 0.0004f * Mathf.Max(0, Mathf.Min(2, this.Level - 1));
                // Level 4-5
                value += 0.00035f * Mathf.Max(0, Mathf.Min(2, this.Level - 3));

                return value;
            }
        }
    }
}