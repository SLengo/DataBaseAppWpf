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
using System.Windows.Media.Media3D;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using HelixToolkit.Wpf;

namespace WpfAndSqlite
{
    /// <summary>
    /// Interaction logic for Poke3D.xaml
    /// </summary>
    public partial class Poke3D : Window
    {
        public Poke3D(string path_to_3d)
        {
            InitializeComponent();

            var importer = new HelixToolkit.Wpf.ModelImporter();
            try
            {
                Model3D MyModel = importer.Load(AppDomain.CurrentDomain.BaseDirectory + path_to_3d);
                model.Content = MyModel;
            }
            catch (Exception e) {
                MessageBox.Show(e.Message);
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (e.ChangedButton == MouseButton.Left)
              this.DragMove();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
