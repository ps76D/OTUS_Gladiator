using System;
using GameEngine;
using GameEngine.ActionsSystem;
using GameEngine.CharacterSystem;
using GameEngine.DaySystem;
using UnityEngine;

namespace PlayerProfileSystem
{
    [Serializable]
    public class PlayerProfile : IDisposable
    {
        /*[Inject]*/
        [SerializeField] private MoneyStorage _moneyStorage;
        [SerializeField] private DayService _dayService;

        
        [SerializeField] private CharacterService _characterService;
        [SerializeField] private MoralService _moralService;
        

        
        /*[Inject]
        private ProfileService _profileService;*/
        
        /*[Inject]
        [SerializeField] private ResourceService _resourceService;
        
        [Inject]
        [SerializeField] private UnitManager _unitManager;*/
        
        /*[Inject]
        private IEnumerable<Resource> _resources;
        
        [Inject]
        private IEnumerable<Unit> _units;*/
        public MoneyStorage MoneyStorage => _moneyStorage;
        public DayService DayService => _dayService;

        public CharacterService CharacterService => _characterService;
        public MoralService MoralService => _moralService;
        
        /*public ResourceService ResourceService => _resourceService;
        public UnitManager UnitManager => _unitManager;*/

        /*[SerializeField] private MoneyData _moneyData;*/
        /*[SerializeField] private int Day;
        public int Day {
            get => Day;
            set => Day = value;
        }*/


        public PlayerProfile(MoneyStorage moneyStorage, DayService dayService,  CharacterService characterService,
            MoralService moralService)
        {
            _moneyStorage = moneyStorage;
            _dayService = dayService;
            _characterService = characterService;
            _moralService = moralService;
        }
        
        /*public void InitializeNewGame()
        {
            _profileService.InitializeNewGameProfile();
            
            /*_resourceService.SetResources(_resources);#1#
            /*_unitManager.SetupUnits(_units);#1#
        }*/
        
        /*public void Initialize()
        {
            _moneyStorage.SetupMoney(_moneyStorage.Money);
            
            /*_resourceService.SetResources(_resources);#1#
            /*_unitManager.SetupUnits(_units);#1#
        }*/
        public void Dispose()
        {
            if (CharacterService != null)
            {
                (CharacterService.CurrentCharacterProfile as IDisposable)?.Dispose();
            }
            
            Debug.Log("Dispose Character Profile");
        }
    }
}