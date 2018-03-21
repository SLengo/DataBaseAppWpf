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
using System.Windows.Shapes;
using Microsoft.Win32;
using System.IO;

namespace WpfAndSqlite
{
    /// <summary>
    /// Interaction logic for PokemonWindow.xaml
    /// </summary>
    public partial class PokemonWindow : Window
    {
        public Models.Pokemon poke { get; set; }

        private string filename;
        private string modelname;

        public PokemonWindow(Models.Pokemon p)
        {
            InitializeComponent();
            this.Owner = Application.Current.MainWindow;
            poke = p;
            this.DataContext = poke;
        }

        private void ChooseImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image files (*.png)|*.png|(*.PNG)|*.PNG|(*.jpg)|*.jpg|(*.jpeg)|*.jpeg";
            if (ofd.ShowDialog() == true)
            {
                filename = ofd.FileName;
                ImagePoke.Source = new BitmapImage(new Uri(filename));
                ChooseImage.Content = System.IO.Path.GetFileName(ofd.FileName);
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (filename != null)
            {
                File.Copy(filename, AppDomain.CurrentDomain.BaseDirectory + "Images/" + System.IO.Path.GetFileName(filename));
                poke.Image = "/Images/" + System.IO.Path.GetFileName(filename);
            }
            if(modelname != null)
            {
                string[] files = Directory.GetFiles(modelname);
                string obj_file = "";
                for (int i = 0; i < files.Length; i++)
                {
                    if (System.IO.Path.GetFileName(files[i]).Contains(".obj"))
                    {
                        obj_file = System.IO.Path.GetFileName(files[i]);
                        break;
                    }
                }
                if (obj_file != "")
                {

                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Models/" + System.IO.Path.GetFileNameWithoutExtension(obj_file));
                    for (int i = 0; i < files.Length; i++)
                    {
                        File.Copy(files[i],
                            AppDomain.CurrentDomain.BaseDirectory + "Models/" + System.IO.Path.GetFileNameWithoutExtension(obj_file) + "/" + System.IO.Path.GetFileName(files[i]), true);
                    }
                    poke.Model = "/Models/" + System.IO.Path.GetFileNameWithoutExtension(obj_file) + "/" + obj_file;
                }
                else
                {
                    MessageBox.Show("В выбранной директории нет obj!");
                }
            }
            this.DialogResult = true;
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChooseModel_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                modelname = dialog.SelectedPath;
                ChooseModel.Content = System.IO.Path.GetDirectoryName(modelname);
            }
        }
    }
}
