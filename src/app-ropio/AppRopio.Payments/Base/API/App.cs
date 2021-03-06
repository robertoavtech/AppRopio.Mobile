﻿using System;
using AppRopio.Base.API;
using AppRopio.Payments.API.Services;
using AppRopio.Payments.API.Services.Fake;
using AppRopio.Payments.API.Services.Implementation;
using MvvmCross.Platform;

namespace AppRopio.Payments.API
{
    public class App : MvvmCross.Core.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            if (ApiSettings.DebugServiceEnabled)
                Mvx.RegisterSingleton<IPaymentService>(() => new PaymentFakeService());
            else
                Mvx.RegisterSingleton<IPaymentService>(() => new PaymentService());
        }
    }
}