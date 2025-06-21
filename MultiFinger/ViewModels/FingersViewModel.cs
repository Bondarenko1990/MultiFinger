using MultiFinger.Models;
using MultiFinger.Models.Figures;
using MultiFinger.Models.Interfaces;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace MultiFinger.ViewModels
{
    public class FingersViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;
        private readonly ITextFileService _fileService;

        // Change this to the path of your data.txt file
        private string _filePath = @"C:\Temp\data.txt";

        private int _selectedIndex;
        private List<FigureBase> _displayedFigures;
        private Dictionary<FingerTrace, List<FigureBase>> _figuresByTrace;
        private bool _isVisibleAxisX = true;
        private bool _isVisibleAxisY = true;
        private bool _showTransformedData = false;
        private double _offsetFromCenter;

        public FingersViewModel(IRegionManager regionManager, ITextFileService fileService)
        {
            _regionManager = regionManager;
            _fileService = fileService;

            InitializeViewModelCommand = new AsyncDelegateCommand(InitializeViewModelExecute);
            SelectedItemChangedCommand = new DelegateCommand<object>(SelectedItemChangedExecute);
            SaveTranformedDataCommand = new AsyncDelegateCommand(SaveTranformedDataExecute);
        }

        public ICommand InitializeViewModelCommand { get; }

        public ICommand SelectedItemChangedCommand { get; }

        public ICommand SaveTranformedDataCommand { get; }

        public List<FigureBase> DisplayedFigures
        {
            get { return _displayedFigures; }
            set { SetProperty(ref _displayedFigures, value); }
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

        public Dictionary<FingerTrace, List<FigureBase>> FiguresByTrace
        {
            get { return _figuresByTrace; }
            set { SetProperty(ref _figuresByTrace, value); }
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

        private void SelectedItemChangedExecute(object parameter)
        {
            if (parameter is KeyValuePair<FingerTrace, List<FigureBase>> figureByTrace)
            {
                DisplayedFigures = figureByTrace.Value;
                OffsetFromCenter = figureByTrace.Key.OffsetFromCenter;
            }
        }

        private async Task InitializeViewModelExecute()
        {
            if (!File.Exists(_filePath))
            {
                MessageBox.Show(
                     $"File not found at:\n{_filePath}\n\nPlease ensure the file exists at the specified path.",
                     "Error",
                     MessageBoxButton.OK,
                     MessageBoxImage.Error);
                return;
            }

            try
            {
                var items = await _fileService.ParseFileAsync(_filePath);

                if (items == null || items.Count == 0)
                {
                    MessageBox.Show(
                        "No data was loaded from the file.",
                        "No Data",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                    return;
                }

                FiguresByTrace = items;
                SelectedIndex = 0;

                var firstItem = FiguresByTrace.FirstOrDefault();
                DisplayedFigures = firstItem.Value;
                OffsetFromCenter = firstItem.Key.OffsetFromCenter;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Could not load the file:\n{_filePath}\n\nReason: {ex.Message}",
                    "Load Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private async Task SaveTranformedDataExecute()
        {
            try
            {
                var result = await _fileService.SaveFileAsync(_filePath);

                if (result)
                {
                    MessageBox.Show("File saved successfully.", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(
                        $"Failed to save file:\n{_filePath}\n\nThe file could not be written. Please check if the file is accessible and not in use.",
                        "Save Error",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Could not save the file:\n{_filePath}\n\nReason: {ex.Message}",
                    "Save Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }            
}
