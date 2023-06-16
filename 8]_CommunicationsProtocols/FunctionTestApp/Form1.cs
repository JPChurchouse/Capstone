using System;
using System.Windows.Forms;
using XKarts.Identifier;

namespace FunctionTestApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var red = Colour.Red;

            Console.WriteLine(red.ToString());
            Console.WriteLine(red);
            

        }
    }
}
