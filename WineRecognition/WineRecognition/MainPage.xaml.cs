using Android.Content;
using Android.Provider;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WineRecognition
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

        }

        private async void OnClick(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            if (btn.Text == "From your Gallery")
            {
                var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Please pick a photo"
                });
                if (result != null)
                {
                    var stream = await result.OpenReadAsync();
                    await Navigation.PushAsync(new ImagePage(stream));
                }
            }

            if (btn.Text == "From your camera")
            {
                var result = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions
                {
                    Title = "Please take a photo"
                });
                if (result != null)
                {
                    var stream = await result.OpenReadAsync();
                    await Navigation.PushAsync(new ImagePage(stream));
                }

                /*var result = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions(){});
                Debug.WriteLine("Debug : " + result.Path);
                if (result != null)
                {
                    await Navigation.PushAsync(new ImagePage(result));
                }*/
            }
        }

        private void OnPress(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.BackgroundColor = Color.White;
            
        }


        private void OnRelease(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.BackgroundColor = Color.FromRgb(33, 150, 243);
        }


    }
}
