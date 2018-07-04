using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GetSomeString
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //http://localhost:8073/trade/orders_test/para/1eff6e9e-95d2-402b-a385-23de49a3a62a,8636dbd5-29af-46de-a57d-e416735a33asd,1177718,365580
            string ipaddress = @"http://localhost:8073/trade/orders_test";
            string para = @"para";
            string acountid = @"1eff6e9e-95d2-402b-a385-23de49a3a62a";
            string correlationOrderID = @"8636dbd5-29af-46de-a57d-e416735a33asd";
            string eventid = @"1177718";

            string marketid = @"365580";
            string selection = @"30001";
            string v1 = @"-1.5";
            string v2 = @"3";
            string v3 = @"1";

            string pirce = "2";
            string amount = "10";
            string result = "5";

            textBox1.Text = ipaddress;
            textBox2.Text = para;
            textBox3.Text = acountid;
            textBox4.Text = correlationOrderID;
            textBox5.Text = eventid;

            textBox6.Text = marketid;
            textBox7.Text = selection;
            textBox8.Text = v1;
            textBox9.Text = v2;
            textBox10.Text = v3;

            textBox11.Text = pirce;
            textBox12.Text = amount;
            textBox13.Text = result;

            textBox1.TextChanged += TextBox_TextChanged;
            textBox2.TextChanged += TextBox_TextChanged;
            textBox3.TextChanged += TextBox_TextChanged;
            textBox4.TextChanged += TextBox_TextChanged;
            textBox5.TextChanged += TextBox_TextChanged;
            textBox6.TextChanged += TextBox_TextChanged;
            textBox7.TextChanged += TextBox_TextChanged;
            textBox8.TextChanged += TextBox_TextChanged;
            textBox9.TextChanged += TextBox_TextChanged;
            textBox10.TextChanged += TextBox_TextChanged;
            textBox11.TextChanged += TextBox_TextChanged;
            textBox12.TextChanged += TextBox_TextChanged;
            textBox13.TextChanged += TextBox_TextChanged;
            
            textBox9.TextChanged += TextBoxV_TextChanged;
            textBox10.TextChanged += TextBoxV_TextChanged;

            TextBox_TextChanged(null, null);

            textBox7.Enabled = false;
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            textBox14.Text =
                textBox1.Text +
                @"/" + textBox2.Text + @"/" + 
                textBox3.Text + @"," +
                textBox4.Text + @"," +
                textBox5.Text + @"," +

                textBox6.Text + @"," +
                textBox7.Text + @"," +
                textBox8.Text + @"," +
                textBox9.Text + @"," +
                textBox10.Text + @"," +

                textBox11.Text + @"," +
                textBox12.Text + @"," +
                textBox13.Text;
        }

        private void TextBoxV_TextChanged(object sender, EventArgs e)
        {
            textBox7.Enabled = true;
            textBox7.Text = textBox9.Text + "000"+ textBox10.Text;
            textBox7.Enabled = false;
        }


        private void label4_Click(object sender, EventArgs e)
        {
            if (sender != null)
                textBox4.Text = Guid.NewGuid().ToString();
        }
    }
}
