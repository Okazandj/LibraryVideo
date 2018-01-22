using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibraryVideo.Models;
using Microsoft.AspNetCore.Hosting;

namespace LibraryVideo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public HomeController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }
        /*
        public ActionResult Index()
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            string contentRootPath = _hostingEnvironment.ContentRootPath;

            return Content(webRootPath + "\n" + contentRootPath);
        }
        */
        protected List<String> ProcessDirectory(string targetDirectory, System.Text.RegularExpressions.Regex regex, string path = "")
        {
            List<String> listFiles = new List<String>();
            try
            {
                foreach (string d in System.IO.Directory.GetDirectories(targetDirectory)) // directories
                {
                    listFiles.AddRange(ProcessDirectory(d, regex, path + "\\" + System.IO.Path.GetFileName(d)));
                    //listFiles.AddRange(ProcessDirectory(targetDirectory + d, regex));
                }
                foreach (var file in System.IO.Directory.GetFiles(targetDirectory))
                {
                    if (regex.Match(file).Success)
                    {
                        listFiles.Add(path + "\\" + System.IO.Path.GetFileNameWithoutExtension(file) + System.IO.Path.GetExtension(file));
                    }
                }
            }
            catch (System.Exception e)
            {
                listFiles.Add("Error during processing the directory: " + targetDirectory);
            }
            /*
            if (System.IO.File.Exists(targetDirectory))
            {
                // This path is a file
                //listFiles.Add(System.IO.Path.GetFileNameWithoutExtension(targetDirectory));
                //listFiles.Add(targetDirectory);
                listFiles.Add(System.IO.Path.GetFileNameWithoutExtension(targetDirectory) + System.IO.Path.GetExtension(targetDirectory));
                return listFiles;
            }
            else if (System.IO.Directory.Exists(targetDirectory))
            {
                // this path is a directory
                // Process the list of files found in the directory.
                string[] fileEntries = System.IO.Directory.GetFiles(targetDirectory);
                foreach (string fileName in fileEntries)
                {
                    if (regex.Match(fileName).Success)
                    {
                        listFiles.Add(System.IO.Path.GetFileNameWithoutExtension(fileName) + System.IO.Path.GetExtension(fileName));
                        //listFiles.Add(System.IO.Path.GetFileNameWithoutExtension(fileName));
                        //listFiles.Add(System.IO.Path.GetFileNameWithoutExtension(targetDirectory) + '/' + System.IO.Path.GetFileNameWithoutExtension(fileName));
                    }
                }
            }
            // else not interresting
            */
            return listFiles;
        }

        public IActionResult Index()
        {
            List<String> listFiles = new List<String>();
            string webRootPath = _hostingEnvironment.WebRootPath;
            var videoDirectoryPath = webRootPath + @"\\videos";
            var imgDirectoryPath = webRootPath + @"\\images";
            
            // Checking images and videos
            System.Text.RegularExpressions.Regex imgRgx = new System.Text.RegularExpressions.Regex(@"^.*\.(jpg|JPG|gif|GIF|doc|DOC|pdf|PDF|png|PNG)$");
            System.Text.RegularExpressions.Regex videoRgx = new System.Text.RegularExpressions.Regex(@"^.*\.(avi|AVI|wmv|WMV|flv|FLV|mpg|MPG|mp4|MP4|mkv|MKV)$");

            List<String> listVideoFileNames = new List<String>(); // ProcessDirectory(videoDirectoryPath, videoRgx);
            List<String> listImgFileNames = ProcessDirectory(imgDirectoryPath, imgRgx);
            return View(new Tuple<List<String>, List<String>>(listVideoFileNames, listImgFileNames));
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
