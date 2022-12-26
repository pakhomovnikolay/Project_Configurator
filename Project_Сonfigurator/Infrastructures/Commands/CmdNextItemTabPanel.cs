﻿using Project_Сonfigurator.Infrastructures.Commands.Base;
using System.Windows.Controls;

namespace Project_Сonfigurator.Infrastructures.Commands
{
    public class CmdNextItemTabPanel : Command
    {
        protected override bool CanExecute(object p) => p is ScrollViewer;

        protected override void Execute(object p)
        {
            if (p is not ScrollViewer MyScrollViewer) return;
            if (MyScrollViewer.HorizontalOffset >= MyScrollViewer.ScrollableWidth) return;
            MyScrollViewer.ScrollToHorizontalOffset(MyScrollViewer.HorizontalOffset + 100);
        }
    }
}
