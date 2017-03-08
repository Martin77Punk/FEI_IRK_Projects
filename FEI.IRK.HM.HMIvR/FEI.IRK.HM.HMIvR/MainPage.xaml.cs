using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FEI.IRK.HM.HMIvR
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            Title = "HMI v Robotike";
            //Padding = new Thickness(0, 20, 0, 0);
            Content = new StackLayout {
                Children = {
                    //new Label { Text = "Predná kamera x:" },
                    new CameraPreview {
                        Camera = CameraOptions.Front,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand
                    }
                }
            };

		}
	}
}
