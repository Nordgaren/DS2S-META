using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using DarkSoulsAutoSplitter.Splits;
using DarkSoulsAutoSplitter.UI;

namespace DarkSoulsAutoSplitter
{
    public class DarkSoulsComponent : IComponent
    {
        public const string Name = "Dark Souls 2 Autosplitter";

        public IDictionary<string, Action> ContextMenuControls => null;

        private LiveSplitState _liveSplitState;

        public DarkSoulsComponent(LiveSplitState state = null)
        {
            _liveSplitState = state;
        }

        public void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
        {
            HorizontalWidth = width;
            //VerticalHeight = height;
            _liveSplitState = state;

            if (_redraw)
            {
                invalidator?.Invalidate(0, 0, width, height);
            }
        }



        #region drawing ===================================================================================================================

        public void DrawHorizontal(Graphics g, LiveSplitState state, float height, Region clipRegion)
        {
            Draw(g);
        }

        public void DrawVertical(Graphics g, LiveSplitState state, float width, Region clipRegion)
        {
            Draw(g);
        }

        private readonly SimpleLabel _label = new SimpleLabel()
        {
            IsMonospaced = true,
            Brush = Brushes.Crimson,
            HorizontalAlignment = StringAlignment.Near,
        };

        private void WriteDebug(string s)
        {
            _redraw = true;
            _debugOutput = s;

        }

        private string _debugOutput = " ";
        private bool _redraw = true;
        public void Draw(Graphics g)
        {
            VerticalHeight = g.MeasureString(_debugOutput, _liveSplitState.LayoutSettings.TextFont).Height;
            DrawBackground(g);
            _label.Text = _debugOutput;
            _label.Brush = new SolidBrush(_liveSplitState.LayoutSettings.TextColor);
            _label.Font = _liveSplitState.LayoutSettings.TextFont;
            _label.Width = HorizontalWidth;
            _label.Height = VerticalHeight;
            _label.Draw(g);
        }

        private void DrawBackground(Graphics g)
        {
            g.FillRectangle(new SolidBrush(_liveSplitState.LayoutSettings.BackgroundColor), 0, 0, HorizontalWidth, VerticalHeight);
        }

        public string ComponentName => Name;
        public float HorizontalWidth { get; private set; } = 0;
        public float MinimumHeight { get; private set; } = 0;
        public float VerticalHeight { get; private set; } = 0;
        public float MinimumWidth { get; private set; } = 0;
        public float PaddingTop => 0;
        public float PaddingBottom => 0;
        public float PaddingLeft => 0;
        public float PaddingRight => 0;
        public void Dispose() { }
        #endregion

        #region Xml settings ==============================================================================================================
        public XmlNode GetSettings(XmlDocument document)
        {
            var viewModel = _mainControlFormsWrapper.GetMainViewModel();


            //var splits = _mainControlFormsWrapper.GetSplits();
            //var wrapper = new List<XmlSerializableWrapper<ISplit>>();
            //foreach (var split in splits)
            //{
            //    switch (split.SplitType)
            //    {
            //        case SplitType.Boss:
            //            wrapper.Add(new XmlSerializableWrapper<ISplit>((BossSplit)split));
            //            break;
            //
            //        case SplitType.Item:
            //            wrapper.Add(new XmlSerializableWrapper<ISplit>((ItemSplit)split));
            //            break;
            //
            //        case SplitType.Box:
            //            wrapper.Add(new XmlSerializableWrapper<ISplit>((BoxSplit)split));
            //            break;
            //    }
            //}

            var settings = new XmlWriterSettings()
            {
                OmitXmlDeclaration = true,
                Indent = true,
            };

            var xml = "";
            using (var stream = new StringWriter())
            using (var writer = XmlWriter.Create(stream, settings))
            {
                //Since splits is a list of interfaces, we need to explicitly specify the extra possible types so that the serializer knows what to expect
                var serializer = new XmlSerializer(viewModel.GetType());
                serializer.Serialize(writer, viewModel);
                xml = stream.ToString();
            }

            XmlDocumentFragment fragment = document.CreateDocumentFragment();
            fragment.InnerXml = xml;

            XmlElement root = document.CreateElement("Settings");
            root.AppendChild(fragment);
            return root;
        }

        public void SetSettings(XmlNode settings)
        {
            var vm = settings.InnerXml.DeserializeXml<MainViewModel>();
            _mainControlFormsWrapper.SetMainViewModel(vm);
            //var wrapped = settings.InnerXml.DeserializeXml<List<XmlSerializableWrapper<ISplit>>>();
            //var splits = wrapped.Select(i => i.Value).ToList();
            //_mainControlFormsWrapper.SetSplits(splits);
        }

        private MainControlFormsWrapper _mainControlFormsWrapper = new MainControlFormsWrapper();
        public System.Windows.Forms.Control GetSettingsControl(LayoutMode mode)
        {
            return _mainControlFormsWrapper;
        }
        #endregion
    }
}
