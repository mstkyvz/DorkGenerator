using System;
using System.Windows.Forms;
using Gecko.DOM;
using System.IO;

namespace Dork_Generator
{
    public partial class Form1 : MetroFramework.Forms.MetroForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] Dorklar = rtb_Dorklar.Text.Split('\n');
            string[] Kelimeler = rtb_kelimeler.Text.Split('\n');
            string[] Sonuc = new string[Kelimeler.Length];
            
            int dorkLenght = Dorklar.Length;
            int count = 0;

            for (int i = 0; i < Kelimeler.Length; i++)
            {
                Sonuc[i] += Dorklar[count];
                count++;
                if (count == dorkLenght)
                    count = 0;
            }

            for (int i = 0; i < Sonuc.Length; i++)
            {
                Sonuc[i] += Kelimeler[i];
                rtb_sonuc.Text += ($"{Sonuc[i]}\n");
            }
        }

        private void metroTextBox4_TextChanged(object sender, EventArgs e)
        {
            if(!int.TryParse(metroTextBox4.Text, out int i)) {
                button2.Enabled = false;
                button2.Text = "Sadece Sayı";
            }
            else {
                button2.Enabled = true;
                button2.Text = "Kelime Çek";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GeckoInputElement icerik = new GeckoInputElement(geckoWebBrowser1.Document.GetElementsByName("qty")[0].DomObject);
            icerik.Value = metroTextBox4.Text;
            GeckoButtonElement button = new GeckoButtonElement(geckoWebBrowser1.Document.GetElementsByName("submit")[0].DomObject);
            button.Click();

            string doc = geckoWebBrowser1.Document.GetElementsByTagName("html")[0].InnerHtml;
            int resultStart = doc.IndexOf("<ul id=\"result\">") +16;
            int resultEnd = doc.Substring(resultStart).IndexOf("</ul>");
            doc = doc.Substring(resultStart, resultEnd);

            doc = doc.Replace("<li class=\"support\">", "").Replace("</li>", "\n").Replace("<i data-original-title=\"<em>Click to <u>save</u> this word!</em>\" class=\"fa fa-heart-o favorite pulsate\" style=\"color: #1abc9c\" data-toggle=\"tooltip\" data-html=\"true\" title=\"\"></i>", "")
                ;
            rtb_kelimeler.Text = doc;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            geckoWebBrowser1.Navigate("https://randomwordgenerator.com");

        }

        private void geckoWebBrowser1_DocumentCompleted(object sender, Gecko.Events.GeckoDocumentCompletedEventArgs e)
        {
            button2.Enabled = true;
            metroTextBox4.Enabled = true;
            button2.Text = "Kelime Çek";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = " Only TXT files |*.txt";
                ofd.ShowDialog();
                rtb_Dorklar.Text = File.ReadAllText(ofd.FileName);
            }
            catch (Exception ex)
            { MessageBox.Show("Bir hatayla karşılaşıldı. Detay:\n" + ex, "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }

        bool rvm = false;

        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult dr = new DialogResult();
            if (rvm == false)
                dr = MessageBox.Show("Dork bölümünün temizlenmesine eminmisiniz?\nNot:Evet derseniz bu mesaj birdaha gösterilmeyecektir.", "Uyarı!", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            else
                rtb_Dorklar.Clear();

            if (dr == DialogResult.Yes)
            {
                rtb_Dorklar.Clear();
                rvm = true;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = " Only TXT files |*.txt";
                ofd.ShowDialog();
                rtb_kelimeler.Text = File.ReadAllText(ofd.FileName);
            }
            catch (Exception ex)
            { MessageBox.Show("Bir hatayla karşılaşıldı. Detay:\n" + ex, "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Information); }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            DialogResult dr = new DialogResult();
            if (rvm == false)
                dr = MessageBox.Show("Dork bölümünün temizlenmesine eminmisiniz?\nNot:Evet derseniz bu mesaj birdaha gösterilmeyecektir.", "Uyarı!", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            else
                rtb_kelimeler.Clear();

            if (dr == DialogResult.Yes)
            {
                rtb_kelimeler.Clear();
                rvm = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
        }
    }
}
