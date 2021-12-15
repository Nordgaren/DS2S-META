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
        public const int BasePtrOffset1 = 0x3;
        public const int BasePtrOffset2 = 0x7;
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
        public const int ParamDataOffset3 = 0xD8;

        public const int LevelUpSoulsParam = 0x580;
        public const int WeaponParam = 0x420;
        public enum WeaponParams
        {
            ReinforceID = 0x8
        }

        public const int WeaponReinforceParam = 0x470;
        public enum WeaponReinforceParams
        {
            MaxUpgrade = 0x48,
            CustomAttrID = 0xE8
        }
        public const int CustomAttrSpecParam = 0x4F0;

        #endregion
    }
}
