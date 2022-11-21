using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PM2E2GRUPO3
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new MainPage();
            MainPage = new NavigationPage(new Views.NuevaUbicacion());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
