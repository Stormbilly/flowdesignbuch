﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace csvviewer
{
    public class Ui
    {
        public event Action MoveNext;

        public void Run() {
            var exit = false;
            do {
                Console.WriteLine();
                Console.WriteLine("F)irst, P)rev, N)ext, L)ast, E)xit");

                var key = Console.ReadKey().KeyChar.ToString().ToUpper();
                Console.WriteLine();

                switch (key) {
                    case "E":
                        exit = true;
                        break;
                    case "N":
                        MoveNext();
                        break;
                }
            } while (!exit);
        }

        public void Display(IEnumerable<Record> records) {
            var columnWidths = CalculateMaxColumnWidths(records);
            var paddedRecords = PadRecords(records, columnWidths);
            var lines = FormatRecords(paddedRecords);
            WriteLines(lines);
        }

        private int[] CalculateMaxColumnWidths(IEnumerable<Record> records) {
            var noOfColumns = records.First().Values.Length;
            var result = new int[noOfColumns];

            for (var i = 0; i < noOfColumns; i++) {
                result[i] = records.Select(record => record.Values[i]).Max(value => value.Length);
            }

            return result;
        }

        private IEnumerable<Record> PadRecords(IEnumerable<Record> records, int[] columnWidths) {
            var result = records.ToArray();
            foreach (var record in result) {
                for (var i = 0; i < record.Values.Length; i++) {
                    record.Values[i] = record.Values[i].PadRight(columnWidths[i]);
                }
            }
            return result;
        }

        private IEnumerable<string> FormatRecords(IEnumerable<Record> paddedRecords) {
            return paddedRecords.Select(record => string.Join("|", record.Values));
        }

        private void WriteLines(IEnumerable<string> lines) {
            lines.ToList().ForEach(Console.WriteLine);
        }
    }
}