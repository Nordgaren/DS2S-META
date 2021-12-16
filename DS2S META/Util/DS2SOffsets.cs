using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS2S_META
{
    internal class DS2SOffsets
    {
        #region BaseA

        public const string BaseAAob = "48 8B 05 ? ? ? ? 48 8B 58 38 48 85 DB 74 ? F6";
        public const string BaseABabyJumpAoB = "49 BA ? ? ? ? ? ? ? ? 41 FF E2 90 74 2E";
        public const int BasePtrOffset1 = 0x3;
        public const int BasePtrOffset2 = 0x7;
        public const int PlayerTypeOffset = 0xB0;
        public enum PlayerType
        {
            ChrNetworkPhantomId = 0x3C,
            TeamType = 0x3D,
            CharType = 0x48
        }
        public const int PlayerNameOffset = 0xA8;
        public enum PlayerName
        {
            Name = 0x114
        }

        public const string ItemGiveFunc = "48 89 5c 24 18 56 57 41 56 48 83 ec 30";
        public const int AvailableItemBagOffset = 0x10;
        public const int ItemGiveWindowPointer = 0x22E0;
        public const string ItemStruct2dDisplay = "40 53 48 83 ec 20 45 33 d2 45 8b d8 48 8b d9 44 89 11";
        public const string DisplayItem = "48 8b 89 d8 00 00 00 48 85 c9 0f 85 40 5e 00 00";

        public const int PlayerBaseMiscOffset = 0xC0;
        public enum PlayerBaseMisc
        {
            Class = 0x64,
            NewGame = 0x68,
            SaveSlot = 0x18A8
        }
        public const int PlayerCtrlOffset = 0xD0;
        public enum PlayerCtrl
        {
            HP = 0x168,
            HPMin = 0x16C,
            HPMax = 0x170,
            HPCap = 0x174,
            SP = 0x1AC,
            SPMax = 0x1B4,
            SpeedModifier = 0x2A8,
        }

        public const int PlayerParamOffset = 0x490;
        public enum PlayerParam
        {
            SoulMemory = 0xF4,
            SoulMemory2 = 0xFC,
            MaxEquipLoad = 0x3C,
            Souls = 0xEC,
            TotalDeaths = 0x1A4,
            HollowLevel = 0x1AC,
            SinnerLevel = 0x1D6,
            SinnerPoints = 0x1D7
        }

        public enum Attributes
        {
            SoulLevel = 0xD0,
            VGR = 0x8,
            END = 0xA,
            VIT = 0xC,
            ATN = 0xE,
            STR = 0x10,
            DEX = 0x12,
            ADP = 0x18,
            INT = 0x14,
            FTH = 0x16
        }

        public enum Covenants
        {
            CurrentCovenant = 0x1AD,
            HeirsDiscovered = 0x1AF,
            HeirsRank = 0x1B9,
            HeirsProgress = 0x1C4,
            BlueSentinelsDiscovered = 0x1B0,
            BlueSentinelsRank = 0x1BA,
            BlueSentinelsProgress = 0x1C6,
            BrotherhoodDiscovered = 0x1B1,
            BrotherhoodRank = 0x1BB,
            BrotherhoodProgress = 0x1CB,
            WayBlueDiscovered = 0x1B2,
            WayBlueRank = 0x1BC,
            WayBlueProgress = 0x1CA,
            RatKingDiscovered = 0x1B3,
            RatKingRank = 0x1BD,
            RatKingProgress = 0x1CC,
            BellDiscovered = 0x1B4,
            BellRank = 0x1BE,
            BellProgress = 0x1CE,
            DragonDiscovered = 0x1B5,
            DragonRank = 0x1BF,
            DragonProgress = 0x1D0,
            CompanyDiscovered = 0x1B6,
            CompanyRank = 0x1C0,
            CompanyProgress = 0x1D2,
            PilgrimsDiscovered = 0x1B7,
            PilgrimsRank = 0x1C1,
            PilgrimsProgress = 0x1D4
        }

        public const int PlayerPositionOffset1 = 0xF8;
        public const int PlayerPositionOffset2 = 0xF0;

        public enum PlayerPosition
        {
            PosY = 0x20,
            PosZ = 0x24,
            PosX = 0x28,
            AngY = 0x34,
            AngZ = 0x38,
            AngX = 0x3C
        }

        public const int PlayerMapDataOffset1 = 0x100;
        public enum Gravity
        {
            Gravity = 0x134
        }
        public const int PlayerMapDataOffset2 = 0x320;
        public const int PlayerMapDataOffset3 = 0x20;
        public enum PlayerMapData
        {
            WarpBase = 0x1A0,
            WarpYA = 0x1A0,
            WarpZA = 0x1A4,
            WarpXA = 0x1A8,
            WarpYB = 0x1B0,
            WarpZB = 0x1B4,
            WarpXB = 0x1B8,
            WarpYC = 0x1C0,
            WarpZC = 0x1C4,
            WarpXC = 0x1C8
        }

        public const int CharacterFlagsOffset = 0x490;

        public const string GiveSoulsFunc = "48 83 ec 28 48 8b 01 48 85 c0 74 23 48 8b 80 b8 00 00 00";

      

        public const int BonfireOffset = 0x70;
        public enum Bonfire
        {
            LastSetBonfire = 0x16C
        }
        public const int BonfireLevelsOffset1 = 0x58;
        public const int BonfireLevelsOffset2 = 0x20;
        public enum BonfireLevels
        {
            FireKeepersDwelling = 0x2,
            Majula = 0x1A,
            CrestfallensRetreat = 0x62,
            CardinalTower = 0x32,
            SoldiersRest = 0x4A,
            ThePlaceUnbeknownst = 0x7A,
            HeidesRuin = 0x4B2,
            TowerofFlame = 0x49A,
            TheBlueCathedral = 0x4CA,
            UnseenPathtoHeide = 0x28A,
            ExileHoldingCells = 0x182,
            McDuffsWorkshop = 0x1CA,
            ServantsQuarters = 0x1E2,
            StraidsCell = 0x16A,
            TheTowerApart = 0x19A,
            TheSaltfort = 0x1FA,
            UpperRamparts = 0x1B2,
            UndeadRefuge = 0x362,
            BridgeApproach = 0x37A,
            UndeadLockaway = 0x392,
            UndeadPurgatory = 0x3AA,
            PoisonPool = 0x242,
            TheMines = 0x212,
            LowerEarthenPeak = 0x22A,
            CentralEarthenPeak = 0x25A,
            UpperEarthenPeak = 0x272,
            ThresholdBridge = 0x2BA,
            IronhearthHall = 0x2A2,
            EygilsIdol = 0x2D2,
            BelfrySolApproach = 0x2EA,
            OldAkelarre = 0x482,
            RuinedForkRoad = 0x4E2,
            ShadedRuins = 0x4FA,
            GyrmsRespite = 0x512,
            OrdealsEnd = 0x52A,
            RoyalArmyCampsite = 0x10A,
            ChapelThreshold = 0x122,
            LowerBrightstoneCove = 0xF2,
            HarvalsRestingPlace = 0x55A,
            GraveEntrance = 0x542,
            UpperGutter = 0x43A,
            CentralGutter = 0x40A,
            HiddenChamber = 0x422,
            BlackGulchMouth = 0x3F2,
            KingsGate = 0x302,
            UnderCastleDrangleic = 0x34A,
            ForgottenChamber = 0x332,
            CentralCastleDrangleic = 0x31A,
            TowerofPrayer = 0x92,
            CrumbledRuins = 0xAA,
            RhoysRestingPlace = 0xC2,
            RiseoftheDead = 0xDA,
            UndeadCryptEntrance = 0x3DA,
            UndeadDitch = 0x3C2,
            Foregarden = 0x13A,
            RitualSite = 0x152,
            DragonAerie = 0x452,
            ShrineEntrance = 0x46A,
            SanctumWalk = 0x572,
            PriestessChamber = 0x58A,
            HiddenSanctumChamber = 0x5BA,
            LairoftheImperfect = 0x5D2,
            SanctumInterior = 0x5EA,
            TowerofPrayerDLC = 0x602,
            SanctumNadir = 0x5A2,
            ThroneFloor = 0x61A,
            UpperFloor = 0x64A,
            Foyer = 0x632,
            LowermostFloor = 0x67A,
            TheSmelterThrone = 0x692,
            IronHallwayEntrance = 0x662,
            OuterWall = 0x6AA,
            AbandonedDwelling = 0x6C2,
            ExpulsionChamber = 0x70A,
            InnerWall = 0x722,
            LowerGarrison = 0x6DA,
            GrandCathedral = 0x6F2
        }
        #endregion

        #region BaseB
        public const string BaseBAoB = "48 8B 0D ? ? ? ? 48 85 C9 74 ? 48 8B 49 18 E8";
        public const int ConnectionOffset = 0x38;
        public enum Connection
        {
            Online = 0x8
        }

        #endregion

        #region FCData
        public const string CameraAoB = "60 02 2c f0 f3 7f 00 00";
        public const int CameraOffset1 = 0x0;
        public const int CameraOffset2 = 0x20;
        public enum Camera
        {
            CamStart = 0x170,
            CamX = 0x1A0,
            CamZ = 0x1A4,
            CamY = 0x1A8
        }
        #endregion

        #region Param

        public enum Param
        {
            TotalParamLength = 0x0,
            TableLength = 0x48
        }

        public const int ParamDataOffset1 = 0x18;
        public const int ParamDataOffset2 = 0xD8;
        public const int ParamDataOffset3 = 0xA8;

        public const int LevelUpSoulsParamOffset = 0x580;
        public const int WeaponParamOffset = 0x420;
        public enum WeaponParam
        {
            ReinforceID = 0x8
        }

        public const int WeaponReinforceParamOffset = 0x470;
        public enum WeaponReinforceParam
        {
            MaxUpgrade = 0x48,
            CustomAttrID = 0xE8
        }
        public const int CustomAttrSpecParamOffset = 0x4F0;

        public const int ArmorParamOffset = 0x4A0;
        public enum ArmorParam
        {
            ReinforceID = 0x8
        }
        public const int ArmorReinforceParamOffset = 0x4B0;
        public enum ArmorReinforceParam
        {
            MaxUpgrade = 0x60,
        }

        public const int ItemParamOffset = 0x20;
        public enum ItemParam
        {
            MaxHeld = 0x4A,
        }

        #endregion
    }
}
