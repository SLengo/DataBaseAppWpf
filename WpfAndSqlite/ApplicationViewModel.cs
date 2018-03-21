using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data.Entity;
using System.Runtime.CompilerServices;

namespace WpfAndSqlite
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        ApplicationContext DataBase;
        RelayCommand addCommand;
        RelayCommand editCommand;
        RelayCommand show3D;

        IEnumerable<Models.Pokemon> pokes;

        public IEnumerable<Models.Pokemon> Pokes
        {
            get { return pokes; }
            set
            {
                pokes = value;
                OnPropertyChanged("Pokes");
            }
        }

        public ApplicationViewModel()
        {
            DataBase = new ApplicationContext();
            DataBase.Pokemons.Load();
            Pokes = DataBase.Pokemons.Local.ToBindingList();
        } 


        public RelayCommand Show3D
        {
            get
            {
                return show3D ?? (
                    show3D = new RelayCommand((selectedItem) =>
                    {
                        if (selectedItem == null || (selectedItem as Models.Pokemon).Model == "") return;
                        else
                        {
                            Poke3D p3d = new Poke3D((selectedItem as Models.Pokemon).Model);
                            p3d.ShowDialog();
                        }
                    })
                    );
            }
        }

        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                   (addCommand = new RelayCommand((o) =>
                   {
                       PokemonWindow pw = new PokemonWindow(new Models.Pokemon());
                       if (pw.ShowDialog() == true)
                       {
                           Models.Pokemon poke_to_base = pw.poke;
                           DataBase.Pokemons.Add(poke_to_base);
                           DataBase.SaveChanges();
                       }
                   }
                   ));
            }
        }

        public RelayCommand EditCommand
        {
            get
            {
                return editCommand ??
                    (editCommand = new RelayCommand(
                        (SelectedItem) =>
                        {
                            if (SelectedItem == null) return;

                            Models.Pokemon sel_poke = SelectedItem as Models.Pokemon;
                            PokemonWindow pw = new PokemonWindow(new Models.Pokemon()
                            {
                                Id = sel_poke.Id,
                                Height = sel_poke.Height,
                                Image = sel_poke.Image,
                                Name = sel_poke.Name,
                                Number = sel_poke.Number,
                                Weight = sel_poke.Weight,
                                Model = sel_poke.Model,
                            });

                            if (pw.ShowDialog() == true)
                            {
                                sel_poke = DataBase.Pokemons.Find(pw.poke.Id);
                                if (sel_poke != null)
                                {
                                    sel_poke.Height = pw.poke.Height;
                                    sel_poke.Image = pw.poke.Image;
                                    sel_poke.Name = pw.poke.Name;
                                    sel_poke.Number = pw.poke.Number;
                                    sel_poke.Weight = pw.poke.Weight;
                                    sel_poke.Model = pw.poke.Model;
                                    DataBase.Entry(sel_poke).State = EntityState.Modified;
                                    DataBase.SaveChanges();
                                }
                            }
                        }
                        ));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
