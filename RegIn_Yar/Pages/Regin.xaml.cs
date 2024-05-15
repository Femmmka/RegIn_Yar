using Microsoft.Win32;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;


namespace RegIn_Yar.Pages
{
    public partial class Regin : Page
    {
        private OpenFileDialog FileDialogImage = new OpenFileDialog();
        private bool BCorrectLogin = false;
        private bool BCorrectPassword = false;
        private bool BCorrectConfirmPassword = false;
        private bool BSetImages = false;

        private void CorrectLogin()
        {
            SetNotification("Login already in use", Brushes.Red);
            BCorrectLogin = false;
        }

        private void InCorrectLogin() =>
            SetNotification("", Brushes.Black);

        public Regin()
        {
            InitializeComponent();
            MainWindow.mainWindow.UserLogIn.HandlerCorrectLogin += CorrectLogin;
            MainWindow.mainWindow.UserLogIn.HandlerInCorrectLogin += InCorrectLogin;

            FileDialogImage.Filter = "PNG (*.png)|*.png|JPG (*.jpg)|*.jpg";
            FileDialogImage.RestoreDirectory = true;
            FileDialogImage.Title = "Choose a photo for your avatar";
        }

        private void SetLogin(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SetLogin();
        }

        private void SetLogin(object sender, System.Windows.RoutedEventArgs e) =>
            SetLogin();

        public void SetLogin()
        {
            Regex regex = new Regex(@"([a-zA-Z0-9._-]{4,}@[a-zA-Z0-9._-]{2,}\.[a-zA-Z0-9_-]{2,})");
            BCorrectLogin = regex.IsMatch(TbLogin.Text);
            if (regex.IsMatch(TbLogin.Text) == true)
            {
                SetNotification("", Brushes.Black);
                MainWindow.mainWindow.UserLogIn.GetUserLogin(TbLogin.Text);
            }
            else
                SetNotification("Invalid login", Brushes.Red);
            OnRegin();
        }

        #region SetPassword
        private void SetPassword(object sender, System.Windows.RoutedEventArgs e) =>
             SetPassword();

        private void SetPassword(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SetPassword();
        }

        public void SetPassword()
        {
            Regex regex = new Regex(@"(?=.*[0-9])(?=.*[!@#$%^&?*\-_=])(?=.*[a-z])(?=.*[A-Z])[0-9a-zA-Z!@#$%^&?*\-_=]{10,}");
            BCorrectPassword = regex.IsMatch(TbPassword.Password);
            if (regex.IsMatch(TbPassword.Password) == true)
            {
                SetNotification("", Brushes.Black);
                if (TbConfirmPassword.Password.Length > 0)
                    ConfirmPassword(true);
                OnRegin();
            }
            else
                SetNotification("Invalid password", Brushes.Red);
        }
        #endregion

        #region SetConfirmPassword
        private void ConfirmPassword(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ConfirmPassword();
        }

        private void ConfirmPassword(object sender, System.Windows.RoutedEventArgs e) =>
            ConfirmPassword();

        public void ConfirmPassword(bool Pass = false)
        {
            BCorrectConfirmPassword = TbConfirmPassword.Password == TbPassword.Password;
            if (TbConfirmPassword.Password != TbPassword.Password)
                SetNotification("Passwords do not match", Brushes.Red);
            else
            {
                SetNotification("", Brushes.Black);
                if (!Pass)
                    SetPassword();
            }
        }
        #endregion

        void OnRegin()
        {
            if (!BCorrectLogin)
                return;
            if (TbName.Text.Length == 0)
                return;
            if (!BCorrectPassword)
                return;
            if (!BCorrectConfirmPassword)
                return;
            MainWindow.mainWindow.UserLogIn.Login = TbLogin.Text;
            MainWindow.mainWindow.UserLogIn.Password = TbPassword.Password;
            MainWindow.mainWindow.UserLogIn.Name = TbName.Text;
            if (BSetImages)
                MainWindow.mainWindow.UserLogIn.Image = File.ReadAllBytes(Directory.GetCurrentDirectory() + @"\IUser.jpg");
            MainWindow.mainWindow.UserLogIn.DateUpdate = DateTime.Now;
            MainWindow.mainWindow.UserLogIn.DateCreate = DateTime.Now;
            MainWindow.mainWindow.OpenPage(new Confirmation(Confirmation.TypeConfirmation.Regin));
        }

        private void SetName(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(Char.IsLetter(e.Text, 0));
        }
        public void SetNotification(string Message, SolidColorBrush _Color)
        {
            LNameUser.Content = Message;
            LNameUser.Foreground = _Color;
        }

        private void SelectImage(object sender, MouseButtonEventArgs e)
        {
            if (FileDialogImage.ShowDialog() == true)
            {
                BitmapImage image = new BitmapImage(new Uri(FileDialogImage.FileName));

                int NewWidth = 0;
                int NewHeight = 0;
                double aspectRatio = image.Width / image.Height;
                if (aspectRatio > 1)
                {
                    NewWidth = 256;
                    NewHeight = (int)(256 / aspectRatio);
                }
                else
                {
                    NewWidth = (int)(256 * aspectRatio);
                    NewHeight = 256;
                }

                image.DecodePixelWidth = NewWidth;
                image.DecodePixelHeight = NewHeight;

                // Saving "IUser.jpg" is omitted for brevity

                DoubleAnimation StartAnimation = new DoubleAnimation();
                StartAnimation.From = 1;
                StartAnimation.To = 0;
                StartAnimation.Duration = TimeSpan.FromSeconds(0.6);
                StartAnimation.Completed += delegate
                {
                    IUser.Source = image;
                    DoubleAnimation EndAnimation = new DoubleAnimation();
                    EndAnimation.From = 0;
                    EndAnimation.To = 1;
                    EndAnimation.Duration = TimeSpan.FromSeconds(1.2);
                    IUser.BeginAnimation(Image.OpacityProperty, EndAnimation);
                };
                IUser.BeginAnimation(Image.OpacityProperty, StartAnimation);
                BSetImages = true;
            }
            else
                BSetImages = false;
        }

        private void OpenLogin(object sender, MouseButtonEventArgs e)
        {
            MainWindow.mainWindow.OpenPage(new Login());
        }
    }
}
