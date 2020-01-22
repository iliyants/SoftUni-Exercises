using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.WebServer.ViewEngine
{
    public interface IViewEngine
    {
        string GetHtml<T>(string viewContent, T model);
    }
}
