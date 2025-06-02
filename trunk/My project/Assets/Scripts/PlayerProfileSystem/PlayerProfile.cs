using System;
using GameEngine;
using GameEngine.CharacterSystem;
using UnityEngine;

namespace PlayerProfileSystem
{
    [Serializable]
    public class PlayerProfile
    {
        /*[Inject]*/
        [SerializeField] private MoneyStorage _moneyStorage;
        [SerializeField] private DayService _dayService;
        [SerializeField] private ActionsService _actionsService;
        
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
        public ActionsService ActionsService => _actionsService;
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


        public PlayerProfile(MoneyStorage moneyStorage, DayService dayService, ActionsService actionsService, CharacterService characterService,
            MoralService moralService)
        {
            _moneyStorage = moneyStorage;
            _dayService = dayService;
            _actionsService = actionsService;
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
    }
}