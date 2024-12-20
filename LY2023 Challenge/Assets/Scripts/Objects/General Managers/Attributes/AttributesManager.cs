using UnityEngine;

namespace LY2023Challenge
{
    public class AttributesManager : MonoBehaviour
    {
        private EquipmentsManager _equipmentsManager;
        private EquipmentsManager EquipmentsManager
        {
            get
            {
                if (_equipmentsManager == null)
                {
                    _equipmentsManager = GetComponent<EquipmentsManager>();
                }

                return _equipmentsManager;
            }
        }

        [Header("Elixir")]
        [SerializeField] private int _elixir = 0;
        public int Elixir
        {
            get => _elixir;
            set => _elixir = value;
        }

        [Header("Attributes")]
        [SerializeField, Range(1, 99)] private int _vigorLevel = 1;
        [SerializeField, Range(1, 99)] private int _mindLevel = 1;
        [SerializeField, Range(1, 99)] private int _enduranceLevel = 1;
        [SerializeField, Range(1, 99)] private int _strengthLevel = 1;
        [SerializeField, Range(1, 99)] private int _dexterityLevel = 1;
        [SerializeField, Range(1, 99)] private int _intelligenceLevel = 1;
        [SerializeField, Range(1, 99)] private int _faithLevel = 1;
        [SerializeField, Range(1, 99)] private int _arcaneLevel = 1;
        #region Level Properties
        public int Vigor
        {
            get => _vigorLevel;
            set => _vigorLevel = value;
        }
        public int Mind
        {
            get => _mindLevel;
            set => _mindLevel = value;
        }
        public int Endurance
        {
            get => _enduranceLevel;
            set => _enduranceLevel = value;
        }
        public int Strength
        {
            get => _strengthLevel;
            set => _strengthLevel = value;
        }
        public int Dexterity
        {
            get => _dexterityLevel;
            set => _dexterityLevel = value;
        }
        public int Intelligence
        {
            get => _intelligenceLevel;
            set => _intelligenceLevel = value;
        }
        public int Faith
        {
            get => _faithLevel;
            set => _faithLevel = value;
        }
        public int Arcane
        {
            get => _arcaneLevel;
            set => _arcaneLevel = value;
        }
        #endregion

        // ---------------------------------------------------------------------------------------------------------------
        [Header("Life - HP")]
        [SerializeField] private float _maxHP;
        [SerializeField] private float _currentHP;
        public float MaxHP
        {
            get
            {
                _maxHP = AttributesCalculator.BaseMaxHP(this.Vigor);

                return _maxHP;
            }
        }
        public float CurrentHP => _currentHP;
        public void IncreaseHealth(float value)
        {
            _currentHP = Mathf.Clamp(_currentHP + value, 0, _maxHP);
        }
        public void TakeDamage(AttributesManager perpetratorAttributes, float value, float pierceValue, bool isMagic)
        {
            float initialHP = _currentHP;

            if (isMagic)
            {
                _currentHP = Mathf.Clamp(_currentHP - (value - Mathf.Max(0, _magicDefense - pierceValue)), 0, _maxHP);
                perpetratorAttributes.IncreaseHealth(Mathf.Abs(initialHP - _currentHP) * perpetratorAttributes.MagicLifeSteal);
            }
            else
            {
                _currentHP = Mathf.Clamp(_currentHP - (value - Mathf.Max(0, _physicalDefense - pierceValue)), 0, _maxHP);
                perpetratorAttributes.IncreaseHealth(Mathf.Abs(initialHP - _currentHP) * perpetratorAttributes.PhysicalLifeSteal);
            }

            if (_currentHP <= 0)
            {
                StopAllCoroutines();
                this.gameObject.SetActive(false);
            }
        }

        // ---------------------------------------------------------------------------------------------------------------
        [Header("FP")]
        [SerializeField] private float _maxFP;
        [SerializeField] private float _currentFP;
        public float MaxFP
        {
            get
            {
                _maxFP = AttributesCalculator.BaseMaxFP(this.Mind);

                return _maxFP;
            }
        }
        public float CurrentFP => _currentFP;
        public void ChangeFP(float addedvalue)
        {
            _currentFP = Mathf.Clamp(_currentFP + addedvalue, 0, _maxFP);
        }

        public void Respawn()
        {
            this.gameObject.SetActive(true);
            _currentHP = this.MaxHP;
            _currentFP = this.MaxFP;
        }

        // ---------------------------------------------------------------------------------------------------------------
        [Header("Movement")]
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _bonusMoveSpeed;
        private bool _isLockedMovement;
        public float MoveSpeed
        {
            get
            {
                if (!_isLockedMovement)
                {
                    return AttributesCalculator.BaseMoveSpeed(this.EquipmentsManager.EquipMoveSpeed, this.BonusMoveSpeed, (this.CurrentEquipLoad / this.MaxEquipLoad));
                }
                else
                {
                    return 0f;
                }
            }
        }
        public float BonusMoveSpeed
        {
            get => _bonusMoveSpeed;
            set => _bonusMoveSpeed = value;
        }
        public bool IsLockedMovement
        {
            get => _isLockedMovement;
            set => _isLockedMovement = value;
        }

        [Header("Equip Load")]
        [SerializeField] private float _maxEquipLoad;
        [SerializeField] private float _currentEquipLoad;
        public float MaxEquipLoad
        {
            get
            {
                _maxEquipLoad = AttributesCalculator.BaseMaxEquipLoad(this.Endurance);

                return _maxEquipLoad;
            }
        }
        public float CurrentEquipLoad => this.EquipmentsManager.EquipLoad;

        // ---------------------------------------------------------------------------------------------------------------
        [Header("Defense")]
        [SerializeField] private float _physicalDefense;
        [SerializeField] private float _magicDefense;
        public float BasePhysicalDefense => AttributesCalculator.BasePhysicalDefense(this.Endurance, this.Strength);
        public float EquipPhysicalDefense => this.EquipmentsManager.EquipPhysicalDefense;
        public float PhysicalDefense
        {
            get
            {
                _physicalDefense = this.BasePhysicalDefense + this.EquipPhysicalDefense;

                return _physicalDefense;
            }
        }
        public float BaseMagicDefense => AttributesCalculator.BaseMagicDefense(this.Endurance, this.Intelligence);
        public float EquipMagicDefense => this.EquipmentsManager.EquipMagicDefense;
        public float MagicDefense
        {
            get
            {
                _magicDefense = this.BaseMagicDefense + this.EquipMagicDefense;

                return _magicDefense;
            }
        }

        // ---------------------------------------------------------------------------------------------------------------
        [Header("Attack")]
        [SerializeField] private float _attackSpeed;
        public float AttackSpeed
        {
            get
            {
                _attackSpeed = AttributesCalculator.BaseAttackSpeed(this.Arcane);

                return _attackSpeed;
            }
        }

        // ---------------------------------------------------------------------------------------------------------------
        [Header("Physical Attack")]
        [SerializeField] private float _physicalDamage;
        [SerializeField] private float _physicalPierce;
        [SerializeField] private float _physicalLifeSteal;
        public float BasePhysicalDamage => AttributesCalculator.BasePhysicalDamage(this.Strength);
        public float EquipPhysicalDamage => this.EquipmentsManager.EquipPhysicalDamage;
        public float PhysicalDamage
        {
            get
            {
                _physicalDamage = this.BasePhysicalDamage + this.EquipPhysicalDamage;

                return _physicalDamage;
            }
        }
        public float BasePhysicalPierce => AttributesCalculator.BasePhysicalPierce(this.Dexterity);
        public float EquipPhysicalPierce => this.EquipmentsManager.EquipPhysicalPierce;
        public float PhysicalPierce
        {
            get
            {
                _physicalPierce = this.BasePhysicalPierce + this.EquipPhysicalPierce;

                return _physicalPierce;
            }
        }
        public float BasePhysicalLifeSteal => AttributesCalculator.BasePhysicalLifeSteal(this.Arcane, this.Dexterity);
        public float EquipPhysicalLifeSteal => this.EquipmentsManager.EquipPhysicalLifeSteal;
        public float PhysicalLifeSteal
        {
            get
            {
                _physicalLifeSteal = this.BasePhysicalLifeSteal + this.EquipPhysicalLifeSteal;

                return _physicalLifeSteal;
            }
        }

        // ---------------------------------------------------------------------------------------------------------------
        [Header("Magic Attack")]
        [SerializeField] private float _magicDamage;
        [SerializeField] private float _magicPierce;
        [SerializeField] private float _magicLifeSteal;
        public float BaseMagicDamage => AttributesCalculator.BaseMagicDamage(this.Intelligence);
        public float EquipMagicDamage => this.EquipmentsManager.EquipMagicDamage;
        public float MagicDamage
        {
            get
            {
                _magicDamage = this.BaseMagicDamage + this.EquipMagicDamage;

                return _magicDamage;
            }
        }
        public float BaseMagicPierce => AttributesCalculator.BaseMagicPierce(this.Faith);
        public float EquipMagicPierce => this.EquipmentsManager.EquipMagicPierce;
        public float MagicPierce
        {
            get
            {
                _magicPierce = this.BaseMagicPierce + this.EquipMagicPierce;

                return _magicPierce;
            }
        }
        public float BaseMagicLifeSteal => AttributesCalculator.BaseMagicLifeSteal(this.Arcane, this.Faith);
        public float EquipMagicLifeSteal => this.EquipmentsManager.EquipMagicLifeSteal;
        public float MagicLifeSteal
        {
            get
            {
                _magicLifeSteal = this.BaseMagicLifeSteal + this.EquipMagicLifeSteal;

                return _magicLifeSteal;
            }
        }

        private void Start()
        {
            this.Respawn();
        }

        private void FixedUpdate()
        {
            Debug.Log(MaxHP + MaxFP + MoveSpeed + AttackSpeed + PhysicalDamage + PhysicalPierce + PhysicalLifeSteal + PhysicalDefense + MagicDamage + MagicPierce + MagicLifeSteal + MagicDefense);
        }
    }
}