using Base64Animator.Common;
using Base64Animator.Converter;
using Base64Animator.Data;
using Base64ConverterCore.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Base64Animator.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Primary Props
        private ImageSource _primaryImage = null;

        public ImageSource PrimaryImage
        {
            get { return _primaryImage; }
            set { _primaryImage = value; OnPropertyChanged(); }
        }

        private string _fileName;

        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; OnPropertyChanged(); }
        }
        #endregion

        #region AnimationType Combobox
        private List<string> _animationTypes = new List<string>();

        public List<string> AnimationTypes { get { return _animationTypes; } set { _animationTypes = value; OnPropertyChanged(); } }

        private int _cbxAnimationTypeSelectedIndex = 0;

        public int CbxAnimationTypeSelectedIndex
        {
            get { return _cbxAnimationTypeSelectedIndex; }
            set { _cbxAnimationTypeSelectedIndex = value; OnPropertyChanged(); }
        }
        #endregion

        #region Units Combobox
        private List<int> _unitIDs = new List<int>();

        public List<int> UnitIDs
        {
            get { return _unitIDs; }
            set { _unitIDs = value; OnPropertyChanged(); }
        }

        private List<int> _formIDs = new List<int>();

        public List<int> FormIDs
        {
            get { return _formIDs; }
            set { _formIDs = value; OnPropertyChanged(); }
        }

        private int _cbxUnitIDsSelectedIndex;

        public int CbxUnitIDsSelectedIndex
        {
            get { return _cbxUnitIDsSelectedIndex; }
            set { _cbxUnitIDsSelectedIndex = value; OnPropertyChanged(); SelectedUnitHaseChanged(); }
        }

        private int _cbxFormIDsSelectedIndex = 0;

        public int CbxFormIDsSelectedIndex
        {
            get { return _cbxFormIDsSelectedIndex; }
            set { _cbxFormIDsSelectedIndex = value; OnPropertyChanged(); UpdateAnimationTypes(); }
        }


        #endregion

        #region frame size
        private int _frameHeight;

        public int FrameHeight
        {
            get { return _frameHeight; }
            set { _frameHeight = value; OnPropertyChanged(); }
        }

        private int _frameWidht;

        public int FrameWidht
        {
            get { return _frameWidht; }
            set { _frameWidht = value; OnPropertyChanged(); }
        }
        #endregion

        private ImageSource _bgSource;

        public ImageSource BgSource
        {
            get { return _bgSource; }
            set { _bgSource = value; OnPropertyChanged(); }
        }


        private CancellationTokenSource _tokenSource = null;

        public MainViewModel()
        {
            XamarinBase64Converter converter = new XamarinBase64Converter();
            BgSource = converter.GetImageSourceFromBase64(Base64Animator.Properties.Resources.BG_base64);
            FileName = "";
            UnitIDs = JsonDataReader.GetAllUnitIDs();
            CbxUnitIDsSelectedIndex = 0;
        }

        private void SelectedUnitHaseChanged()
        {
            FormIDs = JsonDataReader.GetAllFormIDs(UnitIDs[CbxUnitIDsSelectedIndex]);
            CbxFormIDsSelectedIndex = 0;
        }

        private void UpdateAnimationTypes()
        {
            int uI = CbxUnitIDsSelectedIndex != -1 ? CbxUnitIDsSelectedIndex : 0;
            int fI = CbxFormIDsSelectedIndex != -1 ? CbxFormIDsSelectedIndex : 0;
            AnimationTypes = JsonDataReader.GetAllAnimationTypes(
                UnitIDs[uI],
                FormIDs[fI]);

            CbxAnimationTypeSelectedIndex = 0;
        }

        public void LunchAnnimation()
        {
            if (_tokenSource != null)
            {
                _tokenSource.Cancel();
                _tokenSource = null;
                //_tokenSource.Dispose();
            }

            int uI = CbxUnitIDsSelectedIndex != -1 ? CbxUnitIDsSelectedIndex : 0;
            int fI = CbxFormIDsSelectedIndex != -1 ? CbxFormIDsSelectedIndex : 0;
            int aT = CbxAnimationTypeSelectedIndex != -1 ? CbxAnimationTypeSelectedIndex : 0;

            List<ImageSource> spriteSheet = JsonDataReader.LoadSpriteSheet(
                UnitIDs[uI],
                FormIDs[fI],
                AnimationTypes[aT]);
            FrameProperties frameprops = JsonDataReader.LoadFrameProperties(
                UnitIDs[uI],
                FormIDs[fI],
                AnimationTypes[aT]);

            if (spriteSheet.Count > 0)
                SpriteController(true, spriteSheet, frameprops);

            /// ///////////////////////////////////// SPRITECONTROLLER //////////////////////////////////////////////////
            #region Sprite Controller
            async void SpriteController(bool loop, List<ImageSource> sptiteSheet, FrameProperties properties)
            {
                // setting frame props
                FrameHeight = properties.height;
                FrameWidht = properties.widht;
                FileName = properties.fileName;

                _tokenSource = new CancellationTokenSource();
                var token = _tokenSource.Token;

                App.Current.Dispatcher.BeginInvokeOnMainThread(async () =>
                {
                    await Task.Run(() => AnimateSpriteSheet(loop, sptiteSheet, token));
                });
            }

            async Task AnimateSpriteSheet(bool loop, List<ImageSource> sptiteSheet, CancellationToken token)
            {
                do
                {
                    foreach (ImageSource sprite in sptiteSheet)
                    {
                        if (token.IsCancellationRequested)
                        {
                            // clean
                            return;
                        }

                        PrimaryImage = sprite;
                        Thread.Sleep(100);
                    }
                }
                while (loop);
            }
            #endregion
            /// ///////////////////////////////////// SPRITECONTROLLER //////////////////////////////////////////////////
        }


    }
}
