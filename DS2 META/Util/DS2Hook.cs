using PropertyHook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DS2_META
{
    public class DS2Hook : PHook, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public int ID => Process?.Id ?? -1;
        public string Version { get; private set; }

        public static bool Reading { get; set; }

        private PHPointer BaseASetup;
        private PHPointer BaseA;
        private PHPointer PlayerName;
        private PHPointer PlayerBaseMisc;
        private PHPointer PlayerBase;
        private PHPointer PlayerDataBase;

        public bool Loaded => PlayerBase.Resolve() != IntPtr.Zero;

        public DS2Hook(int refreshInterval, int minLifetime) :
            base(refreshInterval, minLifetime, p => p.MainWindowTitle == "DARK SOULS II")
        {
            Version = "None";

            BaseASetup = RegisterAbsoluteAOB(DS2Offsets.BaseAAob);

            OnHooked += DSHook_OnHooked;
            OnUnhooked += DSHook_OnUnhooked;
        }

        public void UpdateProperties()
        {
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Vigor));
            OnPropertyChanged(nameof(Endurance));
            OnPropertyChanged(nameof(Vitality));
            OnPropertyChanged(nameof(Attunement));
            OnPropertyChanged(nameof(Strength));
            OnPropertyChanged(nameof(Dexterity));
            OnPropertyChanged(nameof(Adaptability));
            OnPropertyChanged(nameof(Intelligence));
            OnPropertyChanged(nameof(Faith));
        }

        private void DSHook_OnHooked(object sender, PHEventArgs e)
        {
            BaseA = CreateBasePointer(BaseAPointBaseANoOff());
            PlayerName = CreateChildPointer(BaseA, (int)DS2Offsets.PlayerNameOffset);
            PlayerBaseMisc = CreateChildPointer(PlayerName, (int)DS2Offsets.PlayerBaseMiscOffset);
            PlayerBase = CreateChildPointer(BaseA, (int)DS2Offsets.PlayerBaseOffset);
            PlayerDataBase = CreateChildPointer(PlayerBase, (int)DS2Offsets.PlayerDataBaseOffset);
            UpdateProperties();
        }
        private void DSHook_OnUnhooked(object sender, PHEventArgs e)
        {
        }
        public IntPtr BaseAPointBaseANoOff(PHPointer pointer)
        {
            var readInt = pointer.ReadInt32(DS2Offsets.BasePtrOffset1);
            var intPtr = pointer.ReadIntPtr(readInt + DS2Offsets.BasePtrOffset2);

            return intPtr;
        }

        public IntPtr BaseAPointBaseANoOff()
        {
            var readInt = BaseASetup.ReadInt32(DS2Offsets.BasePtrOffset1);
            var intPtr = BaseASetup.ReadIntPtr(readInt + DS2Offsets.BasePtrOffset2);

            return intPtr;
        }

        #region Stats
        public string Name
        {
            get => Loaded ? PlayerName.ReadString((int)DS2Offsets.PlayerName.Name, Encoding.Unicode, 0x22) : "";
            set => PlayerName.WriteString((int)DS2Offsets.PlayerName.Name, Encoding.Unicode, 0x22, value);
        }

        public byte Class
        {
            get => Loaded ? PlayerBaseMisc.ReadByte((int)DS2Offsets.PlayerBaseMisc.Class) : (byte)255;
            set => PlayerBaseMisc.WriteByte((int)DS2Offsets.PlayerBaseMisc.Class, value);
        }
        public int SoulLevel
        {
            get => Loaded ? PlayerDataBase.ReadInt32((int)DS2Offsets.Attributes.SoulLevel) : 0;
            set => PlayerDataBase.WriteInt32((int)DS2Offsets.Attributes.SoulLevel, value);
        }
        public int SoulMemory
        {
            get => Loaded ? PlayerDataBase.ReadInt32((int)DS2Offsets.PlayerDataBase.SoulMemory) : 0;
            set => PlayerDataBase.WriteInt32((int)DS2Offsets.PlayerDataBase.SoulMemory, value);
        }
        public int SoulMemory2
        {
            get => Loaded ? PlayerDataBase.ReadInt32((int)DS2Offsets.PlayerDataBase.SoulMemory2) : 0;
            set => PlayerDataBase.WriteInt32((int)DS2Offsets.PlayerDataBase.SoulMemory2, value);
        }
        private short _vigor;
        public short Vigor
        {
            get => Loaded ? PlayerDataBase.ReadInt16((int)DS2Offsets.Attributes.VGR) : (short)0;
            set
            {
                if (Reading) return;
                PlayerDataBase.WriteInt16((int)DS2Offsets.Attributes.VGR, value);
                UpdateSoulLevel();
            }
        }
        private short _endurance;
        public short Endurance
        {
            get => Loaded ? PlayerDataBase.ReadInt16((int)DS2Offsets.Attributes.END) : (short)0;
            set 
            { 
                if (Reading) return;
                PlayerDataBase.WriteInt16((int)DS2Offsets.Attributes.END, value);
                UpdateSoulLevel();
            }
        }
        private short _vitality;
        public short Vitality
        {
            get => Loaded ? PlayerDataBase.ReadInt16((int)DS2Offsets.Attributes.VIT) : (short)0;
            set
            {
                if (Reading) return;
                PlayerDataBase.WriteInt16((int)DS2Offsets.Attributes.VIT, value);
                UpdateSoulLevel();
            }
        }
        private short _attunement;
        public short Attunement
        {
            get => Loaded ? PlayerDataBase.ReadInt16((int)DS2Offsets.Attributes.ATN) : (short)0;
            set
            {
                if (Reading) return;
                PlayerDataBase.WriteInt16((int)DS2Offsets.Attributes.ATN, value);
                UpdateSoulLevel();
            }
        }
        private short _strength;
        public short Strength
        {
            get => Loaded ? PlayerDataBase.ReadInt16((int)DS2Offsets.Attributes.STR) : (short)0;
            set
            {
                if (Reading) return;
                PlayerDataBase.WriteInt16((int)DS2Offsets.Attributes.STR, value);
                UpdateSoulLevel();
            }
        }
        private short _dexterity;
        public short Dexterity
        {
            get => Loaded ? PlayerDataBase.ReadInt16((int)DS2Offsets.Attributes.DEX) : (short)0;
            set
            {
                if (Reading) return;
                PlayerDataBase.WriteInt16((int)DS2Offsets.Attributes.DEX, value);
                UpdateSoulLevel();
            }
        }
        private short _adaptability;
        public short Adaptability
        {
            get => Loaded ? PlayerDataBase.ReadInt16((int)DS2Offsets.Attributes.ADP) : (short)0;
            set
            {
                if (Reading) return;
                PlayerDataBase.WriteInt16((int)DS2Offsets.Attributes.ADP, value);
                UpdateSoulLevel();
            }
        }
        private short _intelligence;
        public short Intelligence
        {
            get => Loaded ? PlayerDataBase.ReadInt16((int)DS2Offsets.Attributes.INT) : (short)0;
            set
            {
                if (Reading) return;
                PlayerDataBase.WriteInt16((int)DS2Offsets.Attributes.INT, value);
                UpdateSoulLevel();
            }
        }
        private short _faith;
        public short Faith
        {
            get => Loaded ? PlayerDataBase.ReadInt16((int)DS2Offsets.Attributes.FTH) : (short)0;
            set
            {
                if (Reading) return;
                PlayerDataBase.WriteInt16((int)DS2Offsets.Attributes.FTH, value);
                UpdateSoulLevel();
            }
        }
        private void UpdateSoulLevel()
        {
            var charClass = DS2Class.All.FirstOrDefault(c => c.ID == Class);
            if (charClass == null) return;

            var soulLevel = GetSoulLevel(charClass);
            SoulLevel = soulLevel;
            var reqSoulMemory = GetRequiredSoulMemory(soulLevel, charClass.SoulLevel);
            if (reqSoulMemory > SoulMemory)
            {
                SoulMemory = reqSoulMemory;
                SoulMemory2 = reqSoulMemory;
            }
        }
        private int GetSoulLevel(DS2Class charClass)
        {
            int sl = charClass.SoulLevel;
            sl += Vigor - charClass.Vigor;
            sl += Attunement - charClass.Attunement;
            sl += Vitality - charClass.Vitality;
            sl += Endurance - charClass.Endurance;
            sl += Strength - charClass.Strength;
            sl += Dexterity - charClass.Dexterity;
            sl += Adaptability - charClass.Adaptability;
            sl += Intelligence - charClass.Intelligence;
            sl += Faith - charClass.Faith;
            return sl;
        }

        private int GetRequiredSoulMemory(int SL, int baseSL)
        {
            int soulMemory = 0;
            for (int i = baseSL; i < SL; i++)
            {
                soulMemory += DS2Level.Levels[i].Cost;
            }
            return soulMemory;
        }

        #endregion
    }
}
