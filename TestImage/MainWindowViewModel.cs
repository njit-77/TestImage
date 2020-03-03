using System.Windows.Forms;
using System.Windows.Input;

namespace TestImage
{
    class MainWindowViewModel : PropertyChangeBase
    {

        #region Field/Property

        private string picPath;

        public string PicPath
        {
            get { return picPath; }
            set
            {
                if (picPath != value)
                {
                    picPath = value;
                    PropertyChangedBaseEx.OnPropertyChanged(this, p => p.PicPath);
                }
            }
        }

        #endregion


        #region Command

        public ICommand LoadPicCommand =>
            new RelayCommand(() =>
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "All Image Files|*.bmp;*.ico;*.gif;*.jpeg;*.jpg;*.png;*.tif;*.tiff|" +
                    "Windows Bitmap(*.bmp)|*.bmp|" +
                    "Windows Icon(*.ico)|*.ico|" +
                    "Graphics Interchange Format (*.gif)|(*.gif)|" +
                    "JPEG File Interchange Format (*.jpg)|*.jpg;*.jpeg|" +
                    "Portable Network Graphics (*.png)|*.png|" +
                    "Tag Image File Format (*.tif)|*.tif;*.tiff";
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        PicPath = openFileDialog.FileName;
                    }
                }
            }, () => true);

        #endregion

    }
}
