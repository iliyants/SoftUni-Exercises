using System;
using System.Collections.Generic;
using System.Text;

namespace SIS.WebServer.ViewEngine
{
    public interface IView
    {
        string GetHtml(object model);
    }
}
