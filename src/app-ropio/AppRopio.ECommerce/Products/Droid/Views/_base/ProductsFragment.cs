﻿using System;
using Android.Util;
using AppRopio.Base.Core.Services.ViewLookup;
using AppRopio.Base.Droid.Views;
using AppRopio.ECommerce.Products.Core.Services;
using AppRopio.ECommerce.Products.Core.ViewModels;
using MvvmCross.Binding.Droid.BindingContext;
using MvvmCross.Droid.Views;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;

namespace AppRopio.ECommerce.Products.Droid.Views
{
    public abstract class ProductsFragment<T> : CommonFragment<T>
        where T : class, IProductsViewModel
    {
        #region Fields

        protected const int MENU_SEARCH_ID = 1;
        protected const int MENU_CARD_INDICATOR_ID = 2;

        #endregion

        #region Constructor

        protected ProductsFragment()
        {
        }

        public ProductsFragment(int layoutId)
            : base(layoutId)
        {
        }

        public ProductsFragment(int layoutId, string title)
            : base(layoutId, title)
        {
        }

        #endregion

        #region Protected

        protected virtual void SetupTitle()
        {
            Title = ViewModel?.Title;
        }

        #endregion

        #region Public

        public override void OnCreateOptionsMenu(Android.Views.IMenu menu, Android.Views.MenuInflater inflater)
        {
            if (ViewModel?.CartIndicatorVM != null)
            {
                var config = Mvx.Resolve<IProductConfigService>().Config;
                var cartIndicatorType = config.Basket.CartIndicator.TypeName;

                var viewLookupService = Mvx.Resolve<IViewLookupService>();

                if (viewLookupService.IsRegistered(cartIndicatorType))
                {
                    if (Activator.CreateInstance(viewLookupService.Resolve(cartIndicatorType), Context) is IMvxAndroidView cartIndicatorView)
                    {
                        cartIndicatorView.BindingContext = new MvxAndroidBindingContext(Context, new MvxSimpleLayoutInflaterHolder(LayoutInflater), ViewModel.CartIndicatorVM);

                        var menuItem = menu.Add(0, MENU_CARD_INDICATOR_ID, 0, new Java.Lang.String("Корзина"));

                        menuItem.SetShowAsAction(Android.Views.ShowAsAction.Always);
                        menuItem.SetActionView(cartIndicatorView as Android.Views.View);
                    }
                }
            }

            if (ViewModel?.SearchEnabled ?? false)
            {
                var menuItem = menu.Add(0, MENU_SEARCH_ID, 0, new Java.Lang.String("Поиск"));

                var typedValue = new TypedValue();
                Activity.Theme.ResolveAttribute(Resource.Attribute.app_products_ic_toolbar_search, typedValue, true);

                try
                {
                    var drawable = Activity.Resources.GetDrawable(typedValue.ResourceId, Activity.Theme);
                    menuItem.SetIcon(drawable);
                }
                catch
                {
                    MvxTrace.Trace(() => $"Not found drawable resource by id: {typedValue.ResourceId}");
                }

                menuItem.SetShowAsAction(Android.Views.ShowAsAction.Always);
            }
        }

        public override bool OnOptionsItemSelected(Android.Views.IMenuItem item)
        {
            switch (item.ItemId)
            {
                case MENU_SEARCH_ID:
                    ViewModel?.ShowSearchCommand.Execute(null);
                    return base.OnOptionsItemSelected(item);
            }

            return base.OnOptionsItemSelected(item);
        }

        public override void OnViewCreated(Android.Views.View view, Android.OS.Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            SetupTitle();

            HasOptionsMenu = true;
        }

        #endregion
    }
}