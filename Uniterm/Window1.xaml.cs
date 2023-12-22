using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Windows.Markup;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace Uniterm
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        DataBase db;
        bool nowy = false, modified = false;
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            cDrawing.ClearAll();
        }

        private void ehMouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (FontFamily f in System.Windows.Media.Fonts.SystemFontFamilies)
            {
                cbFonts.Items.Add(f);
            }
            if (cbFonts.Items.Count > 0)
                cbFonts.SelectedIndex = 0;

            for (int i = 8; i <= 40; i++)
            {
                cbfSize.Items.Add(i);
            }
            cbfSize.SelectedIndex = 4;


            db = new DataBase();
          //  DataTable dt = db.CreateDataTable("select name from uniterms;");

            lbUniterms.SelectionChanged -= ehlbUNitermsSelectionChanged;
            lbUniterms.Items.Clear();


          /*  foreach (DataRow dr in dt.Rows)
            {
                lbUniterms.Items.Add(dr["name"]);
            }*/
            modified = false;
            nowy = false;
            lbUniterms.SelectionChanged += ehlbUNitermsSelectionChanged;
        }

        private void ehCBFontsChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                MyDrawing.fontFamily = new FontFamily(e.AddedItems[0].ToString());
                modified = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void ehcbfSizeChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                MyDrawing.fontsize = (int)e.AddedItems[0];
                modified = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddUniterm au = new AddUniterm();

            au.ShowDialog();

            if (au.tbA.Text.Length > 250 || au.tbB.Text.Length > 250)
            {
                MessageBox.Show("Zbyt długi tekst!\n Maksymalna długość tekstu to 250 znaków!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MyDrawing.sA = au.tbA.Text;
            MyDrawing.sB = au.tbB.Text;

            MyDrawing.sOp = au.rbSr.IsChecked == true ? " ; " : " , ";

            btnRedraw_Click(sender, e);

            modified = true;

        }

        private void btnAddEl_Click(object sender, RoutedEventArgs e)
        {
            AddElem ae = new AddElem();

            ae.ShowDialog();
            if (ae.tbA.Text.Length > 250 || ae.tbB.Text.Length > 250 || ae.tbC.Text.Length > 250)
            {
                MessageBox.Show("Zbyt długi tekst!\n Maksymalna długość tekstu to 250 znaków!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            MyDrawing.eA = ae.tbA.Text;
            MyDrawing.eB = ae.tbB.Text;
            MyDrawing.eC = ae.tbC.Text;

            btnRedraw_Click(sender, e);
            modified = true;
        }

        private void btnRedraw_Click(object sender, RoutedEventArgs e)
        {
            cDrawing.ClearAll();

            DrawingVisual dv = new DrawingVisual();
            using (DrawingContext dc = dv.RenderOpen())
            {
                MyDrawing md = new MyDrawing(dc);

                md.Redraw();
                dc.Close();
            }
            cDrawing.AddElement(dv);

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

            char operacja = 'X';
            
            switch (MessageBox.Show("Co zamienić?\n [Tak]==A, [Nie]==B", "Zamień", MessageBoxButton.YesNoCancel, MessageBoxImage.Question))
            {
                case MessageBoxResult.Yes: operacja = 'A';
                
                    break;
                case MessageBoxResult.No: operacja = 'B';
                
                    break;
                case MessageBoxResult.Cancel: return;
            }

            cDrawing.ClearAll();
            MyDrawing.oper = operacja;
            btnRedraw_Click(sender, e);
            modified = true;
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
           // Int32 fontsize_1 = (Int32)MyDrawing.fontsize;
            try
            {

                string sql = "insert into uniterms values('{0}','{1}','{2}','{3}','{4}','{5}','{6}',''{7}','{8}',{9},'{10}','{11}');";
                if (nowy)
                {
                    sql = "insert into uniterms values('" + tbName.Text + "','" + tbDescription.Text + "','" +
                        MyDrawing.sA + "','" + MyDrawing.sB + "','" + MyDrawing.sOp + "','" + MyDrawing.eA + "','" +
                        MyDrawing.eB + "','" + MyDrawing.eC + "'," + (Int32) MyDrawing.fontsize  + ",'" + MyDrawing.fontFamily + "','" + MyDrawing.oper + "');";
                }
                else
                {
                    
                    sql = "UPDATE uniterms SET " +
      "description = '" + tbDescription.Text +
      "',sA = '" + MyDrawing.sA +
      "',sB ='" + MyDrawing.sB +
      "',sOp ='" + MyDrawing.sOp +
      "',eA = '" + MyDrawing.eA +
      "',eB = '" + MyDrawing.eB +
      "',eC = '" + MyDrawing.eC +
      "',fontSize =" + (Int32)MyDrawing.fontsize +
      ",fontFamily = '" + MyDrawing.fontFamily +
      "',switched ='" + MyDrawing.oper +
        "' WHERE name ='" + tbName.Text + "';"; //C:\SLOWIK\Uniterm\Uniterm\ClassDiagram2.cd
                }

                db.RunQuery(sql);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Wystąpił błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            Window_Loaded(sender, e);

            lbUniterms.SelectionChanged -= ehlbUNitermsSelectionChanged;
            lbUniterms.SelectedValue = tbName.Text;
            lbUniterms.SelectionChanged += ehlbUNitermsSelectionChanged;

          
        }

        private bool CheckSave()
        {

            if (!modified)
                return true;
            else
            {
                switch (MessageBox.Show("Chcesz zapisać?", "Zapis", MessageBoxButton.YesNoCancel, MessageBoxImage.Question))
                {
                    case MessageBoxResult.Yes:
                        {
                            MenuItem_Click_1(null, null);
                            modified = false;
                            nowy = false;
                            return true;
                        }
                    case MessageBoxResult.No:
                        {
                            modified = false;
                            nowy = false;
                            return true;
                        }
                    case MessageBoxResult.Cancel: return false;
                    default: return false;
                }
            }

        }

        private void ehlbUNitermsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CheckSave())
            {
                DataRow dr;
                try
                {
                    dr = db.CreateDataRow(String.Format("select * from uniterms where name = '{0}';", lbUniterms.SelectedItem.ToString()));


                    MyDrawing.eA = (string)dr["eA"];
                    MyDrawing.eB = (string)dr["eB"];
                    MyDrawing.eC = (string)dr["eC"];

                    MyDrawing.sA = (string)dr["sA"];
                    MyDrawing.sB = (string)dr["sB"];
                    MyDrawing.sOp = (string)dr["sOp"];

                    MyDrawing.fontFamily = new FontFamily((string)dr["fontFamily"]);
                    
                    MyDrawing.fontsize = (Int32)dr["fontSize"];

                    MyDrawing.oper = ((string)dr["switched"])[0]; ;


                    tbName.Text = (string)dr["name"];
                    tbDescription.Text = (string)dr["description"];

                    cbFonts.SelectedValue = MyDrawing.fontFamily;
                    cbfSize.SelectedValue = (Int32)MyDrawing.fontsize;

                    cDrawing.ClearAll();



                    DrawingVisual dv = new DrawingVisual();
                    cDrawing.Width = 5000;
                    cDrawing.Height = 5000;

                    btnRedraw_Click(sender, e);
                    nowy = false;
                    modified = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void ehNowyClick(object sender, RoutedEventArgs e)
        {
            MyDrawing.ClearAll();
            cDrawing.ClearAll();
            nowy = true;
            modified = false;
        }

        private void tbDescKeyUP(object sender, KeyEventArgs e)
        {
            modified = true;
        }

        private void HorScroll_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TranslateTransform tt = new TranslateTransform();
            tt.X = -HorScroll.Value;
            tt.Y = -VerScroll.Value;

            cDrawing.RenderTransform = tt;
        }





    }
}
