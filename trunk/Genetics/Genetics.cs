using System;
using System.Collections.Generic;
using System.Text;

namespace Genetics
{
    public enum Nucleotide { Adenine, Thymine, Guanine, Cytosine };

    public struct BasePair
    {
        Nucleotide nucA, nucB;
        public Nucleotide NucA
        {
            get { return nucA; }
        }
        public Nucleotide NucB
        {
            get { return nucB; }
        }

        public BasePair(Nucleotide nucA, Nucleotide nucB)
        {
            this.nucA = nucA;
            this.nucB = nucB;
        }

        public override string ToString()
        {
            return Utils.NucleotideAbbrString(nucA) + Utils.NucleotideAbbrString(NucB);
        }
    }

    public class Gene
    {
        List<BasePair> dna;
        public List<BasePair> Dna
        {
            get { return dna; }
        }

        public Gene(List<BasePair> dna)
        {
            this.dna = dna;
        }

        public override string ToString()
        {
            string result = "";
            foreach (BasePair bp in dna)
                result += bp.ToString();
            return result;
        }
    }

    public class Chromosome
    {
        List<Gene> genes;
        public List<Gene> Genes
        {
            get { return genes; }
        }

        public Chromosome(List<Gene> genes)
        {
            this.genes = genes;
        }

        public override string ToString()
        {
            string result = "";
            foreach (Gene gene in genes)
                result += gene.ToString();
            return result;
        }
    }

    public struct ChromosomePair
    {
        Chromosome chrom1, chrom2;
        public Chromosome Chrom1
        {
            get { return chrom1; }
            set { chrom1 = value; }
        }
        public Chromosome Chrom2
        {
            get { return chrom2; }
            set { chrom2 = value; }
        }

        public ChromosomePair(Chromosome chrom1, Chromosome chrom2)
        {
            this.chrom1 = chrom1;
            this.chrom2 = chrom2;
        }

        public override string ToString()
        {
            return chrom1.ToString() + chrom2.ToString();
        }
    }

    public class Nucleus
    {
        List<ChromosomePair> chromosomePairs;
        public List<ChromosomePair> ChromosomePairs
        {
            get { return chromosomePairs; }
        }

        public Nucleus(List<ChromosomePair> chromosomePairs)
        {
            this.chromosomePairs = chromosomePairs;
        }

        public override string ToString()
        {
            string result = "";
            foreach (ChromosomePair cp in chromosomePairs)
                result += cp.ToString();
            return result;
        }
    }

    public class GameteNucleus
    {
        List<Chromosome> chromosomes;
        public List<Chromosome> Chromosomes
        {
            get { return chromosomes; }
        }

        public GameteNucleus(List<Chromosome> chromosomes)
        {
            this.chromosomes = chromosomes;
        }
    }

    public struct MeiosisResult
    {
        public GameteNucleus Nuc1, Nuc2, Nuc3, Nuc4;

        public MeiosisResult(GameteNucleus nuc1, GameteNucleus nuc2, GameteNucleus nuc3, GameteNucleus nuc4)
        {
            Nuc1 = nuc1;
            Nuc2 = nuc2;
            Nuc3 = nuc3;
            Nuc4 = nuc4;
        }
    }
}
