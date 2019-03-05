using FireworksFramework.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestAppDesigner.Models;
using FireworksFramework.Managers;

namespace TestAppDesigner.ViewModels
{
    public class TestAppViewModel : ObservableObject
    {
        TestAppModel _testAppModel;

        public TestAppModel TestAppModel { get { return _testAppModel; } set { _testAppModel = value; RaisePropertyChangedEvent("TestAppModel"); } }
        public void LoadData()
        {
            TestAppModel = new TestAppModel() { DocumentText = DocumentManager.DocumentManagerInstance.Document.ToString() };
        }
    }
}
