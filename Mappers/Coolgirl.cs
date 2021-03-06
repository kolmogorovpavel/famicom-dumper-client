﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Cluster.Famicom.Mappers
{
    public class Coolgirl : IMapper
    {
        public string Name
        {
            get { return "Coolgirl"; }
        }

        public int Number
        {
            get { return -1; }
        }
        public string UnifName
        {
            get { return "COOLGIRL"; }
        }

        public int DefaultPrgSize
        {
            get { return 1024 * 1024 * 64; }
        }

        public int DefaultChrSize
        {
            get { return 0; }
        }

        public void DumpPrg(FamicomDumperConnection dumper, List<byte> data, int size)
        {
            int prgBanks = size / 0x8000;
            Console.Write("Reset... ");
            dumper.Reset();
            Console.WriteLine("OK");

            for (int bank = 0; bank < prgBanks; bank++)
            {
                byte r0 = (byte)(bank >> 7);
                byte r1 = (byte)(bank << 1);
                dumper.WriteCpu(0x5000, r0);
                dumper.WriteCpu(0x5001, r1);
                dumper.WriteCpu(0x5002, 0xFE);

                Console.Write("Reading PRG bank #{0}/{1}... ", bank, prgBanks);
                data.AddRange(dumper.ReadCpu(0x8000, 0x8000));
                Console.WriteLine("OK");
            }
            Console.WriteLine("Done!");
        }

        public void DumpChr(FamicomDumperConnection dumper, List<byte> data, int size)
        {
            return; // Нету тут CHR
        }

        public void EnablePrgRam(FamicomDumperConnection dumper)
        {
            dumper.Reset();
            dumper.WriteCpu(0x5007, 0x01); // enable SRAM
            dumper.WriteCpu(0x5005, 0x02); // select bank
        }
    }
}
