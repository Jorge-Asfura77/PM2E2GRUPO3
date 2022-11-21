using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace PM2E2GRUPO3.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Mapa : ContentPage
    {
        double _latitud, _longitud;
        String _descripcion;

        public Mapa(double latitud, double longitud, String descripcion)
        {
            InitializeComponent();
            _latitud = latitud;
            _longitud = longitud;
            _descripcion = descripcion;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                var location = await Geolocation.GetLocationAsync();

                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    //await DisplayAlert("Aviso", "Si se leyó la ubicacion: "+location.Latitude +", "+location.Longitude, "OK");
                }
                else
                {
                    await DisplayAlert("Aviso", "El GPS está desactivado o no puede reconocerse", "OK");
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                await DisplayAlert("Aviso", "Este dispositivo no soporta la versión de GPS utilizada", "OK");
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                //await DisplayAlert("Aviso", "Handle not enabled on device exception: "+fneEx, "OK");
                await DisplayAlert("Aviso", "La ubicación está desactivada", "OK");

            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                await DisplayAlert("Aviso", "La aplicación no puede acceder a su ubicación.\n\n" +
                    "Habilite los permisos de ubicación en los ajustes del dispositivo", "OK");
            }
            catch (Exception ex)
            {
                // Unable to get location
                await DisplayAlert("Aviso", "No se ha podido obtener la localización (error de gps)", "OK");
            }

            Pin pin = new Pin();
            pin.Label = _descripcion;
            pin.Type = PinType.Place;
            pin.Position = new Position(_latitud, _longitud);
            mapa.Pins.Add(pin);
            mapa.IsShowingUser = true; //muestra la ubicacion del usuario en donde se encuentra
            mapa.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(_latitud, _longitud), Distance.FromMeters(100.0)));

        }

        private async void btndriving_Clicked(object sender, EventArgs e)
        {
            try
            {
                var location = new Location(_latitud, _longitud);
                var options = new MapLaunchOptions { NavigationMode = NavigationMode.Driving };

                await Xamarin.Essentials.Map.OpenAsync(location, options);
            }
            catch (Exception error)
            {
                await DisplayAlert("Aviso", "Error: " + error, "OK");
            }
        }

        private async void btnvolver_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}