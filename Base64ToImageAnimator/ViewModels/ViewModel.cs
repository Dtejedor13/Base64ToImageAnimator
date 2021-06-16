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

        List<ImageSource> sheet = new List<ImageSource>();

        public ViewModel()
        {
            sheet = JsonDataReader.LoadSpriteSheet();
        }

        public void LunchAnnimation()
        {
            SpriteController(true);
        }

        /// ///////////////////////////////////// SPRITECONTROLLER //////////////////////////////////////////////////
        private async void SpriteController(bool loop)
        {
            _ = App.Current.Dispatcher.Invoke(async () =>
              {
                  await Task.Run(() => AnimateSpriteSheet(loop));
              });
        }

        private async Task AnimateSpriteSheet(bool loop)
        {
            do
            {
                foreach (ImageSource sprite in sheet)
                {
                    PrimaryImage = sprite;
                    Thread.Sleep(100);
                }
            }
            while (loop);
        }
        /// ///////////////////////////////////// SPRITECONTROLLER //////////////////////////////////////////////////

        
    }
}
