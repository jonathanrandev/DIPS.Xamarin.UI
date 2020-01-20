﻿using System;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DIPS.Xamarin.UI.Controls.Sheet
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SheetView : ContentView
    {
        private readonly SheetBehavior m_sheetBehaviour;

        public SheetView(SheetBehavior sheetBehavior)
        {
            InitializeComponent();
            OuterSheetFrame.BindingContext = m_sheetBehaviour = sheetBehavior;
        }

        /// <summary>
        /// The height that the sheet content needs if it should display all of its content
        /// </summary>
        public double SheetContentHeighRequest =>
            SheetContent.Content.Height + HandleBoxView.Height + OuterSheetFrame.Padding.Top + OuterSheetFrame.Padding.Bottom + OuterSheetFrame.CornerRadius;

        public Frame SheetFrame => OuterSheetFrame;

        private double m_newY;
        private void OnDrag(object sender, PanUpdatedEventArgs e)
        {
            if (!m_sheetBehaviour.IsDraggable) return;
            if (m_newY == 0) m_newY = SheetFrame.TranslationY;

            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    break;
                case GestureStatus.Running:

                    var translationY = (Device.RuntimePlatform == Device.Android) ? OuterSheetFrame.TranslationY : m_newY;
                    var newYTranslation = e.TotalY + translationY;
                    //Hack to remove jitter from android 
                    if (Device.RuntimePlatform == Device.Android)
                    {
                        e = new PanUpdatedEventArgs(e.StatusType, e.GestureId, 0, newYTranslation);
                        newYTranslation = e.TotalY;
                    }

                    m_sheetBehaviour.UpdatePosition(newYTranslation);
                    break;
                case GestureStatus.Completed:
                    m_newY = SheetFrame.TranslationY;
                    //Snap?
                    break;
                case GestureStatus.Canceled:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Initialize()
        {
            if (m_sheetBehaviour.Alignment == AlignmentOptions.Top)
            {
                SheetGrid.RowDefinitions[0].Height = GridLength.Star;
                SheetGrid.RowDefinitions[1].Height = GridLength.Auto;
                Grid.SetRow(SheetContentGrid, 0);
                Grid.SetRow(HandleBoxView, 1);
            }

            switch (m_sheetBehaviour.VerticalContentAlignment)
            {
                case ContentAlignment.Fit:
                    SheetContentGrid.VerticalOptions = m_sheetBehaviour.Alignment == AlignmentOptions.Top ? LayoutOptions.EndAndExpand : LayoutOptions.StartAndExpand;
                    break;
                case ContentAlignment.Fill:
                    SheetContentGrid.VerticalOptions = LayoutOptions.Fill;
                    break;
                default:
                    break;
            }
        }
    }
}