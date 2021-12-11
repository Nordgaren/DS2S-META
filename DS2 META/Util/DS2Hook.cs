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
        public IntPtr BaseAddress => Process?.MainModule.BaseAddress ?? IntPtr.Zero;
        public string Version { get; private set; }

        public static bool Reading { get; set; }

        private PHPointer BaseASetup;
        private PHPointer GiveSoulsPtr;
        private PHPointer BaseA;
        private PHPointer PlayerName;
        private PHPointer AvailableItemBag;
        private PHPointer ItemGiveWindow;
        private PHPointer PlayerBaseMisc;
        private PHPointer PlayerCtrl;
        private PHPointer PlayerPosition;
        private PHPointer PlayerGravity;
        private PHPointer PlayerMapData;
        private PHPointer PlayerParam;

        public bool Loaded => PlayerCtrl != null && PlayerCtrl.Resolve() != IntPtr.Zero;

        public DS2Hook(int refreshInterval, int minLifetime) :
            base(refreshInterval, minLifetime, p => p.MainWindowTitle == "DARK SOULS II")
        {
            Version = "None";

            BaseASetup = RegisterAbsoluteAOB(DS2Offsets.BaseAAob);
            GiveSoulsPtr = RegisterAbsoluteAOB(DS2Offsets.GiveSoulsAoB);

            OnHooked += DSHook_OnHooked;
            OnUnhooked += DSHook_OnUnhooked;
        }
        private void DSHook_OnHooked(object sender, PHEventArgs e)
        {
            BaseA = CreateBasePointer(BaseAPointBaseANoOff(BaseASetup));
            PlayerName = CreateChildPointer(BaseA, (int)DS2Offsets.PlayerNameOffset);
            AvailableItemBag = CreateChildPointer(PlayerName, (int)DS2Offsets.AvailableItemBagOffset, (int)DS2Offsets.AvailableItemBagOffset);
            ItemGiveWindow = CreateChildPointer(BaseA, (int)DS2Offsets.ItemGiveWindowPointer);
            PlayerBaseMisc = CreateChildPointer(PlayerName, (int)DS2Offsets.PlayerBaseMiscOffset);
            PlayerCtrl = CreateChildPointer(BaseA, (int)DS2Offsets.PlayerCtrlOffset);
            PlayerPosition = CreateChildPointer(PlayerCtrl, (int)DS2Offsets.PlayerPositionOffset1, (int)DS2Offsets.PlayerPositionOffset2);
            PlayerGravity = CreateChildPointer(PlayerCtrl, (int)DS2Offsets.PlayerMapDataOffset1);
            PlayerParam = CreateChildPointer(PlayerCtrl, (int)DS2Offsets.PlayerParamOffset);
            PlayerMapData = CreateChildPointer(PlayerGravity, (int)DS2Offsets.PlayerMapDataOffset2, (int)DS2Offsets.PlayerMapDataOffset3);
            UpdateStatsProperties();
            
        }
        private void DSHook_OnUnhooked(object sender, PHEventArgs e)
        {
        }
        public void UpdateStatsProperties()
        {
            OnPropertyChanged(nameof(SoulLevel));
            OnPropertyChanged(nameof(Souls));
            OnPropertyChanged(nameof(SoulMemory));
            OnPropertyChanged(nameof(SoulMemory2));
            OnPropertyChanged(nameof(HollowLevel));
            OnPropertyChanged(nameof(SinnerLevel));
            OnPropertyChanged(nameof(SinnerPoints));
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
            OnPropertyChanged(nameof(HealthMax));
            OnPropertyChanged(nameof(HealthCap));
            OnPropertyChanged(nameof(Stamina));
            OnPropertyChanged(nameof(MaxStamina));
            OnPropertyChanged(nameof(PosX));
            OnPropertyChanged(nameof(PosY));
            OnPropertyChanged(nameof(PosZ));
            OnPropertyChanged(nameof(StableX));
            OnPropertyChanged(nameof(StableY));
            OnPropertyChanged(nameof(StableZ));
            OnPropertyChanged(nameof(Gravity));
        }
        public IntPtr BaseAPointBaseANoOff(PHPointer pointer)
        {
            var readInt = pointer.ReadInt32(DS2Offsets.BasePtrOffset1);
            return pointer.ReadIntPtr(readInt + DS2Offsets.BasePtrOffset2);
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
        public int HealthMax
        {
            get => Loaded ? PlayerCtrl.ReadInt32((int)DS2Offsets.PlayerCtrl.HPMax) : 0;
            set => PlayerCtrl.WriteInt32((int)DS2Offsets.PlayerCtrl.HPMax, value);
        }
        public int HealthCap
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
        }
        public float PosY
        {
            get => Loaded ? PlayerPosition.ReadSingle((int)DS2Offsets.PlayerPosition.PosY) : 0;
        }
        public float PosZ
        {
            get => Loaded ? PlayerPosition.ReadSingle((int)DS2Offsets.PlayerPosition.PosZ) : 0;
        }
        public float StableX
        {
            get => Loaded ? PlayerMapData.ReadSingle((int)DS2Offsets.PlayerMapData.WarpXA) : 0;
        }
        public float StableY
        {
            get => Loaded ? PlayerMapData.ReadSingle((int)DS2Offsets.PlayerMapData.WarpYA) : 0;
        }
        public float StableZ
        {
            get => Loaded ? PlayerMapData.ReadSingle((int)DS2Offsets.PlayerMapData.WarpZA) : 0;
        }
        public float Speed
        {
            set 
            {
                if (Reading || !Loaded) return;
                PlayerCtrl.WriteSingle((int)DS2Offsets.PlayerCtrl.SpeedModifier, value); 
            }
        }
        public bool Gravity
        {
            get => Loaded ? PlayerGravity.ReadBoolean((int)DS2Offsets.Gravity.Gravity) : false;
            set => PlayerGravity.WriteBoolean((int)DS2Offsets.Gravity.Gravity, value);
        }

        public void PosWarp(float x, float y, float z)
        {
            PlayerMapData.WriteSingle((int)DS2Offsets.PlayerMapData.WarpXA, x);
            PlayerMapData.WriteSingle((int)DS2Offsets.PlayerMapData.WarpYA, y);
            PlayerMapData.WriteSingle((int)DS2Offsets.PlayerMapData.WarpZA, z);

            PlayerMapData.WriteSingle((int)DS2Offsets.PlayerMapData.WarpXB, x);
            PlayerMapData.WriteSingle((int)DS2Offsets.PlayerMapData.WarpYB, y);
            PlayerMapData.WriteSingle((int)DS2Offsets.PlayerMapData.WarpZB, z);

            PlayerMapData.WriteSingle((int)DS2Offsets.PlayerMapData.WarpXC, x);
            PlayerMapData.WriteSingle((int)DS2Offsets.PlayerMapData.WarpYC, y);
            PlayerMapData.WriteSingle((int)DS2Offsets.PlayerMapData.WarpZC, z);
        }
        #endregion

        #region Stats
        public string Name
        {
            get => Loaded ? PlayerName.ReadString((int)DS2Offsets.PlayerName.Name, Encoding.Unicode, 0x22) : "";
            set
            {
                if (Reading || !Loaded) return;
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
        public byte SinnerLevel
        {
            get => Loaded ? PlayerParam.ReadByte((int)DS2Offsets.PlayerParam.SinnerLevel) : (byte)0;
            set => PlayerParam.WriteByte((int)DS2Offsets.PlayerParam.SinnerLevel, value);
        }
        public byte SinnerPoints
        {
            get => Loaded ? PlayerParam.ReadByte((int)DS2Offsets.PlayerParam.SinnerPoints) : (byte)0;
            set => PlayerParam.WriteByte((int)DS2Offsets.PlayerParam.SinnerPoints, value);
        }
        public byte HollowLevel
        {
            get => Loaded ? PlayerParam.ReadByte((int)DS2Offsets.PlayerParam.HollowLevel) : (byte)0;
            set => PlayerParam.WriteByte((int)DS2Offsets.PlayerParam.HollowLevel, value);
        }
        public int Souls
        {
            get => Loaded ? PlayerParam.ReadInt32((int)DS2Offsets.PlayerParam.Souls) : 0;
        }

        public short Vigor
        {
            get => Loaded ? PlayerParam.ReadInt16((int)DS2Offsets.Attributes.VGR) : (short)0;
            set
            {
                if (Reading || !Loaded) return;
                PlayerParam.WriteInt16((int)DS2Offsets.Attributes.VGR, value);
                UpdateSoulLevel();
            }
        }
        public short Endurance
        {
            get => Loaded ? PlayerParam.ReadInt16((int)DS2Offsets.Attributes.END) : (short)0;
            set 
            { 
                if (Reading || !Loaded) return;
                PlayerParam.WriteInt16((int)DS2Offsets.Attributes.END, value);
                UpdateSoulLevel();
            }
        }
        public short Vitality
        {
            get => Loaded ? PlayerParam.ReadInt16((int)DS2Offsets.Attributes.VIT) : (short)0;
            set
            {
                if (Reading || !Loaded) return;
                PlayerParam.WriteInt16((int)DS2Offsets.Attributes.VIT, value);
                UpdateSoulLevel();
            }
        }
        public short Attunement
        {
            get => Loaded ? PlayerParam.ReadInt16((int)DS2Offsets.Attributes.ATN) : (short)0;
            set
            {
                if (Reading || !Loaded) return;
                PlayerParam.WriteInt16((int)DS2Offsets.Attributes.ATN, value);
                UpdateSoulLevel();
            }
        }
        public short Strength
        {
            get => Loaded ? PlayerParam.ReadInt16((int)DS2Offsets.Attributes.STR) : (short)0;
            set
            {
                if (Reading || !Loaded) return;
                PlayerParam.WriteInt16((int)DS2Offsets.Attributes.STR, value);
                UpdateSoulLevel();
            }
        }
        public short Dexterity
        {
            get => Loaded ? PlayerParam.ReadInt16((int)DS2Offsets.Attributes.DEX) : (short)0;
            set
            {
                if (Reading || !Loaded) return;
                PlayerParam.WriteInt16((int)DS2Offsets.Attributes.DEX, value);
                UpdateSoulLevel();
            }
        }
        public short Adaptability
        {
            get => Loaded ? PlayerParam.ReadInt16((int)DS2Offsets.Attributes.ADP) : (short)0;
            set
            {
                if (Reading || !Loaded) return;
                PlayerParam.WriteInt16((int)DS2Offsets.Attributes.ADP, value);
                UpdateSoulLevel();
            }
        }
        public short Intelligence
        {
            get => Loaded ? PlayerParam.ReadInt16((int)DS2Offsets.Attributes.INT) : (short)0;
            set
            {
                if (Reading || !Loaded) return;
                PlayerParam.WriteInt16((int)DS2Offsets.Attributes.INT, value);
                UpdateSoulLevel();
            }
        }
        public short Faith
        {
            get => Loaded ? PlayerParam.ReadInt16((int)DS2Offsets.Attributes.FTH) : (short)0;
            set
            {
                if (Reading || !Loaded) return;
                PlayerParam.WriteInt16((int)DS2Offsets.Attributes.FTH, value);
                UpdateSoulLevel();
            }
        }
        public void GiveSouls(int souls)
        {
            var asm = (byte[])DS2Assembly.AddSouls.Clone();

            var bytes = BitConverter.GetBytes(PlayerParam.Resolve().ToInt64());
            Array.Copy(bytes, 0, asm, 0x6, 8);
            bytes = BitConverter.GetBytes(souls);
            Array.Copy(bytes, 0, asm, 0x11, 4);
            bytes = BitConverter.GetBytes(GiveSoulsPtr.Resolve().ToInt64());
            Array.Copy(bytes, 0, asm, 0x17, 8);
            Execute(asm);
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
        public void ResetSoulMemory()
        {
            var charClass = DS2Class.All.FirstOrDefault(c => c.ID == Class);
            if (charClass == null) return;

            var soulLevel = GetSoulLevel(charClass);
            var reqSoulMemory = GetRequiredSoulMemory(soulLevel, charClass.SoulLevel);

            SoulMemory = reqSoulMemory;
            SoulMemory2 = reqSoulMemory;
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

        #region Items
        public void GetItem(int amount, uint item)
        {
            var itemStruct = Allocate(0x8A);
            Kernel32.WriteBytes(Handle, itemStruct + 0x4, BitConverter.GetBytes(item));
            Kernel32.WriteBytes(Handle, itemStruct + 128, BitConverter.GetBytes(amount));

            var asm = (byte[])DS2Assembly.GetItem.Clone();

            var bytes = BitConverter.GetBytes(amount);
            Array.Copy(bytes, 0, asm, 0x9, 4);
            bytes = BitConverter.GetBytes(itemStruct.ToInt64());
            Array.Copy(bytes, 0, asm, 0xF, 8);
            var lol = AvailableItemBag.Resolve();
            bytes = BitConverter.GetBytes(AvailableItemBag.Resolve().ToInt64());
            Array.Copy(bytes, 0, asm, 0x1C, 8);
            bytes = BitConverter.GetBytes(BaseAddress.ToInt64() + 0x1A8C67);
            Array.Copy(bytes, 0, asm, 0x29, 8);
            bytes = BitConverter.GetBytes(amount);
            Array.Copy(bytes, 0, asm, 0x36, 4);
            bytes = BitConverter.GetBytes(itemStruct.ToInt64());
            Array.Copy(bytes, 0, asm, 0x3C, 8);
            bytes = BitConverter.GetBytes(BaseAddress.ToInt64() + 0x5D8C0);
            Array.Copy(bytes, 0, asm, 0x54, 8);
            bytes = BitConverter.GetBytes(ItemGiveWindow.Resolve().ToInt64());
            Array.Copy(bytes, 0, asm, 0x66, 8);
            bytes = BitConverter.GetBytes(BaseAddress.ToInt64() + 0x4F9E70);
            Array.Copy(bytes, 0, asm, 0x70, 8);

            Debug.WriteLine(string.Join(" ", asm));

            Execute(asm);
            Free(itemStruct);
        }
        #endregion
    }
}
