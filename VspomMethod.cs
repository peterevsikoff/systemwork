using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace systemwork
{
    public class VspomMethod
    {
        public static List<SysDtg> ReadfileOption(out int selI, out string txt_eachmonthT, out string txt_eachkvT,
            out string txt_eachhalfyearT, out string txt_eachyearT, DateTime dt, out string path/*, out List<SysDtg> lstdtg*/)
        {
            List<ListTasks> listTasks = new List<ListTasks>();
            int for_cmbeachweek = 0;
            int for_txteachmonth = 0;
            int for_txteachkv = 0;
            int for_txthalfyear = 0;
            int for_txteachyear = 0;

            selI = 0;
            txt_eachmonthT = "";
            txt_eachkvT = "";
            txt_eachhalfyearT = "";
            txt_eachyearT = "";
            List<SysDtg> lstdtg = new List<SysDtg>();

            path = "";

            //lstdtg.Add(new SysDtg { IsExecute = false, Comment = "", Task = "" });
            

            
           


            //lbl_status.Text = mass_files[0].Substring(mass_files[0].IndexOf('k')+3, mass_files[0].Length-10);

            //lbl_status.Text = "Заместитель командира батальона по ИР";

            StreamReader fileOption = new StreamReader("options.txt", Encoding.GetEncoding(1251));
            string path_to_file="";
            while (!fileOption.EndOfStream)
            {
                string line = fileOption.ReadLine();
                string[] mas = line.Split('*');
                if (mas[0] == "еженедельно")
                {
                    if (mas[1] == "1")
                    {
                        //cmbeachweek.SelectedIndex = 0;
                        for_cmbeachweek = 1;
                        selI = 0;
                    }
                    if (mas[1] == "2")
                    {
                        //cmbeachweek.SelectedIndex = 1;
                        for_cmbeachweek = 2;
                        selI = 1;
                    }
                    if (mas[1] == "3")
                    {
                        //cmbeachweek.SelectedIndex = 2;
                        for_cmbeachweek = 3;
                        selI = 2;
                    }
                    if (mas[1] == "4")
                    {
                        //cmbeachweek.SelectedIndex = 3;
                        for_cmbeachweek = 4;
                        selI = 3;
                    }
                    if (mas[1] == "5")
                    {
                        //cmbeachweek.SelectedIndex = 4;
                        for_cmbeachweek = 5;
                        selI = 4;
                    }
                    if (mas[1] == "6")
                    {
                        //cmbeachweek.SelectedIndex = 5;
                        for_cmbeachweek = 6;
                        selI = 5;
                    }
                    if (mas[1] == "7")
                    {
                        //cmbeachweek.SelectedIndex = 6;
                        for_cmbeachweek = 7;
                        selI = 6;
                    }
                }
                if (mas[0] == "ежемесячно")
                {
                    for_txteachmonth = int.Parse(mas[1]);
                    txt_eachmonthT = mas[1];
                }
                if (mas[0] == "ежеквартально")
                {
                    for_txteachkv = int.Parse(mas[1]);
                    txt_eachkvT = mas[1];
                }
                if (mas[0] == "раз_в_полгода")
                {
                    for_txthalfyear = int.Parse(mas[1]);
                    txt_eachhalfyearT = mas[1];
                }
                if (mas[0] == "ежегодно")
                {
                    for_txteachyear = int.Parse(mas[1]);
                    txt_eachyearT = mas[1];
                }
                if (mas[0] == "должность")
                {
                    path_to_file = "tasks\\" + mas[1] + ".txt";
                    path = mas[1];
                }
            }
            fileOption.Close();
           

            StreamReader fileIn = new StreamReader(/*"tasks.txt"*/path_to_file, Encoding.GetEncoding(1251));
            while (!fileIn.EndOfStream)
            {
                string line = fileIn.ReadLine();
                string[] mas = line.Split('*');
                if (mas[0] == "ежедневно")
                {
                    //MessageBox.Show(mas[1]);
                    listTasks.Add(new ListTasks { Period = "ежедневно", TaskinList = mas[1] });
                }
                    
                if (mas[0] == "еженедельно")
                    listTasks.Add(new ListTasks { Period = "еженедельно", TaskinList = mas[1] });
                if (mas[0] == "ежемесячно")
                    listTasks.Add(new ListTasks { Period = "ежемесячно", TaskinList = mas[1] });
                if (mas[0] == "ежеквартально")
                    listTasks.Add(new ListTasks { Period = "ежеквартально", TaskinList = mas[1] });
                if (mas[0] == "раз_в_полгода")
                    listTasks.Add(new ListTasks { Period = "раз_в_полгода", TaskinList = mas[1] });
                if (mas[0] == "ежегодно")
                    listTasks.Add(new ListTasks { Period = "ежегодно", TaskinList = mas[1] });
            }
            fileIn.Close();



            var eachday = from d in listTasks
                          where d.Period == "ежедневно"
                          select d;
            foreach (var c in eachday)
            {
                lstdtg.Add(new SysDtg { IsExecute = false, Comment = "", Task = c.TaskinList, Per = "ежедневно" });
            }

            int day = (int)dt.DayOfWeek;


            if (day == for_cmbeachweek || day == for_cmbeachweek - 1 || day == for_cmbeachweek + 1)// день +-1
            {
                var eachweek = from d in listTasks
                               where d.Period == "еженедельно"
                               select d;
                foreach (var c in eachweek)
                {
                    lstdtg.Add(new SysDtg { IsExecute = false, Comment = "", Task = c.TaskinList, Per = "еженедельно" });
                }
            }

            int dayMonth = (int)dt.Day;
            if (dayMonth == for_txteachmonth || dayMonth == for_txteachmonth - 1 || dayMonth == for_txteachmonth + 1)//+-1 days
            {
                var eachmonth = from d in listTasks
                                where d.Period == "ежемесячно"
                                select d;
                foreach (var c in eachmonth)
                {
                    lstdtg.Add(new SysDtg { IsExecute = false, Comment = "", Task = c.TaskinList, Per = "ежемесячно" });
                }
            }
            int month = (int)dt.Month;
            if (month == 3 && dayMonth == for_txteachkv || month == 3 && dayMonth == for_txteachkv - 1 || month == 3 && dayMonth == for_txteachkv + 1 ||
                month == 6 && dayMonth == for_txteachkv || month == 6 && dayMonth == for_txteachkv - 1 || month == 6 && dayMonth == for_txteachkv + 1 ||
                month == 9 && dayMonth == for_txteachkv || month == 9 && dayMonth == for_txteachkv - 1 || month == 9 && dayMonth == for_txteachkv + 1 ||
                month == 12 && dayMonth == for_txteachkv || month == 12 && dayMonth == for_txteachkv - 1 || month == 12 && dayMonth == for_txteachkv + 1)
            {
                var eachkv = from d in listTasks
                             where d.Period == "ежеквартально"
                             select d;
                foreach (var c in eachkv)
                {
                    lstdtg.Add(new SysDtg { IsExecute = false, Comment = "", Task = c.TaskinList, Per = "ежеквартально" });
                }
            }

            if (month == 6 && dayMonth == for_txthalfyear || month == 6 && dayMonth == for_txthalfyear - 1 || month == 6 && dayMonth == for_txthalfyear + 1 ||
                month == 12 && dayMonth == for_txthalfyear || month == 12 && dayMonth == for_txthalfyear - 1 || month == 12 && dayMonth == for_txthalfyear + 1)
            {
                var eachhalfyear = from d in listTasks
                                   where d.Period == "раз_в_полгода"
                                   select d;
                foreach (var c in eachhalfyear)
                {
                    lstdtg.Add(new SysDtg { IsExecute = false, Comment = "", Task = c.TaskinList, Per = "раз в полгода" });
                }
            }
            if (month == 12 && dayMonth == for_txteachyear || month == 12 && dayMonth == for_txteachyear - 1 || month == 12 && dayMonth == for_txteachyear + 1)
            {
                var eachyear = from d in listTasks
                               where d.Period == "ежегодно"
                               select d;
                foreach (var c in eachyear)
                {
                    lstdtg.Add(new SysDtg { IsExecute = false, Comment = "", Task = c.TaskinList, Per = "ежегодно" });
                }
            }
            return lstdtg;
        }
    }
}
