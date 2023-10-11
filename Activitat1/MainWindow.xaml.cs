using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Diagnostics;

namespace Activitat1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private string paraula = "";
        private int ronda = 0;

        private void Button_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show(ronda.ToString());

            //Obtenim el botó que ha activat el event click i en guardem el contingut (content)
            Button btn = (Button)sender;
            string lletra = btn.Content.ToString();

            if (btn.Tag != null && btn.Tag.ToString() == "backspace") //Icona backspace - Esborrar
            {
                esborrarLletra();
            }
            else if (btn.Tag != null && btn.Tag.ToString() == "return") //Icona return - Validar
            {
                if (ronda < 5)
                {
                    comparador();
                } else
                {
                    MessageBox.Show("T'has quedat sense oportunitats, sort a la pròxima", "", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    Application.Current.Shutdown();
                }
            }
            else
            {
                afegirLletra(lletra);
            }
        }

        private void afegirLletra(string lletra)
        {
            bool modificat = false;

            //En el corresponent label, li assignem el content, segons la lletra seleccionada
            if (paraula.Length < 5)
            {
                for (int i = 0; i < taulerLletres.RowDefinitions.Count; i++)
                {
                    for (int j = 0; j < taulerLletres.ColumnDefinitions.Count; j++)
                    {
                        //Busquem el label posicionat a la fila-columna corresponent i si està buit, hi afegim el caràcter
                        if (taulerLletres.Children
                        .Cast<UIElement>()
                        .FirstOrDefault(e => Grid.GetRow(e) == i && Grid.GetColumn(e) == j) is Label lbl && string.IsNullOrEmpty(lbl.Content?.ToString())) //'?' ens serveix per evitar que si el resultat és null no salit l'excepció si no que retorna un valor null directament
                        {
                            lbl.Content = lletra.ToUpper();
                            modificat = true;
                            break;
                        }
                    }

                    if (modificat)
                    {
                        break;
                    }
                }

                paraula += lletra;
            }
        }

        private void esborrarLletra()
        {
            if (paraula.Length > 0)
            {
                //Amb el mètode substring, obtenim un nou string reduït per un caràcter (el que es troba en la última posició)
                paraula = paraula.Substring(0, paraula.Length - 1);

                //Busquem l'últim de la fila en que estem textblock que té contingut i l'esborrem
                int row = ronda;
                Label ultimLbl = null;
                for (int j = taulerLletres.ColumnDefinitions.Count - 1; j >= 0; j--)
                {
                    if (taulerLletres.Children
                        .Cast<UIElement>()
                        .FirstOrDefault(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == j) is Label lbl && !string.IsNullOrEmpty(lbl.Content?.ToString()))
                    {
                        ultimLbl = lbl;
                        break;
                    }
                }
                if (ultimLbl != null)
                {
                    ultimLbl.Content = "";
                }
            }
            else
            {
                MessageBox.Show("No ha inserit cap caràcter", "", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void comparador()
        {
            if (paraula.Length < 5)
            {
                MessageBox.Show("La paraula ha de ser de cinc caràcters", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                paraula = "";
                ronda++;
            }
        }
    }
}
