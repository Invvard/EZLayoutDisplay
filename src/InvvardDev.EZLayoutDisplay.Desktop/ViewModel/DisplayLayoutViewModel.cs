using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using InvvardDev.EZLayoutDisplay.Desktop.Model;
using InvvardDev.EZLayoutDisplay.Desktop.Service.Interface;
using InvvardDev.EZLayoutDisplay.Desktop.View;

namespace InvvardDev.EZLayoutDisplay.Desktop.ViewModel
{
    public class DisplayLayoutViewModel : ViewModelBase
    {
        #region Fields

        private readonly IWindowService _windowService;
        private readonly ILayoutService _layoutService;

        private ICommand _lostFocusCommand;

        private ObservableCollection<KeyTemplate> _layoutTemplate;
        private string _windowTitle;

        #endregion

        #region MyRegion

        public string WindowTitle
        {
            get => _windowTitle;
            set => Set(ref _windowTitle, value);
        }

        public ObservableCollection<KeyTemplate> LayoutTemplate
        {
            get => _layoutTemplate;
            set => Set(ref _layoutTemplate, value);
        }

        #endregion

        #region Relay commands

        /// <summary>
        /// Lost focus command.
        /// </summary>
        public ICommand LostFocusCommand =>
            _lostFocusCommand
            ?? (_lostFocusCommand = new RelayCommand(LostFocus));

        #endregion

        public DisplayLayoutViewModel(IWindowService windowService, ILayoutService layoutService)
        {
            _windowService = windowService;
            _layoutService = layoutService;

            SetLabelUi();
            PopulateModel();
        }

        private async void PopulateModel()
        {
            if (IsInDesignModeStatic)
            {
                LayoutTemplate = new ObservableCollection<KeyTemplate>(new List<KeyTemplate> {
                                                                                                 new KeyTemplate(0, 0, 1.5, vOffset: 0.37),
                                                                                                 new KeyTemplate(1.5, 0, vOffset: 0.37, isGlowing: true),
                                                                                                 new KeyTemplate(2.5, 0, vOffset: 129, isGlowing: true),
                                                                                                 new KeyTemplate(3.5, 0, isGlowing: true),
                                                                                                 new KeyTemplate(0, 1, 1.5, vOffset: .37, isGlowing: true),
                                                                                                 new KeyTemplate(1.5, 1, vOffset: .37, isGlowing: true),
                                                                                                 new KeyTemplate(2.5, 1, vOffset: .129, isGlowing: true),

                                                                                                 new KeyTemplate(2.5, 2, hOffset: .37, vOffset: .87, rotationAngle: 30),
                                                                                                 new KeyTemplate(3.5, 2, hOffset: .24, vOffset: 1.37, rotationAngle: 30),
                                                                                                 new KeyTemplate(3.5, 3, hOffset: -.26, vOffset: 1.24, rotationAngle: 30),
                                                                                                 new KeyTemplate(1.5, 3, height: 2, vOffset: .24, rotationAngle: 30),
                                                                                                 new KeyTemplate(2.5, 3, height: 2, hOffset: -.13, vOffset: .74, rotationAngle: 30),
                                                                                                 new KeyTemplate(3.5, 4, hOffset: -.76, vOffset: 1.11, rotationAngle: 30),

                                                                                                 new KeyTemplate(5, 2, hOffset: -.24, vOffset: 1.37, rotationAngle: -30, rotationOrigin:"1,0"),
                                                                                                 new KeyTemplate(6, 2, hOffset: -.37, vOffset: .87, rotationAngle: -30, rotationOrigin:"1,0"),
                                                                                                 new KeyTemplate(5, 3, hOffset: .26, vOffset: 1.24, rotationAngle: -30, rotationOrigin:"1,0"),
                                                                                                 new KeyTemplate(5, 4, hOffset: .76, vOffset: 1.11, rotationAngle: -30, rotationOrigin:"1,0"),
                                                                                                 new KeyTemplate(6, 3, height: 2, hOffset: .13, vOffset: .74, rotationAngle: -30, rotationOrigin:"1,0"),
                                                                                                 new KeyTemplate(7, 3, height: 2, vOffset: .24, rotationAngle: -30, rotationOrigin:"1,0"),
                                                                                             });
            }
            else
            {
                var definition = await _layoutService.GetLayoutTemplate();
                LayoutTemplate = new ObservableCollection<KeyTemplate>(definition);
            }
        }

        #region Private methods

        private void SetLabelUi()
        {
            WindowTitle = "Ergodox Layout";
        }

        private void LostFocus()
        {
            _windowService.CloseWindow<DisplayLayoutWindow>();
        }

        #endregion
    }
}