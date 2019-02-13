﻿///////////////////////////////////////////////
// Copyright (C) 2010-2019 ISWIX, LLC
// Web: http://www.iswix.com
// All Rights Reserved
///////////////////////////////////////////////
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using DiffPlex.DiffBuilder.Model;

namespace FireworksFramework.Services
{
    public class SideBySideDiffModelVisualizer
    {
        public static SideBySideDiffModel GetBefore(DependencyObject obj) { return (SideBySideDiffModel)obj.GetValue(BeforeProperty); }
        public static void SetBefore(DependencyObject obj, SideBySideDiffModel value) { obj.SetValue(BeforeProperty, value); }
        public static readonly DependencyProperty BeforeProperty = DependencyProperty.RegisterAttached("Before", typeof(SideBySideDiffModel), typeof(SideBySideDiffModelVisualizer), new PropertyMetadata(null, new PropertyChangedCallback(BeforeChanged)));
        private static void BeforeChanged(DependencyObject dep, DependencyPropertyChangedEventArgs e)
        {
            var richTextBox = (RichTextBox)dep;
            var diff = (SideBySideDiffModel)e.NewValue;

            var zippedDiffs = Enumerable.Zip(diff.OldText.Lines, diff.NewText.Lines, (oldLine, newLine) => new OldNew<DiffPiece> { Old = oldLine, New = newLine }).ToList();
            ShowDiffs(richTextBox, zippedDiffs, line => line.Old, piece => piece.Old);
        }

        public static SideBySideDiffModel GetAfter(DependencyObject obj) { return (SideBySideDiffModel)obj.GetValue(AfterProperty); }
        public static void SetAfter(DependencyObject obj, SideBySideDiffModel value) { obj.SetValue(AfterProperty, value); }
        public static readonly DependencyProperty AfterProperty = DependencyProperty.RegisterAttached("After", typeof(SideBySideDiffModel), typeof(SideBySideDiffModelVisualizer), new PropertyMetadata(null, new PropertyChangedCallback(AfterChanged)));
        private static void AfterChanged(DependencyObject dep, DependencyPropertyChangedEventArgs e)
        {
            var richTextBox = (RichTextBox)dep;
            var diff = (SideBySideDiffModel)e.NewValue;

            var zippedDiffs = Enumerable.Zip(diff.OldText.Lines, diff.NewText.Lines, (oldLine, newLine) => new OldNew<DiffPiece> { Old = oldLine, New = newLine }).ToList();
            ShowDiffs(richTextBox, zippedDiffs, line => line.New, piece => piece.New);
        }











        private static void ShowDiffs(RichTextBox diffBox, System.Collections.Generic.List<OldNew<DiffPiece>> lines, Func<OldNew<DiffPiece>, DiffPiece> lineSelector, Func<OldNew<DiffPiece>, DiffPiece> pieceSelector)
        {
            diffBox.Document.Blocks.Clear();
            foreach (var line in lines)
            {
                var synchroLineLength = Math.Max(line.Old.Text?.Length ?? 0, line.New.Text?.Length ?? 0);
                var lineSubPieces = Enumerable.Zip(line.Old.SubPieces, line.New.SubPieces, (oldPiece, newPiece) => new OldNew<DiffPiece> { Old = oldPiece, New = newPiece, Length = Math.Max(oldPiece.Text?.Length ?? 0, newPiece.Text?.Length ?? 0) });

                var oldNewLine = lineSelector(line);
                switch (oldNewLine.Type)
                {
                    case ChangeType.Unchanged: AppendParagraph(diffBox, oldNewLine.Text ?? string.Empty); break;
                    case ChangeType.Imaginary: AppendParagraph(diffBox, new string(BreakingSpace, synchroLineLength), Brushes.Gray, Brushes.LightCyan); break;
                    case ChangeType.Inserted: AppendParagraph(diffBox, oldNewLine.Text ?? string.Empty, Brushes.LightGreen); break;
                    case ChangeType.Deleted: AppendParagraph(diffBox, oldNewLine.Text ?? string.Empty, Brushes.OrangeRed); break;
                    case ChangeType.Modified:
                        var paragraph = AppendParagraph(diffBox, string.Empty);
                        foreach (var subPiece in lineSubPieces)
                        {
                            var oldNewPiece = pieceSelector(subPiece);
                            switch (oldNewPiece.Type)
                            {
                                case ChangeType.Unchanged: paragraph.Inlines.Add(NewRun(oldNewPiece.Text ?? string.Empty, Brushes.Yellow)); break;
                                case ChangeType.Imaginary: paragraph.Inlines.Add(NewRun(oldNewPiece.Text ?? string.Empty)); break;
                                case ChangeType.Inserted: paragraph.Inlines.Add(NewRun(oldNewPiece.Text ?? string.Empty, Brushes.LightGreen)); break;
                                case ChangeType.Deleted: paragraph.Inlines.Add(NewRun(oldNewPiece.Text ?? string.Empty, Brushes.OrangeRed)); break;
                                case ChangeType.Modified: paragraph.Inlines.Add(NewRun(oldNewPiece.Text ?? string.Empty, Brushes.Yellow)); break;
                                default: throw new ArgumentException();
                            }
                            paragraph.Inlines.Add(NewRun(new string(BreakingSpace, subPiece.Length - (oldNewPiece.Text ?? string.Empty).Length), Brushes.Gray, Brushes.LightCyan));
                        }
                        break;
                    default: throw new ArgumentException();
                }
            }
        }


        private class OldNew<T>
        {
            public T Old { get; set; }
            public T New { get; set; }
            public int Length { get; set; }
        }



        private static char BreakingSpace = '-';

        private static Paragraph AppendParagraph(RichTextBox textBox, string text, Brush background = null, Brush foreground = null)
        {
            var paragraph = new Paragraph(new Run(text))
            {
                LineHeight = 0.5,
                Background = background ?? Brushes.Transparent,
                Foreground = foreground ?? Brushes.Black,
                BorderBrush = Brushes.DarkGray,
                BorderThickness = new Thickness(0, 0, 0, 0),
            };

            textBox.Document.Blocks.Add(paragraph);
            return paragraph;
        }

        private static Run NewRun(string text, Brush background = null, Brush foreground = null) => new Run(text)
        {
            Background = background ?? Brushes.Transparent,
            Foreground = foreground ?? Brushes.Black,
        };
    }
}

