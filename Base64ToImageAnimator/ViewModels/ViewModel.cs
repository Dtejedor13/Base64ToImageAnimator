using Base64ConverterCore.Models;
using Base64ToImageAnimator.Common;
using Base64ToImageAnimator.Data;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Base64ToImageAnimator.ViewModels
{
    public class ViewModel : ViewModelBase
    {
        private ImageSource _primaryImage = null;

        public ImageSource PrimaryImage
        {
            get { return _primaryImage; }
            set { _primaryImage = value; OnPropertyChanged(); }
        }

        #region frame size
        private int _frameHeight;

        public int FrameHeight
        {
            get { return _frameHeight; }
            set { _frameHeight = value; OnPropertyChanged();}
        }

        private int _frameWidht;

        public int FrameWidht
        {
            get { return _frameWidht; }
            set { _frameWidht = value; OnPropertyChanged(); }
        }
        #endregion

        private string _fileName;

        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; OnPropertyChanged(); }
        }

        public ViewModel()
        {

        }

        public void LunchAnnimation()
        {
            List<ImageSource> spriteSheet = JsonDataReader.LoadSpriteSheet();
            FrameProperties frameprops = JsonDataReader.LoadFrameProperties();
            SpriteController(true, spriteSheet, frameprops);
        }

        /// ///////////////////////////////////// SPRITECONTROLLER //////////////////////////////////////////////////
        #region Sprite Controller
        private async void SpriteController(bool loop, List<ImageSource> sptiteSheet, FrameProperties properties)
        {
            // setting frame props
            FrameHeight = properties.height;
            FrameWidht = properties.widht;
            FileName = properties.fileName;

            _ = App.Current.Dispatcher.Invoke(async () =>
              {
                  await Task.Run(() => AnimateSpriteSheet(loop, sptiteSheet));
              });
        }

        private async Task AnimateSpriteSheet(bool loop, List<ImageSource> sptiteSheet)
        {
            do
            {
                foreach (ImageSource sprite in sptiteSheet)
                {
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
