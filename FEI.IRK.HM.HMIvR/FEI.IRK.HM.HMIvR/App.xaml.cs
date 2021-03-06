﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace FEI.IRK.HM.HMIvR
{
	public partial class App : Application
	{
        
		public App ()
		{
            MainPage = new FEI.IRK.HM.HMIvR.MainPage();
		}

		protected override void OnStart ()
		{
            // Handle when your app starts
            DeviceSensors.Current.StartSensorsReading();
        }

		protected override void OnSleep ()
		{
            // Handle when your app sleeps
            DeviceSensors.Current.StopSensorsReading();
        }

		protected override void OnResume ()
		{
            // Handle when your app resumes
            DeviceSensors.Current.StartSensorsReading();
        }
	}
}
