using Plugin.AudioRecorder;
using PM2E2GRUPO3.Controllers;
using PM2E2GRUPO3.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PM2E2GRUPO3.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UbicacionesSalvadas : ContentPage
    {
        ItemTappedEventArgs elemento = null;

        public UbicacionesSalvadas()
        {
            InitializeComponent();
        }

        private async void recargar()
        {
            lblcargando.Text = "Cargando...";
            ListaSitios.ItemsSource = null;
            elemento = null;
            btnactualizar.IsEnabled = false;
            btnescucharaudio.IsEnabled = false;
            btnvermapa.IsEnabled = false;
            ListaSitios.ItemsSource = await SitioController.GetAllSite();

            int c = 0;
            foreach (var item in ListaSitios.ItemsSource)
            {
                c++;
            }
            frmcargando.IsVisible = false;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            recargar();
        }

        private async void nuevaubicacion_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NuevaUbicacion());
        }

        private void ListaSitios_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            elemento = e;
            btnactualizar.IsEnabled = true;
            btnescucharaudio.IsEnabled = true;
            btnvermapa.IsEnabled = true;
        }

        private void btnactualizar_Clicked(object sender, EventArgs e)
        {
            var sitio = (Sitio)elemento.Item;
            Navigation.PushAsync(new ModificarUbicacion(sitio));
            
            /*var secondPage = new ModificarUbicacion(sitio);
            
            secondPage.BindingContext = new { sitio };
            Navigation.PushAsync(secondPage);*/
        }

        private async void btnvermapa_Clicked(object sender, EventArgs e)
        {
            var sitio = (Sitio)elemento.Item;
            await Navigation.PushAsync(new Mapa(Double.Parse(sitio.Latitud), Double.Parse(sitio.Longitud), sitio.Descripcion)); // Abre el Page de Mapa
        }

        private void btnescucharaudio_Clicked(object sender, EventArgs e)
        {
            var sitio = (Sitio)elemento.Item;

            // Crea un archivo temporal y obtiene 
            // su ruta:
            string archivoTemp = Path.GetTempFileName();
            File.WriteAllBytes(archivoTemp, sitio.AudioFile);

            AudioPlayer audioPlayer = new AudioPlayer();
            audioPlayer.Play(archivoTemp);

            File.Delete(archivoTemp);
        }
    }
}