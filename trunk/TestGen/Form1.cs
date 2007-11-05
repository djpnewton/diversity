using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Genetics;

namespace TestGen
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";

            Random r = new Random();
            Nucleus nuc1 = Utils.RandomNucleus(r);
            Nucleus nuc2 = Utils.RandomNucleus(r);

            List<Nucleus> currentGen = new List<Nucleus>();
            currentGen.Add(nuc1);
            currentGen.Add(nuc2);

            textBox1.Text += "\r\n Gene Variation in first two Nucleai: " + Utils.GeneVariation(nuc1, nuc2).ToString();
            textBox1.Text += "\r\n" + nuc1.ToString();
            textBox1.Text += "\r\n" + nuc2.ToString();
            textBox1.Text += "\r\n";


            List<Nucleus> distantGeneration = DoGenerations(currentGen, 10, r);

            if (distantGeneration.Count > 1)
                textBox1.Text += "\r\n Gene Variation in first two Nucleai: " + Utils.GeneVariation(distantGeneration[0], distantGeneration[1]);
            foreach (Nucleus child in distantGeneration)
                textBox1.Text += "\r\n" + child.ToString();
        }

        List<Nucleus> DoGenerations(List<Nucleus> currentGeneration, int iterations, Random r)
        {
            List<Nucleus> reproduced = new List<Nucleus>();
            List<Nucleus> nextGen = new List<Nucleus>();

            foreach (Nucleus nuc1 in currentGeneration)
            {
                if (reproduced.Contains(nuc1))
                    continue;
                foreach(Nucleus nuc2 in currentGeneration)
                {
                    if (nuc1 != nuc2 && !reproduced.Contains(nuc2))
                    {
                        List<Nucleus> children = Reproduce(nuc1, nuc2, r);
                        foreach (Nucleus child in children)
                            nextGen.Add(child);
                        reproduced.Add(nuc1);
                        reproduced.Add(nuc2);
                        break;
                    }
                }
            }

            iterations -= 1;

            if (iterations > 0)
                return DoGenerations(nextGen, iterations, r);
            else
                return nextGen;
        }

        List<Nucleus> Reproduce(Nucleus nuc1, Nucleus nuc2, Random r)
        {
            List<Nucleus> children = new List<Nucleus>();
            int numChildren = r.Next(2, 5);
            while (numChildren > 0)
            {
                children.Add(Utils.Fertilization(Utils.MeiosisS(r, nuc1), Utils.MeiosisS(r, nuc1)));
                numChildren -= 1;
            }
            return  children;
        }
    }
}