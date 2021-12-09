using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS2_META
{
    internal class DS2Offsets
    {
        public const string BaseAAob = "48 8B 05 ? ? ? ? 48 8B 58 38 48 85 DB 74 ? F6";
        public const int BasePtrOffset1 = 0x3;
        public const int BasePtrOffset2 = 0x7;
        public const int PlayerBaseOffset = 0xD0;
        public enum PlayerBase
        {
            HP = 0x168,
            HPMin = 0x16C,
            HPMax = 0x170,
            HPCap = 0x174,
            SP = 0x1AC,
            SPMax = 0x1B4,
            SpeedModifier = 0x2AB,
        }

        public const int PlayerDataBaseOffset = 0x490;
        public enum PlayerDataBase
        {
            SoulMemory = 0xF4,
            SoulMemory2 = 0xFC,
            MaxEquipLoad = 0x3C,
            TotalGetSoul = 0xEC,
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

        public const int CharacterFlagsOffset = 0x490;

    }
}
