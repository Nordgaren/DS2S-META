using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveSplit.DarkSouls2.Splits;

namespace LiveSplit.DarkSouls2.UI
{
    /// <summary>
    /// Interaction logic for MainControl.xaml
    /// </summary>
    public partial class MainControl : UserControl
    {
        public MainControl()
        {
            InitializeComponent();

            _mainViewModel = (MainViewModel)DataContext;
        }

        private MainViewModel _mainViewModel;

        public List<ISplit> GetSplits()
        {
            return _mainViewModel.GetSplits();
        }

        public void SetSplits(List<ISplit> splits)
        {
            _mainViewModel.SetSplits(splits);

        }


        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            _mainViewModel.Splits.Add(new SplitViewModel());
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (SplitsListView.SelectedItem != null && SplitsListView.SelectedItem is SplitViewModel s)
            {
                _mainViewModel.Splits.Remove(s);
            }
        }


        #region Drag and drop ==============================================================================================================


        private ListViewItem _draggedListViewItem;
        private DateTime? _dragStartDateTime;
        private bool _dragging = false;
        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_draggedListViewItem == null)
            {
                if (sender is ListViewItem item)
                {
                    _dragStartDateTime = DateTime.Now;
                    _draggedListViewItem = item;
                }
            }
        }

        private void ListViewItem_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _dragStartDateTime = null;
            _draggedListViewItem = null;
            _dragging = false;
        }

        private void ListViewItem_MouseMove(object sender, MouseEventArgs e)
        {
            //If dragging for x milliseconds
            if (!_dragging && _draggedListViewItem != null && _dragStartDateTime != null &&  (DateTime.Now - _dragStartDateTime.Value).Milliseconds > 200)
            {
                _dragging = true;
                DragDrop.DoDragDrop(_draggedListViewItem, _draggedListViewItem.DataContext, DragDropEffects.Move);
            }
        }

        private double _insertBorderThickness = 3;
        private ListViewItem _target;
        private int _insertIndex = 0;
        private int _removeIndex = 0;
        private void ListViewItem_DragOver(object sender, DragEventArgs e)
        {
            if (_target != null)
            {
                _target.BorderThickness = new Thickness(0, 0, 0, 0);
            }

            _target = (ListViewItem)sender;
            var mouseY = e.GetPosition(_target).Y;
            var height = _target.ActualHeight;

            CalculateDragPlacement((SplitViewModel)_draggedListViewItem.DataContext, (SplitViewModel)_target.DataContext, height, mouseY, out bool top, out _insertIndex, out _removeIndex);

            //This creates a visual line to indicate where the dragged item will appear when dropping it
            _target.BorderThickness = top ? new Thickness(0, _insertBorderThickness, 0, 0) : new Thickness(0, 0, 0, _insertBorderThickness);
            
            //var targetSplitViewModel = ((ListBoxItem)(sender)).DataContext as SplitViewModel;
            SplitsListView.SelectedItem = (SplitViewModel)_target.DataContext;
        }

        private void ListViewItem_Drop(object sender, DragEventArgs e)
        {
            var splitViewModel = (SplitViewModel)_draggedListViewItem.DataContext;
            _mainViewModel.Splits.Insert(_insertIndex, splitViewModel);
            _mainViewModel.Splits.RemoveAt(_removeIndex);

            SplitsListView.SelectedItem = splitViewModel;

            _dragStartDateTime = null;
            _draggedListViewItem = null;
            _dragging = false;
            if (_target != null)
            {
                _target.BorderThickness = new Thickness(0, 0, 0, 0);
            }
            _target = null;
        }

        private void CalculateDragPlacement(SplitViewModel source, SplitViewModel target, double height, double mouseY, out bool top, out int insertIndex, out int removeIndex)
        {
            var sourceIndex = _mainViewModel.Splits.IndexOf(source);
            var targetIndex = _mainViewModel.Splits.IndexOf(target);
            
            insertIndex = 0;
            removeIndex = 0;

            //Insert at top or at bottom?
            top = mouseY < (height / 2);
            
            //Inserting an item in the list will change all the indices. After inserting, we remove the original. Have to adjust it's index
            var removeOffset = sourceIndex < targetIndex ? 0 : 1;
            
            //Adjust to insert above or bellow the target element
            insertIndex = top ? targetIndex : targetIndex + 1;
            removeIndex = sourceIndex + removeOffset;
        }
        
        #endregion
    }
}
