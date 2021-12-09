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

        private PHPointer BaseASetup;
        private PHPointer BaseA;
        private PHPointer PlayerBase;
        private PHPointer PlayerDataBase;

        public bool Loaded = false;

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
            PlayerBase = CreateChildPointer(BaseA, (int)DS2Offsets.PlayerBaseOffset);
            PlayerDataBase = CreateChildPointer(PlayerBase, (int)DS2Offsets.PlayerDataBaseOffset);
            Loaded = true;
            UpdateProperties();
        }
        private void DSHook_OnUnhooked(object sender, PHEventArgs e)
        {
            Loaded = false;
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
        private short _vigor;
        public short Vigor
        {
            get => Loaded ? PlayerDataBase.ReadInt16((int)DS2Offsets.Attributes.VGR) : (short)0;
            set => PlayerDataBase.WriteInt16((int)DS2Offsets.Attributes.VGR, value);
        }
        private short _endurance;
        public short Endurance
        {
            get => Loaded ? PlayerDataBase.ReadInt16((int)DS2Offsets.Attributes.END) : (short)0;
            set => PlayerDataBase.WriteInt16((int)DS2Offsets.Attributes.END, value);
        }
        private short _vitality;
        public short Vitality 
        {
            get => Loaded ? PlayerDataBase.ReadInt16((int)DS2Offsets.Attributes.VIT) : (short)0;
            set => PlayerDataBase.WriteInt16((int)DS2Offsets.Attributes.VIT, value);
        }
        private short _attunement;
        public short Attunement
        {
            get => Loaded ? PlayerDataBase.ReadInt16((int)DS2Offsets.Attributes.ATN) : (short)0;
            set => PlayerDataBase.WriteInt16((int)DS2Offsets.Attributes.ATN, value);
        }
        private short _strength;
        public short Strength
        {
            get => Loaded ? PlayerDataBase.ReadInt16((int)DS2Offsets.Attributes.STR) : (short)0;
            set => PlayerDataBase.WriteInt16((int)DS2Offsets.Attributes.STR, value);
        }
        private short _dexterity;
        public short Dexterity
        {
            get => Loaded ? PlayerDataBase.ReadInt16((int)DS2Offsets.Attributes.DEX) : (short)0;
            set => PlayerDataBase.WriteInt16((int)DS2Offsets.Attributes.DEX, value);
        }
        private short _adaptability;
        public short Adaptability
        {
            get => Loaded ? PlayerDataBase.ReadInt16((int)DS2Offsets.Attributes.ADP) : (short)0;
            set => PlayerDataBase.WriteInt16((int)DS2Offsets.Attributes.ADP, value);
        }
        private short _intelligence;
        public short Intelligence
        {
            get => Loaded ? PlayerDataBase.ReadInt16((int)DS2Offsets.Attributes.INT) : (short)0;
            set => PlayerDataBase.WriteInt16((int)DS2Offsets.Attributes.INT, value);
        }
        private short _faith;
        public short Faith
        {
            get => Loaded ? PlayerDataBase.ReadInt16((int)DS2Offsets.Attributes.FTH) : (short)0;
            set => PlayerDataBase.WriteInt16((int)DS2Offsets.Attributes.FTH, value);
        }
        #endregion
    }
}
