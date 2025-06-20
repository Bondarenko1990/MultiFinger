using MultiFinger.Models;
using MultiFinger.Models.Figures;
using MultiFinger.Models.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace MultiFinger.ViewModels
{
    public class DrawFiguresViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private readonly ITextFileService _fileService;

        private int _selectedIndex;
        private List<FigureBase> _figures;
        private Dictionary<FingerTrace, List<FigureBase>> _Items;
        private bool _isVisibleAxisX = true;
        private bool _isVisibleAxisY = true;
        private bool _showTransformedData = false;
        private double _offsetFromCenter;

        public DrawFiguresViewModel(IRegionManager regionManager, ITextFileService fileService)
        {
            _regionManager = regionManager;
            _fileService = fileService;

            InitializeViewModelCommand = new DelegateCommand(InitializeViewModelExecute);

            SelectedItemChangedCommand = new DelegateCommand<object>(SelectedItemChangedExecute);

            SaveTranformedDataCommand = new DelegateCommand(SaveTranformedDataExecute);
        }

        public ICommand InitializeViewModelCommand { get; }

        public ICommand SelectedItemChangedCommand { get; }

        public ICommand SaveTranformedDataCommand { get; }

        public List<FigureBase> Figures
        {
            get { return _figures; }
            set { SetProperty(ref _figures, value); }
        }

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { SetProperty(ref _selectedIndex, value); }
        }

        public double OffsetFromCenter
        {
            get { return _offsetFromCenter; }
            set { SetProperty(ref _offsetFromCenter, value); }
        }

        public Dictionary<FingerTrace, List<FigureBase>> Items
        {
            get { return _Items; }
            set { SetProperty(ref _Items, value); }
        }

        public bool IsVisibleAxisX
        {
            get { return _isVisibleAxisX; }
            set { SetProperty(ref _isVisibleAxisX, value); }
        }

        public bool IsVisibleAxisY
        {
            get { return _isVisibleAxisY; }
            set { SetProperty(ref _isVisibleAxisY, value); }
        }

        public bool ShowTransformedData
        {
            get { return _showTransformedData; }
            set { SetProperty(ref _showTransformedData, value); }
        }

        private async void InitializeViewModelExecute()
        {
            // Please add path to your file
            if (!File.Exists("C:\\Temp\\data.txt"))
            {
                MessageBox.Show("File not found. Please ensure the file exists at the specified path.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var figures = await _fileService.ParseFileAsync("C:\\Temp\\data.txt");
            Items = figures;
            if (Items == null || Items.Count == 0) return;
            SelectedIndex = 0;
            Figures = Items.FirstOrDefault().Value;
            OffsetFromCenter = Items.FirstOrDefault().Key.OffsetFromCenter;
        }

        private void SelectedItemChangedExecute(object parameter)
        {
            if (parameter is KeyValuePair<FingerTrace, List<FigureBase>> kvp)
            {
                Figures = kvp.Value;
                OffsetFromCenter = kvp.Key.OffsetFromCenter;
            }
        }

        private async void SaveTranformedDataExecute()
        {
            // Please add path to your file
            var result = await _fileService.SaveFileAsync("C:\\Temp\\data.txt");

            if (result)
            {
                MessageBox.Show("File saved successfully.", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Failed to save file.", "Save Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }            
}
