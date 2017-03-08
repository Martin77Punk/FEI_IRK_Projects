using FEI.IRK.HM.HMIvR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.Hardware;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Hardware.Camera2;

[assembly: ExportRenderer(typeof(FEI.IRK.HM.HMIvR.CameraPreview), typeof(FEI.IRK.HM.HMIvR.Droid.AndroidCameraRenderer))]
namespace FEI.IRK.HM.HMIvR.Droid
{
    public class AndroidCameraRenderer : ViewRenderer<CameraPreview, AndroidCamera>
    {
        AndroidCamera cameraPreview;

        protected override void OnElementChanged(ElementChangedEventArgs<CameraPreview> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                cameraPreview = new AndroidCamera(Context);
                SetNativeControl(cameraPreview);
            }

            if (e.OldElement != null)
            {
                // Unsubscribe
                cameraPreview.Click -= OnCameraPreviewClicked;
            }
            if (e.NewElement != null)
            {
                /*try {
                    int camFacing;
                    if (e.NewElement.Camera == CameraOptions.Front)
                    {
                        camFacing = (int)CameraFacing.Front;
                    }
                    else
                    {
                        camFacing = (int)CameraFacing.Back;
                    }
                        

                    Control.Preview = Camera.Open(camFacing);
                } catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(@"			ERROR: " + ex.Message + "; STACK: " + ex.StackTrace);
                }
                */
                Control.Preview = Camera.Open((int)e.NewElement.Camera);

                // Subscribe
                cameraPreview.Click += OnCameraPreviewClicked;
            }
        }

        void OnCameraPreviewClicked(object sender, EventArgs e)
        {
            if (cameraPreview.IsPreviewing)
            {
                cameraPreview.Preview.StopPreview();
                cameraPreview.IsPreviewing = false;
            }
            else
            {
                cameraPreview.Preview.StartPreview();
                cameraPreview.IsPreviewing = true;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Control.Preview.Release();
            }
            base.Dispose(disposing);
        }

    }
}