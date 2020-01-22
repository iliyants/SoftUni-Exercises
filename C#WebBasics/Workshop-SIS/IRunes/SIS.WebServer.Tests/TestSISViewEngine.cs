
using SIS.WebServer.ViewEngine;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace SIS.WebServer.Tests{
    public class TestSISViewEngine
    {
        [Theory]
        [InlineData("TestWithoutCSharpCode")]
        [InlineData("UseForForeachAndIf")]
        [InlineData("UseModelData")]
        public void TestGetHtml(string testFileName)
        {
            IViewEngine viewEngine = new SISViewEngine();
            var viewFileName = $"ViewTests/{testFileName}.html";
            var expectedFileName = $"ViewTests/{testFileName}.Result.html";

            var viewContent = File.ReadAllText(viewFileName);

            var expectedResult = File.ReadAllText(expectedFileName); 

            var actualResult = viewEngine.GetHtml<object>(viewContent, new TestViewModel()
            {
                StringValue = "str",
                ListValues = new List<string>()
                {
                    "123","val1",string.Empty
                }
            });


            Assert.Equal(expectedResult.TrimEnd(), actualResult.TrimEnd());
        }
    }
}
