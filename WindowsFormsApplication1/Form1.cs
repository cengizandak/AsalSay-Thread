using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Data.SqlClient;
namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            

        }
      
        public static int baslangic = 1;
        public static int limitt = 0;
        public int threadno = 0;
        public int d = 0;
        public static int bitiss = 0;
        public static int baslangicc = 0;
       public  bool donecek_deger = false;
       public static bool []don = new bool[100000];
        public int[]  tut = new int[100000];
        private volatile bool stopRun = false;
        private void Kaydet(int sayi,int id)
        {
            SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-RPONHSG\\CENGIZ; Initial Catalog=yazlabb;User Id=cengiz;password=cengiz123;");
            baglanti.Open();
            SqlCommand kmt = new SqlCommand("Insert into asalolanlar(asalsayi,id) VALUES('" + sayi + "','" + id + "') ", baglanti);
            kmt.ExecuteNonQuery();
            baglanti.Close();

        }
        private void Basla(double limit,int sayi,int id,int thread_sayisi)
        {

            int limit_ = 0; ;

            if (threadno == 0)
            {
                baslangic = 1;
                limitt = limitt + (int)limit;
                limit_ = (int)limit;
            }

           else if(threadno>0&&threadno<(thread_sayisi-1))
            {
                baslangic = baslangic + (int)limit;
                limitt = limitt +(int) limit;
                limit_ = (int)limit;
            }
            else if (threadno ==(thread_sayisi - 1))
            {
                
                baslangic = baslangic + (int)limit;
                limitt = limitt + (int)Math.Ceiling(limit);
                limit_ = (int)Math.Ceiling(limit);
                Console.WriteLine("girdi limit : " + (int)Math.Ceiling(limit) + " ve " + limitt);
            }







            int sayac = 1;
            int[] bolecek = new int[limit_];
            Console.WriteLine(sayi+" "+limitt + " " + baslangic);
            SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-RPONHSG\\CENGIZ; Initial Catalog=yazlabb;User Id=cengiz;password=cengiz123;");
            baglanti.Open();

            SqlCommand kmt = new SqlCommand("Select asalsayi FROM asalolanlar WHERE id<='" + limitt + "'and id>='" + baslangic + "'", baglanti);
            SqlDataReader rdr = kmt.ExecuteReader();
            int s = 0;
            while (rdr.Read() && s < limit_)
            {

                bolecek[s] = (int)rdr["asalsayi"];


                s++;
            }

            rdr.Close();
            bitiss = bolecek[limit_ - 1];
            baslangicc = bolecek[sayac - 1];
            threadno++;

            while (sayac <= limit_)
            {
                donecek_deger = false;

                if (sayi % bolecek[sayac - 1] == 0)
                {

                    donecek_deger = true;
                    tut[threadno-1] = bolecek[sayac - 1];
                    don[threadno-1] = true; break;
                }
                else { donecek_deger = false; don[threadno-1] = false; }

                sayac++;
                }Console.WriteLine("donecek deger " + donecek_deger.ToString());

            
            baglanti.Close();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            int b, c;
            b = 5;
            c = 2;
            double a = (double)7 / 2;
            Console.WriteLine(Math.Ceiling(a));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.RunWorkerAsync();

        }
       

        private void button2_Click(object sender, EventArgs e)
        {


            bool deger = false;
            int asal = 0;
            int no = 0;
            backgroundWorker1.CancelAsync();
           for(int i=0;;i++)
            {
                if (don[i] == true) { no = i; deger = true; asal = tut[i]; break; }
                else {deger = false; }
                if (threadno< i) break;

            }
            if (deger == true) { MessageBox.Show("Durdugunuz deger " + d + " Sayı asal değildir  Thread "+ (no+1) +" daki : "+ asal + "e Bölünmektedir"); }
            else MessageBox.Show("Durdugunuz deger : " + d + " Sayı asaldır");
            Thread.Sleep(122);
            Application.Exit();


        }
      

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
          
            int son_deger = 0;
            int son_id = 0;
            int sayac = 0;
            int kac_tane = 0;
            double limit = 0;
            button2.Enabled = true;

            Thread[] yaz_lab;
            Thread kaydet;
            SqlConnection baglanti = new SqlConnection("Data Source=DESKTOP-RPONHSG\\CENGIZ; Initial Catalog=yazlabb;User Id=cengiz;password=cengiz123;");
            baglanti.Open();
            SqlCommand kmt = new SqlCommand("Select TOP 1 * FROM asalolanlar ORDER BY id DESC ", baglanti);

            SqlDataReader rdr = kmt.ExecuteReader();
            while (rdr.Read())
            {

                son_deger = (int)rdr["asalsayi"];

                son_id = (int)rdr["id"];

            }
            rdr.Close();
            int tavan_deger = (int)Math.Sqrt(son_deger);

            kmt = new SqlCommand("Select *FROM asalolanlar", baglanti);
            rdr = kmt.ExecuteReader();
            while (rdr.Read())
            {
                if ((int)rdr["asalsayi"] <= tavan_deger)
                    sayac++;
            }
            rdr.Close();
            if (sayac < 100) kac_tane = 2;
            else { kac_tane = (int)(sayac / 100) + 1; }


            double hesap = sayac / kac_tane;

            for (int j = son_deger + 2; j < 1490000; j = j + 2)
            {


               
                d = j;

                if (backgroundWorker1.CancellationPending ==false)
                
                {
                    button2.Enabled = true;
                    limitt = 0;
                    baslangicc = 0;
                    bitiss = 0;
                    baslangic = 1;
                    threadno = 0;
                    hesap = 0;
                    sayac = 0;
                   
                    textBox1.Clear();
                    listBox1.Items.Clear();
                    textBox1.Text = j.ToString();
                    donecek_deger = false;
                    tavan_deger = (int)Math.Sqrt(j);
                    baglanti.Close();
                    baglanti.Open();
                    kmt = new SqlCommand("Select *FROM asalolanlar", baglanti);
                    rdr = kmt.ExecuteReader();
                    while (rdr.Read())
                    {
                        if ((int)rdr["asalsayi"] <= tavan_deger)
                            sayac++;
                    }
                    rdr.Close();
                    kmt = new SqlCommand("Select TOP 1 * FROM asalolanlar ORDER BY id DESC ", baglanti);

                    rdr = kmt.ExecuteReader();
                    while (rdr.Read())
                    {

                        son_id = (int)rdr["id"];

                    }
                    rdr.Close();
                    if (sayac < 100) kac_tane = 2;
                    else { kac_tane = (int)(sayac / 100) + 1; }
                    yaz_lab = new Thread[kac_tane];
                    hesap = (double)sayac / kac_tane;
                    Console.WriteLine("hesap : "+hesap+" kac tane" + kac_tane+"sayac"+sayac);
                    for (int i = 0; i < kac_tane; i++)
                    {
                        button2.Enabled = true;
                        yaz_lab[i] = new Thread(() => Basla(hesap, j, (son_id + 1), kac_tane));
                        yaz_lab[i].Start();
                        yaz_lab[i].Join(1000);

                        listBox1.Items.Add((i + 1) + ".Thread Baslangıc nosu : " + baslangicc + " Bitis nosu :  " + bitiss + " Asal mi ? " + donecek_deger.ToString());
                        Thread.Sleep(590);
                       
                    }

                    bool deger = false;
                    for(int i=0;;i++)
                    {
                if (don[i] == true){ deger = true; break; }
                else {deger = false; }
                if (threadno< i) break;

                     }
                    
                    if (deger==false) { Console.WriteLine("Kaydedilecek j deger i :" + j + " " + son_id); kaydet = new Thread(() => Kaydet(j, son_id + 1)); kaydet.Start(); kaydet.Join(1000); }

                }
                else { e.Cancel = true; }


                baglanti.Close();
            }
          
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
    }
    

