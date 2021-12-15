using PropertyHook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DS2S_META
{
    public class DS2SHook : PHook, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public string ID => Process?.Id.ToString() ?? "Not Hooked";
        public IntPtr BaseAddress => Process?.MainModule.BaseAddress ?? IntPtr.Zero;
        public string Version { get; private set; }

        public static bool Reading { get; set; }

        private PHPointer BaseASetup;
        private PHPointer GiveSoulsFunc;
        private PHPointer ItemGiveFunc;
        private PHPointer ItemStruct2dDisplay;
        private PHPointer DisplayItem;

        private PHPointer BaseA;
        private PHPointer PlayerName;
        private PHPointer AvailableItemBag;
        private PHPointer ItemGiveWindow;
        private PHPointer PlayerBaseMisc;
        private PHPointer PlayerCtrl;
        private PHPointer PlayerPosition;
        private PHPointer PlayerGravity;
        private PHPointer PlayerParam;
        private PHPointer PlayerMapData;
        private PHPointer Bonfire;

        private PHPointer LevelUpSoulsParam;
        private PHPointer WeaponParam;
        private PHPointer WeaponReinforceParam;
        private PHPointer CustomAttrSpecParam;
        private PHPointer ArmorParam;
        private PHPointer ArmorReinforceParam;
        private PHPointer ItemParam;

        private PHPointer BaseBSetup;
        private PHPointer BaseB;
        private PHPointer Connection;

        private PHPointer CameraSetup;
        private PHPointer Camera;
        private PHPointer Camera2;

        public bool Loaded => PlayerCtrl != null && PlayerCtrl.Resolve() != IntPtr.Zero;

        public bool Focused => Hooked && User32.GetForegroundProcessID() == Process.Id;

        public DS2SHook(int refreshInterval, int minLifetime) :
            base(refreshInterval, minLifetime, p => p.MainWindowTitle == "DARK SOULS II")
        {
            Version = "None";

            BaseASetup = RegisterAbsoluteAOB(DS2SOffsets.BaseAAob);
            GiveSoulsFunc = RegisterAbsoluteAOB(DS2SOffsets.GiveSoulsFunc);
            ItemGiveFunc = RegisterAbsoluteAOB(DS2SOffsets.ItemGiveFunc);
            ItemStruct2dDisplay = RegisterAbsoluteAOB(DS2SOffsets.ItemStruct2dDisplay);
            DisplayItem = RegisterAbsoluteAOB(DS2SOffsets.DisplayItem); 

            BaseBSetup = RegisterAbsoluteAOB(DS2SOffsets.BaseBAoB);

            CameraSetup = RegisterAbsoluteAOB(DS2SOffsets.CameraAoB);

            OnHooked += DS2Hook_OnHooked;
            OnUnhooked += DS2Hook_OnUnhooked;
        }
        private void DS2Hook_OnHooked(object sender, PHEventArgs e)
        {
            BaseA = CreateBasePointer(BasePointerFromSetupPointer(BaseASetup));
            PlayerName = CreateChildPointer(BaseA, (int)DS2SOffsets.PlayerNameOffset);
            AvailableItemBag = CreateChildPointer(PlayerName, (int)DS2SOffsets.AvailableItemBagOffset, (int)DS2SOffsets.AvailableItemBagOffset);
            ItemGiveWindow = CreateChildPointer(BaseA, (int)DS2SOffsets.ItemGiveWindowPointer);
            PlayerBaseMisc = CreateChildPointer(PlayerName, (int)DS2SOffsets.PlayerBaseMiscOffset);
            PlayerCtrl = CreateChildPointer(BaseA, (int)DS2SOffsets.PlayerCtrlOffset);
            PlayerPosition = CreateChildPointer(PlayerCtrl, (int)DS2SOffsets.PlayerPositionOffset1, (int)DS2SOffsets.PlayerPositionOffset2);
            PlayerGravity = CreateChildPointer(PlayerCtrl, (int)DS2SOffsets.PlayerMapDataOffset1);
            PlayerParam = CreateChildPointer(PlayerCtrl, (int)DS2SOffsets.PlayerParamOffset);
            PlayerMapData = CreateChildPointer(PlayerGravity, (int)DS2SOffsets.PlayerMapDataOffset2, (int)DS2SOffsets.PlayerMapDataOffset3);
            Bonfire = CreateChildPointer(BaseA, (int)DS2SOffsets.BonfireOffset);

            LevelUpSoulsParam = CreateChildPointer(BaseA, (int)DS2SOffsets.ParamDataOffset1, (int)DS2SOffsets.LevelUpSoulsParamOffset, (int)DS2SOffsets.ParamDataOffset2);
            WeaponParam = CreateChildPointer(BaseA, (int)DS2SOffsets.ParamDataOffset1, (int)DS2SOffsets.WeaponParamOffset, (int)DS2SOffsets.ParamDataOffset2);
            WeaponReinforceParam = CreateChildPointer(BaseA, (int)DS2SOffsets.ParamDataOffset1, (int)DS2SOffsets.WeaponReinforceParamOffset, (int)DS2SOffsets.ParamDataOffset2);
            CustomAttrSpecParam = CreateChildPointer(BaseA, (int)DS2SOffsets.ParamDataOffset1, (int)DS2SOffsets.CustomAttrSpecParamOffset, (int)DS2SOffsets.ParamDataOffset2);
            ArmorParam = CreateChildPointer(BaseA, (int)DS2SOffsets.ParamDataOffset1, (int)DS2SOffsets.ArmorParamOffset, (int)DS2SOffsets.ParamDataOffset2);
            ArmorReinforceParam = CreateChildPointer(BaseA, (int)DS2SOffsets.ParamDataOffset1, (int)DS2SOffsets.ArmorReinforceParamOffset, (int)DS2SOffsets.ParamDataOffset2);
            ItemParam = CreateChildPointer(BaseA, (int)DS2SOffsets.ParamDataOffset3, (int)DS2SOffsets.ItemParamOffset, (int)DS2SOffsets.ParamDataOffset2);

            BaseB = CreateBasePointer(BasePointerFromSetupPointer(BaseBSetup));
            Connection = CreateChildPointer(BaseB, (int)DS2SOffsets.ConnectionOffset);

            Camera = CreateBasePointer(BaseAddress + 0x160B8D0, (int)DS2SOffsets.CameraOffset1);
            Camera2 = CreateChildPointer(Camera, (int)DS2SOffsets.CameraOffset2);

            GetLevelRequirements();
            BuildOffsetDictionary(WeaponParam, WeaponParamOffsetDict, "WEAPON_PARAM");
            BuildOffsetDictionary(WeaponReinforceParam, WeaponReinforceParamOffsetDict, "WEAPON_REINFORCE_PARAM");
            BuildOffsetDictionary(CustomAttrSpecParam, CustomAttrOffsetDict, "CUSTOM_ATTR_SPEC_PARAM");
            BuildOffsetDictionary(ArmorParam, ArmorParamOffsetDict, "ARMOR_PARAM");
            BuildOffsetDictionary(ArmorReinforceParam, ArmorReinforceParamOffsetDict, "ARMOR_REINFORCE_PARAM");
            BuildOffsetDictionary(ItemParam, ItemParamOffsetDict, "ITEM_PARAM");
            UpdateStatsProperties();
        }

       

        

        private void DS2Hook_OnUnhooked(object sender, PHEventArgs e)
        {
        }

        public void UpdateMainProperties()
        {
            OnPropertyChanged(nameof(ID));
            OnPropertyChanged(nameof(Online));
            //OnPropertyChanged(nameof(LastBonfireObj));
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
            OnPropertyChanged(nameof(AngX));
            OnPropertyChanged(nameof(AngY));
            OnPropertyChanged(nameof(AngZ));
            OnPropertyChanged(nameof(StableX));
            OnPropertyChanged(nameof(StableY));
            OnPropertyChanged(nameof(StableZ));
            OnPropertyChanged(nameof(Gravity));
        }
        public IntPtr BasePointerFromSetupPointer(PHPointer pointer)
        {
            var readInt = pointer.ReadInt32(DS2SOffsets.BasePtrOffset1);
            return pointer.ReadIntPtr(readInt + DS2SOffsets.BasePtrOffset2);
        }

        #region Player
        public int Health
        {
            get => Loaded ? PlayerCtrl.ReadInt32((int)DS2SOffsets.PlayerCtrl.HP) : 0;
            set 
            {
                if (Reading || !Loaded) return;
                PlayerCtrl.WriteInt32((int)DS2SOffsets.PlayerCtrl.HP, value);
                OnPropertyChanged(nameof(Health));
            }
        }
        public int HealthMax
        {
            get => Loaded ? PlayerCtrl.ReadInt32((int)DS2SOffsets.PlayerCtrl.HPMax) : 0;
            set => PlayerCtrl.WriteInt32((int)DS2SOffsets.PlayerCtrl.HPMax, value);
        }
        public int HealthCap
        {
            get => Loaded ? PlayerCtrl.ReadInt32((int)DS2SOffsets.PlayerCtrl.HPCap) : 0;
            set => PlayerCtrl.WriteInt32((int)DS2SOffsets.PlayerCtrl.HPCap, value);
        }
        public float Stamina
        {
            get => Loaded ? PlayerCtrl.ReadSingle((int)DS2SOffsets.PlayerCtrl.SP) : 0;
            set 
            { 
                if (Reading || !Loaded) return;
                PlayerCtrl.WriteSingle((int)DS2SOffsets.PlayerCtrl.SP, value);
                OnPropertyChanged(nameof(Stamina));
            }
        }
        public float MaxStamina
        {
            get => Loaded ? PlayerCtrl.ReadSingle((int)DS2SOffsets.PlayerCtrl.SPMax) : 0;
            set => PlayerCtrl.WriteSingle((int)DS2SOffsets.PlayerCtrl.SPMax, value);
        }
        public float PosX
        {
            get => Loaded ? PlayerPosition.ReadSingle((int)DS2SOffsets.PlayerPosition.PosX) : 0;
        }
        public float PosY
        {
            get => Loaded ? PlayerPosition.ReadSingle((int)DS2SOffsets.PlayerPosition.PosY) : 0;
        }
        public float PosZ
        {
            get => Loaded ? PlayerPosition.ReadSingle((int)DS2SOffsets.PlayerPosition.PosZ) : 0;
        }
        public float AngX
        {
            get => Loaded ? PlayerPosition.ReadSingle((int)DS2SOffsets.PlayerPosition.AngX) : 0;
            set => PlayerPosition.WriteSingle((int)DS2SOffsets.PlayerPosition.AngX, value);
        }
        public float AngY
        {
            get => Loaded ? PlayerPosition.ReadSingle((int)DS2SOffsets.PlayerPosition.AngY) : 0;
            set => PlayerPosition.WriteSingle((int)DS2SOffsets.PlayerPosition.AngY, value);
        }
        public float AngZ
        {
            get => Loaded ? PlayerPosition.ReadSingle((int)DS2SOffsets.PlayerPosition.AngZ) : 0;
            set => PlayerPosition.WriteSingle((int)DS2SOffsets.PlayerPosition.AngZ, value);
        }
        public float StableX
        {
            get => Loaded ? PlayerMapData.ReadSingle((int)DS2SOffsets.PlayerMapData.WarpXA) : 0;
            set
            {
                PlayerMapData.WriteSingle((int)DS2SOffsets.PlayerMapData.WarpXA, value);
                PlayerMapData.WriteSingle((int)DS2SOffsets.PlayerMapData.WarpXB, value);
                PlayerMapData.WriteSingle((int)DS2SOffsets.PlayerMapData.WarpXC, value);
            }
        }
        public float StableY
        {
            get => Loaded ? PlayerMapData.ReadSingle((int)DS2SOffsets.PlayerMapData.WarpYA) : 0;
            set
            {
                PlayerMapData.WriteSingle((int)DS2SOffsets.PlayerMapData.WarpYA, value);
                PlayerMapData.WriteSingle((int)DS2SOffsets.PlayerMapData.WarpYB, value);
                PlayerMapData.WriteSingle((int)DS2SOffsets.PlayerMapData.WarpYC, value);
            }
        }
        public float StableZ
        {
            get => Loaded ? PlayerMapData.ReadSingle((int)DS2SOffsets.PlayerMapData.WarpZA) : 0;
            set
            {
                PlayerMapData.WriteSingle((int)DS2SOffsets.PlayerMapData.WarpZA, value);
                PlayerMapData.WriteSingle((int)DS2SOffsets.PlayerMapData.WarpZB, value);
                PlayerMapData.WriteSingle((int)DS2SOffsets.PlayerMapData.WarpZC, value);
            }
        }
        public byte[] CameraData
        {
            get => Camera2.ReadBytes((int)DS2SOffsets.Camera.CamStart, 0x4C);
            set => Camera2.WriteBytes((int)DS2SOffsets.Camera.CamStart, value);
        }
        public float CamX
        {
            get => CameraSetup.ReadSingle((int)DS2SOffsets.Camera.CamX);
            set => CameraSetup.WriteSingle((int)DS2SOffsets.Camera.CamX, value);
        }
        public float CamY
        {
            get => CameraSetup.ReadSingle((int)DS2SOffsets.Camera.CamY);
            set => CameraSetup.WriteSingle((int)DS2SOffsets.Camera.CamY, value);
        }
        public float CamZ
        {
            get => CameraSetup.ReadSingle((int)DS2SOffsets.Camera.CamZ);
            set => CameraSetup.WriteSingle((int)DS2SOffsets.Camera.CamZ, value);
        }
        public float Speed
        {
            set 
            {
                if (Reading || !Loaded) return;
                PlayerCtrl.WriteSingle((int)DS2SOffsets.PlayerCtrl.SpeedModifier, value); 
            }
        }
        public bool Gravity
        {
            get => Loaded ? PlayerGravity.ReadBoolean((int)DS2SOffsets.Gravity.Gravity) : false;
            set => PlayerGravity.WriteBoolean((int)DS2SOffsets.Gravity.Gravity, value);
        }
        public int LastBonfireID
        {
            get => Loaded ? Bonfire.ReadInt32((int)DS2SOffsets.Bonfire.LastSetBonfire) : 0;
            set => Bonfire.WriteInt32((int)DS2SOffsets.Bonfire.LastSetBonfire, value);
        }
        //public DS2SBonfire LastBonfireObj
        //{
        //    get => Loaded ? DS2SBonfire.All.FirstOrDefault(b => b.ID == LastBonfireID) : new DS2SBonfire(0, "None");
        //    //set => Bonfire.WriteInt32((int)DS2SOffsets.Bonfire.LastSetBonfire, value.ID);
        //}

        public bool Online
        {
            get => Hooked ? Connection.ReadInt32((int)DS2SOffsets.Connection.Online) > 0 : false;
        }
        #endregion

        #region Stats
        public string Name
        {
            get => Loaded ? PlayerName.ReadString((int)DS2SOffsets.PlayerName.Name, Encoding.Unicode, 0x22) : "";
            set
            {
                if (Reading || !Loaded) return;
                if (Name == value) return;
                PlayerName.WriteString((int)DS2SOffsets.PlayerName.Name, Encoding.Unicode, 0x22, value);
                OnPropertyChanged(nameof(Name));
            }
        }

        public byte Class
        {
            get => Loaded ? PlayerBaseMisc.ReadByte((int)DS2SOffsets.PlayerBaseMisc.Class) : (byte)255;
            set
            {
                if (Reading || !Loaded) return;
                PlayerBaseMisc.WriteByte((int)DS2SOffsets.PlayerBaseMisc.Class, value);
            }
        }
        public int SoulLevel
        {
            get => Loaded ? PlayerParam.ReadInt32((int)DS2SOffsets.Attributes.SoulLevel) : 0;
            set => PlayerParam.WriteInt32((int)DS2SOffsets.Attributes.SoulLevel, value);
        }
        public int SoulMemory
        {
            get => Loaded ? PlayerParam.ReadInt32((int)DS2SOffsets.PlayerParam.SoulMemory) : 0;
            set => PlayerParam.WriteInt32((int)DS2SOffsets.PlayerParam.SoulMemory, value);
        }
        public int SoulMemory2
        {
            get => Loaded ? PlayerParam.ReadInt32((int)DS2SOffsets.PlayerParam.SoulMemory2) : 0;
            set => PlayerParam.WriteInt32((int)DS2SOffsets.PlayerParam.SoulMemory2, value);
        }
        public byte SinnerLevel
        {
            get => Loaded ? PlayerParam.ReadByte((int)DS2SOffsets.PlayerParam.SinnerLevel) : (byte)0;
            set => PlayerParam.WriteByte((int)DS2SOffsets.PlayerParam.SinnerLevel, value);
        }
        public byte SinnerPoints
        {
            get => Loaded ? PlayerParam.ReadByte((int)DS2SOffsets.PlayerParam.SinnerPoints) : (byte)0;
            set => PlayerParam.WriteByte((int)DS2SOffsets.PlayerParam.SinnerPoints, value);
        }
        public byte HollowLevel
        {
            get => Loaded ? PlayerParam.ReadByte((int)DS2SOffsets.PlayerParam.HollowLevel) : (byte)0;
            set => PlayerParam.WriteByte((int)DS2SOffsets.PlayerParam.HollowLevel, value);
        }
        public int Souls
        {
            get => Loaded ? PlayerParam.ReadInt32((int)DS2SOffsets.PlayerParam.Souls) : 0;
        }

        public short Vigor
        {
            get => Loaded ? PlayerParam.ReadInt16((int)DS2SOffsets.Attributes.VGR) : (short)0;
            set
            {
                if (Reading || !Loaded) return;
                PlayerParam.WriteInt16((int)DS2SOffsets.Attributes.VGR, value);
                UpdateSoulLevel();
            }
        }
        public short Endurance
        {
            get => Loaded ? PlayerParam.ReadInt16((int)DS2SOffsets.Attributes.END) : (short)0;
            set 
            { 
                if (Reading || !Loaded) return;
                PlayerParam.WriteInt16((int)DS2SOffsets.Attributes.END, value);
                UpdateSoulLevel();
            }
        }
        public short Vitality
        {
            get => Loaded ? PlayerParam.ReadInt16((int)DS2SOffsets.Attributes.VIT) : (short)0;
            set
            {
                if (Reading || !Loaded) return;
                PlayerParam.WriteInt16((int)DS2SOffsets.Attributes.VIT, value);
                UpdateSoulLevel();
            }
        }
        public short Attunement
        {
            get => Loaded ? PlayerParam.ReadInt16((int)DS2SOffsets.Attributes.ATN) : (short)0;
            set
            {
                if (Reading || !Loaded) return;
                PlayerParam.WriteInt16((int)DS2SOffsets.Attributes.ATN, value);
                UpdateSoulLevel();
            }
        }
        public short Strength
        {
            get => Loaded ? PlayerParam.ReadInt16((int)DS2SOffsets.Attributes.STR) : (short)0;
            set
            {
                if (Reading || !Loaded) return;
                PlayerParam.WriteInt16((int)DS2SOffsets.Attributes.STR, value);
                UpdateSoulLevel();
            }
        }
        public short Dexterity
        {
            get => Loaded ? PlayerParam.ReadInt16((int)DS2SOffsets.Attributes.DEX) : (short)0;
            set
            {
                if (Reading || !Loaded) return;
                PlayerParam.WriteInt16((int)DS2SOffsets.Attributes.DEX, value);
                UpdateSoulLevel();
            }
        }
        public short Adaptability
        {
            get => Loaded ? PlayerParam.ReadInt16((int)DS2SOffsets.Attributes.ADP) : (short)0;
            set
            {
                if (Reading || !Loaded) return;
                PlayerParam.WriteInt16((int)DS2SOffsets.Attributes.ADP, value);
                UpdateSoulLevel();
            }
        }
        public short Intelligence
        {
            get => Loaded ? PlayerParam.ReadInt16((int)DS2SOffsets.Attributes.INT) : (short)0;
            set
            {
                if (Reading || !Loaded) return;
                PlayerParam.WriteInt16((int)DS2SOffsets.Attributes.INT, value);
                UpdateSoulLevel();
            }
        }
        public short Faith
        {
            get => Loaded ? PlayerParam.ReadInt16((int)DS2SOffsets.Attributes.FTH) : (short)0;
            set
            {
                if (Reading || !Loaded) return;
                PlayerParam.WriteInt16((int)DS2SOffsets.Attributes.FTH, value);
                UpdateSoulLevel();
            }
        }
        public void GiveSouls(int souls)
        {
            var asm = (byte[])DS2SAssembly.AddSouls.Clone();

            var bytes = BitConverter.GetBytes(PlayerParam.Resolve().ToInt64());
            Array.Copy(bytes, 0, asm, 0x6, 8);
            bytes = BitConverter.GetBytes(souls);
            Array.Copy(bytes, 0, asm, 0x11, 4);
            bytes = BitConverter.GetBytes(GiveSoulsFunc.Resolve().ToInt64());
            Array.Copy(bytes, 0, asm, 0x17, 8);
            Execute(asm);
        }
        private void UpdateSoulLevel()
        {
            var charClass = DS2SClass.All.FirstOrDefault(c => c.ID == Class);
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
        private int GetSoulLevel(DS2SClass charClass)
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
            var charClass = DS2SClass.All.FirstOrDefault(c => c.ID == Class);
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
                var index = i <= 850 ? i : 850;
                soulMemory += Levels[index];
            }
            return soulMemory;
        }

        public static List<int> Levels = new List<int>();
        private void GetLevelRequirements()
        {
            var paramName = LevelUpSoulsParam.ReadString(0xC, Encoding.UTF8, 0x18);
            if (paramName != "CHR_LEVEL_UP_SOULS_PARAM")
                throw new InvalidOperationException("Incorrect Param Pointer: LEVEL_UP_SOULS_PARAM");

            var tableLength = LevelUpSoulsParam.ReadInt32((int)DS2SOffsets.Param.TableLength);
            var paramID = 0x40;
            var paramOffset = 0x48;
            var nextParam = 0x18;
            var slOffset = 0x8;

            while (paramID < tableLength)
            {
                var soulReqOffset = LevelUpSoulsParam.ReadInt32(paramOffset);
                var soulReq = LevelUpSoulsParam.ReadInt32(soulReqOffset + slOffset);
                Levels.Add(soulReq);

                paramID += nextParam;
                paramOffset += nextParam;
            }
        }

        #endregion

        #region Items
        public void GetItem(int item, short amount, byte upgrade, byte infusion, bool silent)
        {
            if (silent)
                GiveItemSilently(item, amount, upgrade, infusion);
            else
                GiveItem(item, amount, upgrade, infusion);
        }

        private void GiveItem(int item, short amount, byte upgrade, byte infusion)
        {
            var itemStruct = Allocate(0x8A);
            Kernel32.WriteBytes(Handle, itemStruct + 0x4, BitConverter.GetBytes(item));
            Kernel32.WriteBytes(Handle, itemStruct + 0x8, BitConverter.GetBytes(float.MaxValue));
            Kernel32.WriteBytes(Handle, itemStruct + 0xC, BitConverter.GetBytes(amount));
            Kernel32.WriteByte(Handle, itemStruct + 0xE, upgrade);
            Kernel32.WriteByte(Handle, itemStruct + 0xF, infusion);

            var asm = (byte[])DS2SAssembly.GetItem.Clone();

            var bytes = BitConverter.GetBytes(0x1);
            Array.Copy(bytes, 0, asm, 0x9, 4);
            bytes = BitConverter.GetBytes(itemStruct.ToInt64());
            Array.Copy(bytes, 0, asm, 0xF, 8);
            bytes = BitConverter.GetBytes(AvailableItemBag.Resolve().ToInt64());
            Array.Copy(bytes, 0, asm, 0x1C, 8);
            bytes = BitConverter.GetBytes(ItemGiveFunc.Resolve().ToInt64());
            Array.Copy(bytes, 0, asm, 0x29, 8);
            bytes = BitConverter.GetBytes(0x1);
            Array.Copy(bytes, 0, asm, 0x36, 4);
            bytes = BitConverter.GetBytes(itemStruct.ToInt64());
            Array.Copy(bytes, 0, asm, 0x3C, 8);
            bytes = BitConverter.GetBytes(ItemStruct2dDisplay.Resolve().ToInt64());
            Array.Copy(bytes, 0, asm, 0x54, 8);
            bytes = BitConverter.GetBytes(ItemGiveWindow.Resolve().ToInt64());
            Array.Copy(bytes, 0, asm, 0x66, 8);
            bytes = BitConverter.GetBytes(DisplayItem.Resolve().ToInt64());
            Array.Copy(bytes, 0, asm, 0x70, 8);

            Execute(asm);
            Free(itemStruct);
        }

        private void GiveItemSilently(int item, short amount, byte upgrade, byte infusion)
        {
            var itemStruct = Allocate(0x8A);
            Kernel32.WriteBytes(Handle, itemStruct + 0x4, BitConverter.GetBytes(item));
            Kernel32.WriteBytes(Handle, itemStruct + 0x8, BitConverter.GetBytes(float.MaxValue));
            Kernel32.WriteBytes(Handle, itemStruct + 0xC, BitConverter.GetBytes(amount));
            Kernel32.WriteByte(Handle, itemStruct + 0xE, upgrade);
            Kernel32.WriteByte(Handle, itemStruct + 0xF, infusion);

            var asm = (byte[])DS2SAssembly.GetItemNoMenu.Clone();

            var bytes = BitConverter.GetBytes(0x1);
            Array.Copy(bytes, 0, asm, 0x6, 4);
            bytes = BitConverter.GetBytes(itemStruct.ToInt64());
            Array.Copy(bytes, 0, asm, 0xC, 8);
            bytes = BitConverter.GetBytes(AvailableItemBag.Resolve().ToInt64());
            Array.Copy(bytes, 0, asm, 0x19, 8);
            bytes = BitConverter.GetBytes(ItemGiveFunc.Resolve().ToInt64());
            Array.Copy(bytes, 0, asm, 0x26, 8);

            Execute(asm);
            Free(itemStruct);
        }
        #endregion

        #region Params

        private static Dictionary<int, int> WeaponParamOffsetDict = new Dictionary<int, int>();
        private static Dictionary<int, int> WeaponReinforceParamOffsetDict = new Dictionary<int, int>();
        private static Dictionary<int, int> CustomAttrOffsetDict = new Dictionary<int, int>();
        private static Dictionary<int, int> ArmorParamOffsetDict = new Dictionary<int, int>();
        private static Dictionary<int, int> ArmorReinforceParamOffsetDict = new Dictionary<int, int>();
        private static Dictionary<int, int> ItemParamOffsetDict = new Dictionary<int, int>();

        private void BuildOffsetDictionary(PHPointer pointer, Dictionary<int, int> dictionary, string expectedParamName)
        {
            var paramName = pointer.ReadString(0xC, Encoding.UTF8, 0x18);
            if (paramName != expectedParamName)
                throw new InvalidOperationException($"Incorrect Param Pointer: {expectedParamName}");

            var tableLength = pointer.ReadInt32((int)DS2SOffsets.Param.TableLength);
            var paramID = 0x40;
            var paramOffset = 0x48;
            var nextParam = 0x18;

            while (paramID < tableLength)
            {
                var weaponID = pointer.ReadInt32(paramID);
                var weaponParamOffset = pointer.ReadInt32(paramOffset);
                dictionary.Add(weaponID, weaponParamOffset);

                paramID += nextParam;
                paramOffset += nextParam;
            }
        }

        internal int GetMaxUpgrade(DS2SItem item)
        {
            switch (item.Type)
            {
                case DS2SItem.ItemType.Weapon:
                    return GetWeaponMaxUpgrade(item.ID);
                case DS2SItem.ItemType.Armor:
                    return GetArmorMaxUpgrade(item.ID);
                case DS2SItem.ItemType.Item:
                case DS2SItem.ItemType.Ring:
                    return 0;
            }

            return 0;
        }

        internal int GetMaxQuantity(DS2SItem item)
        {
            switch (item.Type)
            {
                case DS2SItem.ItemType.Ring:
                case DS2SItem.ItemType.Weapon:
                case DS2SItem.ItemType.Armor:
                    return 1;
                case DS2SItem.ItemType.Item:
                    return GetMaxItemQuantity(item.ID);
            }

            return 0;
        }
        private int GetArmorMaxUpgrade(int id)
        {
            return ArmorReinforceParam.ReadInt32(ArmorReinforceParamOffsetDict[id - 10000000] + (int)DS2SOffsets.ArmorReinforceParam.MaxUpgrade);
        }

        private int GetWeaponMaxUpgrade(int id)
        {
            var reinforceParamID = WeaponParam.ReadInt32(WeaponParamOffsetDict[id] + (int)DS2SOffsets.WeaponParam.ReinforceID);
            return WeaponReinforceParam.ReadInt32(WeaponReinforceParamOffsetDict[reinforceParamID] + (int)DS2SOffsets.WeaponReinforceParam.MaxUpgrade);
        }
        private int GetMaxItemQuantity(int id)
        {
            return ItemParam.ReadInt16(ItemParamOffsetDict[id] + (int)DS2SOffsets.ItemParam.MaxHeld);
        }

        internal List<DS2SInfusion> GetWeaponInfusions(int id)
        {
            var infusions = new List<DS2SInfusion>();
            var reinforceParamID = WeaponParam.ReadInt32(WeaponParamOffsetDict[id] + (int)DS2SOffsets.WeaponParam.ReinforceID);
            var customAttrID = WeaponReinforceParam.ReadInt32(WeaponReinforceParamOffsetDict[reinforceParamID] + (int)DS2SOffsets.WeaponReinforceParam.CustomAttrID);
            var bitField = CustomAttrSpecParam.ReadInt32(CustomAttrOffsetDict[customAttrID]);

            if (bitField == 0)
                return new List<DS2SInfusion>() { DS2SInfusion.Normal };

            if ((bitField & 1) != 0)
                infusions.Add(DS2SInfusion.Normal);

            if ((bitField & 2) != 0)
                infusions.Add(DS2SInfusion.Fire);

            if ((bitField & 4) != 0)
                infusions.Add(DS2SInfusion.Magic);

            if ((bitField & 8) != 0)
                infusions.Add(DS2SInfusion.Lightning);

            if ((bitField & 16) != 0)
                infusions.Add(DS2SInfusion.Dark);

            if ((bitField & 32) != 0)
                infusions.Add(DS2SInfusion.Poison);

            if ((bitField & 64) != 0)
                infusions.Add(DS2SInfusion.Bleed);

            if ((bitField & 128) != 0)
                infusions.Add(DS2SInfusion.Raw);

            if ((bitField & 256) != 0)
                infusions.Add(DS2SInfusion.Enchanted);

            if ((bitField & 512) != 0)
                infusions.Add(DS2SInfusion.Mundane);

            return infusions;
        }

        #endregion
    }
}
