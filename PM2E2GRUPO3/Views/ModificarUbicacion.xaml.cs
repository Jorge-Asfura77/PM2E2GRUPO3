using Nancy.Json;
using PM2E2GRUPO3.Controllers;
using PM2E2GRUPO3.Models;
using SignaturePad.Forms;
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
    public partial class ModificarUbicacion : ContentPage
    {
        Sitio _sitio;
        public ModificarUbicacion(Sitio sitio)
        {
            InitializeComponent();

            /*try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var strokesSignature = serializer.DeserializeObject(sitio.firma);
                var a = strokesSignature.GetType();
                PadView.Strokes = (IEnumerable<IEnumerable<Point>>)strokesSignature;
            }
            catch (Exception error)
            {
                DisplayAlert("Aviso", "" + error, "OK");
            }*/
            _sitio = sitio;
            latitud.Text = sitio.Latitud;
            longitud.Text = sitio.Longitud;
            descripcion.Text = sitio.Descripcion;
        }

        private void cleandescripcion_Clicked(object sender, EventArgs e)
        {
            descripcion.Text = null;
        }

        private async void btnactualizar_Clicked(object sender, EventArgs e)
        {
            bool flag1 = false;

            if (descripcion.Text == null || descripcion.Text == "")
            {
                flag1 = true;
                await DisplayAlert("Operación Fallida", "Se necesita una breve descripción de la ubicación.", "OK");
            }

            if (!flag1)
            {
                //byte[] ImageBytes = null;
                //var firma = PadView.Strokes;
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
                    //var image = await PadView.GetImageStreamAsync(SignatureImageFormat.Png);

                    //Pasamos la firma a imagen a base 64
                    //var mStream = (MemoryStream)image;
                    //byte[] data = mStream.ToArray();
                    //string base64Val = Convert.ToBase64String(data);
                    //ImageBytes = Convert.FromBase64String(base64Val);
                }
                catch (Exception error)
                {
                    //await DisplayAlert("Aviso", "No has escrito tu firma", "OK");
                    //return;
                }


                try
                {
                    //var serializer = new JavaScriptSerializer();
                    //var trazado = serializer.Serialize(firma);

                    Sitio sitio = new Sitio
                    {
                        Id = _sitio.Id,
                        Descripcion = descripcion.Text,
                        Latitud = latitud.Text,
                        Longitud = longitud.Text,
                        //FirmaDigital = ImageBytes,
                        //firma = trazado
                    };

                    await SitioController.UpdateSitio(sitio);
                    await DisplayAlert("Aviso", "Sitio modificado con éxito", "Aceptar");
                    await Navigation.PopAsync();
                }
                catch (Exception error)
                {
                    await DisplayAlert("Aviso", "" + error, "OK");
                }


            }
        }

        private async void btneliminar_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Aviso", "¿Seguro que desea eliminar?", "Confirmar", "Volver");
            if (answer)
            {
                await SitioController.DeleteSite("" + _sitio.Id);
                await Navigation.PopAsync();
            }
        }
    }
}