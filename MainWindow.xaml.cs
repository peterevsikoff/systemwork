using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace systemwork
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string date_ekran = "";
        DateTime dt;
        int schet = 0;
        public MainWindow()
        {
            InitializeComponent();
            //string date_ekran = "";
            dt = DateTime.Today;
            date_ekran = dt.ToString("D");//DateTime.Today.ToString("D");

            

            schet = 0;
            lblDate.Content = date_ekran;
            lbl_task_today.Content = "Задачи на сегодня: ";
            btnSaveChanged.IsEnabled = true;
            //List<SysDtg> lstdtg = new List<SysDtg>();

            int cmbIn = 0;
            string txt_eachmonthT;
            string txt_eachkvT;
            string txt_eachhalfyearT;
            string txt_eachyearT;
            string path;

            dtgTask.ItemsSource = VspomMethod.ReadfileOption(out cmbIn, out txt_eachmonthT, out txt_eachkvT,
            out txt_eachhalfyearT, out txt_eachyearT, dt, out path/*, out lstdtg*/);
            cmbeachweek.SelectedIndex = cmbIn;
            txt_eachmonth.Text = txt_eachmonthT;
            txt_eachkv.Text = txt_eachkvT;
            txt_eachhalfyear.Text = txt_eachhalfyearT;
            txt_eachyear.Text = txt_eachyearT;


            string[] mass_files = Directory.GetFiles("tasks");
            string[] new_mass = new string[mass_files.Length];
            int i = 0;
            foreach (string m in mass_files)
            {
                new_mass[i] = mass_files[i].Substring(mass_files[i].IndexOf('k') + 3, mass_files[i].Length - 10);
                i++;
            }
            cmb_status.ItemsSource = new_mass;
            lbl_status.Text = path;
            int j = 0;
            int a = 0;
            foreach (string m in new_mass)
            {
                if (m == path)
                {
                    a = j;
                }
                j++;
            }
            cmb_status.SelectedIndex = a;
            //dtgTask.ItemsSource = lstdtg;
        }

        private void btnSaveChanged_Click(object sender, RoutedEventArgs e)
        {
            //StreamWriter fileOut = new StreamWriter("text.txt", true);
            
            //foreach (SysDtg s in dtgTask.ItemsSource)
            //{
            //    string vyp = "";
            //    if (s.IsExecute == true)
            //    {
            //        vyp = "выполнено";
            //    }
            //    else
            //    {
            //        vyp = "не выполнено";
            //    }
            //    fileOut.WriteLine(lblDate.Content.ToString() + " " + vyp + " " + s.Task.ToString() + " " + s.Comment.ToString());
            //}
            
            //fileOut.Close();
            

            // путь к документу
            string d = DateTime.Today.ToString("D");
            string pathDocument = /*AppDomain.CurrentDomain.BaseDirectory + */"itog/" + d + ".docx";

            // создаём документ
            DocX document = DocX.Create(pathDocument);
            
            // Вставляем параграф и указываем текст
            document.InsertParagraph("Отчет выполнения системы работы - " + lbl_status.Text + " - " + d)
                .Font("Times New Roman")
                .FontSize(15)
                .Bold()
                .Alignment = Alignment.center;

            int kolvo_row = dtgTask.Items.Count;
            
            // создаём таблицу с 3 строками и 2 столбцами
            Xceed.Document.NET.Table table = document.AddTable(kolvo_row + 1, 3);
            table.SetColumnWidth(0, 100);
            table.SetColumnWidth(1, 300);
            // располагаем таблицу по центру
            table.Alignment = Alignment.center;
            // меняем стандартный дизайн таблицы
            table.Design = TableDesign.TableGrid;

            // заполнение ячейки текстом
            table.Rows[0].Cells[0].Paragraphs[0].Append("Отметка о выполнении").Font("Times New Roman").FontSize(13).Alignment=Alignment.center;
            table.Rows[0].Cells[1].Paragraphs[0].Append("Задачи").Font("Times New Roman").FontSize(13).Alignment = Alignment.center;
            table.Rows[0].Cells[2].Paragraphs[0].Append("Примечание").Font("Times New Roman").FontSize(13).Alignment = Alignment.center;




            int i = 0;
            foreach (SysDtg s in dtgTask.ItemsSource)
            {
                i++;
                string vyp = "";
                if (s.IsExecute == true)
                {
                    vyp = "выполнено";
                }
                else
                {
                    vyp = "не выполнено";
                }
                table.Rows[i].Cells[0].Paragraphs[0].Append(vyp).Font("Times New Roman").FontSize(13);
                table.Rows[i].Cells[1].Paragraphs[0].Append(s.Task).Font("Times New Roman").FontSize(13);
                table.Rows[i].Cells[2].Paragraphs[0].Append(s.Comment).Font("Times New Roman").FontSize(13);
                //fileOut.WriteLine(lblDate.Content.ToString() + " " + vyp + " " + s.Task.ToString() + " " + s.Comment.ToString());
            }
            // создаём параграф и вставляем таблицу
            document.InsertParagraph().InsertTableAfterSelf(table);
            // сохраняем документ
            document.Save();

            MessageBox.Show("Информация успешно сохранена!");

        }

        private void btn_Save_Options_Click(object sender, RoutedEventArgs e)
        {
            //ComboBoxItem selectedItem = (ComboBoxItem)cmb_status.SelectedItem;
            //string v = selectedItem.Content.ToString();
            //string CmbTitle = (cmb_status.SelectedItem as ComboBoxItem).Content.ToString();
            //MessageBox.Show(cmb_status.Text);
            
            StreamWriter fileOp = new StreamWriter("options.txt", false, Encoding.GetEncoding(1251));
            fileOp.WriteLine("еженедельно" + "*" + (cmbeachweek.SelectedIndex + 1).ToString() + "\n" + "ежемесячно*"
                + txt_eachmonth.Text + "\n" + "ежеквартально*"
                + txt_eachkv.Text + "\n" + "раз_в_полгода*"
                + txt_eachhalfyear.Text + "\n" + "ежегодно*"
                + txt_eachyear.Text + "\n" + "должность*" + cmb_status.Text);
            fileOp.Close();
            MessageBox.Show("Изменения сохранены!");

            int cmbIn = 0;
            string txt_eachmonthT;
            string txt_eachkvT;
            string txt_eachhalfyearT;
            string txt_eachyearT;
            string path;

            dtgTask.ItemsSource = VspomMethod.ReadfileOption(out cmbIn, out txt_eachmonthT, out txt_eachkvT,
            out txt_eachhalfyearT, out txt_eachyearT, dt, out path/*, out lstdtg*/);
            cmbeachweek.SelectedIndex = cmbIn;
            txt_eachmonth.Text = txt_eachmonthT;
            txt_eachkv.Text = txt_eachkvT;
            txt_eachhalfyear.Text = txt_eachhalfyearT;
            txt_eachyear.Text = txt_eachyearT;

            string[] mass_files = Directory.GetFiles("tasks");
            string[] new_mass = new string[mass_files.Length];
            int i = 0;
            foreach (string m in mass_files)
            {
                new_mass[i] = mass_files[i].Substring(mass_files[i].IndexOf('k') + 3, mass_files[i].Length - 10);
                i++;
            }
            cmb_status.ItemsSource = new_mass;
            lbl_status.Text = path;
            int j = 0;
            int a = 0;
            foreach (string m in new_mass)
            {
                if (m == path)
                {
                    a = j;
                }
                j++;
            }
            cmb_status.SelectedIndex = a;
        }

        private void btn_date_rev_Click(object sender, RoutedEventArgs e)
        {
            dt = dt.AddDays(-1);
            date_ekran = dt.ToString("D");
            lblDate.Content = date_ekran;
            schet--;
            if (schet == -1)//(dt.AddDays(1) == DateTime.Today)
            {
                lbl_task_today.Content = "Задачи которые были вчера:";
                btnSaveChanged.IsEnabled = false;
            }
            else if (schet == 0)
            {
                lbl_task_today.Content = "Задачи на сегодня:";
                btnSaveChanged.IsEnabled = true;
            }
            else if (schet == -2)
            {
                lbl_task_today.Content = "Задачи которые были позавчера:";
                btnSaveChanged.IsEnabled = false;
            }
            else if (schet < -2)
            {
                lbl_task_today.Content = "Задачи которые были:";
                btnSaveChanged.IsEnabled = false;
            }
            if (schet == 1)//(dt.AddDays(1) == DateTime.Today)
            {
                lbl_task_today.Content = "Задачи на завтра:";
                btnSaveChanged.IsEnabled = false;
            }
            else if (schet == 2)
            {
                lbl_task_today.Content = "Задачи на послезавтра:";
                btnSaveChanged.IsEnabled = false;
            }
            int cmbIn = 0;
            string txt_eachmonthT;
            string txt_eachkvT;
            string txt_eachhalfyearT;
            string txt_eachyearT;
            string path;

            dtgTask.ItemsSource = VspomMethod.ReadfileOption(out cmbIn, out txt_eachmonthT, out txt_eachkvT,
            out txt_eachhalfyearT, out txt_eachyearT, dt, out path/*, out lstdtg*/);
            cmbeachweek.SelectedIndex = cmbIn;
            txt_eachmonth.Text = txt_eachmonthT;
            txt_eachkv.Text = txt_eachkvT;
            txt_eachhalfyear.Text = txt_eachhalfyearT;
            txt_eachyear.Text = txt_eachyearT;
        }

        private void btn_date_forw_Click(object sender, RoutedEventArgs e)
        {
            dt = dt.AddDays(1);
            date_ekran = dt.ToString("D");
            lblDate.Content = date_ekran;
            schet++;
            if (schet == 1)//(dt.AddDays(1) == DateTime.Today)
            {
                lbl_task_today.Content = "Задачи на завтра:";
                btnSaveChanged.IsEnabled = false;
            }
            else if (schet == 0)
            {
                lbl_task_today.Content = "Задачи на сегодня:";
                btnSaveChanged.IsEnabled = true;
            }
            if (schet == -1)//(dt.AddDays(1) == DateTime.Today)
            {
                lbl_task_today.Content = "Задачи которые были вчера:";
                btnSaveChanged.IsEnabled = false;
            }
            else if (schet == -2)
            {
                lbl_task_today.Content = "Задачи которые были позавчера:";
                btnSaveChanged.IsEnabled = false;
            }
            else if (schet == 2)
            {
                lbl_task_today.Content = "Задачи на послезавтра:";
                btnSaveChanged.IsEnabled = false;
            }
            else if (schet > 2)
            {
                lbl_task_today.Content = "Задачи на:";
                btnSaveChanged.IsEnabled = false;
            }
            int cmbIn = 0;
            string txt_eachmonthT;
            string txt_eachkvT;
            string txt_eachhalfyearT;
            string txt_eachyearT;
            string path;

            dtgTask.ItemsSource = VspomMethod.ReadfileOption(out cmbIn, out txt_eachmonthT, out txt_eachkvT,
            out txt_eachhalfyearT, out txt_eachyearT, dt, out path/*, out lstdtg*/);
            cmbeachweek.SelectedIndex = cmbIn;
            txt_eachmonth.Text = txt_eachmonthT;
            txt_eachkv.Text = txt_eachkvT;
            txt_eachhalfyear.Text = txt_eachhalfyearT;
            txt_eachyear.Text = txt_eachyearT;
        }

        private readonly SolidColorBrush brush_eachday = new SolidColorBrush(Color.FromArgb(100, 0, 250,0));
        private readonly SolidColorBrush brush_eachweek = new SolidColorBrush(Color.FromArgb(100, 0, 200, 0));
        private readonly SolidColorBrush brush_eachmonth = new SolidColorBrush(Color.FromArgb(100, 0, 150, 0));
        private readonly SolidColorBrush brush_eachkv = new SolidColorBrush(Color.FromArgb(100, 0, 100, 0));
        private readonly SolidColorBrush brush_eachhalfyear = new SolidColorBrush(Color.FromArgb(100, 0, 50, 0));
        private readonly SolidColorBrush brush_eachyear = new SolidColorBrush(Color.FromArgb(100, 0, 10, 0));


        private readonly SolidColorBrush nb = new SolidColorBrush(Colors.White);

        private void gridProducts_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            SysDtg product = (SysDtg)e.Row.DataContext;

            if (product.Per == "ежедневно")
                e.Row.Background = brush_eachday;
            else if (product.Per == "еженедельно")
                e.Row.Background = brush_eachweek;
            else if (product.Per == "ежемесячно")
                e.Row.Background = brush_eachmonth;
            else if (product.Per == "ежеквартально")
                e.Row.Background = brush_eachkv;
            else if (product.Per == "раз_в_полгода")
                e.Row.Background = brush_eachhalfyear;
            else if (product.Per == "ежегодно")
                e.Row.Background = brush_eachyear;
            else
                e.Row.Background = nb;
        }
    }
}

