using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using PM2E2GRUPO3.Controllers;
using PM2E2GRUPO3.Models;
using SignaturePad.Forms;
using System.IO;
using Plugin.AudioRecorder;
using Nancy.Json;

namespace PM2E2GRUPO3.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NuevaUbicacion : ContentPage
    {
        bool flag2 = false;

        private readonly AudioRecorderService audioRecorderService = new AudioRecorderService();

        SitioController sitiosApi;
        List<Sitio> ListaSitios;

        public NuevaUbicacion()
        {
            InitializeComponent();
            checkInternet();
            getLocation();

            sitiosApi = new SitioController();
            ListaSitios = new List<Sitio>();

            flag2 = false;
        }


        ////FOTOGRAFIAS

        ////Convertir en arreglo de bits
        //Plugin.Media.Abstractions.MediaFile Filefoto = null;
        //private Byte[] ConvertImageToByteArray()
        //{
        //    if (Filefoto != null)
        //    {
        //        using (MemoryStream memory = new MemoryStream())
        //        {
        //            Stream stream = Filefoto.GetStream();
        //            stream.CopyTo(memory);
        //            return memory.ToArray();
        //        }

        //    }
        //    return null;

        //}

        ////Abrir camara
        //private async void tomarFoto_Clicked(object sender, EventArgs e)
        //{

        //    //var
        //    Filefoto = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
        //    {
        //        Directory = "MisFotos",
        //        Name = "test.jpg",
        //        SaveToAlbum = true,
        //    });

        //    await DisplayAlert("path directorio", Filefoto.Path, "ok");

        //    if (Filefoto != null)
        //    {
        //        foto.Source = ImageSource.FromStream(() =>
        //        {
        //            return Filefoto.GetStream();
        //        });
        //    }

        //}




        private async void grabarvoz_Clicked(object sender, EventArgs e)
        {
            
            var status = await Permissions.RequestAsync<Permissions.Microphone>();
            var status2 = await Permissions.RequestAsync<Permissions.StorageRead>();
            var status3 = await Permissions.RequestAsync<Permissions.StorageWrite>();
            if (status != PermissionStatus.Granted && status2 != PermissionStatus.Granted && status3 != PermissionStatus.Granted)
            {
                return; // si no tiene los permisos no avanza
            }

            //onda1.IsVisible = true;
            //onda2.IsVisible = true;
            ondaespacio.IsVisible = true;
            ondaespacio.Text = "Grabando...";
            //imgmicro.Source = "voice.png";
            btnsalvar.IsEnabled = false;
            grabarvoz.IsVisible = false;
            detenervoz.IsVisible = true;

            await audioRecorderService.StartRecording();

            flag2 = true;
        }

        private async void detenervoz_Clicked(object sender, EventArgs e)
        {
            //onda1.IsVisible = false;
            //onda2.IsVisible = false;
            ondaespacio.IsVisible = true;
            ondaespacio.Text = "Nota de voz guardada";
            //imgmicro.Source = "voiceoff.png";
            btnsalvar.IsEnabled = true;
            grabarvoz.IsVisible = true;
            detenervoz.IsVisible = false;

            await audioRecorderService.StopRecording();

        }


        #region Location
        private void cleanLocation()
        {
            latitud.Text = null;
            longitud.Text = null;
        }

        public async void getLocation()
        {
            try
            {
                var location = await Geolocation.GetLocationAsync();

                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    latitud.Text = "" + location.Latitude;
                    longitud.Text = "" + location.Longitude;
                    //await DisplayAlert("Aviso", "Si se leyó la ubicacion: "+location.Latitude +", "+location.Longitude, "OK");
                }
                else
                {
                    await DisplayAlert("Aviso", "El GPS está desactivado o no puede reconocerse", "OK");
                    cleanLocation();
                }

            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
                await DisplayAlert("Aviso", "Este dispositivo no soporta la versión de GPS utilizada", "OK");
                cleanLocation();
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
                //await DisplayAlert("Aviso", "Handle not enabled on device exception: "+fneEx, "OK");
                await DisplayAlert("Aviso", "La ubicación está desactivada", "OK");
                cleanLocation();

            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
                await DisplayAlert("Aviso", "La aplicación no puede acceder a su ubicación.\n\n" +
                    "Habilite los permisos de ubicación en los ajustes del dispositivo", "OK");
                cleanLocation();
            }
            catch (Exception ex)
            {
                // Unable to get location
                await DisplayAlert("Aviso", "No se ha podido obtener la localización (error de gps)", "OK");
                cleanLocation();
            }
        }
        #endregion
        #region Internet
        public async void checkInternet()
        {
            //await DisplayAlert("Aviso", "si", "OK");
            var current = Connectivity.NetworkAccess;

            if (current != NetworkAccess.Internet)
            {
                // Connection to internet is available
                await DisplayAlert("Aviso", "Usted no tiene acceso a Internet.\nEl acceso a Internet es requerido para el buen funcionamiento de la aplicación.", "OK");
            }

        }
        #endregion

        private async void btnsalvar_Clicked(object sender, EventArgs e)
        {

            ////Guardar foto
            //if (Filefoto == null)
            //{
            //    await DisplayAlert("Operación finalizada", "No se tomó ninguna fotografía", "Hecho");
            //    return;
            //}

            //var foto = new Foto
            //{
            //    foto = ConvertImageToByteArray(),
            //};

            //var result = await App.DBase.SavePhoto(foto);

            //if (result > 0)
            //{
            //    await DisplayAlert("Operación finalizada", "La fotografía se guardó correctamente", "Hecho");
            //}
            //else
            //{
            //    await DisplayAlert("Operación finalizada", "Error al guardar la fotografía", "Hecho");
            //}





            bool flag1 = false;
            if (latitud.Text == null || longitud.Text == null)
            {
                flag1 = true;
                await DisplayAlert("Operación Fallida", "Se necesitan las coordenadas de su ubicación para guardar.", "OK");
            }

            if(descripcion.Text == null || descripcion.Text == "")
            {
                flag1 = true;
                await DisplayAlert("Operación Fallida", "Se necesita una breve descripción de la ubicación.", "OK");
            }

            if (!flag1)
            {
                byte[] ImageBytes = null;
                byte[] AudioBytes = null;
                var firma = PadView.Strokes;
/*
                List<string> list = new List<string>();

                foreach (var item in firma)
                {
                    list.Add(item.ToString());
                }
                foreach (var item in list)
                {
                    Console.WriteLine(item);
                }
                Console.ReadLine();*/

                //obtenemos la firma
                try
                {
                    var image = await PadView.GetImageStreamAsync(SignatureImageFormat.Png);

                    //Pasamos la firma a imagen a base 64
                    var mStream = (MemoryStream)image;
                    byte[] data = mStream.ToArray();
                    string base64Val = Convert.ToBase64String(data);
                    ImageBytes = Convert.FromBase64String(base64Val);
                }
                catch (Exception error)
                {
                    await DisplayAlert("Aviso", "No has escrito tu firma", "OK");
                    return;
                }

                //obtenemos el audio
                try
                {
                    var audio = audioRecorderService.GetAudioFileStream();

                    //Pasamos el audio a imagen a base 64
                    /*var mStream2 = (FileStream)audio;
                    MemoryStream a = new MemoryStream();
                    await mStream2.CopyToAsync(a);
                    byte[] data2 = a.ToArray();
                    string base64Val2 = Convert.ToBase64String(data2);
                    AudioBytes = Convert.FromBase64String(base64Val2);*/

                    AudioBytes = File.ReadAllBytes(audioRecorderService.GetAudioFilePath());
                }
                catch (Exception error)
                {
                    if (flag2)
                    {
                        await DisplayAlert("Aviso", "No has hablado fuerte al grabar tu nota de voz", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Aviso", "No has grabado una nota de voz", "OK");
                    }
                    
                    return;
                }

                try
                {
                    byte[] a;
                    var serializer = new JavaScriptSerializer();

                    //var b = firma.Count();
                    //var b = firma.First(c => c. == id).Name;
                    var b = serializer.Serialize(firma);

                    /*for (int i = 0; i < firma.Count(); i++)
                    {
                        for (int j = 0; i < 10; i++)
                        {
                            for (int k = 0; k < 10; k++)
                            {

                            }
                        }
                    }*/

                    a = null;

                    Sitio sitio = new Sitio
                    {
                        Descripcion = descripcion.Text,
                        Latitud = latitud.Text,
                        Longitud = longitud.Text,
                        FirmaDigital = ImageBytes,
                        AudioFile = AudioBytes,
                        firma = b
                    };

                    await SitioController.CreateSite(sitio);
                    await DisplayAlert("Aviso", "Sitio adicionado con éxito", "OK");
                    PadView.Clear();
                    descripcion.Text = null;

                }
                catch (Exception error)
                {
                    await DisplayAlert("Aviso", ""+error, "OK");
                }

                
            }
        }

        private async void btnubicaciones_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UbicacionesSalvadas());
        }

        private void cleandescripcion_Clicked(object sender, EventArgs e)
        {
            descripcion.Text = null;
        }
    }
}