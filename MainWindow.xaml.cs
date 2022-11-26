using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Net;

namespace WPF_ExamV3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<Characters> characters = new List<Characters>();
        public MainWindow()
        {
            InitializeComponent();

            getChars();

            foreach (Characters c in MainWindow.characters)
            {
                chars_listBox.Items.Add(c.Name);
            }
        }

        private void getChars()
        {
            string url = "https://rickandmortyapi.com/api/character";
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            string response;

            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = streamReader.ReadToEnd();
            }

            JObject job = JObject.Parse(json: response);

            List<JToken> results = job["results"].Children().ToList();

            foreach (JToken result in results)
            {
                Characters character = result.ToObject<Characters>();

                characters.Add(character);
            }
        }

        private void Chars_listBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            if (chars_listBox.SelectedItem == null)
                return;

            string selection = chars_listBox.SelectedItem.ToString();


            var image = MainWindow.characters.Where(x => x.Name.Equals(selection)).Select(y => y).ToList();


            string imageURL = "";
            string character = "";
            string status = "";
            string species = "";
            string gender = "";
            

            foreach (Characters s in image)
            {

                imageURL = s.Image;
                character = s.Name;
                status = s.Status;
                species = s.Species;
                gender = s.Gender;

            }

            showImage(imageURL, character, status, species, gender);
        }

        private void showImage(string imageURL, string name, string status, string species, string gender)
        {
            var image = new Image();
            var fullFilePath = imageURL;

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
            bitmap.EndInit();

            image.Source = bitmap;
            grid_image.Children.Add(image);
            lbl_char_page.Content = name;
            lbl_char_page2.Content = status;
            lbl_char_page3.Content = species;
            lbl_char_page4.Content = gender;
            grid_image.Visibility = Visibility.Visible;
            
        }

    }
}
