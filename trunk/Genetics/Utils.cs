using System;
using System.Collections.Generic;
using System.Text;

namespace Genetics
{
    public static class Utils
    {
        const int BasePairsPerGene = 1;
        const int GenesPerChromosome = 5;
        const int ChromosomePairsPerNucleus = 4;

        public static string NucleotideAbbrString(Nucleotide nuc)
        {
            switch (nuc)
            {
                case Nucleotide.Adenine:
                    return "A";
                case Nucleotide.Cytosine:
                    return "C";
                case Nucleotide.Guanine:
                    return "G";
                case Nucleotide.Thymine:
                    return "T";
            }
            return "";
        }

        public static BasePair RandomBasePair(Random r)
        {
            if (r.Next(2) == 0)
            {
                // use Adenine and Thymine
                if (r.Next(2) == 0)
                    return new BasePair(Nucleotide.Adenine, Nucleotide.Thymine);
                else
                    return new BasePair(Nucleotide.Thymine, Nucleotide.Adenine);
            }
            else
            {
                // use Guanine and Cytosine
                if (r.Next(2) == 0)
                    return new BasePair(Nucleotide.Guanine, Nucleotide.Cytosine);
                else
                    return new BasePair(Nucleotide.Cytosine, Nucleotide.Guanine);
            }
        }

        public static Gene RandomGene(Random r)
        {
            List<BasePair> basePairs = new List<BasePair>();
            for (int i = 0; i < BasePairsPerGene; i++)
                basePairs.Add(RandomBasePair(r));
            return new Gene(basePairs);
        }

        public static Chromosome RandomChromosome(Random r)
        {
            List<Gene> genes = new List<Gene>();
            for (int i = 0; i < GenesPerChromosome; i++)
                genes.Add(RandomGene(r));
            return new Chromosome(genes);
        }

        public static Nucleus RandomNucleus(Random r)
        {
            List<ChromosomePair> chromosomePairs = new List<ChromosomePair>();
            for (int i = 0; i < ChromosomePairsPerNucleus; i++)
                chromosomePairs.Add(new ChromosomePair(RandomChromosome(r), RandomChromosome(r)));
            return new Nucleus(chromosomePairs);
        }

        public static GameteNucleus MeiosisS(Random r, Nucleus nuc)
        {
            MeiosisResult me = Meiosis(r, nuc);
            int num = r.Next(0, 3);
            switch (num)
            {
                case 0:
                    return me.Nuc1;
                case 1:
                    return me.Nuc2;
                case 2:
                    return me.Nuc3;
                default:
                    return me.Nuc4;
            }
        }

        public static MeiosisResult Meiosis(Random r, Nucleus nuc)
        {
            // Interphase S period (chromosomes replicate)
            List<ChromosomePair> chromPairs1 = new List<ChromosomePair>(DeepCopy(nuc.ChromosomePairs));
            List<ChromosomePair> chromPairs2 = new List<ChromosomePair>(DeepCopy(nuc.ChromosomePairs));

            // Prophase I - Pachytene stage (chromosomal crossover)
            for (int i = 0; i < chromPairs1.Count; i++)
            {
                ChromosomalCrossover(r, chromPairs1[i]);
                ChromosomalCrossover(r, chromPairs2[i]);
            }

            // Anaphase II (separate chromosome pairs)
            List<Chromosome> chroms1 = new List<Chromosome>();
            List<Chromosome> chroms2 = new List<Chromosome>();
            foreach (ChromosomePair chromPair in chromPairs1)
            {
                chroms1.Add(chromPair.Chrom1);
                chroms2.Add(chromPair.Chrom2);
            }
            List<Chromosome> chroms3 = new List<Chromosome>();
            List<Chromosome> chroms4 = new List<Chromosome>();
            foreach (ChromosomePair chromPair in chromPairs2)
            {
                chroms3.Add(chromPair.Chrom1);
                chroms4.Add(chromPair.Chrom2);
            }

            // Telophase II (four new gametes)
            return new MeiosisResult(new GameteNucleus(chroms1), new GameteNucleus(chroms2),
                new GameteNucleus(chroms3), new GameteNucleus(chroms4));
        }

        static List<ChromosomePair> DeepCopy(List<ChromosomePair> chromPairs)
        {
            List<ChromosomePair> chromPairsCopy = new List<ChromosomePair>();
            foreach (ChromosomePair chromPair in chromPairs)
                chromPairsCopy.Add(DeepCopy(chromPair));
            return chromPairsCopy;
        }

        static ChromosomePair DeepCopy(ChromosomePair chromPair)
        {
            return new ChromosomePair(DeepCopy(chromPair.Chrom1), DeepCopy(chromPair.Chrom2));
        }

        static Chromosome DeepCopy(Chromosome chrom)
        {
            List<Gene> genesCopy = new List<Gene>();
            foreach (Gene gene in chrom.Genes)
                genesCopy.Add(DeepCopy(gene));
            return new Chromosome(genesCopy);
        }

        static Gene DeepCopy(Gene gene)
        {
            List<BasePair> dnaCopy = new List<BasePair>();
            foreach (BasePair bp in gene.Dna)
                dnaCopy.Add(bp);
            return new Gene(dnaCopy);
        }

        // http://en.wikipedia.org/wiki/Chromosomal_crossover

        static void ChromosomalCrossover(Random r, ChromosomePair chromPair)
        {
            ChromosomalCrossover(r, chromPair.Chrom1, chromPair.Chrom2);
        }

        static void ChromosomalCrossover(Random r, Chromosome chromA, Chromosome chromB)
        {
            System.Diagnostics.Debug.Assert(chromA.Genes.Count == chromA.Genes.Count, "ChromosomalCrossover Failed: chromosomes have different number of genes");
            bool swappingGenes = false;
            Gene temp;
            for (int i = 0; i < chromA.Genes.Count; i++)
            {
                if (r.Next(11) > 8)
                    swappingGenes = !swappingGenes;
                if (swappingGenes)
                {
                    temp = chromA.Genes[i];
                    chromA.Genes[i] = chromB.Genes[i];
                    chromB.Genes[i] = temp;
                }
            }
        }

        public static Nucleus Fertilization(GameteNucleus nuc1, GameteNucleus nuc2)
        {
            List<Chromosome> chroms1 = nuc1.Chromosomes;
            List<Chromosome> chroms2 = nuc2.Chromosomes;
            System.Diagnostics.Debug.Assert(chroms1.Count == chroms2.Count, "Fertilization Failed: gametes have different number of chromosomes");
            List<ChromosomePair> chromosomePairs = new List<ChromosomePair>();
            for (int i = 0; i < chroms1.Count; i++)
                chromosomePairs.Add(new ChromosomePair(chroms1[i], chroms2[i]));
            return new Nucleus(chromosomePairs);
        }

        public static bool GeneticallyEqual(GameteNucleus nuc1, GameteNucleus nuc2)
        {
            if (nuc1.Chromosomes.Count != nuc2.Chromosomes.Count)
                return false;
            for (int i = 0; i < nuc1.Chromosomes.Count; i++)
                if (!GeneticallyEqual(nuc1.Chromosomes[i], nuc2.Chromosomes[i]))
                    return false;
            return true;
        }

        public static bool GeneticallyEqual(Nucleus nuc1, Nucleus nuc2)
        {
            if (nuc1.ChromosomePairs.Count != nuc2.ChromosomePairs.Count)
                return false;
            for (int i = 0; i < nuc1.ChromosomePairs.Count; i++)
                if (!GeneticallyEqual(nuc1.ChromosomePairs[i], nuc2.ChromosomePairs[i]))
                    return false;
            return true;
        }

        public static bool GeneticallyEqual(ChromosomePair chromPair1, ChromosomePair chromPair2)
        {
            if (!GeneticallyEqual(chromPair1.Chrom1, chromPair2.Chrom1))
                return false;
            if (!GeneticallyEqual(chromPair1.Chrom2, chromPair2.Chrom2))
                return false;
            return true;
        }

        public static bool GeneticallyEqual(Chromosome chrom1, Chromosome chrom2)
        {
            if (chrom1.Genes.Count != chrom2.Genes.Count)
                return false;
            for (int i = 0; i < chrom1.Genes.Count; i++)
                if (!GeneticallyEqual(chrom1.Genes[i], chrom2.Genes[i]))
                    return false;
            return true;
        }

        public static bool GeneticallyEqual(Gene gene1, Gene gene2)
        {
            if (gene1.Dna.Count != gene2.Dna.Count)
                return false;
            for (int i = 0; i < gene1.Dna.Count; i++)
                if (!GeneticallyEqual(gene1.Dna[i], gene2.Dna[i]))
                    return false;
            return true;
        }

        public static bool GeneticallyEqual(BasePair bp1, BasePair bp2)
        {
            if (bp1.NucA == bp2.NucA && bp1.NucB == bp2.NucB)
                return true;
            return false;
        }

        public static int GeneVariation(Nucleus baseNucleus, Nucleus compNucleus)
        {
            int variation = 0;
            for (int j = 0; j < compNucleus.ChromosomePairs.Count; j++)
            {
                variation += GeneVariation(baseNucleus.ChromosomePairs[j].Chrom1, compNucleus.ChromosomePairs[j].Chrom1);
                variation += GeneVariation(baseNucleus.ChromosomePairs[j].Chrom2, compNucleus.ChromosomePairs[j].Chrom2);
            }
            return variation;
        }

        public static int GeneVariation(Chromosome chrom1, Chromosome chrom2)
        {
            int variation = 0;
            for (int i = 0; i < chrom1.Genes.Count; i++)
                if (!Utils.GeneticallyEqual(chrom1.Genes[i], chrom2.Genes[i]))
                    variation += 1;
            return variation;
        }
    }
}
