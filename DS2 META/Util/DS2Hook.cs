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
        private PHPointer PlayerCtrl;
        private PHPointer PlayerPosition;
        private PHPointer PlayerParam;

        public bool Loaded => PlayerCtrl != null && PlayerCtrl.Resolve() != IntPtr.Zero;

        public DS2Hook(int refreshInterval, int minLifetime) :
            base(refreshInterval, minLifetime, p => p.MainWindowTitle == "DARK SOULS II")
        {
            Version = "None";

            BaseASetup = RegisterAbsoluteAOB(DS2Offsets.BaseAAob);

            OnHooked += DSHook_OnHooked;
            OnUnhooked += DSHook_OnUnhooked;
        }
        private void DSHook_OnHooked(object sender, PHEventArgs e)
        {
            BaseA = CreateBasePointer(BaseAPointBaseANoOff());
            PlayerName = CreateChildPointer(BaseA, (int)DS2Offsets.PlayerNameOffset);
            PlayerBaseMisc = CreateChildPointer(PlayerName, (int)DS2Offsets.PlayerBaseMiscOffset);
            PlayerCtrl = CreateChildPointer(BaseA, (int)DS2Offsets.PlayerCtrlOffset);
            PlayerPosition = CreateChildPointer(PlayerCtrl, (int)DS2Offsets.PlayerPositionOffset1, (int)DS2Offsets.PlayerPositionOffset2);
            PlayerParam = CreateChildPointer(PlayerCtrl, (int)DS2Offsets.PlayerParamOffset);
            UpdateStatsProperties();
        }
        private void DSHook_OnUnhooked(object sender, PHEventArgs e)
        {
        }
        public void UpdateStatsProperties()
        {
            OnPropertyChanged(nameof(SoulLevel));
            OnPropertyChanged(nameof(SoulMemory));
            OnPropertyChanged(nameof(SoulMemory2));
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

        public void UpdatePlayerProperties()
        {
            OnPropertyChanged(nameof(Health));
            OnPropertyChanged(nameof(MaxHealth));
            OnPropertyChanged(nameof(ModMaxHealth));
            OnPropertyChanged(nameof(Stamina));
            OnPropertyChanged(nameof(MaxStamina));
            OnPropertyChanged(nameof(PosX));
            OnPropertyChanged(nameof(PosY));
            OnPropertyChanged(nameof(PosZ));
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

        #region Player
        public int Health
        {
            get => Loaded ? PlayerCtrl.ReadInt32((int)DS2Offsets.PlayerCtrl.HP) : 0;
            set 
            {
                if (Reading || !Loaded) return;
                PlayerCtrl.WriteInt32((int)DS2Offsets.PlayerCtrl.HP, value);
                OnPropertyChanged(nameof(Health));
            }
        }
        public int MaxHealth
        {
            get => Loaded ? PlayerCtrl.ReadInt32((int)DS2Offsets.PlayerCtrl.HPMax) : 0;
            set => PlayerCtrl.WriteInt32((int)DS2Offsets.PlayerCtrl.HPMax, value);
        }
        public int ModMaxHealth
        {
            get => Loaded ? PlayerCtrl.ReadInt32((int)DS2Offsets.PlayerCtrl.HPCap) : 0;
            set => PlayerCtrl.WriteInt32((int)DS2Offsets.PlayerCtrl.HPCap, value);
        }
        public float Stamina
        {
            get => Loaded ? PlayerCtrl.ReadSingle((int)DS2Offsets.PlayerCtrl.SP) : 0;
            set 
            { 
                if (Reading || !Loaded) return;
                PlayerCtrl.WriteSingle((int)DS2Offsets.PlayerCtrl.SP, value);
                OnPropertyChanged(nameof(Stamina));
            }
        }
        public float MaxStamina
        {
            get => Loaded ? PlayerCtrl.ReadSingle((int)DS2Offsets.PlayerCtrl.SPMax) : 0;
            set => PlayerCtrl.WriteSingle((int)DS2Offsets.PlayerCtrl.SPMax, value);
        }
        public float PosX
        {
            get => Loaded ? PlayerPosition.ReadSingle((int)DS2Offsets.PlayerPosition.PosX) : 0;
            set => PlayerPosition.WriteSingle((int)DS2Offsets.PlayerPosition.PosX, value);
        }
        public float PosY
        {
            get => Loaded ? PlayerPosition.ReadSingle((int)DS2Offsets.PlayerPosition.PosY) : 0;
            set => PlayerPosition.WriteSingle((int)DS2Offsets.PlayerPosition.PosY, value);
        }
        public float PosZ
        {
            get => Loaded ? PlayerPosition.ReadSingle((int)DS2Offsets.PlayerPosition.PosZ) : 0;
            set => PlayerPosition.WriteSingle((int)DS2Offsets.PlayerPosition.PosZ, value);
        }
        #endregion

        #region Stats
        public string Name
        {
            get => Loaded ? PlayerName.ReadString((int)DS2Offsets.PlayerName.Name, Encoding.Unicode, 0x22) : "";
            set
            {
                if (Reading) return;
                PlayerName.WriteString((int)DS2Offsets.PlayerName.Name, Encoding.Unicode, 0x22, value);
                OnPropertyChanged(nameof(Name));
            }
        }

        public byte Class
        {
            get => Loaded ? PlayerBaseMisc.ReadByte((int)DS2Offsets.PlayerBaseMisc.Class) : (byte)255;
            set => PlayerBaseMisc.WriteByte((int)DS2Offsets.PlayerBaseMisc.Class, value);
        }
        public int SoulLevel
        {
            get => Loaded ? PlayerParam.ReadInt32((int)DS2Offsets.Attributes.SoulLevel) : 0;
            set => PlayerParam.WriteInt32((int)DS2Offsets.Attributes.SoulLevel, value);
        }
        public int SoulMemory
        {
            get => Loaded ? PlayerParam.ReadInt32((int)DS2Offsets.PlayerParam.SoulMemory) : 0;
            set => PlayerParam.WriteInt32((int)DS2Offsets.PlayerParam.SoulMemory, value);
        }
        public int SoulMemory2
        {
            get => Loaded ? PlayerParam.ReadInt32((int)DS2Offsets.PlayerParam.SoulMemory2) : 0;
            set => PlayerParam.WriteInt32((int)DS2Offsets.PlayerParam.SoulMemory2, value);
        }
        public int Souls
        {
            get => Loaded ? PlayerParam.ReadInt32((int)DS2Offsets.PlayerParam.Souls) : 0;
            set
            {
                if (Reading) return;
                var diff = value - Souls;
                if (diff < 0)
                    diff = 0;
                PlayerParam.WriteInt32((int)DS2Offsets.PlayerParam.Souls, value);
                SoulMemory += diff;
                SoulMemory2 += diff;
            }
        }
        private short _vigor;
        public short Vigor
        {
            get => Loaded ? PlayerParam.ReadInt16((int)DS2Offsets.Attributes.VGR) : (short)0;
            set
            {
                if (Reading) return;
                PlayerParam.WriteInt16((int)DS2Offsets.Attributes.VGR, value);
                UpdateSoulLevel();
            }
        }
        private short _endurance;
        public short Endurance
        {
            get => Loaded ? PlayerParam.ReadInt16((int)DS2Offsets.Attributes.END) : (short)0;
            set 
            { 
                if (Reading) return;
                PlayerParam.WriteInt16((int)DS2Offsets.Attributes.END, value);
                UpdateSoulLevel();
            }
        }
        private short _vitality;
        public short Vitality
        {
            get => Loaded ? PlayerParam.ReadInt16((int)DS2Offsets.Attributes.VIT) : (short)0;
            set
            {
                if (Reading) return;
                PlayerParam.WriteInt16((int)DS2Offsets.Attributes.VIT, value);
                UpdateSoulLevel();
            }
        }
        private short _attunement;
        public short Attunement
        {
            get => Loaded ? PlayerParam.ReadInt16((int)DS2Offsets.Attributes.ATN) : (short)0;
            set
            {
                if (Reading) return;
                PlayerParam.WriteInt16((int)DS2Offsets.Attributes.ATN, value);
                UpdateSoulLevel();
            }
        }
        private short _strength;
        public short Strength
        {
            get => Loaded ? PlayerParam.ReadInt16((int)DS2Offsets.Attributes.STR) : (short)0;
            set
            {
                if (Reading) return;
                PlayerParam.WriteInt16((int)DS2Offsets.Attributes.STR, value);
                UpdateSoulLevel();
            }
        }
        private short _dexterity;
        public short Dexterity
        {
            get => Loaded ? PlayerParam.ReadInt16((int)DS2Offsets.Attributes.DEX) : (short)0;
            set
            {
                if (Reading) return;
                PlayerParam.WriteInt16((int)DS2Offsets.Attributes.DEX, value);
                UpdateSoulLevel();
            }
        }
        private short _adaptability;
        public short Adaptability
        {
            get => Loaded ? PlayerParam.ReadInt16((int)DS2Offsets.Attributes.ADP) : (short)0;
            set
            {
                if (Reading) return;
                PlayerParam.WriteInt16((int)DS2Offsets.Attributes.ADP, value);
                UpdateSoulLevel();
            }
        }
        private short _intelligence;
        public short Intelligence
        {
            get => Loaded ? PlayerParam.ReadInt16((int)DS2Offsets.Attributes.INT) : (short)0;
            set
            {
                if (Reading) return;
                PlayerParam.WriteInt16((int)DS2Offsets.Attributes.INT, value);
                UpdateSoulLevel();
            }
        }
        private short _faith;
        public short Faith
        {
            get => Loaded ? PlayerParam.ReadInt16((int)DS2Offsets.Attributes.FTH) : (short)0;
            set
            {
                if (Reading) return;
                PlayerParam.WriteInt16((int)DS2Offsets.Attributes.FTH, value);
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
